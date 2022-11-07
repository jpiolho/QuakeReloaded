﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QuakeReloaded.Utilities;

public class NativeString : IDisposable
{
    private bool _disposedValue;
    private IntPtr _pointer;

    public NativeString(string str)
    {
        _pointer = Marshal.StringToHGlobalAnsi(str);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            Marshal.FreeHGlobal(_pointer);

            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~NativeString()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public IntPtr ToPointer() => _pointer;

    public static implicit operator IntPtr(NativeString nstr) => nstr._pointer;
}
