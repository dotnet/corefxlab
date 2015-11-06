using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Text.Json.Tests
{
    public class Json
    {
        public Object Object { get; set; }

        public List<Value> GetValueFromPropertyName(string str)
        {
            return GetValueFromPropertyName(new Utf8String(str), Object);
        }

        public List<Value> GetValueFromPropertyName(Utf8String str)
        {
            return GetValueFromPropertyName(str, Object);
        }

        public List<Value> GetValueFromPropertyName(string str, Object obj)
        {
            return GetValueFromPropertyName(new Utf8String(str), obj);
        }

        public List<Value> GetValueFromPropertyName(Utf8String str, Object obj)
        {
            var values = new List<Value>();

            if (obj == null || obj.Members == null) return values;

            foreach (var member in obj.Members)
            {
                if (member == null || member.Pairs == null) return values;

                foreach (var pair in member.Pairs)
                {
                    if (pair == null || pair.Value == null) return values;

                    if (pair.Value.Type == Value.ValueType.Object)
                    {
                        values.AddRange(GetValueFromPropertyName(str, pair.Value.ObjectValue));
                    }

                    if (pair.Value.Type == Value.ValueType.Array)
                    {
                        if (pair.Value.ArrayValue == null || pair.Value.ArrayValue.Elements == null) return values;

                        foreach (var element in pair.Value.ArrayValue.Elements)
                        {
                            if (element == null || element.Values == null) return values;

                            foreach (var value in element.Values)
                            {
                                if (value != null && value.Type == Value.ValueType.Object)
                                {
                                    values.AddRange(GetValueFromPropertyName(str, value.ObjectValue));
                                }
                            }
                        }
                    }

                    if (new Utf8String(pair.Name) == str)
                    {
                        values.Add(pair.Value);
                    }
                }
            }
            return values;
        }

        public override string ToString()
        {
            return Object == null ? "" : OutputObject(Object);
        }

        private string OutputObject(Object obj)
        {
            var str = "";

            if (obj == null || obj.Members == null) return str;

            foreach (var member in obj.Members)
            {
                str += "{";
                str += OutputMembers(member);
                str += "}";
            }

            return str;
        }

        private string OutputMembers(Members members)
        {
            var str = "";

            if (members == null || members.Pairs == null) return str;

            for (var i = 0; i < members.Pairs.Count; i++)
            {
                str += OutputPair(members.Pairs[i]);
                if (i < members.Pairs.Count - 1)
                {
                    str += ",";
                }
            }
            return str;
        }

        private string OutputPair(Pair pair)
        {
            var str = "";

            if (pair == null) return str;

            str += "\"" + pair.Name + "\":";
            str += OutputValue(pair.Value);
            return str;
        }

        private string OutputArray(Array array)
        {
            var str = "";

            if (array == null || array.Elements == null) return str;

            foreach (var element in array.Elements)
            {
                str += "[";
                str += OutputElements(element);
                str += "]";
            }

            return str;
        }

        private string OutputElements(Elements elements)
        {
            var str = "";

            if (elements == null || elements.Values == null) return str;

            for (var i = 0; i < elements.Values.Count; i++)
            {
                str += OutputValue(elements.Values[i]);
                if (i < elements.Values.Count - 1)
                {
                    str += ",";
                }
            }
            return str;
        }

        private string OutputValue(Value value)
        {
            var str = "";

            if (value == null) return str;

            var type = value.Type;
            switch (type)
            {
                case Value.ValueType.String:
                    str += "\"" + value.StringValue + "\"";
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
        public List<Members> Members { get; set; }
    }

    public class Members
    {
        public List<Pair> Pairs { get; set; }
    }

    public class Pair
    {
        public string Name { get; set; }
        public Value Value { get; set; }
    }

    public class Array
    {
        public List<Elements> Elements { get; set; }
    }

    public class Elements
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

        public string StringValue
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

        private string _string;
        private double _number;
        private Object _object;
        private Array _array;
        private const bool True = true;
        private const bool False = false;
        private const string Null = "null";
    }
}