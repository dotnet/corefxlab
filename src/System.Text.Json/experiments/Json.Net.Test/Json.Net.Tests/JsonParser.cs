using System;

namespace Json.Net.Tests
{
    public struct JsonParser : IDisposable
    {
        private byte[] _buffer;
        private int _index;
        private int _end;
        private int _dbIndex;
        private int _insideObject;
        private int _insideArray;
        public JsonTokenType TokenType;
        private bool _jsonStartIsObject;
        
        private const int RowSize = 9;  // Do not change, unless you also change FindLocation

        public enum JsonTokenType
        {
            // Start = 0 state reserved for internal use
            ObjectStart = 1,
            ObjectEnd = 2,
            ArrayStart = 3,
            ArrayEnd = 4,
            Property = 5,
            Value = 6
        };

        public enum JsonValueType
        {
            String,
            Number,
            Object,
            Array,
            True,
            False,
            Null
        }

        public JsonParser(byte[] buffer, int lengthOfJson)
        {
            _buffer = buffer;
            _insideObject = 0;
            _insideArray = 0;
            TokenType = 0;
            _index = 0;
            _end = lengthOfJson;

            var nextByte = _buffer[_index];
            while (isWhiteSpace(nextByte) || nextByte == 0)
            {
                _index++;
                nextByte = _buffer[_index];
            }

            _dbIndex = _end + 1;

            _jsonStartIsObject = _buffer[_index] == '{';
        }

        public void Dispose()
        {
        }

