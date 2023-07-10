# GeNet.Equals

Automatically generates `Object.Equals`, `IEquality<T>.Equals`, `GetHashCode`, `operator ==`, and `operator !=` for
your struct and class types.

## Getting started

Add a `PackageReference` to the latest version of `GeNet.Equals` in your project's csproj file like so:

```csharp
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <!-- Your project properties -->
    </PropertyGroup>
    
    <ItemGroup>
        <!-- Other PackageReference entries may exist here already. Add ours after. -->
        <PackageReference Include="GeNet.Equals" Version="1.0.0" />
    </ItemGroup>
</Project>
```

The version attribute should be set to the latest version released on https://nuget.org/

Alternatively, you can use the `dotnet` CLI to add a reference:

```shell
dotnet add ./path/to/your.csproj package GeNet.Equals
```

## Using this generator

The source generator will look for types declared in your source that are adorned with the `[GenerateEquals]` attribute.
In order to use this generator, you must add a package reference to it in your project as described above. You must then
use the namespace `GeNet` and mark the target class as `partial`. Lastly, once you adorn the type with `[GenerateEquals]`,
the source generator will take over and will implement `object.Equals`, `IEquality<T>.Equals`, `GetHashCode`,
`operator ==`, and `operator !=`.

The default implementation examines each of the field and property members of the type. Keep in mind that custom types
used as members may also need to have their equality members defined. If there is a field or property that you wish to
be ignored in the generator's implementation, adorn that member with the `[IgnoredByEquals]` attribute.

For example:

```csharp
using GeNet;

[GenerateEquals]
public partial struct MyStruct
{
    private int _field1; // Included
    private int _field2; // Included
    public string Property1 { get; set; } // Included
    
    [IgnoredByEquals]
    public float Property2 { get; set; } // Excluded
}
```

# About GeNet

GeNet is a suite of .NET / Roslyn source generators. Its aim is to reduce the need for alternative methods of generating
code such as text templating, reflection, and IL weaving.
