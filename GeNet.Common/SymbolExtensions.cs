using Microsoft.CodeAnalysis;

namespace GeNet.Common;

public static class SymbolExtensions
{
    public static bool ImplementsInterface(this ISymbol symbol, INamedTypeSymbol namedInterface) =>
        symbol switch
        {
            IFieldSymbol fs =>
                fs.Type.Equals(namedInterface, SymbolEqualityComparer.Default) ||
                fs.Type.AllInterfaces.Contains(namedInterface),
            IPropertySymbol ps =>
                ps.Type.Equals(namedInterface, SymbolEqualityComparer.Default) ||
                ps.Type.AllInterfaces.Contains(namedInterface),
            ITypeSymbol ts =>
                ts.Equals(namedInterface, SymbolEqualityComparer.Default) ||
                ts.AllInterfaces.Contains(namedInterface),
            _ => false
        };

    public static bool HasAttribute(this ISymbol symbol, ISymbol attributeSymbol) =>
        symbol.GetAttributes().Any(a => a.AttributeClass?.Equals(attributeSymbol, SymbolEqualityComparer.Default) ?? false);

}
