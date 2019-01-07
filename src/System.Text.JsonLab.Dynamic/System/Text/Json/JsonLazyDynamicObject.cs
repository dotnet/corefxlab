// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            if (jsonObject.Type != JsonTokenType.Number)
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
            if (jsonObject.Type != JsonTokenType.String) {
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
                case JsonTokenType.Number:
                    result = (object)(int)jsonObject;
                    break;
                case JsonTokenType.True:
                    result = (object)true;
                    break;
                case JsonTokenType.False:
                    result = (object)false;
                    break;
                case JsonTokenType.Null:
                    result = null;
                    break;
                case JsonTokenType.String:
                    result = (string)jsonObject;
                    break;
                case JsonTokenType.StartObject:
                    result = new JsonLazyDynamicObject(jsonObject);
                    break;
                case JsonTokenType.StartArray:
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

            if (_dom.Type == JsonTokenType.StartArray) {
                var resultObject = _dom[index];

                switch (resultObject.Type) {
                    case JsonTokenType.Number:
                        result = (object)(int)resultObject;
                        break;
                    case JsonTokenType.True:
                        result = (object)true;
                        break;
                    case JsonTokenType.False:
                        result = (object)false;
                        break;
                    case JsonTokenType.Null:
                        result = null;
                        break;
                    case JsonTokenType.String:
                        result = (string)resultObject;
                        break;
                    case JsonTokenType.StartObject:
                        result = new JsonLazyDynamicObject(resultObject);
                        break;
                    case JsonTokenType.StartArray:
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
