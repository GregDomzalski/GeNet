// See https://aka.ms/new-console-template for more information

using System.Buffers;
using System.Buffers.Binary;
using System.Text;
using GeNet.Disposable;
using GeNet.Equals;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DebugProject;

public static class Program
{
    public static void Main()
    {
        var source = @"using System;
namespace GeNet.UnitTests;

public partial class Test : IDisposable
{
    public IDisposable TestField {get;set;} = null!;
}";

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

        IEnumerable<PortableExecutableReference> references = new[]
        {
            MetadataReference.CreateFromFile(typeof(IDisposable).Assembly.Location),
        };

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[] { syntaxTree },
            references: references);

        GenerateDisposeGenerator generator = new();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGenerators(compilation);

        var results = driver.GetRunResult();
    }
}

public interface IBinarySerializable
{
    public void Serialize(Stream stream);
    public byte[] Serialize();
    public int Deserialize(Stream stream);
    public int Deserialize(ReadOnlySpan<byte> span);
}

public class Base : IBinarySerializable
{
    /// <inheritdoc />
    public void Serialize(Stream stream)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public byte[] Serialize()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public int Deserialize(Stream stream)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public int Deserialize(ReadOnlySpan<byte> span)
    {
        throw new NotImplementedException();
    }
}

// [GenerateSerializer]
public class TestClass : IBinarySerializable
{
    // TODO: Null terminated strings, byte count string length, separate field for length
    private string _field1;
    // TODO: Big/Little endian
    private int _field2;
    private int _field3;
    private Base _field4;
    // TODO: Static size array, dynamic size w/ length field
    private IList<int> _field5 = new List<int>(5);

    #region Generated code

    public string Field1 { get => _field1; set => _field1 = value; }
    public int Field2 { get => _field2; set => _field2 = value; }
    public int Field3 { get => _field3; set => _field3 = value; }
    public Base Field4 { get => _field4; set => _field4 = value; }
    public IList<int> Field5 => _field5; // No set when IEnumerable and has value set

    public TestClass(Stream stream)
    {
        using var sr = new StreamReader(stream);

    }

    public TestClass(ReadOnlySpan<byte> serialized)
    {
        _field1 = Encoding.UTF8.GetString(serialized);
        var length = Encoding.UTF8.GetByteCount(_field1);

        _field2 = BinaryPrimitives.ReadInt32LittleEndian(serialized.Slice(length, 4));
        _field3 = BinaryPrimitives.ReadInt32LittleEndian(serialized.Slice(length, 4));
    }

    public void Serialize(Stream stream)
    {
        using var sw = new StreamWriter(stream);

        sw.Write(_field1);
        sw.Write(_field2);
        sw.Write(_field3);
        sw.Write(_field4.Serialize());
    }

    public byte[] Serialize()
    {
        using var memoryStream = new MemoryStream();
        Serialize(memoryStream);
        return memoryStream.ToArray();
    }

    public int Deserialize(Stream stream) => throw new NotImplementedException();
    public int Deserialize(ReadOnlySpan<byte> span) => throw new NotImplementedException();

    #endregion
}
