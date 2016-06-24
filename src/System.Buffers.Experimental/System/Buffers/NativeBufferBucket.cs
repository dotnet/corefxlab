// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    unsafe struct NativeBuffer { 

        public byte* _memory;
        public int _length;
        public NativeBuffer(void* memory, int length)
        {
            _memory = (byte*)memory;
            _length = length;
        }
    }

    internal unsafe sealed class NativeBufferBucket : IDisposable
    {
        private volatile int _index;
        private IntPtr _allocatedMemory;
        private NativeBuffer?[] _buffers;
        private int _elementsInBuffer;
        private SpinLock _lock;

        private bool _disposed;

        internal NativeBufferBucket(int elementsInBuffer, int numberOfBuffers)
        {
            _index = 0;
            _elementsInBuffer = elementsInBuffer;
            _lock = new SpinLock();

            int bufferLength = numberOfBuffers * _elementsInBuffer;
            _allocatedMemory = Marshal.AllocHGlobal(bufferLength * Marshal.SizeOf(typeof(byte)));
            _buffers = new NativeBuffer?[numberOfBuffers];

            for (int i = 0; i < bufferLength; i+= _elementsInBuffer)
            {
                _buffers[i / _elementsInBuffer] = new NativeBuffer((_allocatedMemory + i).ToPointer(), _elementsInBuffer);
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
            Marshal.FreeHGlobal(_allocatedMemory);
            if (disposing)
            {
                _disposed = true; // don't touch in the finalizer
            }
        }

        internal NativeBuffer Rent()
        {
            if (_disposed)
                throw new ObjectDisposedException("NativeBufferBucket");

            NativeBuffer buffer;

            // Use a lightweight spinlock for our super-short lock
            bool taken = false;
            _lock.Enter(ref taken);
            Debug.Assert(taken);

            // Check if all of our buffers have been used
            if (_index >= _buffers.Length)
            {
                // We can safely exit
                _lock.Exit(false);
                buffer = new NativeBuffer(Marshal.AllocHGlobal(_elementsInBuffer * Marshal.SizeOf(typeof(byte))).ToPointer(), _elementsInBuffer);
            }
            else
            {
                buffer = _buffers[_index].Value;
                _buffers[_index] = null;
                _index++;
                _lock.Exit(false);
            }

            return buffer;
        }

        internal void Return(NativeBuffer buffer)
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
                Marshal.FreeHGlobal(new IntPtr(buffer._memory));
            }
            else
            {
                _index--;
                _buffers[_index] = buffer;
            }

            _lock.Exit(false);
        }
    }
}
