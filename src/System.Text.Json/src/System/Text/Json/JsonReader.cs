// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Parsing;
using System.Text.Utf8;

namespace System.Text.Json
{
    public struct JsonReader : IDisposable
    {
        private readonly Utf8String _str;
        private int _index;

        private static readonly List<Utf8CodeUnit> EmptyString = new List<Utf8CodeUnit>
        {
            new Utf8CodeUnit((byte) ' '),
            new Utf8CodeUnit((byte) '\n'),
            new Utf8CodeUnit((byte) '\r'),
            new Utf8CodeUnit((byte) '\t')
        };

        private static readonly Utf8CodeUnit QuoteString = new Utf8CodeUnit((byte) '"');
        private static readonly Utf8CodeUnit ColonString = new Utf8CodeUnit((byte) ':');
        private static readonly Utf8CodeUnit CommaString = new Utf8CodeUnit((byte) ',');
        private static readonly Utf8CodeUnit SquareOpenString = new Utf8CodeUnit((byte) '[');
        private static readonly Utf8CodeUnit SquareCloseString = new Utf8CodeUnit((byte) ']');
        private static readonly Utf8CodeUnit CurlyOpenString = new Utf8CodeUnit((byte) '{');
        private static readonly Utf8CodeUnit CurlyCloseString = new Utf8CodeUnit((byte) '}');

        public JsonReader(Utf8String str)
        {
            _str = str;
            _index = 0;
        }

        public void Dispose()
        {
        }

        public Utf8String ReadObjectStart()
        {
            SkipAll();
            return ReadToByte(CurlyOpenString, true);
        }

        public Utf8String ReadObjectEnd()
        {
            SkipAll();
            return ReadToByte(CurlyCloseString, true);
        }

        public Utf8String ReadArrayStart()
        {
            SkipAll();
            return ReadToByte(SquareOpenString, true);
        }

        public Utf8String ReadArrayEnd()
        {
            SkipAll();
            return ReadToByte(SquareCloseString, true);
        }

        public Utf8String ReadProperty()
        {
            SkipAll();
            // TODO: Use _str.SubstringFrom(new Utf8String("\"")).Substring(1).SubstringTo(new Utf8String("\""));
            ReadToByte(QuoteString, true);
            return ReadToByte(QuoteString, false);
        }

        [CLSCompliant(false)]
        public uint ReadPropertyValueInt()
        {
            SkipAll();
            uint value;
            int bytesConsumed;
            var substr = _str.Substring(_index);
            if (!InvariantParser.TryParse(substr, out value, out bytesConsumed))
            {
                return 0;
            }
            _index += bytesConsumed;
            return value;
        }

        public Utf8String ReadPropertyValueString()
        {
            return ReadProperty();
        }

        public Utf8String ReadMember()
        {
            return ReadProperty();
        }

        public Utf8String ReadMemberValueString()
        {
            return ReadProperty();
        }

        [CLSCompliant(false)]
        public uint ReadMemberValueInt()
        {
            return ReadPropertyValueInt();
        }

        private Utf8String ReadToByte(Utf8CodeUnit codeUnit, bool includeCodeUnit)
        {
            SkipAll();

            var count = 1;
            while (_str[_index] != codeUnit)
            {
                count++;
                _index++;
            }

            if (!includeCodeUnit)
            {
                count--;
                _index--;
            }

            var utf8Bytes = new byte[count];
            for (var i = 0; i < count; i++)
            {
                utf8Bytes[i] = (byte) _str[_index - count + i + 1];
            }
            _index ++;

            if (!includeCodeUnit)
            {
                _index++;
            }

            return new Utf8String(utf8Bytes);
        }

        private void SkipAll()
        {
            SkipEmpty();
            SkipColon();
            SkipComma();
            SkipEmpty();
        }

        private void SkipEmpty()
        {
            var nextByte = _str[_index];
            while (EmptyString.Contains(nextByte))
            {
                _index++;
                nextByte = _str[_index];
            }
        }

        private void SkipColon()
        {
            SkipEmpty();
            var nextByte = _str[_index];
            while (nextByte == ColonString)
            {
                _index++;
                nextByte = _str[_index];
            }
        }

        private void SkipComma()
        {
            SkipEmpty();
            var nextByte = _str[_index];
            while (nextByte == CommaString)
            {
                _index++;
                nextByte = _str[_index];
            }
        }
    }
}
