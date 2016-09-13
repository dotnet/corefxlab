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
            if (obj != null)
            {
                return GetTyped<T>(obj, (uint)offset + (uint)index);
            }
            else
            {
                return Unsafe.Read<T>((byte*)offset.ToPointer() + index.ToUInt32() * Unsafe.SizeOf<T>());
            }
        }

        /// <summary>
        /// Takes a (possibly null) object reference, plus an offset in bytes, plus an index,
        /// adds them, and safely stores the value of type T in a way that the
        /// GC will be okay with.
        /// </summary>
        public static unsafe void Set<T>(object obj, UIntPtr offset, UIntPtr index, T val)
        {
            if (obj != null)
            {
                SetTyped(obj, (uint)offset + (uint)index, val);
            }
            else
            {
                Unsafe.Write((byte*)offset.ToPointer() + index.ToUInt32() * Unsafe.SizeOf<T>(), val);
            }
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

        private static unsafe T GetTyped<T>(object obj, uint index)
        {
            T val;
            if (typeof(T) == typeof(byte))
            {
                var arr = Unsafe.As<byte[]>(obj);
                fixed (byte* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(char))
            {
                var arr = obj as char[];
                if (arr != null)
                {
                    fixed (char* ptr = &arr[0])
                    {
                        val = Unsafe.Read<T>(ptr + index);
                    }
                }
                else
                {
                    var s = (string)obj;
                    fixed (char* ptr = s)
                    {
                        val = Unsafe.Read<T>(ptr + index);
                    }
                }
            }
            else if (typeof(T) == typeof(sbyte))
            {
                var arr = Unsafe.As<sbyte[]>(obj);
                fixed (sbyte* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(short))
            {
                var arr = Unsafe.As<short[]>(obj);
                fixed (short* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(ushort))
            {
                var arr = Unsafe.As<ushort[]>(obj);
                fixed (ushort* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(int))
            {
                var arr = Unsafe.As<int[]>(obj);
                fixed (int* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(uint))
            {
                var arr = Unsafe.As<uint[]>(obj);
                fixed (uint* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(long))
            {
                var arr = Unsafe.As<long[]>(obj);
                fixed (long* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(ulong))
            {
                var arr = Unsafe.As<ulong[]>(obj);
                fixed (ulong* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(IntPtr))
            {
                var arr = Unsafe.As<IntPtr[]>(obj);
                fixed (IntPtr* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(UIntPtr))
            {
                var arr = Unsafe.As<UIntPtr[]>(obj);
                fixed (UIntPtr* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(float))
            {
                var arr = Unsafe.As<float[]>(obj);
                fixed (float* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(double))
            {
                var arr = Unsafe.As<double[]>(obj);
                fixed (double* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                var arr = Unsafe.As<bool[]>(obj);
                fixed (bool* ptr = &arr[0])
                {
                    val = Unsafe.Read<T>(ptr + index);
                }
            }
            else
            {
                var arr = obj as T[];
                val = arr[index];
            }

            return val;
        }

        private static unsafe void SetTyped<T>(object obj, uint index, T val)
        {
            if (typeof(T) == typeof(byte))
            {
                var arr = Unsafe.As<byte[]>(obj);
                fixed (byte* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(char))
            {
                var arr = obj as char[];
                if (arr != null)
                {
                    fixed (char* ptr = &arr[0])
                    {
                        Unsafe.Write(ptr + index, val);
                    }
                }
                else
                {
                    var s = obj as string;
                    fixed (char* ptr = s)
                    {
                        Unsafe.Write(ptr + index, val);
                    }
                }
            }
            else if (typeof(T) == typeof(sbyte))
            {
                var arr = Unsafe.As<sbyte[]>(obj);
                fixed (sbyte* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(short))
            {
                var arr = Unsafe.As<short[]>(obj);
                fixed (short* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(ushort))
            {
                var arr = Unsafe.As<ushort[]>(obj);
                fixed (ushort* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(int))
            {
                var arr = Unsafe.As<int[]>(obj);
                fixed (int* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(uint))
            {
                var arr = Unsafe.As<uint[]>(obj);
                fixed (uint* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(long))
            {
                var arr = Unsafe.As<long[]>(obj);
                fixed (long* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(ulong))
            {
                var arr = Unsafe.As<ulong[]>(obj);
                fixed (ulong* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(IntPtr))
            {
                var arr = Unsafe.As<IntPtr[]>(obj);
                fixed (IntPtr* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(UIntPtr))
            {
                var arr = Unsafe.As<UIntPtr[]>(obj);
                fixed (UIntPtr* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(float))
            {
                var arr = Unsafe.As<float[]>(obj);
                fixed (float* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(double))
            {
                var arr = Unsafe.As<double[]>(obj);
                fixed (double* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                var arr = Unsafe.As<bool[]>(obj);
                fixed (bool* ptr = &arr[0])
                {
                    Unsafe.Write(ptr + index, val);
                }
            }
            else
            {
                var arr = obj as T[];
                arr[index] = val;
            }
        }
    }
}
