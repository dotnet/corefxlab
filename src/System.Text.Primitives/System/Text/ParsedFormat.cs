// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public static partial class Format
    {
        public const byte NoPrecision = Byte.MaxValue;

        // once we have a non allocating conversion from string to ReadOnlySpan<char>, we can remove this overload
        public static Format.Parsed Parse(string format)
        {
            if (format == null || format.Length == 0)
            {
                return default(Format.Parsed);
            }

            uint precision = NoPrecision;
            if (format.Length > 1)
            {
                if (!InvariantParser.TryParse(format, 1, format.Length - 1, out precision))
                {
                    throw new NotImplementedException("Unable to parse precision specification");
                }

                if (precision > Parsed.MaxPrecision)
                {
                    // TODO: this is a contract violation
                    throw new Exception("PrecisionValueOutOfRange");
                }
            }

            var specifier = format[0];
            return new Parsed(specifier, (byte)precision);
        }

        // TODO: format should be ReadOnlySpan<T>
        public static Format.Parsed Parse(Span<char> format)
        {
            if (format.Length == 0)
            {
                return default(Format.Parsed);
            }

            uint precision = NoPrecision;
            if (format.Length > 1)
            {
                var span = format.Slice(1, format.Length - 1);

                if (!InvariantParser.TryParse(span, out precision))
                {
                    throw new NotImplementedException("UnableToParsePrecision");
                }

                if (precision > Parsed.MaxPrecision)
                {
                    // TODO: this is a contract violation
                    throw new Exception("PrecisionValueOutOfRange");
                }
            }

            // TODO: this is duplicated from above. It needs to be refactored
            var specifier = format[0];
            return new Parsed(specifier, (byte)precision);
        }

        public static Format.Parsed Parse(char format)
        {
            return new Parsed(format, NoPrecision);
        }
 
        public struct Parsed
        {
            internal const byte MaxPrecision = 99;

            public char Symbol;
            public byte Precision;

            public Parsed(char symbol, byte precision = NoPrecision)
            {
                Symbol = symbol;
                Precision = precision;
            }
            public bool IsHexadecimal
            {
                get { return Symbol == 'X' || Symbol == 'x'; }
            }
            public bool HasPrecision
            {
                get { return Precision != NoPrecision; }
            }

            public bool IsDefault {
                get { return Symbol == (char)0; }
            }

            public static Format.Parsed HexUppercase = new Format.Parsed('X', Format.NoPrecision);
            public static Format.Parsed HexLowercase = new Format.Parsed('x', Format.NoPrecision);

            public static implicit operator Parsed(char symbol)
            {
                return new Parsed() { Symbol = symbol };
            }
        }
    }
}
