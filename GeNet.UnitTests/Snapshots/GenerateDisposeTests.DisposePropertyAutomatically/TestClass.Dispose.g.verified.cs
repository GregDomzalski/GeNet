﻿//HintName: TestClass.Dispose.g.cs
//-----------------------------------------------------------------------------
/// <auto-generated>
///     This code was generated by https://github.com/GregDomzalski/GeNet
///
///     Changes to this file may cause incorrect behavior and will be lost if the code
///     is regenerated.
/// </auto-generated>
//-----------------------------------------------------------------------------

#nullable enable

public partial class TestClass
{
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) { return; }
        if (disposing) { DisposeManaged(); }
        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(TestClass));
        }

    }

    private void DisposeManaged()
    {
        TestProp1.Dispose();
        TestProp2.Dispose();
    }

}

