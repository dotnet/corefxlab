// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using System.Text.Utf8;

namespace System.Text.Json
{
    public class JsonLazyDynamicObject : DynamicObject
    {
        JsonObject _dom;

        private JsonLazyDynamicObject(JsonObject dom)
        {
            _dom = dom;
        }

        public static JsonLazyDynamicObject Parse(Utf8String text)
        {
            var dom = JsonObject.Parse(text.Bytes);
            var result = new JsonLazyDynamicObject(dom);
            return result;
        }

        public bool TryGetUInt32(Utf8String propertyName, out uint value)
        {
            JsonObject jsonObject;
            if (!_dom.TryGetValue(propertyName, out jsonObject))
            {
                value = default(uint);
                return false;
            }
            if (jsonObject.Type != JsonObject.JsonValueType.Number)
            {
                throw new InvalidOperationException();
            }
            value = (uint)jsonObject;
            return true;
        }

        public bool TryGetString(Utf8String propertyName, out Utf8String value)
        {
            JsonObject jsonObject;
            if (!_dom.TryGetValue(propertyName, out jsonObject)) {
                value = default(Utf8String);
                return false;
            }
            if (jsonObject.Type != JsonObject.JsonValueType.String) {
                throw new InvalidOperationException();
            }
            value = (Utf8String)jsonObject;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            JsonObject jsonObject;
            if (!_dom.TryGetValue(binder.Name, out jsonObject)) {
                result = default(object);
                return false;
            }

            switch (jsonObject.Type) {
                case JsonObject.JsonValueType.Number:
                    result = (object)(int)jsonObject;
                    break;
                case JsonObject.JsonValueType.True:
                    result = (object)true;
                    break;
                case JsonObject.JsonValueType.False:
                    result = (object)false;
                    break;
                case JsonObject.JsonValueType.Null:
                    result = null;
                    break;
                case JsonObject.JsonValueType.String:
                    result = (string)jsonObject;
                    break;
                case JsonObject.JsonValueType.Array:
                case JsonObject.JsonValueType.Object:
                default:
                    throw new NotImplementedException();
            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }
    }
}