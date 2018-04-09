// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text.Utf8.Resources;

// TODO: Make this struct serializable.

namespace System.Text
{
    /// <summary>
    /// Represents a Unicode scalar value.
    /// </summary>
    /// <remarks>
    /// A Unicode scalar value is an unsigned integer in the range U+0000..U+D7FF, inclusive;
    /// or within the range U+E000..U+10FFFF, inclusive.
    /// </remarks>
    public readonly struct UnicodeScalar : IComparable<UnicodeScalar>, IEquatable<UnicodeScalar>
    {
        private readonly uint _value;

        /// <summary>
        /// Creates a <see cref="UnicodeScalar"/> from the provided UTF-16 code unit.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="ch"/> represents a UTF-16 surrogate code point
        /// U+D800..U+DFFF, inclusive.
        /// </exception>
        public UnicodeScalar(char ch)
            : this(ch, false)
        {
            if (UnicodeHelpers.IsSurrogateCodePoint(_value))
            {
                throw new ArgumentOutOfRangeException(
                    message: Strings.Argument_NotValidUnicodeScalar,
                    paramName: nameof(ch));
            }
        }

        /// <summary>
        /// Creates a <see cref="UnicodeScalar"/> from the provided Unicode scalar value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="scalarValue"/> does not represent a value Unicode scalar value.
        /// </exception>
        public UnicodeScalar(int scalarValue)
            : this((uint)scalarValue)
        {
        }

        /// <summary>
        /// Creates a <see cref="UnicodeScalar"/> from the provided Unicode scalar value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="scalarValue"/> does not represent a value Unicode scalar value.
        /// </exception>
        public UnicodeScalar(uint scalarValue)
            : this(scalarValue, false)
        {
            if (!UnicodeHelpers.IsValidUnicodeScalar(_value))
            {
                throw new ArgumentOutOfRangeException(
                    message: Strings.Argument_NotValidUnicodeScalar,
                    paramName: nameof(scalarValue));
            }
        }

        // non-validating ctor
        private UnicodeScalar(uint scalarValue, bool ignored)
        {
            _value = scalarValue;
        }

        /// <summary>
        /// Compares two <see cref="UnicodeScalar"/> instances for equality.
        /// </summary>
        public static bool operator ==(UnicodeScalar a, UnicodeScalar b) => (a._value == b._value);

        /// <summary>
        /// Compares two <see cref="UnicodeScalar"/> instances for inequality.
        /// </summary>
        public static bool operator !=(UnicodeScalar a, UnicodeScalar b) => (a._value != b._value);

        /// <summary>
        /// Returns true iff this scalar value is ASCII ([ U+0000..U+007F ])
        /// and therefore representable by a single UTF-8 code unit.
        /// </summary>
        public bool IsAscii => UnicodeHelpers.IsAsciiCodePoint(_value);

        /// <summary>
        /// Returns true iff this scalar value is within the BMP ([ U+0000..U+FFFF ])
        /// and therefore representable by a single UTF-16 code unit.
        /// </summary>
        public bool IsBmp => UnicodeHelpers.IsBmpCodePoint(_value);

        /// <summary>
        /// A <see cref="UnicodeScalar"/> instance that represents the Unicode replacement character U+FFFD.
        /// </summary>
        public static UnicodeScalar ReplacementChar => DangerousCreateWithoutValidation(UnicodeHelpers.ReplacementChar);

        /// <summary>
        /// Returns the length in code units (<see cref="Char"/>) of the
        /// UTF-16 sequence required to represent this scalar value.
        /// </summary>
        /// <remarks>
        /// The return value will be 1 or 2.
        /// </remarks>
        public int Utf16SequenceLength => UnicodeHelpers.GetUtf16SequenceLength(_value);

        /// <summary>
        /// Returns the length in code units (<see cref="Utf8Char"/>) of the
        /// UTF-8 sequence required to represent this scalar value.
        /// </summary>
        /// <remarks>
        /// The return value will be 1 through 4, inclusive.
        /// </remarks>
        public int Utf8SequenceLength => UnicodeHelpers.GetUtf8SequenceLength(_value);

        /// <summary>
        /// Returns the Unicode scalar value as an unsigned integer.
        /// </summary>
        public uint Value => _value;

        /// <summary>
        /// Compares this <see cref="UnicodeScalar"/> instance to another <see cref="UnicodeScalar"/> instance.
        /// </summary>
        public int CompareTo(UnicodeScalar other) => this._value.CompareTo(other._value);

        /// <summary>
        /// Creates a <see cref="UnicodeScalar"/> from the provided Unicode scalar value
        /// without validating the input value for well-formedness.
        /// </summary>
        /// <remarks>
        /// The caller is expected to have validated that <paramref name="scalarValue"/> is
        /// a valid value. The behavior of this type is undefined if the input value is invalid.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static UnicodeScalar DangerousCreateWithoutValidation(uint scalarValue) => new UnicodeScalar(scalarValue, false);

        /// <summary>
        /// Returns true iff this <see cref="UnicodeScalar"/> instance is equal to the provided object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return (obj is UnicodeScalar) && Equals((UnicodeScalar)obj);
        }

