// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Buffers
{
    public abstract class OwnedBuffer<T> : IDisposable, IKnown
    {
        bool _disposed;
        int _referenceCount;

        public abstract int Length { get; }

        public bool HasOutstandingReferences { 
            get { 
                return _referenceCount > 0 
                        || (ReferenceCountingSettings.OwnedMemory == ReferenceCountingMethod.ReferenceCounter
                            && ReferenceCounter.HasReference(this)); 
            } 
        }

        protected OwnedBuffer() { }
        
        public Buffer<T> Buffer => new Buffer<T>(this, Length);
        public ReadOnlyBuffer<T> ReadOnlyBuffer => new ReadOnlyBuffer<T>(this, Length);

        public abstract Span<T> Span { get; }
        public abstract Span<T> GetSpan(int index, int length);

        public static implicit operator OwnedBuffer<T>(T[] array) => new OwnedArray<T>(array);

        public void Dispose()
        {
            if (HasOutstandingReferences) throw new InvalidOperationException("outstanding references detected.");
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            _disposed = true;
        }

        public bool IsDisposed => _disposed;

        public void AddReference()
        {
            Interlocked.Increment(ref _referenceCount);
        }

        public void Release()
        {
            if(Interlocked.Decrement(ref _referenceCount) == 0)
                OnZeroReferences();
        }

        protected virtual void OnZeroReferences()
        { }

    #region Used by Memory<T>

        internal protected abstract unsafe bool TryGetPointerInternal(out void* pointer);

        internal protected abstract bool TryGetArrayInternal(out ArraySegment<T> buffer);

        // TODO: this is taken from SpanHelpers. If we move this type to System.Memory, we should remove this helper
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IntPtr Add(IntPtr start, int index)
        {
            Debug.Assert(start.ToInt64() >= 0);
            Debug.Assert(index >= 0);

            unsafe
            {
                if (sizeof(IntPtr) == sizeof(int)) {
                    // 32-bit path.
                    uint byteLength = (uint)index * (uint)Unsafe.SizeOf<T>();
                    return (IntPtr)(((byte*)start) + byteLength);
                }
                else {
                    // 64-bit path.
                    ulong byteLength = (ulong)index * (ulong)Unsafe.SizeOf<T>();
                    return (IntPtr)(((byte*)start) + byteLength);
                }
            }
        }

        protected void ThrowObjectDisposed()
        {
            throw new ObjectDisposedException(nameof(Buffer<T>));
        }
        protected void ThrowIndexOutOfRange()
        {
            throw new IndexOutOfRangeException();
        }
        #endregion
    }

    interface IKnown
    {
        void AddReference();
        void Release();
    }

    public struct DisposableReservation<T> : IDisposable
    {
        OwnedBuffer<T> _owner;

        internal DisposableReservation(OwnedBuffer<T> owner)
        {
            _owner = owner;
            switch(ReferenceCountingSettings.OwnedMemory) {
                case ReferenceCountingMethod.Interlocked:
                    ((IKnown)_owner).AddReference();
                    break;
                case ReferenceCountingMethod.ReferenceCounter:
                    ReferenceCounter.AddReference(_owner);
                    break;
                case ReferenceCountingMethod.None:
                    break;
            }
        }

        public Span<T> Span => _owner.Span;

        public void Dispose()
        {
            switch (ReferenceCountingSettings.OwnedMemory) {
                case ReferenceCountingMethod.Interlocked:
                    ((IKnown)_owner).Release();
                    break;
                case ReferenceCountingMethod.ReferenceCounter:
                    ReferenceCounter.Release(_owner);
                    break;
                case ReferenceCountingMethod.None:
                    break;
            }
            _owner = null;
        }
    }
}

namespace System.Runtime
{
    public enum ReferenceCountingMethod {
        Interlocked,
        ReferenceCounter,
        None
    };

    public class ReferenceCountingSettings {
        public static ReferenceCountingMethod OwnedMemory = ReferenceCountingMethod.Interlocked;
    }
}