// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Text.Parsing;
using System.Text.Utf8;

namespace System.Text.Json
{
    public struct JsonReader : IDisposable
    {
        private Utf8String _str;
        private int _index;
        private int _insideArray;
        private int _insideObject;
        private bool _insideNumber;
        public JsonTokenType TokenType;

        public enum JsonTokenType
        {
            Start,
            ObjectStart,
            ObjectEnd,
            ArrayStart,
            ArrayEnd,
            String,
            Colon,
            Comma,
            Value,
            True,
            False,
            Null,
            Number,
            Finish
        };

        private static readonly Utf8CodeUnit[] EmptyString =
        {
            new Utf8CodeUnit((byte) ' '),
            new Utf8CodeUnit((byte) '\n'),
            new Utf8CodeUnit((byte) '\r'),
            new Utf8CodeUnit((byte) '\t')
        };

        private static readonly Utf8CodeUnit[] StringInNumbers =
        {
            new Utf8CodeUnit((byte) '-'),
            new Utf8CodeUnit((byte) '+'),
            new Utf8CodeUnit((byte) 'e'),
            new Utf8CodeUnit((byte) 'E'),
            new Utf8CodeUnit((byte) '.')
        };

        private static readonly Utf8CodeUnit QuoteString = new Utf8CodeUnit((byte) '"');
        private static readonly Utf8CodeUnit ColonString = new Utf8CodeUnit((byte) ':');
        private static readonly Utf8CodeUnit CommaString = new Utf8CodeUnit((byte) ',');
        private static readonly Utf8CodeUnit SquareOpenString = new Utf8CodeUnit((byte) '[');
        private static readonly Utf8CodeUnit SquareCloseString = new Utf8CodeUnit((byte) ']');
        private static readonly Utf8CodeUnit CurlyOpenString = new Utf8CodeUnit((byte) '{');
        private static readonly Utf8CodeUnit CurlyCloseString = new Utf8CodeUnit((byte) '}');
        private static readonly Utf8CodeUnit DashString = new Utf8CodeUnit((byte) '-');
        private static readonly Utf8CodeUnit PeriodString = new Utf8CodeUnit((byte) '.');
        private static readonly Utf8CodeUnit ZeroString = new Utf8CodeUnit((byte) '0');
        private static readonly Utf8CodeUnit NineString = new Utf8CodeUnit((byte) '9');
        private static readonly Utf8CodeUnit StartTrueString = new Utf8CodeUnit((byte) 't');
        private static readonly Utf8CodeUnit StartFalseString = new Utf8CodeUnit((byte) 'f');
        private static readonly Utf8CodeUnit StartNullString = new Utf8CodeUnit((byte) 'n');

        private static readonly Utf8String TrueString = new Utf8String("true");
        private static readonly Utf8String FalseString = new Utf8String("false");
        private static readonly Utf8String NullString = new Utf8String("null");

        public JsonReader(Utf8String str)
        {
            _str = str;
            _index = -1;
            _insideArray = 0;
            _insideObject = 0;
            _insideNumber = false;
            TokenType = JsonTokenType.Start;
        }

        public void Dispose()
        {
        }

        public bool Read()
        {
            return _index < _str.Length;
        }

        public Utf8String ReadWhiteSpace()
        {
            return ReadNextByte();
        }

        public Utf8String ReadObjectStart()
        {
            return ReadNextByte();
        }

        public Utf8String ReadObjectEnd()
        {
            return ReadNextByte();
        }

        public Utf8String ReadArrayStart()
        {
            return ReadNextByte();
        }

        public Utf8String ReadArrayEnd()
        {
            return ReadNextByte();
        }

        public Utf8String ReadComma()
        {
            return ReadNextByte();
        }

        public Utf8String ReadColon()
        {
            return ReadNextByte();
        }

        public bool ReadStart()
        {
            MoveToNextTokenType();
            return true;
        }

        public bool ReadFinish()
        {
            MoveToNextTokenType();
            return true;
        }

        public bool ReadTrue()
        {
            if (!Read())
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");
            var utf8Bytes = new byte[4];
            for (var i = 0; i < utf8Bytes.Length; i++)
            {
                utf8Bytes[i] = (byte) _str[_index];
                _index++;
            }
            var utf8BytesString = new Utf8String(utf8Bytes);
            if (utf8BytesString != TrueString)
            {
                throw new FormatException("Invalid json, tried to read 'true'.");
            }
            MoveToNextTokenType();
            return true;
        }

        public bool ReadFalse()
        {
            if (!Read())
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");
            var utf8Bytes = new byte[4];
            for (var i = 0; i < utf8Bytes.Length; i++)
            {
                utf8Bytes[i] = (byte) _str[_index];
                _index++;
            }
            var utf8BytesString = new Utf8String(utf8Bytes);
            if (utf8BytesString != FalseString)
            {
                throw new FormatException("Invalid json, tried to read 'false'.");
            }
            MoveToNextTokenType();
            return true;
        }

        public object ReadNull()
        {
            if (!Read())
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");
            var utf8Bytes = new byte[4];
            for (var i = 0; i < utf8Bytes.Length; i++)
            {
                utf8Bytes[i] = (byte) _str[_index];
                _index++;
            }
            var utf8BytesString = new Utf8String(utf8Bytes);
            if (utf8BytesString != NullString)
            {
                throw new FormatException("Invalid json, tried to read 'null'.");
            }
            MoveToNextTokenType();
            return true;
        }

