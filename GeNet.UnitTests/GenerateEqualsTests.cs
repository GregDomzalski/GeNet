namespace GeNet.UnitTests;

// Test cases:
// - Records / Interfaces - not supported
// - What about inheritance? Checking base structures and allowing inheritance

[UsesVerify]
public class EqualsGeneratorTests
{
    [Fact]
    public Task SingleField()
    {
        const string source = @"using GeNet;

[GenerateEquals]
public class TestClass
{
    public string _field1;
}";

        return SnapshotHelper.Verify(source);
    }

    [Fact]
    public Task SingleProperty()
    {
        const string source = @"using GeNet;

[GenerateEquals]
public class TestClass
{
    public string Prop1 { get; set; }
}";

        return SnapshotHelper.Verify(source);
    }

    [Fact]
    public Task NoMembers()
    {
        const string source = @"using GeNet;

[GenerateEquals]
public class TestClass
{
}";

        return SnapshotHelper.Verify(source);
    }

    [Fact]
    public Task SameClassNameDifferentNamespaces()
    {
        const string source = @"using GeNet;

namespace N1
{
[GenerateEquals]
public class TestClass
{
    public string _field1;
}
}

namespace N2
{
[GenerateEquals]
public class TestClass
{
    public string _field1;
}
}";

        return SnapshotHelper.Verify(source);
    }

    [Fact]
    public Task ClassWithTypeParameter()
    {
        const string source = @"using GeNet;

namespace N1
{
[GenerateEquals]
public class TestClass<T>
{
    public T _field1;
}
}";

        return SnapshotHelper.Verify(source);
    }

    [Fact]
    public Task MultiplePartials()
    {
        const string source = @"using GeNet;

namespace N1
{
[GenerateEquals]
public partial class TestClass
{
    public string _field1;
}

public partial class TestClass
{
    public string _field2;
}
}";

        return SnapshotHelper.Verify(source);
    }

    [Fact]
    public Task Struct()
    {
        const string source = @"using GeNet;

[GenerateEquals]
public partial struct TestStruct
{
    public string _field1;
}";

        return SnapshotHelper.Verify(source);
    }

    [Fact]
    public Task InheritanceCallsBase()
    {
        const string source = @"using GeNet;

[GenerateEquals]
public partial class Derived : Base
{
    public string _field2;
}

[GenerateEquals]
public class Base
{
    public string _field1;
}";

        return SnapshotHelper.Verify(source);
    }

    [Fact]
    public Task RecordAndInterfacesNotSupported()
    {
        const string source = @"using GeNet;

[GenerateEquals]
public interface ITest
{
    public string Prop1 { get; set; }
}

[GenerateEquals]
public record Test(string Prop1);";

        return SnapshotHelper.Verify(source);
    }
}
