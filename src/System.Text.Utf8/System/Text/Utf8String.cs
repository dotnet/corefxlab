// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Utf8.Resources;

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

        // private ctor roughly corresponding to FastAllocateString
        private Utf8String(int length)
        {
            if (length < 0)
            {
                throw new OutOfMemoryException(); // something overflowed in the caller
            }

            _length = length;
            _data = new byte[length + 1]; // zero-inited; array ctor will check for overflow
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

        public static Utf8String Concat(Utf8String str0, Utf8String str1)
        {
            Utf8String singleValueToReturn = Empty;

            if (!IsNullOrEmpty(str0))
            {
                singleValueToReturn = str0;
            }

            if (!IsNullOrEmpty(str1))
            {
                if (singleValueToReturn.Length == 0)
                {
                    singleValueToReturn = str1;
                }
                else
                {
                    goto AllocateAndCopy;
                }
            }

            return singleValueToReturn;

        AllocateAndCopy:

            // overflow will be checked by the ctor or the CopyTo routine
            return CreateInternal(str0.Length + str1.Length, (str0, str1), (span, state) =>
            {
                str0.Bytes.CopyTo(span);
                str1.Bytes.CopyTo(span.Slice(str0.Length));
            });
        }

        public static Utf8String Concat(Utf8String str0, Utf8String str1, Utf8String str2)
        {
            Utf8String singleValueToReturn = Empty;

            if (!IsNullOrEmpty(str0))
            {
                singleValueToReturn = str0;
            }

            if (!IsNullOrEmpty(str1))
            {
                if (singleValueToReturn.Length == 0)
                {
                    singleValueToReturn = str1;
                }
                else
                {
                    goto AllocateAndCopy;
                }
            }

            if (!IsNullOrEmpty(str2))
            {
                if (singleValueToReturn.Length == 0)
                {
                    singleValueToReturn = str2;
                }
                else
                {
                    goto AllocateAndCopy;
                }
            }

            return singleValueToReturn;

        AllocateAndCopy:

            // overflow will be checked by the ctor or the CopyTo routine
            return CreateInternal(str0.Length + str1.Length + str2.Length, (str0, str1, str2), (span, state) =>
            {
                str0.Bytes.CopyTo(span);
                str1.Bytes.CopyTo(span = span.Slice(str0.Length));
                str2.Bytes.CopyTo(span.Slice(str1.Length));
            });
        }

        public static Utf8String Concat(Utf8String str0, Utf8String str1, Utf8String str2, Utf8String str3)
        {
            Utf8String singleValueToReturn = Empty;

            if (!IsNullOrEmpty(str0))
            {
                singleValueToReturn = str0;
            }

            if (!IsNullOrEmpty(str1))
            {
                if (singleValueToReturn.Length == 0)
                {
                    singleValueToReturn = str1;
                }
                else
                {
                    goto AllocateAndCopy;
                }
            }

            if (!IsNullOrEmpty(str2))
            {
                if (singleValueToReturn.Length == 0)
                {
                    singleValueToReturn = str2;
                }
                else
                {
                    goto AllocateAndCopy;
                }
            }

            if (!IsNullOrEmpty(str3))
            {
                if (singleValueToReturn.Length == 0)
                {
                    singleValueToReturn = str3;
                }
                else
                {
                    goto AllocateAndCopy;
                }
            }

            return singleValueToReturn;

        AllocateAndCopy:

            // overflow will be checked by the ctor or the CopyTo routine
            return CreateInternal(str0.Length + str1.Length + str2.Length + str3.Length, (str0, str1, str2, str3), (span, state) =>
            {
                str0.Bytes.CopyTo(span);
                str1.Bytes.CopyTo(span = span.Slice(str0.Length));
                str2.Bytes.CopyTo(span = span.Slice(str1.Length));
                str3.Bytes.CopyTo(span.Slice(str2.Length));
            });
        }

        public static Utf8String Concat(params Utf8String[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Length <= 1)
            {
                return (values.Length == 0) ? Empty : (values[0] ?? Empty);
            }

            int totalRequiredLength = 0;
            foreach (var item in values)
            {
                if (item != null)
                {
                    // it's ok if this overflows; it'll be checked later
                    totalRequiredLength += item.Length;
                }
            }

            // overflow will be checked by the ctor or the CopyTo routine
            return CreateInternal(totalRequiredLength, values, (span, innerValues) =>
            {
                // It's possible that the underlying 'values' array could be mutated while
                // we're working with it. If the destination buffer is too small, the
                // CopyTo method will throw an exception; if the destination buffer is too
                // large, we'll throw at the end of this method. If by coincidence the buffer
                // happens to be correctly sized even in the face of mutating data, just let
                // it slide.

                foreach (var item in innerValues)
                {
                    if (item != null)
                    {
                        item.Bytes.CopyTo(span);
                        span = span.Slice(item.Length);
                    }
                }

                if (!span.IsEmpty)
                {
                    // String.Concat(params string[]) has logic to retry the loop in the event of
                    // the 'values' array being mutated. But honestly, nobody should be doing this,
                    // so we'll just throw rather than try to recover.
                    throw new InvalidOperationException();
                }
            });
        }

        public bool Contains(ReadOnlySpan<Utf8Char> value) => throw null;

        public bool Contains(UnicodeScalar value)
        {
            // TODO: Create a proper optimized Contains method.
            return (IndexOf(value) >= 0);
        }

        public bool Contains(Utf8Char value)
        {
            // TODO: Call a proper Contains method when it's introduced on MemoryExtensions.
            return (IndexOf(value) >= 0);
        }

        public bool Contains(Utf8String value) => throw null;

        public static Utf8String Create<TState>(int length, TState state, SpanAction<byte, TState> action) => CreateFromUserInputCommon(length, state, action, validateInput: true);

        public static Utf8String Create<TState>(int length, TState state, SpanAction<Utf8Char, TState> action) => CreateFromUserInputCommon(length, state, action, validateInput: true);

        private static Utf8String CreateInternal<TState>(int length, TState state, SpanAction<byte, TState> action)
        {
            var retVal = new Utf8String(length);
            action(MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(retVal.Bytes), length), state);
            return retVal;
        }

        private static Utf8String CreateFromUserInputCommon<TState, TCodeUnit>(int length, TState state, SpanAction<TCodeUnit, TState> action, bool validateInput)
            where TCodeUnit : struct
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (length <= 0)
            {
                if (length < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(length));
                }

                return Empty;
            }

            var retVal = new Utf8String(length);
            action(MemoryMarshal.CreateSpan(ref Unsafe.As<byte, TCodeUnit>(ref MemoryMarshal.GetReference(retVal.Bytes)), length), state);

            if (validateInput && !retVal.IsWellFormed())
            {
                throw new ArgumentException(Strings.Argument_CreateCallbackReturnedIllFormedUtf8String);
            }

            return retVal;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Utf8String DangerousCreateWithoutValidation<TState>(int length, TState state, SpanAction<byte, TState> action) => CreateFromUserInputCommon(length, state, action, validateInput: false);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static Utf8String DangerousCreateWithoutValidation<TState>(int length, TState state, SpanAction<Utf8Char, TState> action) => CreateFromUserInputCommon(length, state, action, validateInput: false);

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

        public bool EndsWith(ReadOnlySpan<Utf8Char> value) => throw null;

        public bool EndsWith(UnicodeScalar value) => throw null;

        public bool EndsWith(Utf8Char value)
        {
            int index = _data.Length - 2; // look at the penultimate byte (if there is one)
            return ((uint)index < (uint)_data.Length) && (_data[index] == (byte)value);
        }

        public bool EndsWith(Utf8String value) => throw null;

        public override bool Equals(object value)
        {
            if (ReferenceEquals(this, value))
            {
                return true; // same objects being compared
            }

            if (!(value is Utf8String valueAsUtf8String) || this._length != valueAsUtf8String._length)
            {
                return false; // value is null, not a Utf8String, or has different length than this
            }

            // TODO: The below method performs some redundant checks, such as address referential equality and length checking.
            // We need a low-level helper routine that elides these checks.
            return this.Bytes.SequenceEqual(valueAsUtf8String.Bytes);
        }

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

        public int IndexOf(UnicodeScalar value)
        {
            // Special-case ASCII since it's only a single UTF-8 code unit.

            if (value.IsAscii)
            {
                return IndexOf((Utf8Char)value.Value);
            }

            Span<byte> buffer = stackalloc byte[4]; // largest possible scalar is four UTF-8 code units
            int actualCodeUnitCount = value.ToUtf8(MemoryMarshal.Cast<byte, Utf8Char>(buffer));

            // TODO: Use an IndexOf method that's optimized for short search targets.

            return Bytes.IndexOf(buffer.Slice(actualCodeUnitCount));
        }

        public int IndexOf(UnicodeScalar value, int startIndex)
        {
            // Special-case ASCII since it's only a single UTF-8 code unit.

            if (value.IsAscii)
            {
                return IndexOf((Utf8Char)value.Value, startIndex);
            }

            Span<byte> buffer = stackalloc byte[4]; // largest possible scalar is four UTF-8 code units
            int actualCodeUnitCount = value.ToUtf8(MemoryMarshal.Cast<byte, Utf8Char>(buffer));

            // TODO: Use an IndexOf method that's optimized for short search targets.

            return Bytes.Slice(startIndex).IndexOf(buffer.Slice(actualCodeUnitCount));
        }

        public int IndexOf(UnicodeScalar value, int startIndex, int count)
        {
            // Special-case ASCII since it's only a single UTF-8 code unit.

            if (value.IsAscii)
            {
                return IndexOf((Utf8Char)value.Value, startIndex, count);
            }

            Span<byte> buffer = stackalloc byte[4]; // largest possible scalar is four UTF-8 code units
            int actualCodeUnitCount = value.ToUtf8(MemoryMarshal.Cast<byte, Utf8Char>(buffer));

            // TODO: Use an IndexOf method that's optimized for short search targets.

            return Bytes.Slice(startIndex, count).IndexOf(buffer.Slice(actualCodeUnitCount));
        }

        public int IndexOf(Utf8Char value) => Bytes.IndexOf((byte)value);

        public int IndexOf(Utf8Char value, int startIndex) => Bytes.Slice(startIndex).IndexOf((byte)value);

        public int IndexOf(Utf8Char value, int startIndex, int count) => Bytes.Slice(startIndex, count).IndexOf((byte)value);

        public int IndexOf(Utf8String value) => throw null;

        public int IndexOf(Utf8String value, int startIndex) => throw null;

        public int IndexOf(Utf8String value, int startIndex, int count) => throw null;

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value) => Bytes.IndexOfAny(MemoryMarshal.Cast<Utf8Char, byte>(value));

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex) => Bytes.Slice(startIndex).IndexOfAny(MemoryMarshal.Cast<Utf8Char, byte>(value));

        public int IndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex, int count) => Bytes.Slice(startIndex, count).IndexOfAny(MemoryMarshal.Cast<Utf8Char, byte>(value));

        public Utf8String Insert(int startIndex, ReadOnlySpan<Utf8Char> value) => throw null;

        public Utf8String Insert(int startIndex, UnicodeScalar value) => throw null;

        public Utf8String Insert(int startIndex, Utf8String value) => throw null;

        public static bool IsNullOrEmpty(Utf8String value) => (value == null || value.Length == 0);

        public static bool IsNullOrWhiteSpace(Utf8String value) => throw null;

        internal bool IsWellFormed() => throw null;

        public int LastIndexOf(ReadOnlySpan<Utf8Char> value) => throw null;

        public int LastIndexOf(ReadOnlySpan<Utf8Char> value, int startIndex) => throw null;

        public int LastIndexOf(ReadOnlySpan<Utf8Char> value, int startIndex, int count) => throw null;

        public int LastIndexOf(UnicodeScalar value) => throw null;

        public int LastIndexOf(UnicodeScalar value, int startIndex) => throw null;

        public int LastIndexOf(UnicodeScalar value, int startIndex, int count) => throw null;

        public int LastIndexOf(Utf8Char value) => Bytes.LastIndexOf((byte)value);

        public int LastIndexOf(Utf8Char value, int startIndex) => Bytes.Slice(startIndex).LastIndexOf((byte)value);

        public int LastIndexOf(Utf8Char value, int startIndex, int count) => Bytes.Slice(startIndex, count).LastIndexOf((byte)value);

        public int LastIndexOf(Utf8String value) => throw null;

        public int LastIndexOf(Utf8String value, int startIndex) => throw null;

        public int LastIndexOf(Utf8String value, int startIndex, int count) => throw null;

        public int LastIndexOfAny(ReadOnlySpan<Utf8Char> value) => Bytes.LastIndexOfAny(MemoryMarshal.Cast<Utf8Char, byte>(value));

        public int LastIndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex) => Bytes.Slice(startIndex).LastIndexOfAny(MemoryMarshal.Cast<Utf8Char, byte>(value));

        public int LastIndexOfAny(ReadOnlySpan<Utf8Char> value, int startIndex, int count) => Bytes.Slice(startIndex, count).LastIndexOfAny(MemoryMarshal.Cast<Utf8Char, byte>(value));

        public Utf8String PadLeft(int totalWidth) => PadLeft(totalWidth, (Utf8Char)' ');

        public Utf8String PadLeft(int totalWidth, Utf8Char paddingChar)
        {
            if (totalWidth <= _length)
            {
                if (totalWidth < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(totalWidth),
                        message: Strings.ValueMustBeNonNegative);
                }
                else
                {
                    return this;
                }
            }

            return CreateInternal(totalWidth, (paddingChar, padLength: totalWidth - _length, source: this), (span, state) =>
            {
                span.Slice(0, state.padLength).Fill((byte)state.paddingChar);
                state.source.Bytes.CopyTo(span.Slice(state.padLength));
            });
        }

        public Utf8String PadRight(int totalWidth) => PadRight(totalWidth, (Utf8Char)' ');

        public Utf8String PadRight(int totalWidth, Utf8Char paddingChar)
        {
            if (totalWidth <= _length)
            {
                if (totalWidth < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(totalWidth),
                        message: Strings.ValueMustBeNonNegative);
                }
                else
                {
                    return this;
                }
            }

            return CreateInternal(totalWidth, (paddingChar, source: this), (span, state) =>
            {
                state.source.Bytes.CopyTo(span);
                span.Slice(state.source.Length).Fill((byte)state.paddingChar);
            });
        }

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

        public bool StartsWith(Utf8Char value)
        {
            // Length > 1 check is so that we're not comparing against the null terminator
            return (_data.Length > 1) && (_data[0] == (byte)value);
        }

        public bool StartsWith(Utf8String value) => throw null;

        public Utf8String Substring(int startIndex)
        {
            // n.b. it's ok to pass a startIndex equal to Length; we'll just return an empty string

            if ((uint)startIndex > (uint)_length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex == 0)
            {
                return this;
            }

            if (startIndex == _length)
            {
                return Empty;
            }

            return CreateInternal(_length - startIndex, (source: this, startIndex), (span, state) =>
            {
                state.source.Bytes.Slice(state.startIndex).CopyTo(span);
            });
        }

        public Utf8String Substring(int startIndex, int length)
        {
            if ((uint)startIndex > (uint)_length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if ((uint)length > (uint)(_length - startIndex))
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (length == _length)
            {
                return this;
            }

            if (length == 0)
            {
                return Empty;
            }

            return CreateInternal(length, (source: this, startIndex, length), (span, state) =>
            {
                state.source.Bytes.Slice(state.startIndex, state.length).CopyTo(span);
            });
        }

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
