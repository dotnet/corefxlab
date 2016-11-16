// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public struct TextFormat
    {
        public const byte NoPrecision = byte.MaxValue;
        internal const byte MaxPrecision = 99;

        public char Symbol;
        public byte Precision;

        // once we have a non allocating conversion from string to ReadOnlySpan<char>, we can remove this overload
        public static TextFormat Parse(string format)
        {
            if (format == null || format.Length == 0)
            {
                return default(TextFormat);
            }

            uint precision = NoPrecision;
            if (format.Length > 1)
            {
                if (!PrimitiveParser.TryParseUInt32(format, 1, format.Length - 1, out precision))
                {
                    throw new NotImplementedException("Unable to parse precision specification");
                }

                if (precision > TextFormat.MaxPrecision)
                {
                    // TODO: this is a contract violation
                    throw new Exception("PrecisionValueOutOfRange");
                }
            }

            var specifier = format[0];
            return new TextFormat(specifier, (byte)precision);
        }

        // TODO: format should be ReadOnlySpan<T>
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

                if (!PrimitiveParser.TryParseUInt32(span, out precision))
                {
                    throw new NotImplementedException("UnableToParsePrecision");
                }

                if (precision > TextFormat.MaxPrecision)
                {
                    // TODO: this is a contract violation
                    throw new Exception("PrecisionValueOutOfRange");
                }
            }

            // TODO: this is duplicated from above. It needs to be refactored
            var specifier = format[0];
            return new TextFormat(specifier, (byte)precision);
        }

        public static TextFormat Parse(char format)
        {
            return new TextFormat(format, NoPrecision);
        }
 
        public TextFormat(char symbol, byte precision = NoPrecision)
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

        public static TextFormat HexUppercase = new TextFormat('X', TextFormat.NoPrecision);
        public static TextFormat HexLowercase = new TextFormat('x', TextFormat.NoPrecision);

        public static implicit operator TextFormat(char symbol)
        {
            return new TextFormat(symbol);
        }      
    }
}
