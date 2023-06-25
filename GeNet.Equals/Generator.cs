namespace GeNet.Equals;

// TODO:
// - What about inheritance? Checking base structures and allowing inheritance

[Generator(LanguageNames.CSharp)]
public class GenerateEqualsGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "GeNet.Equals.Attribute.g.cs",
            SourceText.From(Attribute.Text, Encoding.UTF8)));

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
        var genEqualsAttrib = context.SemanticModel.Compilation.GetTypeByMetadataName("GeNet.GenerateEqualsAttribute");

        if (genEqualsAttrib is null)
        {
            return null;
        }

        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var relevantMembers = Enumerable.Empty<ISymbol>()
            .Concat(typeSymbol.GetMembers().OfType<IFieldSymbol>().Where(f => f.IsImplicitlyDeclared == false))
            .Concat(typeSymbol.GetMembers().OfType<IPropertySymbol>())
            .Distinct(SymbolEqualityComparer.Default)
            .ToImmutableArray();

        if (relevantMembers.Length == 0)
        {
            return null;
        }

        return new GenerationInfo
        {
            ContainingNamespace = typeSymbol.ContainingNamespace.IsGlobalNamespace ? null : typeSymbol.ContainingNamespace.ToString(),
            Kind = typeSymbol.TypeKind == TypeKind.Class ? "class" : "struct",
            Name = typeSymbol.Name,
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
            var sourceText = EqualsCodeBuilder.Generate(context, typeToGenerate);
            context.AddSource($"{typeToGenerate.Name}.Equals.g.cs", sourceText);
        }
    }
}
