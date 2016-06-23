// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    public struct FormattingData
    {
        private static FormattingData s_invariantUtf16;
        private static FormattingData s_invariantUtf8;
        private byte[][] _digitsAndSymbols; // this could be flatten into a single array
        private Encoding _encoding;

        public enum Encoding
        {
            Utf16 = 0,
            Utf8 = 1,
        }

        public FormattingData(byte[][] digitsAndSymbols, Encoding encoding)
        {
            _digitsAndSymbols = digitsAndSymbols;
            _encoding = encoding;
        }

        // it might be worth compacting the data into a single byte array.
        // Also, it would be great if we could freeze it.
        static FormattingData()
        {
            var utf16digitsAndSymbols = new byte[][] {
                new byte[] { 48, 0, }, // digit 0
                new byte[] { 49, 0, },
                new byte[] { 50, 0, },
                new byte[] { 51, 0, },
                new byte[] { 52, 0, },
                new byte[] { 53, 0, },
                new byte[] { 54, 0, },
                new byte[] { 55, 0, },
                new byte[] { 56, 0, },
                new byte[] { 57, 0, }, // digit 9
                new byte[] { 46, 0, }, // decimal separator
                new byte[] { 44, 0, }, // group separator
                new byte[] { 73, 0, 110, 0, 102, 0, 105, 0, 110, 0, 105, 0, 116, 0, 121, 0, }, // Infinity
                new byte[] { 45, 0, }, // minus sign 
                new byte[] { 43, 0, }, // plus sign 
                new byte[] { 78, 0, 97, 0, 78, 0, }, // NaN
                new byte[] { 69, 0, }, // E
            };

            s_invariantUtf16 = new FormattingData(utf16digitsAndSymbols, Encoding.Utf16);

            var utf8digitsAndSymbols = new byte[][] {
                new byte[] { 48, },
                new byte[] { 49, },
                new byte[] { 50, },
                new byte[] { 51, },
                new byte[] { 52, },
                new byte[] { 53, },
                new byte[] { 54, },
                new byte[] { 55, },
                new byte[] { 56, },
                new byte[] { 57, }, // digit 9
                new byte[] { 46, }, // decimal separator
                new byte[] { 44, }, // group separator
                new byte[] { 73, 110, 102, 105, 110, 105, 116, 121, },
                new byte[] { 45, }, // minus sign
                new byte[] { 43, }, // plus sign
                new byte[] { 78, 97, 78, }, // NaN
                new byte[] { 69, }, // E
            };

            s_invariantUtf8 = new FormattingData(utf8digitsAndSymbols, Encoding.Utf8);
        }

        public static FormattingData InvariantUtf16
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return s_invariantUtf16;
            }
        }
        public static FormattingData InvariantUtf8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return s_invariantUtf8;
            }
        }

        public bool TryWriteDigit(ulong digit, Span<byte> buffer, out int bytesWritten)
        {
            Precondition.Require(digit < 10);
            return TryWriteDigitOrSymbol(digit, buffer, out bytesWritten);
        }

        public bool TryWriteSymbol(Symbol symbol, Span<byte> buffer, out int bytesWritten)
        {
            var symbolIndex = (ushort)symbol;
            return TryWriteDigitOrSymbol(symbolIndex, buffer, out bytesWritten);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryWriteDigitOrSymbol(ulong digitOrSymbolIndex, Span<byte> buffer, out int bytesWritten)
        {
            byte[] bytes = _digitsAndSymbols[digitOrSymbolIndex];
            bytesWritten = bytes.Length;
            if (bytesWritten > buffer.Length)
            {
                bytesWritten = 0;
                return false;
            }

            if (bytesWritten == 2)
            {
                buffer[0] = bytes[0];
                buffer[1] = bytes[1];
                return true;
            }

            if (bytesWritten == 1)
            {
                buffer[0] = bytes[0];
                return true;
            }

            buffer.Set(bytes);
            return true;
        }

        public enum Symbol : ushort
        {
            DecimalSeparator = 10,
            GroupSeparator = 11,
            InfinitySign = 12,
            MinusSign = 13,
            PlusSign = 14,          
            NaN = 15,
            Exponent = 16,
        }

        public bool IsInvariantUtf16
        {
            get { return _digitsAndSymbols == s_invariantUtf16._digitsAndSymbols; }
        }
        public bool IsInvariantUtf8
        {
            get { return _digitsAndSymbols == s_invariantUtf8._digitsAndSymbols; }
        }

        public bool IsUtf16
        {
            get { return _encoding == Encoding.Utf16; }
        }
        public bool IsUtf8
        {
            get { return _encoding == Encoding.Utf8; }
        }
    }
}
