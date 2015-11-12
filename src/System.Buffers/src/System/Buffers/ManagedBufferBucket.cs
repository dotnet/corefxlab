// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading;

namespace System.Buffers
{
    internal sealed class ManagedBufferBucket<T> where T : struct
    {
        private volatile int _index;
        private T[][] _data;
        private int _bufferSize;
        private SpinLock _lock;

        internal ManagedBufferBucket(int bufferSize, int numberOfBuffers)
        {
            _index = 0;
            _bufferSize = bufferSize;
            _lock = new SpinLock();
            _data = new T[numberOfBuffers][];
            for (int i = 0; i < numberOfBuffers; i++)
            {
                _data[i] = new T[bufferSize];
            }
        }

        internal T[] Rent()
        {
            T[] buffer;

            // Use a SpinLock since it is super lightweight
            // and our lock is very short lived
            bool taken = false;
            _lock.Enter(ref taken);
            Debug.Assert(taken);

            // Be quick and do an unsynchronized read to see if all
            // of our buffers have been used
            if (_index >= _data.Length)
            {
                // Exit the lock as soon as possible
                _lock.Exit(false);
                buffer = new T[_bufferSize];
            }
            else
            {
                buffer = _data[_index];
                _data[_index] = null;
                _index++;

                // Exit the lock as soon as possible
                _lock.Exit(false);
            }

            return buffer;
        }

        internal void Return(ref T[] buffer)
        {
            // Use a SpinLock since it is super lightweight
            // and our lock is very short lived
            bool taken = false;
            _lock.Enter(ref taken);
            Debug.Assert(taken);

            // If we have space to put the buffer back, do it. If we don't
            // then there was a buffer alloc'd that was returned instead so
            // we can just drop this buffer
            if (_index == 0)
                buffer = null;
            else
            {
                _index--;
                _data[_index] = buffer;
            }

            _lock.Exit(false);
        }
    }
}