        /// <summary>
        /// Returns true iff this <see cref="UnicodeScalar"/> instance is equal to the provided <see cref="UnicodeScalar"/> instance.
        /// </summary>
        public bool Equals(UnicodeScalar other) => this._value.Equals(other._value);

        /// <summary>
        /// Returns a hash code for this <see cref="UnicodeScalar"/> instance suitable for use in a dictionary.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => _value.GetHashCode();

        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="value"/> is a valid Unicode scalar
        /// value, i.e., is in [ U+0000..U+D7FF ], inclusive; or [ U+E000..U+10FFFF ], inclusive.
        /// </summary>
        public static bool IsValid(int value) => UnicodeHelpers.IsValidUnicodeScalar((uint)value);

        /// <summary>
        /// Returns a <see cref="String"/> representation of this <see cref="UnicodeScalar"/> instance.
        /// </summary>
        public override string ToString()
        {
            Span<char> chars = stackalloc char[2]; // worst case
            return new String(chars.Slice(0, ToUtf16(chars)));
        }

        /// <summary>
        /// Writes this scalar value as a UTF-16 sequence to the output buffer, returning
        /// the number of code units (<see cref="Char"/>) written.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="output"/> is too short to contain the output.
        /// The required length can be queried ahead of time via the <see cref="Utf16SequenceLength"/> property.
        /// </exception>
        public int ToUtf16(Span<char> output) => ToUtf16(output, _value);

        private static int ToUtf16(Span<char> output, uint value)
        {
            if (UnicodeHelpers.IsBmpCodePoint(value) && output.Length > 0)
            {
                output[0] = (char)value;
                return 1;
            }
            else if (output.Length > 1)
            {
                // TODO: This logic can be optimized into a single unaligned write, endianness-dependent.

                output[0] = (char)((value + ((0xD800U - 0x40U) << 10)) >> 10); // high surrogate
                output[1] = (char)((value & 0x3FFFU) + 0xDC00U); // low surrogate
                return 2;
            }
            else
            {
                throw new ArgumentException(
                    message: Strings.Argument_OutputBufferTooSmall,
                    paramName: nameof(output));
            }
        }

        /// <summary>
        /// Writes this scalar value as a UTF-8 sequence to the output buffer, returning
        /// the number of code units (<see cref="Utf8Char"/>) written.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="output"/> is too short to contain the output.
        /// The required length can be queried ahead of time via the <see cref="Utf8SequenceLength"/> property.
        /// </exception>
        public int ToUtf8(Span<Utf8Char> output) => ToUtf8(output, _value);

        private static int ToUtf8(Span<Utf8Char> output, uint value)
        {
            // TODO: This logic can be optimized into fewer unaligned writes, endianness-dependent.
            // TODO: Consider using BMI2 (pext, pdep) when it comes online.
            // TODO: Consider using hardware-accelerated byte swapping (bswap, movbe) if available.

            var outputAsBytes = MemoryMarshal.AsBytes(output);

            if (UnicodeHelpers.IsAsciiCodePoint(value) && output.Length > 0)
            {
                outputAsBytes[0] = (byte)value;
                return 1;
            }
            else if (value < 0x800U && output.Length > 1)
            {
                outputAsBytes[0] = (byte)((value + (0xC0U << 6)) >> 6);
                outputAsBytes[1] = (byte)((value & 0x3FU) + 0x80U);
                return 2;
            }
            else if (value < 0x10000U && output.Length > 2)
            {
                outputAsBytes[0] = (byte)((value + (0xE0U << 12)) >> 12);
                outputAsBytes[1] = (byte)(((value >> 6) & 0x3FU) + 0x80U);
                outputAsBytes[2] = (byte)((value & 0x3FU) + 0x80U);
                return 3;
            }
            else if (output.Length > 3)
            {
                outputAsBytes[0] = (byte)((value + (0xF0U << 18)) >> 18);
                outputAsBytes[1] = (byte)(((value >> 12) & 0x3FU) + 0x80U);
                outputAsBytes[2] = (byte)(((value >> 6) & 0x3FU) + 0x80U);
                outputAsBytes[3] = (byte)((value & 0x3FU) + 0x80U);
                return 4;
            }
            else
            {
                throw new ArgumentException(
                    message: Strings.Argument_OutputBufferTooSmall,
                    paramName: nameof(output));
            }
        }

        /// <summary>
        /// Returns a <see cref="Utf8String"/> representation of this <see cref="UnicodeScalar"/> instance.
        /// </summary>
        public Utf8String ToUtf8String()
        {
            Span<Utf8Char> utf8Chars = stackalloc Utf8Char[4]; // worst case
            return Utf8String.DangerousCreateWithoutValidation(utf8Chars.Slice(0, ToUtf8(utf8Chars)));
        }
    }
}
