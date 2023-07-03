using GeNet.Equals;
using GeNet.Disposable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GeNet.UnitTests;

public static class SnapshotHelper
{
    public static Task Verify(string source, IIncrementalGenerator generator, IEnumerable<PortableExecutableReference>? references = null)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[] { syntaxTree },
            references: references);

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGenerators(compilation);

        return Verifier.Verify(driver)
            .UseDirectory("Snapshots")
            .UseUniqueDirectory();
    }

    public static Task VerifyEqualsGenerator(string source) =>
        Verify(source, new GenerateEqualsGenerator());

    public static Task VerifyDisposableGenerator(string source) =>
        Verify(source, new GenerateDisposeGenerator(), new []
        {
            MetadataReference.CreateFromFile(typeof(IDisposable).Assembly.Location)
        });
}
