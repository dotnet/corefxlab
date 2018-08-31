// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers.Text
{
    public static partial class Utf16Parser
    {
        public static bool TryParseGuid(ReadOnlySpan<char> text, out Guid value, out int charactersConsumed, StandardFormat format = default)
        {
            if (format.IsDefault) format = 'D';
            switch (format.Symbol)
            {
                case 'D': return TryParseGuidCore(text, false, ' ', ' ', out value, out charactersConsumed);
                case 'B': return TryParseGuidCore(text, true, '{', '}', out value, out charactersConsumed);
                case 'P': return TryParseGuidCore(text, true, '(', ')', out value, out charactersConsumed);
                case 'N': return TryParseGuidN(text, out value, out charactersConsumed);
                default: throw new FormatException();
            }
        }

        private static bool TryParseGuidN(ReadOnlySpan<char> text, out Guid value, out int charactersConsumed)
        {
            charactersConsumed = 0;
            if (text.Length < 32) return false;

            if (!Hex.TryParseUInt32(text.Slice(0, 8), out uint i1, out int justConsumed) || justConsumed != 8) return false; // 8 digits
            if (!Hex.TryParseUInt16(text.Slice(8, 4), out ushort i2, out justConsumed) || justConsumed != 4) return false; // next 4 digfits
            if (!Hex.TryParseUInt16(text.Slice(12, 4), out ushort i3, out justConsumed) || justConsumed != 4) return false; // next 4 digits
            if (!Hex.TryParseUInt16(text.Slice(16, 4), out ushort i4, out justConsumed) || justConsumed != 4) return false; // next 4 digits
            if (!Hex.TryParseUInt64(text.Slice(20), out ulong i5, out justConsumed) || justConsumed != 12) return false; // next 12 digits

            charactersConsumed = 32;
            value = new Guid((int)i1, (short)i2, (short)i3, (byte)(i4 >> 8), (byte)i4,
                (byte)(i5 >> 40), (byte)(i5 >> 32), (byte)(i5 >> 24), (byte)(i5 >> 16), (byte)(i5 >> 8), (byte)i5);
            return true;
        }

        // {8-4-4-4-12}, where number is the number of hex digits, and {/} are ends.
        private static bool TryParseGuidCore(ReadOnlySpan<char> text, bool ends, char begin, char end, out Guid value, out int charactersConsumed)
        {
            charactersConsumed = 0;
            var expectedCodingUnits = 36 + (ends ? 2 : 0); // 32 hex digits + 4 delimiters + 2 optional ends

            if (text.Length < expectedCodingUnits) return false;

            if (ends)
            {
                if (text[0] != begin) return false;
                text = text.Slice(1); // skip begining
            }

            if (!Hex.TryParseUInt32(text, out uint i1, out int justConsumed)) return false;
            if (justConsumed != 8) return false; // 8 digits
            if (text[justConsumed] != '-') return false;
            text = text.Slice(9); // justConsumed + 1 for delimiter

            if (!Hex.TryParseUInt16(text, out ushort i2, out justConsumed)) return false;
            if (justConsumed != 4) return false; // 4 digits
            if (text[justConsumed] != '-') return false;
            text = text.Slice(5); // justConsumed + 1 for delimiter

            if (!Hex.TryParseUInt16(text, out ushort i3, out justConsumed)) return false;
            if (justConsumed != 4) return false; // 4 digits
            if (text[justConsumed] != '-') return false;
            text = text.Slice(5); // justConsumed + 1 for delimiter

            if (!Hex.TryParseUInt16(text, out ushort i4, out justConsumed)) return false;
            if (justConsumed != 4) return false; // 4 digits
            if (text[justConsumed] != '-') return false;
            text = text.Slice(5);// justConsumed + 1 for delimiter

            if (!Hex.TryParseUInt64(text, out ulong i5, out justConsumed)) return false;
            if (justConsumed != 12) return false; // 12 digits

            if (ends && text[justConsumed] != end) return false;

            charactersConsumed = expectedCodingUnits;
            value = new Guid((int)i1, (short)i2, (short)i3, (byte)(i4 >> 8), (byte)i4,
                (byte)(i5 >> 40), (byte)(i5 >> 32), (byte)(i5 >> 24), (byte)(i5 >> 16), (byte)(i5 >> 8), (byte)i5);

            return true;
        }
    }  
}
