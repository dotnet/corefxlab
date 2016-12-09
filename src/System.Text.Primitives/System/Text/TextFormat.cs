// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace System.Text
{
    public struct TextFormat : IEquatable<TextFormat>
    {
        public static TextFormat HexUppercase = new TextFormat('X', NoPrecision);
        public static TextFormat HexLowercase = new TextFormat('x', NoPrecision);

        public const byte NoPrecision = byte.MaxValue;
        internal const byte MaxPrecision = 99;

        byte _format;
        byte _precision;

        public char Symbol
        {
            get { return (char)_format; }
            set { _format = (byte)value; } //TODO: do we want to validate here?
        }
        public byte Precision => _precision;

        public TextFormat(char symbol, byte precision = NoPrecision)
        {
            _format = (byte)symbol; //TODO: do we want to validate here?
            _precision = precision;
        }

        public static implicit operator TextFormat(char symbol) => new TextFormat(symbol);

        public static TextFormat Parse(ReadOnlySpan<char> format)
        {
            if (format.Length == 0)
            {
                return default(TextFormat);
            }

            uint precision = NoPrecision;
            if (format.Length > 1)
            {
                var span = format.Slice(1, format.Length - 1);

                if (!Internal.InternalParser.TryParseUInt32(span, out precision))
                {
                    throw new NotImplementedException("UnableToParsePrecision");
                }

                if (precision > MaxPrecision)
                {
                    // TODO: this is a contract violation
                    throw new Exception("PrecisionValueOutOfRange");
                }
            }

            // TODO: this is duplicated from above. It needs to be refactored
            var specifier = format[0];
            return new TextFormat(specifier, (byte)precision);
        }

        public static TextFormat Parse(char format) => new TextFormat(format, NoPrecision);

        // once we have a non allocating conversion from string to ReadOnlySpan<char>, we can remove this overload
        public static TextFormat Parse(string format)
        {
            if (format == null) return default(TextFormat);
            return Parse(format.Slice());
        }
         
        public bool IsHexadecimal => Symbol == 'X' || Symbol == 'x';

        public bool HasPrecision => Precision != NoPrecision;

        public bool IsDefault => _format == 0 && _precision == 0;

        public override string ToString()
        {
            return string.Format("{0}:{1}", Symbol, Precision);
        }

        public static bool operator==(TextFormat left, TextFormat right) => left.Equals(right);
        public static bool operator !=(TextFormat left, TextFormat right) => !left.Equals(right);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if (!(obj is TextFormat)) return false;
            return Equals((TextFormat)obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Equals(TextFormat other)
        {
            return Symbol.Equals(other.Symbol) && Precision.Equals(other.Precision);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return CombineHashCodes(Symbol.GetHashCode(), Precision.GetHashCode());
        }

        static int CombineHashCodes(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }
    }
}
