namespace DebugProject;

public class DisposeExamples : IDisposable
{
    public StreamReader StreamReader { get; set; }
    public StreamWriter StreamWriter { get; set; }

    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            DisposeManaged();
        }

        try
        {
            DisposeUnmanaged();
        }
#pragma warning disable CA1031
        catch
#pragma warning restore CA1031
        {
            // ignored
        }

        _disposed = true;
    }

    // Only if DisposeUnmanaged is not null. And only for class / record class
    ~DisposeExamples()
    {
        Dispose(false);
    }

    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(DisposeManaged));
        }
    }

    protected virtual void DisposeUnmanaged() { }
    protected virtual void DisposeManaged() { }
}
