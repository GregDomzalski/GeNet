namespace GeNet.Equals;

[Generator(LanguageNames.CSharp)]
public class GenerateEqualsGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource(
                "GeNet.Equals.GenerateEqualsAttribute.g.cs",
                SourceText.From(Attributes.GenerateEqualsText, Encoding.UTF8));

            ctx.AddSource(
                "GeNet.Equals.IgnoredByEqualsAttribute.g.cs",
                SourceText.From(Attributes.IgnoredByEqualsText, Encoding.UTF8));
        });

        var equalityTypes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "GeNet.GenerateEqualsAttribute",
                predicate: SyntaxFilter,
                transform: SemanticTransform)
            .Where(static t => t is not null)
            .Select(static (t, _) => t!)
            .Collect();

        context.RegisterSourceOutput(equalityTypes, GenerateSource);
    }

    private static bool SyntaxFilter(SyntaxNode syntaxNode, CancellationToken _) =>
        SyntaxFilters.IsClassOrStruct(syntaxNode);

    private static GenerationInfo? SemanticTransform(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        var genEqualsAttrib = context.SemanticModel.Compilation.GetTypeByMetadataName("GeNet.GenerateEqualsAttribute");

        if (genEqualsAttrib is null)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var ignoreAttribute = context.SemanticModel.Compilation.GetTypeByMetadataName("GeNet.IgnoredByEqualsAttribute");

        if (ignoreAttribute is null)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var relevantMembers = Enumerable.Empty<ISymbol>()
            .Concat(typeSymbol.GetMembers().OfType<IFieldSymbol>().Where(f => f.IsImplicitlyDeclared == false))
            .Concat(typeSymbol.GetMembers().OfType<IPropertySymbol>().Where(p => p.GetMethod is not null))
            .Distinct(SymbolEqualityComparer.Default)
            .Where(s => !s.HasAttribute(ignoreAttribute) && !s.IsStatic)
            .ToImmutableArray();

        if (relevantMembers.Length == 0)
        {
            return null;
        }

        return new GenerationInfo
        {
            ContainingNamespace = typeSymbol.ContainingNamespace.IsGlobalNamespace ? null : typeSymbol.ContainingNamespace.ToString(),
            Kind = typeSymbol.TypeKind == TypeKind.Class ? "class" : "struct",
            ValueType = typeSymbol.IsValueType,
            Name = typeSymbol.Name,
            QualifiedName = typeSymbol.ToDisplayString(),
            Members = relevantMembers
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

            var fileName = FileName.Create(typeToGenerate.QualifiedName, "Equals");
            var sourceText = EqualsCodeBuilder.Generate(context, typeToGenerate);

            context.AddSource(fileName, sourceText);
        }
    }
}
