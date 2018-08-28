// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using System.Text.Utf8;

namespace System.Text.JsonLab
{
    public class JsonLazyDynamicObject : DynamicObject, IDisposable
    {
        //TODO: no spans on the heap
        JsonObject _dom => default;

        private JsonLazyDynamicObject(JsonObject dom)
        {
            //TODO: no spans on the heap
            //_dom = dom;
        }

        public static JsonLazyDynamicObject Parse(ReadOnlySpan<byte> utf8Json)
        {
            JsonObject dom = JsonObject.Parse(utf8Json);
            var result = new JsonLazyDynamicObject(dom);
            return result;
        }

        public bool TryGetUInt32(Utf8Span propertyName, out uint value)
        {
            if (!_dom.TryGetValue(propertyName, out JsonObject jsonObject))
            {
                value = default;
                return false;
            }
            if (jsonObject.Type != JsonValueType.Number)
            {
                throw new InvalidOperationException();
            }
            value = (uint)jsonObject;
            return true;
        }

        public bool TryGetString(Utf8Span propertyName, out Utf8Span value)
        {
            if (!_dom.TryGetValue(propertyName, out JsonObject jsonObject))
            {
                value = default;
                return false;
            }
            if (jsonObject.Type != JsonValueType.String) {
                throw new InvalidOperationException();
            }
            value = (Utf8Span)jsonObject;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!_dom.TryGetValue(binder.Name, out JsonObject jsonObject))
            {
                result = default;
                return false;
            }

            switch (jsonObject.Type) {
                case JsonValueType.Number:
                    result = (object)(int)jsonObject;
                    break;
                case JsonValueType.True:
                    result = (object)true;
                    break;
                case JsonValueType.False:
                    result = (object)false;
                    break;
                case JsonValueType.Null:
                    result = null;
                    break;
                case JsonValueType.String:
                    result = (string)jsonObject;
                    break;
                case JsonValueType.Object:
                    result = new JsonLazyDynamicObject(jsonObject);
                    break;
                case JsonValueType.Array:
                    result = new JsonLazyDynamicObject(jsonObject);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if(indexes.Length != 1 || !(indexes[0] is int)) {
                result = null;
                return false;
            }

            var index = (int)indexes[0];

            if (_dom.Type == JsonValueType.Array) {
                var resultObject = _dom[index];

                switch (resultObject.Type) {
                    case JsonValueType.Number:
                        result = (object)(int)resultObject;
                        break;
                    case JsonValueType.True:
                        result = (object)true;
                        break;
                    case JsonValueType.False:
                        result = (object)false;
                        break;
                    case JsonValueType.Null:
                        result = null;
                        break;
                    case JsonValueType.String:
                        result = (string)resultObject;
                        break;
                    case JsonValueType.Object:
                        result = new JsonLazyDynamicObject(resultObject);
                        break;
                    case JsonValueType.Array:
                        result = new JsonLazyDynamicObject(resultObject);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                return true;
            }

            result = null;
            return false;
        }

        public void Dispose()
        {
            _dom.Dispose();
        }
    }
}
