namespace GeNet.Equals;

internal static class EqualsCodeBuilder
{
    public static SourceText Generate(SourceProductionContext context, GenerationInfo generationInfo)
    {
        var comparisons = generationInfo.Members.Select(s => $"{s.Name} == other.{s.Name}");
        var hashes = generationInfo.Members.Select(s => $"hashCode.Add({s.Name});");

        SourceBuilder sb = new();

        sb.AppendHeader();
        sb.AppendLine("#nullable enable\n");

        var name = generationInfo.Name;
        var kind = generationInfo.Kind;

        using (sb.Namespace(generationInfo.ContainingNamespace))
        {
            using (sb.Block($"public partial {kind} {name} : IEquatable<{name}>"))
            {
                EmitObjectEquals(sb, name);
                if (generationInfo.ValueType) EmitEquatableEqualsValueType(sb, name, comparisons);
                else EmitEquatableEqualsRefType(sb, name, comparisons);
                EmitGetHashCode(sb, hashes);
                EmitEqualityOperator(sb, name);
                EmitInequalityOperator(sb, name);
            }
        }

        return sb.ToSourceText();
    }

    private static void EmitObjectEquals(SourceBuilder sb, string name)
    {
        using (sb.Block($"public override bool Equals(object? obj)"))
        {
            using (sb.Block($"if (obj is {name} other)"))
            {
                sb.AppendLine($"return {name}.Equals(this, other);");
            }
            sb.AppendLine($"return base.Equals(obj);");
        }
    }

    private static void EmitEquatableEqualsRefType(SourceBuilder sb, string name, IEnumerable<string> comparisons)
    {
        using (sb.Block($"public bool Equals({name}? other)"))
        {
            sb.AppendLine("if (other is null) return false;");
            sb.AppendLines(comparisons, "return", "&&", ";");
        }
    }

    private static void EmitEquatableEqualsValueType(SourceBuilder sb, string name, IEnumerable<string> comparisons)
    {
        using (sb.Block($"public bool Equals({name} other)"))
        {
            sb.AppendLines(comparisons, "return", "&&", ";");
        }
    }

    private static void EmitGetHashCode(SourceBuilder sb, IEnumerable<string> hashes)
    {
        using (sb.Block($"public override int GetHashCode()"))
        {
            sb.AppendLine("HashCode hashCode = new HashCode();");
            sb.AppendLines(hashes);
            sb.AppendLine("return hashCode.GetHashCode();");
        }
    }

    private static void EmitEqualityOperator(SourceBuilder sb, string name)
    {
        using (sb.Block($"public static bool operator ==({name} left, {name} right)"))
        {
            sb.AppendLine("return left.Equals(right);");
        }
    }

    private static void EmitInequalityOperator(SourceBuilder sb, string name)
    {
        using (sb.Block($"public static bool operator !=({name} left, {name} right)"))
        {
            sb.AppendLine("return !(left == right);");
        }
    }
}
