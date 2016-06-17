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
        public static T Get<T>(object obj, UIntPtr offset, UIntPtr index) { return default(T); }

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
        public static void Set<T>(object obj, UIntPtr offset, UIntPtr index, T val) { }

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
        public static int ElemOffset<T>(T[] arr) { return default(int); }

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
        public static UIntPtr ComputeAddress(object obj, UIntPtr offset) { return UIntPtr.Zero; }
               
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
        public static IntPtr CountOfU<T, U>(uint countOfT) { return default(IntPtr); }

        [ILSub(@"
            .maxstack 3
            ldarg.1
            ldarg.0
            ldarg.2
            cpblk
            ret")]
        public static void Copy(UIntPtr source, UIntPtr destination, int byteCount)
        {}
    }
}
