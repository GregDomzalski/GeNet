using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GeNet.Common;

public static class SemanticFilters
{
    public static TypeDeclarationSyntax? HasAttribute(
        GeneratorSyntaxContext context,
        string attributeFullName,
        CancellationToken cancellationToken)
    {
        if (context.Node is not TypeDeclarationSyntax syntaxNode) return null;

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        // Justification: LINQ introduces unnecessary allocations and/or boxing.
        foreach (var attributeListSyntax in syntaxNode.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                {
                    continue;
                }

                var attributedType = attributeSymbol.ContainingType;

                if (attributedType.ToDisplayString() == attributeFullName)
                {
                    return syntaxNode;
                }
            }
        }

        return null;
    }

    public static ITypeSymbol? HasBaseType(
        GeneratorSyntaxContext context,
        string baseTypeName,
        CancellationToken cancellationToken)
    {
        if (context.Node is not TypeDeclarationSyntax syntaxNode) return null;

        var symbol = context.SemanticModel.GetDeclaredSymbol(syntaxNode) as ITypeSymbol;
        var baseType = context.SemanticModel.Compilation.GetTypeByMetadataName(baseTypeName);

        if (symbol is null) return null;
        if (baseType is null) return null;

        return symbol.AllInterfaces.Contains(baseType) ? symbol : null;
    }
}
