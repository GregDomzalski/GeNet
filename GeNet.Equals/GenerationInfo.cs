namespace GeNet.Equals;

internal class GenerationInfo
{
    public string? ContainingNamespace { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string QualifiedName { get; set; } = string.Empty;
    public bool ValueType { get; set; }
    public ImmutableArray<ISymbol> Members { get; set; }
}
