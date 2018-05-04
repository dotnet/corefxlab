// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
    /// <summary>
    /// A UTF-8 string.
    /// </summary>
    public sealed class Utf8String : IComparable, IComparable<Utf8String>, IConvertible, IEnumerable<Utf8Char>, IEquatable<Utf8String>
    {
        private readonly int _length; // the length in 8-bit code units of this string; guaranteed non-negative
        private readonly byte[] _data; // the backing data for this string; guaranteed size (_length + 1) and null-terminated

        // private ctor for singleton Empty instance
        private Utf8String()
            : this(0)
        {
        }

        // private ctor roughly corresponding to FastAllocateString; caller must check parameters
        private Utf8String(int length)
        {
            _length = length;
            _data = new byte[length + 1]; // zero-inited
        }

        public Utf8String(ReadOnlySpan<byte> value) => throw null;

        public Utf8String(ReadOnlySpan<char> value) => throw null;

        public Utf8String(ReadOnlySpan<Utf8Char> value) => throw null;

        public Utf8String(string value) => throw null;

        public static bool operator ==(Utf8String a, Utf8String b) => Equals(a, b);

        public static bool operator !=(Utf8String a, Utf8String b) => !Equals(a, b);

        public static implicit operator ReadOnlySpan<Utf8Char>(Utf8String value) => (value != null) ? value.Chars : default;

        public ref readonly Utf8Char this[int index]
        {
            get => ref Chars[index];
        }

        public ReadOnlySpan<byte> Bytes
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                // The logic below looks weird, but it's a performance optimization.
                // We know as an implementation detail that the _data field is always at least one
                // element in length. But the JIT doesn't know this, so it'll emit a bounds check if we use
                // the simple "ref _data[0]" syntax. Bouncing the logic through MemoryMarshal
                // eliminates that check. However, there's a new issue: the ReadOnlySpan ctor has
                // a check for a null array parameter. By querying and throwing away the Length
                // property, this forces a dereference of the object, which allows the JIT to reason
                // that for the remainder of the method _data must be non-null, so it can eliminate
                // the null check code paths in the ReadOnlySpan ctor.
                //
                // n.b. The use of _length rather than _data.Length in the returned span. We don't expose the
                // null terminator to the caller.

                var unused = _data.Length;
                return MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(new ReadOnlySpan<byte>(_data)), _length);
            }
        }

        public ReadOnlySpan<Utf8Char> Chars => MemoryMarshal.Cast<byte, Utf8Char>(Bytes);

        public static readonly Utf8String Empty = new Utf8String();

        public int Length => _length;

        public ScalarSequence Scalars => throw null;

        public int CompareTo(Utf8String other) => throw null;

        public static Utf8String Concat(IEnumerable<Utf8String> values) => throw null;

        public Utf8String Concat(Utf8String str0, Utf8String str1) => throw null;

        public Utf8String Concat(Utf8String str0, Utf8String str1, Utf8String str2) => throw null;

        public Utf8String Concat(Utf8String str0, Utf8String str1, Utf8String str3, Utf8String str4) => throw null;

        public Utf8String Concat(params Utf8String[] values) => throw null;

        public bool Contains(ReadOnlySpan<Utf8Char> value) => throw null;

        public bool Contains(UnicodeScalar value) => throw null;

        public bool Contains(Utf8Char value) => throw null;

        public bool Contains(Utf8String value) => throw null;

        public static Utf8String Create<TState>(int length, TState state, SpanAction<byte, TState> action) => throw null;

        public static Utf8String Create<TState>(int length, TState state, SpanAction<Utf8Char, TState> action) => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Utf8String DangerousCreateWithoutValidation<TState>(int length, TState state, SpanAction<byte, TState> action) => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Utf8String DangerousCreateWithoutValidation<TState>(int length, TState state, SpanAction<Utf8Char, TState> action) => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Utf8String DangerousCreateWithoutValidation(ReadOnlySpan<byte> value)
        {
            if (value.IsEmpty)
            {
                return Empty;
            }

            var retVal = new Utf8String(value.Length);
            value.CopyTo(retVal._data); // TODO: remove unneeded bounds check; guaranteed not to overwrite null terminator
            return retVal;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Utf8String DangerousCreateWithoutValidation(ReadOnlySpan<Utf8Char> value) => DangerousCreateWithoutValidation(MemoryMarshal.Cast<Utf8Char, byte>(value));

        public static bool EndsWith(ReadOnlySpan<Utf8Char> value) => throw null;

        public static bool EndsWith(UnicodeScalar value) => throw null;

        public static bool EndsWith(Utf8Char value) => throw null;

        public static bool EndsWith(Utf8String value) => throw null;

        public override bool Equals(object value) => throw null;

        public bool Equals(Utf8String value) => Equals(this, value);

        public bool Equals(string value) => throw null;

        public static bool Equals(Utf8String a, Utf8String b)
        {
            if (ReferenceEquals(a, b))
            {
                return true; // same objects being compared
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false; // one null and one non-null object, or two non-null objects of different length
            }

            // TODO: The below method performs some redundant checks, such as address referential equality and length checking.
            // We need a low-level helper routine that elides these checks.
            return a.Bytes.SequenceEqual(b.Bytes);
        }

        public Enumerator GetEnumerator() => throw null;

        public override int GetHashCode() => Marvin.ComputeHash32(Bytes, Marvin.Utf8StringSeed);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref readonly Utf8Char GetPinnableReference()
        {
            // See comments in get_Bytes for why this code is written as such.

            var unused = _data.Length;
            return ref Unsafe.As<byte, Utf8Char>(ref MemoryMarshal.GetReference(new ReadOnlySpan<byte>(_data)));
        }

        public int IndexOf(ReadOnlySpan<Utf8Char> value) => throw null;

        public int IndexOf(ReadOnlySpan<Utf8Char> value, int startIndex) => throw null;

        public int IndexOf(ReadOnlySpan<Utf8Char> value, int startIndex, int count) => throw null;

        public int IndexOf(UnicodeScalar value) => throw null;

        public int IndexOf(UnicodeScalar value, int startIndex) => throw null;

        public int IndexOf(UnicodeScalar value, int startIndex, int count) => throw null;

        public int IndexOf(Utf8Char value) => throw null;

        public int IndexOf(Utf8Char value, int startIndex) => throw null;

        public int IndexOf(Utf8Char value, int startIndex, int count) => throw null;

        public int IndexOf(Utf8String value) => throw null;

        public int IndexOf(Utf8String value, int startIndex) => throw null;

        public int IndexOf(Utf8String value, int startIndex, int count) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex, int count) => throw null;

        public Utf8String Insert(int startIndex, ReadOnlySpan<Utf8Char> value) => throw null;

        public Utf8String Insert(int startIndex, UnicodeScalar value) => throw null;

        public Utf8String Insert(int startIndex, Utf8String value) => throw null;

        public static bool IsNullOrEmpty(Utf8String value) => (value == null || value.Length == 0);

        public static bool IsNullOrWhiteSpace(Utf8String value) => throw null;

        public int LastIndexOf(ReadOnlySpan<Utf8Char> value) => throw null;

        public int LastIndexOf(ReadOnlySpan<Utf8Char> value, int startIndex) => throw null;

        public int LastIndexOf(ReadOnlySpan<Utf8Char> value, int startIndex, int count) => throw null;

        public int LastIndexOf(UnicodeScalar value) => throw null;

        public int LastIndexOf(UnicodeScalar value, int startIndex) => throw null;

        public int LastIndexOf(UnicodeScalar value, int startIndex, int count) => throw null;

        public int LastIndexOf(Utf8Char value) => throw null;

        public int LastIndexOf(Utf8Char value, int startIndex) => throw null;

        public int LastIndexOf(Utf8Char value, int startIndex, int count) => throw null;

        public int LastIndexOf(Utf8String value) => throw null;

        public int LastIndexOf(Utf8String value, int startIndex) => throw null;

        public int LastIndexOf(Utf8String value, int startIndex, int count) => throw null;

        public int LastIndexOfAny(ReadOnlySpan<Utf8Char> value) => throw null;

        public int LastIndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex) => throw null;

        public int LastIndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex, int count) => throw null;

        public Utf8String PadLeft(int totalWidth) => throw null;

        public Utf8String PadLeft(int totalWidth, Utf8Char paddingChar) => throw null;

        public Utf8String PadRight(int totalWidth) => throw null;

        public Utf8String PadRight(int totalWidth, Utf8Char paddingChar) => throw null;

        public Utf8String Remove(int startIndex) => throw null;

        public Utf8String Remove(int startIndex, int count) => throw null;

        public Utf8String Replace(ReadOnlySpan<Utf8Char> oldValue, ReadOnlySpan<Utf8Char> newValue) => throw null;

        public Utf8String Replace(Utf8String oldValue, Utf8String newValue) => throw null;

        public Utf8String[] Split(ReadOnlySpan<Utf8Char> separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(ReadOnlySpan<Utf8Char> separator, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(UnicodeScalar separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(UnicodeScalar separator, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8Char separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8Char separator, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8Char[] separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8Char[] separator, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8String separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8String separator, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8String[] separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8String[] separator, StringSplitOptions options = default) => throw null;

        public bool StartsWith(ReadOnlySpan<Utf8Char> value) => throw null;

        public bool StartsWith(UnicodeScalar value) => throw null;

        public bool StartsWith(Utf8Char value) => throw null;

        public bool StartsWith(Utf8String value) => throw null;

        public Utf8String Substring(int startIndex) => throw null;

        public Utf8String Substring(int startIndex, int length) => throw null;

        public Utf8String ToLowerInvariant() => throw null;

        public override string ToString() => throw null;

        public string ToString(IFormatProvider provider) => throw null;

        public Utf8String ToUpperInvariant() => throw null;

        public Utf8String Trim() => throw null;

        public Utf8String Trim(Utf8Char trimChar) => throw null;

        public Utf8String Trim(ReadOnlySpan<Utf8Char> trimChars) => throw null;

        public Utf8String TrimEnd() => throw null;

        public Utf8String TrimEnd(Utf8Char trimChar) => throw null;

        public Utf8String TrimEnd(ReadOnlySpan<Utf8Char> trimChars) => throw null;

        public Utf8String TrimStart() => throw null;

        public Utf8String TrimStart(Utf8Char trimChar) => throw null;

        public Utf8String TrimStart(ReadOnlySpan<Utf8Char> trimChars) => throw null;

        TypeCode IConvertible.GetTypeCode() => throw null;

        int IComparable.CompareTo(object obj) => throw null;

        bool IConvertible.ToBoolean(IFormatProvider provider) => throw null;

        byte IConvertible.ToByte(IFormatProvider provider) => throw null;

        char IConvertible.ToChar(IFormatProvider provider) => throw null;

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw null;

        decimal IConvertible.ToDecimal(IFormatProvider provider) => throw null;

        double IConvertible.ToDouble(IFormatProvider provider) => throw null;

        short IConvertible.ToInt16(IFormatProvider provider) => throw null;

        int IConvertible.ToInt32(IFormatProvider provider) => throw null;

        long IConvertible.ToInt64(IFormatProvider provider) => throw null;

        sbyte IConvertible.ToSByte(IFormatProvider provider) => throw null;

        float IConvertible.ToSingle(IFormatProvider provider) => throw null;

        string IConvertible.ToString(IFormatProvider provider) => throw null;

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => throw null;

        ushort IConvertible.ToUInt16(IFormatProvider provider) => throw null;

        uint IConvertible.ToUInt32(IFormatProvider provider) => throw null;

        ulong IConvertible.ToUInt64(IFormatProvider provider) => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;

        IEnumerator<Utf8Char> IEnumerable<Utf8Char>.GetEnumerator() => throw null;

        public struct Enumerator : IEnumerator<Utf8Char>
        {
            public Utf8Char Current => throw null;

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            public void Reset() => throw null;

            object IEnumerator.Current => throw null;
        }

        public struct ScalarSequence : IEnumerable<UnicodeScalar>
        {
            public ScalarEnumerator GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;

            IEnumerator<UnicodeScalar> IEnumerable<UnicodeScalar>.GetEnumerator() => throw null;
        }

        public struct ScalarEnumerator : IEnumerator<UnicodeScalar>
        {
            public UnicodeScalar Current => throw null;

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            public void Reset() => throw null;

            object IEnumerator.Current => throw new NotImplementedException();
        }
    }
}
