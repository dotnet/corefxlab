// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Formatting
{
    public static partial class Format
    {
        public const byte NoPrecision = Byte.MaxValue;

        // once we have a non allocating conversion from string to ReadOnlySpan<char>, we can remove this overload
        public static Format.Parsed Parse(string format)
        {
            if (format == null || format.Length == 0)
            {
                return new Format.Parsed() { Symbol = Symbol.G, Precision = NoPrecision };
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
                    throw new Exception(Strings.PrecisionValueOutOfRange);
                }
            }

            var specifier = format[0];
            return CreateFormatFromChar((byte)precision, specifier);
        }

        public static Format.Parsed Parse(ReadOnlySpan<char> format)
        {
            if (format.Length == 0)
            {
                return new Format.Parsed() { Symbol = Symbol.G, Precision = NoPrecision };
            }

            uint precision = NoPrecision;
            if (format.Length > 1)
            {
                var span = format.Slice(1, format.Length - 1);

                if (!InvariantParser.TryParse(span, out precision))
                {
                    throw new NotImplementedException(Strings.UnableToParsePrecision);
                }

                if (precision > Parsed.MaxPrecision)
                {
                    // TODO: this is a contract violation
                    throw new Exception(Strings.PrecisionValueOutOfRange);
                }
            }

            // TODO: this is duplicated from above. It needs to be refactored
            var specifier = format[0];
            return CreateFormatFromChar((byte)precision, specifier);
        }

        public static Format.Parsed Parse(char format)
        {
            return CreateFormatFromChar(NoPrecision, format);
        }

        private static Parsed CreateFormatFromChar(byte precision, char specifier)
        {
            switch (specifier)
            {
                case 'B':
                case 'b':
                    return new Format.Parsed() { Symbol = Symbol.B, Precision = precision };
                case 'D':
                case 'd':
                    return new Format.Parsed() { Symbol = Symbol.D, Precision = precision };
                case 'E':
                case 'e':
                    return new Format.Parsed() { Symbol = Symbol.E, Precision = precision };
                case 'F':
                case 'f':
                    return new Format.Parsed() { Symbol = Symbol.F, Precision = precision };
                case 'G':
                case 'g':
                    if (precision == 0)
                    {
                        precision = NoPrecision;
                    }
                    return new Format.Parsed() { Symbol = Symbol.G, Precision = precision };
                case 'N':
                case 'n':
                    return new Format.Parsed() { Symbol = Symbol.N, Precision = precision };
                case 'O':
                case 'o':
                    return new Format.Parsed() { Symbol = Symbol.O, Precision = precision };
                case 'P':
                case 'p':
                    return new Format.Parsed() { Symbol = Symbol.P, Precision = precision };
                case 'R':
                case 'r':
                    return new Format.Parsed() { Symbol = Symbol.R, Precision = precision };
                case 'X':
                    return new Format.Parsed() { Symbol = Symbol.X, Precision = precision };
                case 'x':
                    return new Format.Parsed() { Symbol = Symbol.XLowercase, Precision = precision };
                default:
                    throw new Exception(Strings.InvalidFormat);
            }
        }

        public struct Parsed
        {
            internal const byte MaxPrecision = 99;

            public Symbol Symbol;
            public byte Precision;

            public bool IsHexadecimal
            {
                get { return Symbol == Symbol.X || Symbol == Symbol.XLowercase; }
            }
            public bool HasPrecision
            {
                get { return Precision != NoPrecision; }
            }

            public bool IsDefault {
                get { return Symbol == Symbol.G; }
            }

            public static Format.Parsed HexUppercase = new Format.Parsed() { Symbol = Symbol.X, Precision = Format.NoPrecision };
            public static Format.Parsed HexLowercase = new Format.Parsed() { Symbol = Symbol.X, Precision = Format.NoPrecision };

            public static implicit operator Parsed(Symbol symbol)
            {
                return new Parsed() { Symbol = symbol };
            }

        }
        public enum Symbol : byte
        {
            G = 0, 
            D,
            B,
            E,
            F,
            N,
            O,
            P,
            R,
            X,
            XLowercase,
        }
    }
}
