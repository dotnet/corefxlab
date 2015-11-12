// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    internal unsafe sealed class NativeBufferBucket<T> : IDisposable where T : struct
    {
        private volatile int _index;
        private IntPtr _buffer;
        private Span<T>?[] _slices;
        private int _elementsInBuffer;
        private SpinLock _lock;

        private bool _disposed;

        internal NativeBufferBucket(int elementsInBuffer, int numberOfBuffers)
        {
            _index = 0;
            _elementsInBuffer = elementsInBuffer;
            _lock = new SpinLock();

            int bufferLength = numberOfBuffers * _elementsInBuffer;
            _buffer = Marshal.AllocHGlobal(bufferLength * Marshal.SizeOf(typeof(T)));
            _slices = new Span<T>?[numberOfBuffers];

            for (int i = 0; i < bufferLength; i+= _elementsInBuffer)
            {
                _slices[i / _elementsInBuffer] = new Span<T>((_buffer + i).ToPointer(), _elementsInBuffer);
            }
        }

        ~NativeBufferBucket()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            Marshal.FreeHGlobal(_buffer);
            if (disposing)
            {
                _disposed = true; // don't touch in the finalizer
            }
        }

        internal Span<T> Rent()
        {
            if (_disposed)
                throw new ObjectDisposedException("NativeBufferBucket");

            Span<T> buffer;

            // Use a lightweight spinlock for our super-short lock
            bool taken = false;
            _lock.Enter(ref taken);
            Debug.Assert(taken);

            // Check if all of our buffers have been used
            if (_index >= _slices.Length)
            {
                // We can safely exit
                _lock.Exit(false);
                buffer = new Span<T>(Marshal.AllocHGlobal(_elementsInBuffer * Marshal.SizeOf(typeof(T))).ToPointer(), _elementsInBuffer);
            }
            else
            {
                buffer = _slices[_index].Value;
                _slices[_index] = null;
                _index++;
                _lock.Exit(false);
            }

            return buffer;
        }

        internal void Return(ref Span<T> buffer)
        {
            if (_disposed)
                throw new ObjectDisposedException("NativeBufferBucket");

            // Use a lightweight spinlock for our super short lock
            bool taken = false;
            _lock.Enter(ref taken);
            Debug.Assert(taken);

            // If we have space to put the buffer back, then do so; otherwise,
            // deallocate the buffer since we must have alloc'd one on-demand
            if (_index <= 0)
            {
                Marshal.FreeHGlobal(new IntPtr(buffer.UnsafePointer));
                buffer = default(Span<T>);
            }
            else
            {
                _index--;
                _slices[_index] = buffer;
            }

            _lock.Exit(false);
        }
    }
}
