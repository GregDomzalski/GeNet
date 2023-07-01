namespace GeNet.Disposable;

internal static class DisposeCodeBuilder
{
    public static void Generate(SourceProductionContext context, GenerationInfo generationInfo)
    {
        SourceBuilder sb = new();

        sb.AppendHeader();
        sb.AppendLine("#nullable enable\n");

        var name = generationInfo.Name;
        var kind = generationInfo.Kind;

        using (sb.Namespace(generationInfo.ContainingNamespace))
        {
            using (sb.Block($"public partial {kind} {name}"))
            {
                EmitIDisposableDispose(sb);
                EmitOverloadedDispose(sb, generationInfo);
                EmitThrowIfDisposed(sb, name);

                if (generationInfo.ExplicitManagedDisposeMethod is null)
                {
                    EmitManagedDispose(sb, generationInfo);
                }

                if (generationInfo.ExplicitUnmanagedDisposeMethod is not null)
                {
                    EmitFinalizer(sb, name);
                }
            }
        }
    }

    private static void EmitIDisposableDispose(SourceBuilder sb)
    {
        using (sb.Block("public void Dispose()"))
        {
            sb.AppendLine("Dispose(true);");
            sb.AppendLine("GC.SuppressFinalize(this);");
        }
    }

    private static void EmitOverloadedDispose(SourceBuilder sb, GenerationInfo generationInfo)
    {
        sb.AppendLine("private bool _disposed;\n");

        using (sb.Block("protected virtual void Dispose(bool disposing)"))
        {
            sb.AppendLine("if (_disposed) { return; }");
            sb.AppendLine("if (disposing) { DisposeManaged(); }");

            if (generationInfo.ExplicitUnmanagedDisposeMethod is null) return;

            sb.AppendLine("try { DisposeUnmanaged(); }");
            sb.AppendLine("catch { /* ignored */ }");
            sb.AppendLine();
            sb.AppendLine("_disposed = true;");
        }
    }

    private static void EmitThrowIfDisposed(SourceBuilder sb, string name)
    {
        using (sb.Block("protected void ThrowIfDisposed()"))
        {
            using (sb.Block("if (_disposed)"))
            {
                sb.AppendLine($"throw new ObjectDisposedException(nameof({name}));");
            }
        }
    }

    private static void EmitManagedDispose(SourceBuilder sb, GenerationInfo generationInfo)
    {
        using (sb.Block("private void DisposeManaged()"))
        {
            var disposeStatements = generationInfo.DisposableMembers
                .Select(m => $"{m.Name}.Dispose();");

            sb.AppendLines(disposeStatements);
        }
    }

    private static void EmitFinalizer(SourceBuilder sb, string name)
    {
        using (sb.Block($"~{name}"))
        {
            sb.AppendLine("Dispose(false);");
        }
    }
}
