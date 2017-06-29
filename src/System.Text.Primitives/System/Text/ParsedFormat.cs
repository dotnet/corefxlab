// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace System.Text
{
    public struct ParsedFormat : IEquatable<ParsedFormat>
    {
        public static ParsedFormat HexUppercase = new ParsedFormat('X', NoPrecision);
        public static ParsedFormat HexLowercase = new ParsedFormat('x', NoPrecision);

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

        public ParsedFormat(char symbol, byte precision = NoPrecision)
        {
            _format = (byte)symbol; //TODO: do we want to validate here?
            _precision = precision;
        }

        public static implicit operator ParsedFormat(char symbol) => new ParsedFormat(symbol);

        public static ParsedFormat Parse(ReadOnlySpan<char> format)
        {
            int formatLength = format.Length;
            if (formatLength == 0)
            {
                return default;
            }

            uint precision = NoPrecision;
            if (formatLength > 1)
            {
                var span = format.Slice(1, formatLength - 1);

                if (!PrimitiveParser.InvariantUtf16.TryParseUInt32(span, out precision))
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
            return new ParsedFormat(specifier, (byte)precision);
        }

        public static ParsedFormat Parse(char format) => new ParsedFormat(format, NoPrecision);

        // once we have a non allocating conversion from string to ReadOnlySpan<char>, we can remove this overload
        public static ParsedFormat Parse(string format)
        {
            if (format == null) return default;
            return Parse(format.AsSpan());
        }

        public bool IsHexadecimal => _format == 'X' || _format == 'x';

        public bool HasPrecision => _precision != NoPrecision;

        public bool IsDefault => _format == 0 && _precision == 0;

        public override string ToString()
        {
            return string.Format("{0}:{1}", _format, _precision);
        }

        public static bool operator ==(ParsedFormat left, ParsedFormat right) => left.Equals(right);
        public static bool operator !=(ParsedFormat left, ParsedFormat right) => !left.Equals(right);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            if (!(obj is ParsedFormat)) return false;
            return Equals((ParsedFormat)obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Equals(ParsedFormat other)
        {
            return _format == other._format && _precision == other._precision;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return CombineHashCodes(_format.GetHashCode(), _precision.GetHashCode());
        }

        static int CombineHashCodes(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }
    }
}
