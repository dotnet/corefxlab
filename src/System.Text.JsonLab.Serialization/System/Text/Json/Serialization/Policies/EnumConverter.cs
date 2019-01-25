// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization.Policies
{
    internal class EnumConverter : IUtf8ValueConverter<Enum>
    {
        public static readonly UTF8Encoding s_utf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        private readonly bool _treatAsString;

        public EnumConverter(bool treatAsString)
        {
            _treatAsString = treatAsString;
        }

        //todo: support [Flags]
        public bool TryGetFromJson(ReadOnlySpan<byte> span, Type type, out Enum value)
        {
            string enumString = s_utf8Encoding.GetString(span);
            if (Enum.TryParse(type, enumString, out object objValue))
            {
                value = (Enum)objValue;
                return true;
            }

            throw new InvalidOperationException("todo");
        }

        public bool TrySetToJson(Enum value, out Span<byte> span)
        {
            string stringVal;
            if (_treatAsString)
            {
                stringVal = value.ToString();
            }
            else
            {
                object objVal = Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));
                stringVal = value.ToString();
            }

            span = Encoding.UTF8.GetBytes(stringVal);
            return true;
        }
    }
}
