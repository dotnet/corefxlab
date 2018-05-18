// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Represents a natural word sized unsigned integer.
    /// Casts exist to convert this to / from other integral types.
    /// None of the conversions is checked.
    /// </summary>
    internal unsafe struct nuint
    {
        private readonly void* _value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private nuint(void* value)
        {
            _value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(nuint value) => (int)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(nuint value) => (uint)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(nuint value) => (long)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(nuint value) => (ulong)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IntPtr(nuint value) => (IntPtr)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nuint(int value) => new nuint((void*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nuint(uint value) => new nuint((void*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nuint(long value) => new nuint((void*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nuint(ulong value) => new nuint((void*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nuint(IntPtr value) => new nuint((void*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(nuint a, nuint b) => a._value < b._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(nuint a, nuint b) => a._value <= b._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(nuint a, nuint b) => a._value > b._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(nuint a, nuint b) => a._value >= b._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(nuint a, nuint b) => a._value == b._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(nuint a, nuint b) => a._value != b._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator +(nuint value, int stride) => new nuint((byte*)value._value + stride);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator ++(nuint value) => value + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator &(nuint value, int andWith)
        {
            if (IntPtr.Size >= 8)
            {
                return new nuint((void*)((ulong)value._value & (ulong)andWith));
            }
            else
            {
                return new nuint((void*)((uint)value._value & (uint)andWith));
            }
        }
    }
}
