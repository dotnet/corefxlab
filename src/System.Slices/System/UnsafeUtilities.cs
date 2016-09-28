// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System
{
    // This class will be removed in post build step, every instance of it will inject IL passed to the constructor as string
    [AttributeUsage(AttributeTargets.Method)]
    class ILSub : Attribute
    {
        public ILSub(string il) { }
    }
}

namespace System.Runtime
{
    /// <summary>
    /// A collection of unsafe helper methods that we cannot implement in C#.
    /// NOTE: these can be used for VeryBadThings(tm), so tread with care...
    /// </summary>
    public sealed class UnsafeUtilities
    {
         [ILSub(@"
            .maxstack 1
            ldarg.0
            ldobj !!T
            ret
         ")]
         public unsafe static T Read<T>(void* source) {
            throw new Exception();
         } // end of method Unsafe::Read

        [ILSub(@"
            .maxstack 2
            ldarg.0
            ldarg.1
            stobj !!T
            ret
        ")]
        public unsafe static void Write<T>(void* destination, T value) { 
        } 

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary> 
        public static unsafe T Reverse<[Primitive]T>(T value) where T : struct
        {
            // note: relying on JIT goodness here!
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte)) {
                return value;
            }
            else if (typeof(T) == typeof(ushort) || typeof(T) == typeof(short)) {
                ushort val = 0;
                UnsafeUtilities.Write(&val, value);
                val = (ushort)((val >> 8) | (val << 8));
                return UnsafeUtilities.Read<T>(&val);
            }
            else if (typeof(T) == typeof(uint) || typeof(T) == typeof(int)
                || typeof(T) == typeof(float)) {
                uint val = 0;
                UnsafeUtilities.Write(&val, value);
                val = (val << 24)
                    | ((val & 0xFF00) << 8)
                    | ((val & 0xFF0000) >> 8)
                    | (val >> 24);
                return UnsafeUtilities.Read<T>(&val);
            }
            else if (typeof(T) == typeof(ulong) || typeof(T) == typeof(long)
                || typeof(T) == typeof(double)) {
                ulong val = 0;
                UnsafeUtilities.Write(&val, value);
                val = (val << 56)
                    | ((val & 0xFF00) << 40)
                    | ((val & 0xFF0000) << 24)
                    | ((val & 0xFF000000) << 8)
                    | ((val & 0xFF00000000) >> 8)
                    | ((val & 0xFF0000000000) >> 24)
                    | ((val & 0xFF000000000000) >> 40)
                    | (val >> 56);
                return UnsafeUtilities.Read<T>(&val);
            }
            else {
                // default implementation
                int len = SizeOf<T>();
                var val = stackalloc byte[len];
                UnsafeUtilities.Write(val, value);
                int to = len >> 1, dest = len - 1;
                for (int i = 0; i < to; i++) {
                    var tmp = val[i];
                    val[i] = val[dest];
                    val[dest--] = tmp;
                }
                return UnsafeUtilities.Read<T>(val);
            }
        }

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
        /// Takes a (possibly null) object reference, plus an offset in bytes,
        /// adds them, and safetly dereferences the target (untyped!) address in
        /// a way that the GC will be okay with.  It yields a value of type T.
        /// </summary>

        /// <summary>
        /// Takes a (possibly null) object reference, plus an offset in bytes, plus an index,
        /// adds them, and safely dereferences the target (untyped!) address in
        /// a way that the GC will be okay with.  It yields a value of type T.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILSub(@"            
            .maxstack 3
            .locals([0] uint8 & addr)
            ldarg.0     // load the object
            stloc.0     // convert the object pointer to a byref
            ldloc.0     // load the object pointer as a byref
            ldarg.1     // load the offset
            add         // add the offset
            ldarg.2     // load the index
            sizeof !!T  // load size of T
            mul         // multiply the index and size of T
            add         // add the result
            ldobj !!T   // load a T value from the computed address
            ret")]
        internal static T Get<T>(object obj, UIntPtr offset, UIntPtr index) { return default(T); }

        /// <summary>
        /// Takes a (possibly null) object reference, plus an offset in bytes, plus an index,
        /// adds them, and safely stores the value of type T in a way that the
        /// GC will be okay with.
        /// </summary>
        [ILSub(@"            
            .maxstack 3
            .locals([0] uint8 & addr)
            ldarg.0     // load the object
            stloc.0     // convert the object pointer to a byref
            ldloc.0     // load the object pointer as a byref
            ldarg.1     // load the offset
            add         // add the offset
            ldarg.2     // load the index
            sizeof !!T  // load size of T
            mul         // multiply the index and size of T
            add         // add the result
            ldarg.3     // load the value to store
            stobj !!T   // store a T value to the computed address
            ret")]
        internal static void Set<T>(object obj, UIntPtr offset, UIntPtr index, T val) { }

        /// <summary>
        /// Computes the number of bytes offset from an array object reference
        /// to its first element, in a way the GC will be okay with.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILSub(@"
            ldarg.0
            ldc.i4 0
            ldelema !!T
            ldarg.0
            sub
            ret")]
        internal static int ElemOffset<T>(T[] arr) { return default(int); }

        /// <summary>
        /// Computes the size of any type T.  This includes managed object types
        /// which C# complains about (because it is architecture dependent).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILSub(@"
            sizeof !!T
            ret")]
        public static int SizeOf<T>() { return default(int); }

        /// <summary>
        /// computes the address of object reference plus an offset in bytes
        /// </summary>
        /// <param name="obj">*must* be pinned (for managed arrays and strings) or can be null (unmanaged arrays)</param>
        /// <param name="offset">offset to add (can be offset to managed array element or pointer to unmanaged array)</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILSub(@"
            .maxstack 2
            .locals ([0] uint8& addr)
            ldarg.0     // load the object
            conv.u      // since the arg must be pinned, there is no need to deal with byref local (as in Get method)
            ldarg.1     // load the offset
            add         // add the offset
            ret")]
        internal static UIntPtr ComputeAddress(object obj, UIntPtr offset) { return UIntPtr.Zero; }
               
        [ILSub(@"
            .maxstack 2
            ldarg.0
            conv.i
            sizeof !!T
            mul  
            sizeof !!U
            div
            ret")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr CountOfU<T, U>(uint countOfT) { return default(IntPtr); }

        [ILSub(@"
            .maxstack 3
            .locals([0] uint8 & destAddr, 
                    [1] uint8 & scrAddr)
            ldarg.2     // load destObj
            stloc.0     // convert the object pointer to a byref
            ldloc.0     // load the object pointer as a byref
            ldarg.3     // load destOffset
            add         // add destOffset
            ldarg.0     // load srcObj
            stloc.1     // convert the object pointer to a byref
            ldloc.1     // load the object pointer as a byref
            ldarg.1     // load srcOffset
            add         // add srcOffset
            ldarg.s 4   // load byteCount
            cpblk
            ret")]
        internal static void CopyBlock(object srcObj, UIntPtr srcOffset, object destObj, UIntPtr destOffset, int byteCount)
        {}

        /// <summary>
        /// Computes the address of an element relative to a pointer -
        /// essentially returning simply: ((T*)root) + offset;
        /// 
        /// This cannot be expressed in C#, and the naive approach
        /// leads to int32 overflow problems; the "fix" to the naive
        /// approach (ulong) leads to suboptimal x86 performance.
        /// </summary>
        [ILSub(@"
        .maxstack 8
        ldarg.0 // load the base pointer
        ldarg.1 // load the offset
        conv.i  // convert the offset to a native integer
        sizeof !!T  // load size of T
        mul // multiply size by offset
        add // add product to base pointer
        ret // return")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void* GetElementAddress<T>(void* root, uint offset)
        {
            // slow version used for illustration only
            return ((byte*)root) + ((ulong)offset * (ulong)SizeOf<T>());
        }
        /// <summary>
        /// Computes the address of an element relative to a pointer -
        /// essentially returning simply: ((T*)root) + offset;
        /// 
        /// This cannot be expressed in C#, and the naive approach
        /// leads to int32 overflow problems; the "fix" to the naive
        /// approach (ulong) leads to suboptimal x86 performance.
        /// </summary>
        [ILSub(@"
        .maxstack 8
        ldarg.0 // load the base pointer
        ldarg.1 // load the offset
        sizeof !!T  // load size of T
        mul // multiply size by offset
        add // add product to base pointer
        ret // return")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe UIntPtr GetElementAddress<T>(UIntPtr root, UIntPtr offset)
        {
            // bad impl for compat only
            return (UIntPtr)GetElementAddress<T>((void*)root, (uint)offset);
        }
    }
}
