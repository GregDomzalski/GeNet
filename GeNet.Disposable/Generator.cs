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

        return new GenerationInfo
        {
            ContainingNamespace = typeSymbol.GetNamespaceName(),
            Kind = typeSymbol.ToTypeKindString(),
            Name = typeSymbol.Name,
            DisposableMembers = relevantMembers,
            ExplicitManagedDisposeMethod = disposeManagedMethod,
            ExplicitUnmanagedDisposeMethod = disposeUnmanagedMethod,
        };
    }

    private static void GenerateSource(
        SourceProductionContext context,
        ImmutableArray<GenerationInfo> types)
    {
        if (types.IsDefaultOrEmpty)
        {
            return;
        }

        foreach (var type in types)
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            DisposeCodeBuilder.Generate(context, type);
            // context.AddSource($"{typeToGenerate.Name}.Disposable.g.cs", sourceText);
        }
    }
}
