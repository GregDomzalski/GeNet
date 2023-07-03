using Microsoft.CodeAnalysis;

namespace GeNet.Common;

public static class MethodSymbolExtensions
{
    public static bool HasParameter(this IMethodSymbol? methodSymbol, Func<IParameterSymbol, bool> predicate) =>
        methodSymbol is not null &&
        methodSymbol.Parameters.Any(predicate);
}
