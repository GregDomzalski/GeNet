using Microsoft.CodeAnalysis.CSharp;

namespace GeNet.Disposable;

public class GenerateDisposeGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var types = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: SyntaxFilter,
                transform: SemanticTransform)
            .Where(static t => t is not null)
            .Select(static (t, _) => t!)
            .Collect();

        context.RegisterSourceOutput(types, GenerateSource);
    }

    private static bool SyntaxFilter(SyntaxNode syntaxNode, CancellationToken _) =>
        SyntaxFilters.IsClassOrStructOrRecord(syntaxNode)
        && SyntaxFilters.HasAtLeastOneBaseType(syntaxNode);

    private static GenerationInfo? SemanticTransform(
        GeneratorSyntaxContext context,
        CancellationToken cancellationToken)
    {
        if (context.Node is not TypeDeclarationSyntax syntaxNode) return null;

        var typeSymbol = context.SemanticModel.GetDeclaredSymbol(syntaxNode) as ITypeSymbol;
        var disposableSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("System.IDisposable");

        if (typeSymbol is null) return null;
        if (disposableSymbol is null) return null;
        if (!typeSymbol.AllInterfaces.Contains(disposableSymbol)) return null;

        cancellationToken.ThrowIfCancellationRequested();

        var relevantMembers = Enumerable.Empty<ISymbol>()
            .Concat(typeSymbol.GetFields())
            .Concat(typeSymbol.GetProperties())
            .Distinct(SymbolEqualityComparer.Default)
            .Where(s => s.ImplementsInterface(disposableSymbol))
            .ToImmutableArray();

        cancellationToken.ThrowIfCancellationRequested();

        var disposeManagedMethod = typeSymbol.FindMethod("DisposeManaged");
        cancellationToken.ThrowIfCancellationRequested();

        var disposeUnmanagedMethod = typeSymbol.FindMethod("DisposeUnmanaged");
        cancellationToken.ThrowIfCancellationRequested();

        var hasBaseDispose = false;
        if (typeSymbol.BaseType is not null)
            hasBaseDispose = typeSymbol.BaseType.ImplementsInterface(disposableSymbol);

        return new GenerationInfo
        {
            ContainingNamespace = typeSymbol.GetNamespaceName(),
            Kind = typeSymbol.ToTypeKindString(),
            Name = typeSymbol.Name,
            DisposableMembers = relevantMembers,
            ExplicitManagedDisposeMethod = disposeManagedMethod,
            ExplicitUnmanagedDisposeMethod = disposeUnmanagedMethod,
            BaseHasDisposeMethod = hasBaseDispose,
            QualifiedName = typeSymbol.ToDisplayString()
        };
    }

    private static void GenerateSource(
        SourceProductionContext context,
        ImmutableArray<GenerationInfo> typesToGenerate)
    {
        if (typesToGenerate.IsDefaultOrEmpty)
        {
            return;
        }

        foreach (var typeToGenerate in typesToGenerate)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var fileName = FileName.Create(typeToGenerate.QualifiedName, "Dispose");
            var sourceText = DisposeCodeBuilder.Generate(context, typeToGenerate);

            context.AddSource(fileName, sourceText);
        }
    }
}
