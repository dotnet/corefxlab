// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace System.Runtime {

    public class DelayedDisposer {

        [ThreadStatic]
        static DelayedDisposeBuffer t_toBeDisposed;

        static public void Add (IDisposable d) {
            t_toBeDisposed.Enqueue (d);
        }

        static public void Flush () {
            t_toBeDisposed.EnsureInitialized ();
            t_toBeDisposed.Drain ();
        }

        struct DelayedDisposeBuffer {
            // This buffer fills from the top (_buffer.Length-1). 
            // _freespace represents the index of the highest used element. 
            // When the buffer is full, _freespace == 0.
            // When the buffer is uninitialized, _freespace == 0. i.e.  _buffer == null => _freespace == 0 
            // These means that the initialialization of thread static can be 
            // put on slow path of when the buffer is full, so we only require a single test on the fast path.  
            int _freeSpace;
            IDisposable[] _buffer;

            private DelayedDisposeBuffer (int size) {
                this._buffer = new IDisposable[size];
                this._freeSpace = _buffer.Length;
            }

            bool IsFullOrUninitialized () {
                return _freeSpace == 0;
            }

            public bool EnsureInitialized () {
                if (_buffer == null) {
                    this = new DelayedDisposeBuffer (32);
                    return false;
                }
                return true;
            }

            private void EnsureLargeEnough () {
                if (_freeSpace < _buffer.Length * 0.75) {
                    var newSize = (int) (_buffer.Length / 0.75) + 4;
                    var newDisposableBuffer = new DelayedDisposeBuffer (newSize);
                    for (int i = _buffer.Length - 1; i >= _freeSpace; i--) {
                        newDisposableBuffer.Enqueue (_buffer[i]);
                    }
                    this = newDisposableBuffer;
                }
            }

            public BloomFilter Drain () {
                var referenced = ReferenceCounter.GetMaybeReferenced ();
                int newFreeSpace = _buffer.Length;
                for (int i = _buffer.Length - 1; i >= _freeSpace; i--) {
                    if (referenced.DoesNotContain (_buffer[i])) {
                        _buffer[i].Dispose ();
                        _buffer[i] = null;
                    } else {
                        _buffer[--newFreeSpace] = _buffer[i];
                    }
                }
                _freeSpace = newFreeSpace;

                EnsureLargeEnough ();

                return referenced;
            }

            public void Enqueue (IDisposable t) {
                if (IsFullOrUninitialized () && EnsureInitialized()) {
                    // Was actually full
                    BloomFilter referenced = Drain ();
                    if (referenced.DoesNotContain (t)) {
                        t.Dispose ();
                        return;
                    }
                }
                _freeSpace--;
                _buffer[_freeSpace] = t;
            }
        }
    }
}