namespace GeNet.UnitTests;

[UsesVerify]
public class GenerateDisposeTests
{
    [Fact]
    public Task DisposePropertyAutomatically()
    {
        const string source = @"using GeNet;
using System;
using System.Runtime.InteropServices;

public class TestClass : IDisposable
{
    public IDisposable TestProp1 { get; }
    public SafeHandle TestProp2 { get; }
}";
        return SnapshotHelper.VerifyDisposableGenerator(source);
    }

    [Fact]
    public Task DisposeFieldAutomatically()
    {
        const string source = @"using GeNet;
using System;
using System.Runtime.InteropServices;

public class TestClass : IDisposable
{
    private IDisposable _testField1;
    public SafeHandle _testField2;
}";
        return SnapshotHelper.VerifyDisposableGenerator(source);
    }

    [Fact]
    public Task UseCustomManagedDisposeFunction()
    {
        const string source = @"using GeNet;
using System;

public class TestClass : IDisposable
{
    private IDisposable _testField1;

    private void DisposeManaged()
    {
        _testField1.Dispose(); // I am disposing this myself.
    }
}";
        return SnapshotHelper.VerifyDisposableGenerator(source);
    }

    [Fact]
    public Task UseCustomDisposeUnmanagedFunction()
    {
        const string source = @"using GeNet;
using System;

public class TestClass : IDisposable
{
    private IntPtr _foo;

    private void DisposeUnmanaged()
    {
        Free(_foo);
    }
}";
        return SnapshotHelper.VerifyDisposableGenerator(source);
    }

    [Fact]
    public Task CallBaseDisposeMethod()
    {
        const string source = @"using GeNet;
using System;

public class Base : IDisposable
{
    IDisposable _baseField;
}

public class Derived : Base
{

}";
        return SnapshotHelper.VerifyDisposableGenerator(source);
    }
}
