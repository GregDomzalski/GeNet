namespace GeNet.Disposable;

internal class GenerationInfo
{
    public string? ContainingNamespace { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ImmutableArray<ISymbol> DisposableMembers { get; set; }
    public IMethodSymbol? ExplicitManagedDisposeMethod { get; set; }
    public IMethodSymbol? ExplicitUnmanagedDisposeMethod { get; set; }
}