        public JsonParseObject Parse()
        {
            int numValues = 0;
            int numPairs = 0;
            int numObj = 0;
            int numArr = 0;

            int topOfStackObj = _buffer.Length - 1;
            int topOfStackArr = _buffer.Length - 1;

            while (Read())
            {
                var tokenType = TokenType;
                switch (tokenType)
                {
                    case JsonTokenType.ObjectStart:
                        CopyNumber(_index);
                        CopyNumber(-1);
                        CopyByte((byte)JsonValueType.Object);
                        PushOnObjectStack(numPairs, topOfStackObj);
                        topOfStackObj -= 8;
                        numPairs = 0;
                        numObj++;
                        break;
                    case JsonTokenType.ObjectEnd:
                        CopyNumberAtLocation(numPairs, FindLocation(numObj - 1, true));
                        numObj--;
                        numPairs += PopFromObjectStack(topOfStackObj);
                        topOfStackObj += 8;
                        break;
                    case JsonTokenType.ArrayStart:
                        CopyNumber(_index);
                        CopyNumber(-1);
                        CopyByte((byte)JsonValueType.Array);
                        PushOnArrayStack(numValues, topOfStackArr);
                        topOfStackArr -= 8;
                        numValues = 0;
                        numArr++;
                        break;
                    case JsonTokenType.ArrayEnd:
                        CopyNumberAtLocation(numValues, FindLocation(numArr - 1, false));
                        numArr--;
                        numValues += PopFromArrayStack(topOfStackArr);
                        topOfStackArr += 8;
                        break;
                    case JsonTokenType.Property:
                        GetName();
                        numPairs++;
                        GetValue();
                        numPairs++;
                        numValues++;
                        numValues++;
                        break;
                    case JsonTokenType.Value:
                        GetValue();
                        numValues++;
                        numPairs++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            /*for(int i = _end+1; i < _dbIndex; i+=RowSize)
            {
                Console.Write(i + ":");
                Console.Write(BitConverter.ToInt32(_buffer, i) + ":");
                Console.Write(i + 4 + ":");
                Console.Write(BitConverter.ToInt32(_buffer, i + 4) + ":");
                Console.Write(i + 8 + ":");
                Console.WriteLine(_buffer[i + 8]);
            }*/

            /*for (int i = _buffer.Length - 1 - 4; i > _buffer.Length - 1 - 48; i-=4)
            {
                Console.WriteLine(BitConverter.ToInt32(_buffer, i));
            }*/

            return new JsonParseObject(_buffer, _end + 1, _dbIndex);
        }

        private void PushOnObjectStack(int val, int topOfStack)
        {
            CopyNumberAtLocation(val, topOfStack - 8);
        }
        private int PopFromObjectStack(int topOfStack)
        {
            return GetIntFrom(topOfStack);
        }

        private void PushOnArrayStack(int val, int topOfStack)
        {
            CopyNumberAtLocation(val, topOfStack - 4);
        }
        private int PopFromArrayStack(int topOfStack)
        {
            return GetIntFrom(topOfStack + 4);
        }

        private int FindLocation(int index, bool lookingForObject)
        {
            int startRow = _end + 1;
            int rowCounter = 0;
            int numFound = 0;

            while (true)
            {
                int numberOfRows = (rowCounter << 3) + rowCounter; // multiply by RowSize which is 9
                int locationStart = startRow + numberOfRows;
                int locationOfTypeCode = locationStart + 8;
                int locationOfLength = locationStart + 4;
                var typeCode = _buffer[locationOfTypeCode];
                var length = GetIntFrom(locationOfLength);

                if (length == -1 && (lookingForObject ? typeCode == (byte)JsonValueType.Object : typeCode == (byte)JsonValueType.Array))
                {
                    numFound++;
                }

                if (index == numFound - 1)
                {
                    return locationOfLength;
                }
                else
                {
                    if (length > 0 && (typeCode == (byte)JsonValueType.Object || typeCode == (byte)JsonValueType.Array))
                    {
                        rowCounter += length;
                    }
                    rowCounter++;
                }
            }
        }

        private int GetIntFrom(int loc)
        {
            return BitConverter.ToInt32(_buffer, loc);
        }

        private bool Read()
        {
            var canRead = _index < _end;
            if (canRead) MoveToNextTokenType();
            return canRead;
        }

        private void GetName()
        {
            SkipEmpty();
            ReadStringValue();
            _index++;
        }

        private JsonValueType GetJsonValueType()
        {
            var nextByte = _buffer[_index];

            while (isWhiteSpace(nextByte))
            {
                _index++;
                nextByte = _buffer[_index];
            }

            if (nextByte == '"')
            {
                return JsonValueType.String;
            }

            if (nextByte == '{')
            {
                return JsonValueType.Object;
            }

            if (nextByte == '[')
            {
                return JsonValueType.Array;
            }

            if (nextByte == 't')
            {
                return JsonValueType.True;
            }

            if (nextByte == 'f')
            {
                return JsonValueType.False;
            }

            if (nextByte == 'n')
            {
                return JsonValueType.Null;
            }

            if (nextByte == '-' || (nextByte >= '0' && nextByte <= '9'))
            {
                return JsonValueType.Number;
            }

            throw new FormatException("Invalid json, tried to read char '" + nextByte + "'.");
        }

        private void GetValue()
        {
            var type = GetJsonValueType();
            SkipEmpty();
            switch (type)
            {
                case JsonValueType.String:
                    ReadStringValue();
                    return;
                case JsonValueType.Number:
                    ReadNumberValue();
                    return;
                case JsonValueType.True:
                    ReadTrueValue();
                    return;
                case JsonValueType.False:
                    ReadFalseValue();
                    return;
                case JsonValueType.Null:
                    ReadNullValue();
                    return;
                case JsonValueType.Object:
                case JsonValueType.Array:
                    return;
                default:
                    throw new ArgumentException("Invalid json value type '" + type + "'.");
            }
        }

        private void ReadStringValue()
        {
            _index++;
            var count = _index;
            do
            {
                while (_buffer[count] != '"')
                {
                    count++;
                }
                count++;
            } while (AreNumOfBackSlashesAtEndOfStringOdd(count - 2));

            var strLength = count - _index;

            CopyData(_index, strLength - 1);

            _index += strLength;

            SkipEmpty();
        }

        private void CopyData(int startingIndex, int length)
        {
            CopyNumber(startingIndex);
            CopyNumber(length);
            _dbIndex += 1;
        }

        private void CopyNumber(int num)
        {
            _buffer[_dbIndex] = (byte)num;
            _buffer[_dbIndex + 1] = (byte)(num >> 8);
            _buffer[_dbIndex + 2] = (byte)(num >> 16);
            _buffer[_dbIndex + 3] = (byte)(num >> 24);
            _dbIndex += 4;
        }

        private void CopyByte(byte num)
        {
            _buffer[_dbIndex] = num;
            _dbIndex += 1;
        }

        private void CopyNumberAtLocation(int num, int loc)
        {
            _buffer[loc] = (byte)num;
            _buffer[loc + 1] = (byte)(num >> 8);
            _buffer[loc + 2] = (byte)(num >> 16);
            _buffer[loc + 3] = (byte)(num >> 24);
        }

        private bool AreNumOfBackSlashesAtEndOfStringOdd(int count)
        {
            var length = count - _index;
            if (length < 0) return false;
            var nextByte = _buffer[count];
            if (nextByte != '\\') return false;
            var numOfBackSlashes = 0;
            while (nextByte == '\\')
            {
                numOfBackSlashes++;
                if ((length - numOfBackSlashes) < 0) return numOfBackSlashes % 2 != 0;
                nextByte = _buffer[count - numOfBackSlashes];
            }
            return numOfBackSlashes % 2 != 0;
        }

        private void ReadNumberValue(bool copyData = false)
        {
            var count = _index;

            var nextByte = _buffer[count];
            if (nextByte == '-')
            {
                count++;
            }

            nextByte = _buffer[count];
            while (nextByte >= '0' && nextByte <= '9')
            {
                count++;
                nextByte = _buffer[count];
            }

            if (nextByte == '.')
            {
                count++;
            }

            nextByte = _buffer[count];
            while (nextByte >= '0' && nextByte <= '9')
            {
                count++;
                nextByte = _buffer[count];
            }

            if (nextByte == 'e' || nextByte == 'E')
            {
                count++;
                nextByte = _buffer[count];
                if (nextByte == '-' || nextByte == '+')
                {
                    count++;
                }
                nextByte = _buffer[count];
                while (nextByte >= '0' && nextByte <= '9')
                {
                    count++;
                    nextByte = _buffer[count];
                }
            }

            var length = count - _index;
            CopyData(_index, count - _index);

            _index += length;
            SkipEmpty();
        }

        private void ReadTrueValue(bool copyData = false)
        {
            CopyData(_index, 4);

            if (_buffer[_index + 1] != 'r' || _buffer[_index + 2] != 'u' || _buffer[_index + 3] != 'e')
            {
                throw new FormatException("Invalid json, tried to read 'true'.");
            }

            _index += 4;

            SkipEmpty();
        }

        private void ReadFalseValue(bool copyData = false)
        {
            CopyData(_index, 5);

            if (_buffer[_index + 1] != 'a' || _buffer[_index + 2] != 'l' || _buffer[_index + 3] != 's' || _buffer[_index + 4] != 'e')
            {
                throw new FormatException("Invalid json, tried to read 'false'.");
            }

            _index += 5;

            SkipEmpty();
        }

        private void ReadNullValue(bool copyData = false)
        {
            CopyData(_index, 4);

            if (_buffer[_index + 1] != 'u' || _buffer[_index + 2] != 'l' || _buffer[_index + 3] != 'l')
            {
                throw new FormatException("Invalid json, tried to read 'null'.");
            }

            _index += 4;

            SkipEmpty();
        }

        private void SkipEmpty()
        {
            var nextByte = _buffer[_index];

            while (isWhiteSpace(nextByte))
            {
                _index++;
                nextByte = _buffer[_index];
            }
        }

        private static bool isWhiteSpace(byte nextByte)
        {
            return nextByte == ' ' || nextByte == '\n' || nextByte == '\r' || nextByte == '\t';
        }

        private void MoveToNextTokenType()
        {
            var nextByte = _buffer[_index];
            while (isWhiteSpace(nextByte))
            {
                _index++;
                nextByte = _buffer[_index];
            }

            switch (TokenType)
            {
                case JsonTokenType.ObjectStart:
                    if (nextByte != '}')
                    {
                        TokenType = JsonTokenType.Property;
                        return;
                    }
                    break;
                case JsonTokenType.ObjectEnd:
                    if (nextByte == ',')
                    {
                        _index++;
                        if (_insideObject == _insideArray)
                        {
                            TokenType = !_jsonStartIsObject ? JsonTokenType.Property : JsonTokenType.Value;
                            return;
                        }
                        TokenType = _insideObject > _insideArray ? JsonTokenType.Property : JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.ArrayStart:
                    if (nextByte != ']')
                    {
                        TokenType = JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.ArrayEnd:
                    if (nextByte == ',')
                    {
                        _index++;
                        if (_insideObject == _insideArray)
                        {
                            TokenType = !_jsonStartIsObject ? JsonTokenType.Property : JsonTokenType.Value;
                            return;
                        }
                        TokenType = _insideObject > _insideArray ? JsonTokenType.Property : JsonTokenType.Value;
                        return;
                    }
                    break;
                case JsonTokenType.Property:
                    if (nextByte == ',')
                    {
                        _index++;
                        return;
                    }
                    break;
                case JsonTokenType.Value:
                    if (nextByte == ',')
                    {
                        _index++;
                        return;
                    }
                    break;
            }

            _index++;
            switch (nextByte)
            {
                case (byte)'{':
                    _insideObject++;
                    TokenType = JsonTokenType.ObjectStart;
                    return;
                case (byte)'}':
                    _insideObject--;
                    TokenType = JsonTokenType.ObjectEnd;
                    return;
                case (byte)'[':
                    _insideArray++;
                    TokenType = JsonTokenType.ArrayStart;
                    return;
                case (byte)']':
                    _insideArray--;
                    TokenType = JsonTokenType.ArrayEnd;
                    return;
                default:
                    throw new FormatException("Unable to get next token type. Check json format.");
            }
        }

    }
}
