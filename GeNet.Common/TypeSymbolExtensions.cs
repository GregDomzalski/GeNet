using Microsoft.CodeAnalysis;

namespace GeNet.Common;

public static class TypeSymbolExtensions
{
    public static IEnumerable<IFieldSymbol> GetFields(this ITypeSymbol typeSymbol, bool includeBackingFields = false)
    {
        Func<IFieldSymbol, bool> predicate = includeBackingFields
            ? static _ => true
            : static f => !f.IsImplicitlyDeclared;

        return typeSymbol.GetMembers().OfType<IFieldSymbol>().Where(predicate);
    }

    public static IEnumerable<IPropertySymbol> GetProperties(this ITypeSymbol typeSymbol) =>
        typeSymbol.GetMembers().OfType<IPropertySymbol>();

    public static string? GetNamespaceName(this ITypeSymbol t) =>
        !t.ContainingNamespace.IsGlobalNamespace
            ? t.ContainingNamespace.ToString()
            : null;

    public static IMethodSymbol? FindMethod(this ITypeSymbol t, string name) =>
        t is INamedTypeSymbol nts
            ? nts.GetMembers(name).OfType<IMethodSymbol>().FirstOrDefault()
            : null;

    public static string ToTypeKindString(this ITypeSymbol t) =>
        t.TypeKind switch
        {
            TypeKind.Class => t.IsRecord ? "record" : "class",
            TypeKind.Struct => t.IsRecord ? "record struct" : "struct",
            TypeKind.Interface => "interface",
            _ => throw new NotSupportedException("This symbol is not used for declarations.")
        };

    public static IMethodSymbol? FindBaseMethod(this ITypeSymbol symbol, string methodName)
    {
        var baseType = symbol.BaseType;

        while (baseType is not null)
        {
            var baseMethod = baseType
                .GetMembers(methodName)
                .OfType<IMethodSymbol>()
                .FirstOrDefault();

            if (baseMethod is not null) return baseMethod;

            baseType = baseType.BaseType;
        }

        return null;
    }
}