        public bool ReadValue()
        {
            MoveToNextTokenType();
            return true;
        }

        public Utf8String ReadString()
        {
            if (!Read())
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

            _index++;
            var count = 0;
            while (_str[_index] != QuoteString)
            {
                count++;
                _index++;
            }

            var utf8Bytes = new byte[count];
            for (var i = 0; i < count; i++)
            {
                utf8Bytes[i] = (byte) _str[_index - count + i];
            }

            MoveToNextTokenType();

            return count == 0 ? new Utf8String("") : new Utf8String(utf8Bytes);
        }

        public double ReadNumber()
        {
            if (!Read())
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

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
                throw new FormatException("Invalid json, tried to read a number.");
            }
            _index += bytesConsumed;

            var wholeNumberPart = result;
            double decimalNumberPart = 0;

            var isDecimal = _str[_index] == PeriodString;

            if (isDecimal)
            {
                _index++;
                substr = _str.Substring(_index);
                if (!InvariantParser.TryParse(substr, out result, out bytesConsumed))
                {
                    throw new FormatException("Invalid json, tried to read a number.");
                }
                _index += bytesConsumed;

                decimalNumberPart = result;
            }

            var number = wholeNumberPart +
                         decimalNumberPart/decimalNumberPart.ToString(CultureInfo.InvariantCulture).Length;

            _index--;
            MoveToNextTokenType();

            return isNegative ? number*-1 : number;
        }

        private Utf8String ReadNextByte()
        {
            if (!Read())
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");
            var nextByte = _str[_index];
            MoveToNextTokenType();
            var utf8Bytes = new byte[1];
            utf8Bytes[0] = (byte) nextByte;
            return new Utf8String(utf8Bytes);
        }

        private void SkipEmpty()
        {
            if (!Read())
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");
            var nextByte = _str[_index];
            while (Array.IndexOf(EmptyString, nextByte) >= 0)
            {
                _index++;
                nextByte = _str[_index];
            }
        }

        private JsonTokenType GetNextTokenType()
        {
            if (!Read() || TokenType == JsonTokenType.Finish)
                return JsonTokenType.Finish;

            SkipEmpty();

            var nextByte = _str[_index];

            if (nextByte == CurlyOpenString)
            {
                _insideObject++;
                return JsonTokenType.ObjectStart;
            }

            if (nextByte == CurlyCloseString)
            {
                _insideObject--;
                return JsonTokenType.ObjectEnd;
            }

            if (nextByte == SquareOpenString)
            {
                _insideArray++;
                return JsonTokenType.ArrayStart;
            }

            if (nextByte == SquareCloseString)
            {
                _insideArray--;
                return JsonTokenType.ArrayEnd;
            }

            if (nextByte == ColonString)
            {
                return JsonTokenType.Colon;
            }

            if (nextByte == CommaString)
            {
                return JsonTokenType.Comma;
            }

            if (TokenType == JsonTokenType.ObjectStart || TokenType == JsonTokenType.ObjectEnd)
            {
                return JsonTokenType.String;
            }

            if (TokenType == JsonTokenType.Comma && _insideObject > 0)
            {
                return _insideObject > _insideArray ? JsonTokenType.String : JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.ArrayStart)
            {
                return JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.Colon)
            {
                return JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.Comma && _insideArray > 0)
            {
                return _insideObject > _insideArray ? JsonTokenType.String : JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.Value && nextByte == QuoteString)
            {
                return JsonTokenType.String;
            }

            if (TokenType == JsonTokenType.Value &&
                (nextByte == DashString || nextByte.Value >= ZeroString.Value || nextByte.Value <= NineString.Value))
            {
                _insideNumber = true;
                return JsonTokenType.Number;
            }

            if (TokenType == JsonTokenType.Number && _insideNumber &&
                (Array.IndexOf(StringInNumbers, nextByte) >= 0 || nextByte.Value >= ZeroString.Value ||
                 nextByte.Value <= NineString.Value))
            {
                return JsonTokenType.Number;
            }

            if (TokenType == JsonTokenType.Number && _insideNumber &&
                !(Array.IndexOf(StringInNumbers, nextByte) >= 0 || nextByte.Value >= ZeroString.Value ||
                  nextByte.Value <= NineString.Value))
            {
                _insideNumber = false;
                return JsonTokenType.Number;
            }

            if (TokenType == JsonTokenType.Value && (nextByte == StartTrueString))
            {
                return JsonTokenType.True;
            }

            if (TokenType == JsonTokenType.Value && (nextByte == StartFalseString))
            {
                return JsonTokenType.False;
            }

            if (TokenType == JsonTokenType.Value && (nextByte == StartNullString))
            {
                return JsonTokenType.Null;
            }

            if (TokenType == JsonTokenType.String && nextByte == QuoteString)
            {
                return JsonTokenType.Colon;
            }

            throw new FormatException("Unable to get next token type. Check json format.");
        }

        private void MoveToNextTokenType()
        {
            if (TokenType != JsonTokenType.Value) _index++;
            TokenType = GetNextTokenType();
        }
    }
}
