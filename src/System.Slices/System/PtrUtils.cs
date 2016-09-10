// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// A collection of unsafe helper methods that we cannot implement in C#.
    /// NOTE: these can be used for VeryBadThings(tm), so tread with care...
    /// </summary>
    sealed class PtrUtils
    {
        // WARNING:
        // The Get and Set methods below do some tricky things.  They accept
        // a managed 'object' and 'native uint' offset, and sometimes manufacture
        // pointers straight into the middle of objects.  To ensure the GC can
        // follow along, it performs these computations in "byref" space.  The
        // other weird thing is that sometimes these computations don't involve
        // manage objects at all!  If the object is null, and the offset is actually
        // just a raw native pointer, these functions still do the "right" thing.
        // That is, the computations, dereferencing, and subsequent coercion into
        // a T value "just work."  This would be a dirty little undocumented trick
        // that made me need to take a shower, were it not for the fact that C++/CLI
        // depends on it working... (okay, I still feel a little dirty.)

        /// <summary>
        /// Takes a (possibly null) object reference, plus an offset in bytes, plus an index,
        /// adds them, and safely dereferences the target (untyped!) address in
        /// a way that the GC will be okay with.  It yields a value of type T.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T Get<T>(object obj, UIntPtr offset, UIntPtr index)
        {
            return Unsafe.Read<T>(*(byte**)Unsafe.AsPointer(ref obj) + offset.ToUInt64() + index.ToUInt32() * Unsafe.SizeOf<T>());
        }

        /// <summary>
        /// Takes a (possibly null) object reference, plus an offset in bytes, plus an index,
        /// adds them, and safely stores the value of type T in a way that the
        /// GC will be okay with.
        /// </summary>
        public static unsafe void Set<T>(object obj, UIntPtr offset, UIntPtr index, T val)
        {
            Unsafe.Write(*(byte**)Unsafe.AsPointer(ref obj) + offset.ToUInt64() + index.ToUInt32() * Unsafe.SizeOf<T>(), val);
        }

        public static unsafe void CopyBlock(object srcObj, UIntPtr srcOffset, object destObj, UIntPtr destOffset, int byteCount)
        {
            Unsafe.CopyBlock(*(byte**)Unsafe.AsPointer(ref destObj) + (ulong)destOffset, *(byte**)Unsafe.AsPointer(ref srcObj) + (ulong)srcOffset, (uint)byteCount);
        }

        /// <summary>
        /// Computes the number of bytes offset from an array object reference
        /// to its first element, in a way the GC will be okay with.
        /// </summary>
        public static unsafe int ElemOffset<T>(T[] arr)
        {
            var handle = GCHandle.Alloc(arr, GCHandleType.Pinned);
            try
            {
                return (int)((byte*)Unsafe.AsPointer(ref arr[0]) - *(byte**)Unsafe.AsPointer(ref arr));
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
