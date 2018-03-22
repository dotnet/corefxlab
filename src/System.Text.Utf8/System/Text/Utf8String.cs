// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Text
{
    /// <summary>
    /// A UTF-8 string.
    /// </summary>
    public sealed class Utf8String : IComparable, IComparable<Utf8String>, IConvertible, IEnumerable<Utf8Char>, IEquatable<Utf8String>
    {
        public Utf8String(ReadOnlySpan<byte> value) => throw null;

        public Utf8String(ReadOnlySpan<char> value) => throw null;

        public Utf8String(ReadOnlySpan<Utf8Char> value) => throw null;

        public Utf8String(string value) => throw null;

        public static bool operator ==(Utf8String a, Utf8String b) => throw null;

        public static bool operator !=(Utf8String a, Utf8String b) => throw null;

        public static implicit operator ReadOnlySpan<Utf8Char>(Utf8String value) => throw null;

        public Utf8Char this[int index] => throw null;

        public ReadOnlySpan<Utf8Char> Chars => throw null;

        public static readonly Utf8String Empty;

        public int Length => throw null;

        public int CompareTo(Utf8String other) => throw null;

        public static Utf8String Concat(IEnumerable<Utf8String> values) => throw null;

        public Utf8String Concat(Utf8String str0, Utf8String str1) => throw null;

        public Utf8String Concat(Utf8String str0, Utf8String str1, Utf8String str2) => throw null;

        public Utf8String Concat(Utf8String str0, Utf8String str1, Utf8String str3, Utf8String str4) => throw null;

        public Utf8String Concat(params Utf8String[] values) => throw null;

        public bool Contains(Utf8Char value) => throw null;

        public bool Contains(Utf8String value) => throw null;

        public static Utf8String Create<TState>(int length, TState state, SpanAction<Utf8Char, TState> action) => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Utf8String DangerousCreateWithoutValidation<TState>(int length, TState state, SpanAction<Utf8Char, TState> action) => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Utf8String DangerousCreateWithoutValidation(ReadOnlySpan<Utf8Char> value) => throw null;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref readonly Utf8Char DangerousGetPinnableReference() => throw null;

        public static bool EndsWith(Utf8Char value) => throw null;

        public static bool EndsWith(Utf8String value) => throw null;

        public override bool Equals(object other) => throw null;

        public bool Equals(Utf8String other) => throw null;

        public Utf8CharEnumerator GetEnumerator() => throw null;

        public override int GetHashCode() => throw null;

        public int IndexOf(Utf8Char value) => throw null;

        public int IndexOf(Utf8Char value, int startIndex) => throw null;

        public int IndexOf(Utf8Char value, int startIndex, int count) => throw null;

        public int IndexOf(Utf8String value) => throw null;

        public int IndexOf(Utf8String value, int startIndex) => throw null;

        public int IndexOf(Utf8String value, int startIndex, int count) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex, int count) => throw null;

        public Utf8String Insert(int startIndex, Utf8String value) => throw null;

        public static bool IsNullOrEmpty(Utf8String value) => throw null;

        public static bool IsNullOrWhiteSpace(Utf8String value) => throw null;

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

        public Utf8String Replace(Utf8String oldValue, Utf8String newValue) => throw null;

        public Utf8String[] Split(Utf8Char separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8Char separator, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8Char[] separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8Char[] separator, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8String separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8String separator, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8String[] separator, int count, StringSplitOptions options = default) => throw null;

        public Utf8String[] Split(Utf8String[] separator, StringSplitOptions options = default) => throw null;

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

        public Utf8String Trim(Utf8Char[] trimChars) => throw null;

        public Utf8String TrimEnd() => throw null;

        public Utf8String TrimEnd(Utf8Char trimChar) => throw null;

        public Utf8String TrimEnd(Utf8Char[] trimChars) => throw null;

        public Utf8String TrimStart() => throw null;

        public Utf8String TrimStart(Utf8Char trimChar) => throw null;

        public Utf8String TrimStart(Utf8Char[] trimChars) => throw null;

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
    }
}
