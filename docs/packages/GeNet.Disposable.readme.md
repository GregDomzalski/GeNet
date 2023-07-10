# GeNet.Disposable

Automatically generate `Dispose` implementations for types that implement `IDisposable`. Allows for easy customization
of resource cleanup that follow best practices.

## Getting started

Add a `PackageReference` to the latest version of `GeNet.Disposable` in your project's csproj file like so:

```csharp
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <!-- Your project properties -->
    </PropertyGroup>
    
    <ItemGroup>
        <!-- Other PackageReference entries may exist here already. Add ours after. -->
        <PackageReference Include="GeNet.Disposable" Version="1.0.0" />
    </ItemGroup>
</Project>
```

The version attribute should be set to the latest version released on https://nuget.org/

Alternatively, you can use the `dotnet` CLI to add a reference:

```shell
dotnet add ./path/to/your.csproj package GeNet.Disposable
```

## Using this generator

The source generator will look for types declared in your source that inherit (either directly or indirectly) from
`IDisposable`. In order to use this generator, you must add a package reference to it in your project as described above.
You must then use the namespace `GeNet` and mark the target class as `partial`. Any class, struct, or record that inherits
from `IDisposable` either directly or indirectly will receive an automatic implementation.

The automatic implementation of `Dispose` will call the `Dispose` method found on any member field or property which
also implements `IDisposable`. If the type is extending a base which also defines `Dispose`, the implementation will
also call the base's `Dispose` method.

You may extend the behavior of `Dispose` by implementing either of two methods:

1. `private void DisposeManaged()` - Called to clean up any managed resources. This will supersede the source generator's
   implementation. You will be responsible for disposing all of this type's disposable members.
2. `private void DisposeUnmanaged()` - Called to clean up any unmanaged or native resources. For example, if you are
   holding an `IntPtr` that represents memory on the native heap, this would be the place to free that memory.

For example:

```csharp
using GeNet;

public partial class Base : IDisposable
{
    public IDisposable DisposableMember { get; set; } // Disposed automatically
    public IntPtr UnmanagedMemoryPtr { get; set; } // Not handled automatically, see DisposeUnmanaged
    
    private void DisposeUnmanaged()
    {
        Marshal.FreeHGlobal(UnmanagedMemoryPtr);
        UnmanagedMemoryPtr = IntPtr.Zero;
    }
}

// Note that this class doesn't directly implement IDisposable. It is picked up from the base class.
// Additionally, this method's `Dispose` method will automatically call the `Dispose` method in `Base`.
public partial class Derived : Base
{
    public SpecialType SpecialDisposable { get; set; } // Could be disposed automatically, but let's do it manually
    
    private void DisposeManaged()
    {
        // Suppose there's a method that you want to call prior to disposing this member.
        // Defining `DisposeManaged` will allow you to override the default behavior of simply calling `Dispose`
        SpecialDispose.Flush();
        SpecialDisposable.Dispose();
    }
}
```

# About GeNet

GeNet is a suite of .NET / Roslyn source generators. Its aim is to reduce the need for alternative methods of generating
code such as text templating, reflection, and IL weaving.
