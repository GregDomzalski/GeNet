using System.Diagnostics.CodeAnalysis;

namespace GeNet.IntegrationTests.Equals;

[GenerateEquals]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible")]
[SuppressMessage("Performance", "CA1822:Mark members as static")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public partial struct TestStruct
{
    public static string StaticField = null!;
    public string Field1;
    public int Field2;

    public static string StaticProperty { get; set; } = null!;
    public string Prop1 { get; set; }
    public int Prop2 { get; set; }

    [IgnoredByEquals]
    public int IgnoredField;
    public int SetOnlyProp { set => _ = value; }
}
