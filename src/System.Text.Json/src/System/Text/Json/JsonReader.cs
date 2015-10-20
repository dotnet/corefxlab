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
        private int _insideObject;
        private int _insideArray;
        public JsonTokenType TokenType;

        public enum JsonTokenType
        {
            Start,
            ObjectStart,
            ObjectEnd,
            ArrayStart,
            ArrayEnd,
            Pair,
            Value,
            Finish
        };

        public enum ValueType
        {
            String,
            Number,
            Object,
            Array,
            True,
            False,
            Null
        }

        private static readonly Utf8CodeUnit[] EmptyString =
        {
            new Utf8CodeUnit((byte) ' '),
            new Utf8CodeUnit((byte) '\n'),
            new Utf8CodeUnit((byte) '\r'),
            new Utf8CodeUnit((byte) '\t')
        };

        private static readonly Utf8CodeUnit QuoteString = new Utf8CodeUnit((byte) '"');
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
            _index = 0;
            _insideObject = 0;
            _insideArray = 0;
            TokenType = JsonTokenType.Start;
        }

        public JsonReader(string str)
        {
            _str = new Utf8String(str);
            _index = 0;
            _insideObject = 0;
            _insideArray = 0;
            TokenType = JsonTokenType.Start;
        }

        public void Dispose()
        {
        }

        public bool Read()
        {
            var canRead = _index < _str.Length;
            if (canRead) MoveToNextTokenType();
            return canRead;
        }

        public Utf8String ReadString()
        {
            SkipEmpty();
            var str = ReadStringValue();
            _index++;
            return str;
        }

        public ValueType GetValueType()
        {
            SkipEmpty();
            var nextByte = _str[_index];

            if (nextByte == CurlyOpenString)
            {
                return ValueType.Object;
            }

            if (nextByte == SquareOpenString)
            {
                return ValueType.Array;
            }

            if (nextByte == DashString || (nextByte.Value >= ZeroString.Value && nextByte.Value <= NineString.Value))
            {
                return ValueType.Number;
            }

            if (nextByte == StartTrueString)
            {
                return ValueType.True;
            }

            if (nextByte == StartFalseString)
            {
                return ValueType.False;
            }

            if (nextByte == StartNullString)
            {
                return ValueType.Null;
            }

            if (nextByte == QuoteString)
            {
                return ValueType.String;
            }

            throw new ArgumentOutOfRangeException();
        }

        public object ReadValue()
        {
            var type = GetValueType();
            SkipEmpty();
            switch (type)
            {
                case ValueType.String:
                    return ReadStringValue();
                case ValueType.Number:
                    return ReadNumberValue();
                case ValueType.True:
                    return ReadTrueValue();
                case ValueType.False:
                    return ReadFalseValue();
                case ValueType.Null:
                    return ReadNullValue();
                case ValueType.Object:
                    return null;
                case ValueType.Array:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Utf8String ReadStringValue()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");
            _index++;

            Utf8String resultString;
            if (!_str.Substring(_index).TrySubstringTo(QuoteString, out resultString))
            {
                return new Utf8String("");
            }

            _index += resultString.Length + 1;

            SkipEmpty();
            return resultString.Length == 0 ? new Utf8String("") : resultString;
        }

        private double ReadNumberValue()
        {
            if (_index >= _str.Length)
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

            int zeroesSkipped;

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

            var lengthOfNumber = decimalNumberPart.ToString(CultureInfo.InvariantCulture).Length;
            zeroesSkipped = bytesConsumed - lengthOfNumber;
            var divider = Math.Pow(10, lengthOfNumber + zeroesSkipped);

            var number = wholeNumberPart + decimalNumberPart/divider;

            SkipEmpty();
            return isNegative ? number*-1 : number;
        }

        private bool ReadTrueValue()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

            if (_str.Substring(_index, TrueString.Length) != TrueString)
            {
                throw new FormatException("Invalid json, tried to read 'true'.");
            }

            _index += TrueString.Length;

            SkipEmpty();
            return true;
        }

        private bool ReadFalseValue()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

            if (_str.Substring(_index, FalseString.Length) != FalseString)
            {
                throw new FormatException("Invalid json, tried to read 'false'.");
            }

            _index += FalseString.Length;

            SkipEmpty();
            return false;
        }

        private object ReadNullValue()
        {
            if (_index >= _str.Length)
                throw new IndexOutOfRangeException("Json length is " + _str.Length + " and reading index " + _index +
                                                   ".");

            if (_str.Substring(_index, NullString.Length) != NullString)
            {
                throw new FormatException("Invalid json, tried to read 'null'.");
            }

            _index += NullString.Length;

            SkipEmpty();
            return null;
        }

        private void SkipEmpty()
        {
            if (_index >= _str.Length)
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
            if (TokenType == JsonTokenType.Finish)
                return JsonTokenType.Finish;

            if (_index >= _str.Length)
            {
                return JsonTokenType.Finish;
            }

            SkipEmpty();

            var nextByte = _str[_index];

            if (TokenType == JsonTokenType.ArrayStart && nextByte != SquareCloseString)
            {
                return JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.ObjectStart && nextByte != CurlyCloseString)
            {
                return JsonTokenType.Pair;
            }

            if (nextByte == CurlyOpenString)
            {
                _index++;
                _insideObject++;
                return JsonTokenType.ObjectStart;
            }

            if (nextByte == CurlyCloseString)
            {
                _index++;
                _insideObject--;
                return JsonTokenType.ObjectEnd;
            }

            if (nextByte == SquareOpenString)
            {
                _index++;
                _insideArray++;
                return JsonTokenType.ArrayStart;
            }

            if (nextByte == SquareCloseString)
            {
                _index++;
                _insideArray--;
                return JsonTokenType.ArrayEnd;
            }

            if (TokenType == JsonTokenType.Pair && nextByte == CommaString)
            {
                _index++;
                return JsonTokenType.Pair;
            }

            if (TokenType == JsonTokenType.Value && nextByte == CommaString)
            {
                _index++;
                return JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.ArrayEnd && nextByte == CommaString)
            {
                _index++;
                return _insideObject > _insideArray ? JsonTokenType.Pair : JsonTokenType.Value;
            }

            if (TokenType == JsonTokenType.ObjectEnd && nextByte == CommaString)
            {
                _index++;
                return _insideObject > _insideArray ? JsonTokenType.Pair : JsonTokenType.Value;
            }

            throw new FormatException("Unable to get next token type. Check json format.");
        }

        private void MoveToNextTokenType()
        {
            TokenType = GetNextTokenType();
        }
    }
}