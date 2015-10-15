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
        public JsonTokenType TokenType;
        private bool _insideArray;

        public enum JsonTokenType
        {
            ObjectStart,
            ObjectEnd,
            ArrayStart,
            ArrayEnd,
            PropertyName,
            PropertyValueAsString,
            PropertyValueAsInt
        };

        private static readonly Utf8CodeUnit[] EmptyString =
        {
            new Utf8CodeUnit((byte) ' '),
            new Utf8CodeUnit((byte) '\n'),
            new Utf8CodeUnit((byte) '\r'),
            new Utf8CodeUnit((byte) '\t')
        };

        private readonly Dictionary<Utf8CodeUnit, JsonTokenType> _mapping;

        private static readonly Utf8CodeUnit QuoteString = new Utf8CodeUnit((byte) '"');
        private static readonly Utf8CodeUnit ColonString = new Utf8CodeUnit((byte) ':');
        private static readonly Utf8CodeUnit CommaString = new Utf8CodeUnit((byte) ',');
        private static readonly Utf8CodeUnit SquareOpenString = new Utf8CodeUnit((byte) '[');
        private static readonly Utf8CodeUnit SquareCloseString = new Utf8CodeUnit((byte) ']');
        private static readonly Utf8CodeUnit CurlyOpenString = new Utf8CodeUnit((byte) '{');
        private static readonly Utf8CodeUnit CurlyCloseString = new Utf8CodeUnit((byte) '}');
        private static readonly Utf8CodeUnit DashString = new Utf8CodeUnit((byte) '-');

        public JsonReader(Utf8String str)
        {
            _str = str;
            _index = 0;
            TokenType = JsonTokenType.ObjectStart;
            _insideArray = false;

            _mapping = new Dictionary<Utf8CodeUnit, JsonTokenType>
            {
                {CurlyOpenString, JsonTokenType.ObjectStart},
                {CurlyCloseString, JsonTokenType.ObjectEnd},
                {SquareOpenString, JsonTokenType.ArrayStart},
                {SquareCloseString, JsonTokenType.ArrayEnd},
                {QuoteString, JsonTokenType.PropertyName}
            };
        }

        public void Dispose()
        {
        }

        public bool Read()
        {
            return _index < _str.Length;
        }

        public Utf8String ReadObjectStart()
        {
            SkipAll();
            var result = ReadToByte(CurlyOpenString, true);
            GetNextTokenType();
            return result;
        }

        public Utf8String ReadObjectEnd()
        {
            SkipAll();
            var result = ReadToByte(CurlyCloseString, true);
            GetNextTokenType();
            return result;
        }

        public Utf8String ReadArrayStart()
        {
            SkipAll();
            var result = ReadToByte(SquareOpenString, true);
            GetNextTokenType();
            return result;
        }

        public Utf8String ReadArrayEnd()
        {
            SkipAll();
            var result = ReadToByte(SquareCloseString, true);
            GetNextTokenType();
            return result;
        }

        public Utf8String ReadProperty()
        {
            SkipAll();
            // TODO: Use _str.SubstringFrom(new Utf8String("\"")).Substring(1).SubstringTo(new Utf8String("\""));
            ReadToByte(QuoteString, true);
            var result = ReadToByte(QuoteString, false);
            GetNextTokenType();
            return result;
        }

        public long ReadPropertyValueAsInt()
        {
            SkipAll();
            var isNegative = _str[_index] == DashString;
            if (isNegative)
            {
                _index++;
            }

            int bytesConsumed;
            var substr = _str.Substring(_index);

            ulong result;
            if (!InvariantParser.TryParse(substr, out result, out bytesConsumed))
            {
                return 0;
            }
            _index += bytesConsumed;

            GetNextTokenType();
            return isNegative ? Convert.ToInt64(result)*-1 : Convert.ToInt64(result);
        }

        public Utf8String ReadPropertyAsString()
        {
            return ReadProperty();
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
            _index++;

            if (!includeCodeUnit)
            {
                _index++;
            }

            return new Utf8String(utf8Bytes);
        }

        private void SkipAll()
        {
            var nextByte = _str[_index];
            while (Array.IndexOf(EmptyString, nextByte) >= 0 || nextByte == CommaString)
            {
                _index++;
                nextByte = _str[_index];
            }
        }

        private void SkipEmpty()
        {
            var nextByte = _str[_index];
            while (Array.IndexOf(EmptyString, nextByte) >= 0)
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

        private void GetNextTokenType()
        {
            if (!Read()) return;
            SkipAll();
            if (_str[_index] == ColonString)
            {
                _index++;
                SkipAll();
                if (!_mapping.TryGetValue(_str[_index], out TokenType))
                {
                    TokenType = JsonTokenType.PropertyValueAsInt;
                }
                if (_str[_index] == QuoteString)
                {
                    TokenType = JsonTokenType.PropertyValueAsString;
                }
            }
            else
            {
                var nextToken = _str[_index];
                var prevTokenType = TokenType;
                if (!_mapping.TryGetValue(nextToken, out TokenType))
                {
                    TokenType = JsonTokenType.PropertyValueAsInt;
                }
                if ((_insideArray || prevTokenType == JsonTokenType.ArrayStart) &&
                    TokenType == JsonTokenType.PropertyName)
                {
                    TokenType = JsonTokenType.PropertyValueAsString;
                }
            }

            if (TokenType == JsonTokenType.ArrayStart)
            {
                _insideArray = true;
            }
            else if (TokenType == JsonTokenType.ArrayEnd)
            {
                _insideArray = false;
            }
        }
    }
}
