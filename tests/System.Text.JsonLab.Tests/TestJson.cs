// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Text.JsonLab.Tests
{
    public class TestDom
    {
        public Object Object { get; set; }

        public Array Array { get; set; }

        public Value Value { get; set; }

        public TestDom this[ReadOnlyMemory<byte> index]
        {
            get
            {
                if (Object == null) throw new NullReferenceException();
                if (Object.Pairs == null) throw new NullReferenceException();

                var json = new TestDom();
                foreach (var pair in Object.Pairs)
                {
                    if (pair.Name.Span.SequenceEqual(index.Span))
                    {
                        switch (pair.Value.Type)
                        {
                            case Value.ValueType.Object:
                                json.Object = pair.Value.ObjectValue;
                                break;
                            case Value.ValueType.Array:
                                json.Array = pair.Value.ArrayValue;
                                break;
                            case Value.ValueType.String:
                            case Value.ValueType.Number:
                            case Value.ValueType.False:
                            case Value.ValueType.True:
                            case Value.ValueType.Null:
                                json.Value = pair.Value;
                                break;
                            default:
                                break;
                        }

                        return json;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        public TestDom this[int index]
        {
            get
            {
                if (Array == null) throw new NullReferenceException();
                if (Array.Values == null) throw new NullReferenceException();
                List<Value> values = Array.Values;
                if (index < 0 || index >= values.Count) throw new IndexOutOfRangeException();
                Value value = values[index];

                var json = new TestDom();
                switch (value.Type)
                {
                    case Value.ValueType.Object:
                        json.Object = value.ObjectValue;
                        break;
                    case Value.ValueType.Array:
                        json.Array = value.ArrayValue;
                        break;
                    case Value.ValueType.String:
                    case Value.ValueType.Number:
                    case Value.ValueType.False:
                    case Value.ValueType.True:
                    case Value.ValueType.Null:
                        json.Value = value;
                        break;
                    default:
                        break;
                }

                return json;
            }
        }

        public static explicit operator string (TestDom json)
        {
            if (json == null || json.Value == null) throw new NullReferenceException();

            if (json.Value.Type == Value.ValueType.String)
            {
                return Encoding.UTF8.GetString(json.Value.StringValue.Span);
            }
            else if (json.Value.Type == Value.ValueType.Null)
            {
                return json.Value.NullValue.ToString();
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public static explicit operator double (TestDom json)
        {
            if (json == null || json.Value == null) throw new NullReferenceException();

            if (json.Value.Type == Value.ValueType.Number)
            {
                return json.Value.NumberValue;

            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public static explicit operator bool (TestDom json)
        {
            if (json == null || json.Value == null) throw new NullReferenceException();

            if (json.Value.Type == Value.ValueType.True)
            {
                return json.Value.TrueValue;
            }
            else if (json.Value.Type == Value.ValueType.False)
            {
                return json.Value.FalseValue;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        public List<Value> GetValueFromPropertyName(string str)
        {
            return GetValueFromPropertyName(new Utf8Span(str), Object);
        }

        public List<Value> GetValueFromPropertyName(Utf8Span str)
        {
            return GetValueFromPropertyName(str, Object);
        }

        public List<Value> GetValueFromPropertyName(string str, Object obj)
        {
            return GetValueFromPropertyName(new Utf8Span(str), obj);
        }

        public List<Value> GetValueFromPropertyName(Utf8Span str, Object obj)
        {
            var values = new List<Value>();

            if (obj == null || obj.Pairs == null) return values;

            foreach (var pair in obj.Pairs)
            {
                if (pair == null || pair.Value == null) return values;

                if (pair.Value.Type == Value.ValueType.Object)
                {
                    values.AddRange(GetValueFromPropertyName(str, pair.Value.ObjectValue));
                }

                if (pair.Value.Type == Value.ValueType.Array)
                {
                    if (pair.Value.ArrayValue == null || pair.Value.ArrayValue.Values == null) return values;
                    
                    foreach (var value in pair.Value.ArrayValue.Values)
                    {
                        if (value != null && value.Type == Value.ValueType.Object)
                        {
                            values.AddRange(GetValueFromPropertyName(str, value.ObjectValue));
                        }
                    }
                }

                if (new Utf8Span(pair.Name.Span) == str)
                {
                    values.Add(pair.Value);
                }
            }

            return values;
        }

        public override string ToString()
        {
            if (Object != null)
            {
                return OutputObject(Object);
            }
            if (Array != null)
            {
                return OutputArray(Array);
            }
            return "";
        }

        private string OutputObject(Object obj)
        {
            var strBuilder = new StringBuilder();

            if (obj == null || obj.Pairs == null) return "";

            strBuilder.Append("{");
            for (var i = 0; i < obj.Pairs.Count; i++)
            {
                strBuilder.Append(OutputPair(obj.Pairs[i]));
                if (i < obj.Pairs.Count - 1)
                {
                    strBuilder.Append(",");
                }
            }
            strBuilder.Append("}");

            return strBuilder.ToString();
        }

        private string OutputPair(Pair pair)
        {
            var str = "";

            if (pair == null) return str;

            str += "\"" + Encoding.UTF8.GetString(pair.Name.Span) + "\":";
            str += OutputValue(pair.Value);
            return str;
        }

        private string OutputArray(Array array)
        {
            var strBuilder = new StringBuilder();

            if (array == null || array.Values == null) return "";

            strBuilder.Append("[");
            for (var i = 0; i < array.Values.Count; i++)
            {
                strBuilder.Append(OutputValue(array.Values[i]));
                if (i < array.Values.Count - 1)
                {
                    strBuilder.Append(",");
                }
            }
            strBuilder.Append("]");

            return strBuilder.ToString();
        }

        private string OutputValue(Value value)
        {
            var str = "";

            if (value == null) return str;

            var type = value.Type;
            switch (type)
            {
                case Value.ValueType.String:
                    str += "\"" + Encoding.UTF8.GetString(value.StringValue.Span) + "\"";
                    break;
                case Value.ValueType.Number:
                    str += value.NumberValue;
                    break;
                case Value.ValueType.Object:
                    str += OutputObject(value.ObjectValue);
                    break;
                case Value.ValueType.Array:
                    str += OutputArray(value.ArrayValue);
                    break;
                case Value.ValueType.True:
                    str += value.TrueValue.ToString().ToLower();
                    break;
                case Value.ValueType.False:
                    str += value.FalseValue.ToString().ToLower();
                    break;
                case Value.ValueType.Null:
                    str += value.NullValue.ToString().ToLower();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return str;
        }
    }

    public class Object
    {
        public List<Pair> Pairs { get; set; }
    }

    public class Pair
    {
        public ReadOnlyMemory<byte> Name { get; set; }
        public Value Value { get; set; }
    }

    public class Array
    {
        public List<Value> Values { get; set; }
    }

    public class Value
    {
        public ValueType Type { get; set; }

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

        public object Raw()
        {
            switch (Type)
            {
                case ValueType.String:
                    return _string;
                case ValueType.Number:
                    return _number;
                case ValueType.Object:
                    return _object;
                case ValueType.Array:
                    return _array;
                case ValueType.True:
                    return True;
                case ValueType.False:
                    return False;
                case ValueType.Null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ReadOnlyMemory<byte> StringValue
        {
            get
            {
                if (Type == ValueType.String)
                {
                    return _string;
                }
                throw new TypeAccessException("Value is not of type 'string'.");
            }
            set
            {
                if (Type == ValueType.String)
                {
                    _string = value;
                }
            }
        }

        public double NumberValue
        {
            get
            {
                if (Type == ValueType.Number)
                {
                    return _number;
                }
                throw new TypeAccessException("Value is not of type 'number'.");
            }
            set
            {
                if (Type == ValueType.Number)
                {
                    _number = value;
                }
            }
        }

        public Object ObjectValue
        {
            get
            {
                if (Type == ValueType.Object)
                {
                    return _object;
                }
                throw new TypeAccessException("Value is not of type 'object'.");
            }
            set
            {
                if (Type == ValueType.Object)
                {
                    _object = value;
                }
            }
        }

        public Array ArrayValue
        {
            get
            {
                if (Type == ValueType.Array)
                {
                    return _array;
                }
                throw new TypeAccessException("Value is not of type 'array'.");
            }
            set
            {
                if (Type == ValueType.Array)
                {
                    _array = value;
                }
            }
        }

        public bool TrueValue
        {
            get
            {
                if (Type == ValueType.True)
                {
                    return True;
                }
                throw new TypeAccessException("Value is not of type 'true'.");
            }
        }

        public bool FalseValue
        {
            get
            {
                if (Type == ValueType.False)
                {
                    return False;
                }
                throw new TypeAccessException("Value is not of type 'false'.");
            }
        }

        public object NullValue
        {
            get
            {
                if (Type == ValueType.Null)
                {
                    return Null;
                }
                throw new TypeAccessException("Value is not of type 'null'.");
            }
        }

        private ReadOnlyMemory<byte> _string;
        private double _number;
        private Object _object;
        private Array _array;
        private const bool True = true;
        private const bool False = false;
        private const string Null = "null";
    }
}
