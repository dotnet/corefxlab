// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information./

.assembly extern mscorlib
{
  .publickeytoken = (B7 7A 5C 56 19 34 E0 89 )                         // .z\V.4..
  .ver 4:0:0:0
}

.module System.Slices.netmodule
.custom instance void [mscorlib]System.CLSCompliantAttribute::.ctor(bool) = ( 01 00 01 00 00 )

.namespace System
{
    /// <summary>
    /// A collection of unsafe helper methods that we cannot implement in C#.
    /// NOTE: these can be used for VeryBadThings(tm), so tread with care...
    /// </summary>
    .class private auto ansi sealed beforefieldinit PtrUtils
        extends [mscorlib]System.Object
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
        .method public hidebysig static !!T Get<T>(
            object obj, native uint offset) cil managed aggressiveinlining
        {
            .maxstack 2
            .locals ([0] uint8& addr)
            ldarg.0     // load the object
            stloc.0     // convert the object pointer to a byref
            ldloc.0     // load the object pointer as a byref
            ldarg.1     // load the offset
            add         // add the offset
            ldobj !!T   // load a T value from the computed address
            ret
        }

        /// <summary>
        /// Takes a (possibly null) object reference, plus an offset in bytes,
        /// adds them, and safely stores the value of type T in a way that the
        /// GC will be okay with.
        /// </summary>
        .method public hidebysig static void Set<T>(
            object obj, native uint offset, !!T val) cil managed aggressiveinlining
        {
            .maxstack 2
            .locals ([0] uint8& addr)
            ldarg.0     // load the object
            stloc.0     // convert the object pointer to a byref
            ldloc.0     // load the object pointer as a byref
            ldarg.1     // load the offset
            add         // add the offset
            ldarg.2     // load the value to store
            stobj !!T   // store a T value to the computed address
            ret
        }

        /// <summary>
        /// Computes the number of bytes offset from an array object reference
        /// to its first element, in a way the GC will be okay with.
        /// </summary>
        .method public hidebysig static int32 ElemOffset<T>(!!T[] arr) cil managed
        {
            ldarg.0
            ldc.i4 0
            ldelema !!T
            ldarg.0
            sub
            ret
        }

        /// <summary>
        /// Computes the size of any type T.  This includes managed object types
        /// which C# complains about (because it is architecture dependent).
        /// </summary>
        .method public hidebysig static int32 SizeOf<T>() cil managed aggressiveinlining
        {
            sizeof !!T
            ret
        }
    }
}

