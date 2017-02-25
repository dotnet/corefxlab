// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// A collection of convenient span helpers, exposed as extension methods.
    /// </summary>
    public static partial class SpanExtensionsLabs
    {
        /// <summary>
        /// Casts a Slice of one primitive type (T) to another primitive type (U).
        /// These types may not contain managed objects, in order to preserve type
        /// safety.  This is checked statically by a Roslyn analyzer.
        /// </summary>
        /// <param name="slice">The source slice, of type T.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<U> Cast<[Primitive]T, [Primitive]U>(this Span<T> slice)
            where T : struct
            where U : struct
        {
            return slice.NonPortableCast<T, U>();
        }

        /// <summary>
        /// Casts a Slice of one primitive type (T) to another primitive type (U).
        /// These types may not contain managed objects, in order to preserve type
        /// safety.  This is checked statically by a Roslyn analyzer.
        /// </summary>
        /// <param name="slice">The source slice, of type T.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<U> Cast<[Primitive]T, [Primitive]U>(this ReadOnlySpan<T> slice)
            where T : struct
            where U : struct
        {
            return slice.NonPortableCast<T, U>();
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<[Primitive]T>(this Span<byte> slice)
            where T : struct
        {
            Contract.RequiresInInclusiveRange(Unsafe.SizeOf<T>(), (uint)slice.Length);
            return Unsafe.As<byte, T>(ref slice.DangerousGetPinnableReference());
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<[Primitive]T>(this ReadOnlySpan<byte> slice)
            where T : struct
        {
            Contract.RequiresInInclusiveRange(Unsafe.SizeOf<T>(), (uint)slice.Length);
            return Unsafe.As<byte, T>(ref slice.DangerousGetPinnableReference());
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead<[Primitive]T>(this ReadOnlySpan<byte> slice, out T value)
            where T : struct
        {
            if (Unsafe.SizeOf<T>() > (uint)slice.Length) {
                value = default(T);
                return false;
            }
            value = Unsafe.As<byte, T>(ref slice.DangerousGetPinnableReference());
            return true;
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRead<[Primitive]T>(this Span<byte> slice, out T value)
            where T : struct
        {
            if (Unsafe.SizeOf<T>() > (uint)slice.Length) {
                value = default(T);
                return false;
            }
            value = Unsafe.As<byte, T>(ref slice.DangerousGetPinnableReference());
            return true;
        }

        /// <summary>
        /// Writes a structure of type T into a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<[Primitive]T>(this Span<byte> slice, T value)
            where T : struct
        {
            Contract.RequiresInInclusiveRange(Unsafe.SizeOf<T>(), (uint)slice.Length);
            Span<T> castedSlice = slice.Cast<byte, T>();
            castedSlice[0] = value;
        }

        /// <summary>
        /// Writes a structure of type T into a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryWrite<[Primitive]T>(this Span<byte> slice, T value)
            where T : struct
        {
            if (Unsafe.SizeOf<T>() > (uint)slice.Length) {
                return false;
            }
            Span<T> castedSlice = slice.Cast<byte, T>();
            castedSlice[0] = value;
            return true;
        }
    }
}

