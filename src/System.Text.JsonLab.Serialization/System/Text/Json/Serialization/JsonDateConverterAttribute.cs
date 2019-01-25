// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    /// <summary>
    /// Converter for ISO 8601 <see cref="DateTime"/> with the format "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK".
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    internal class JsonDateConverterAttribute : PropertyValueConverterAttribute
    {
        private static readonly IUtf8ValueConverter<DateTime> s_dateConverter = new DateConverter();

        public JsonDateConverterAttribute()
        {
            PropertyType = typeof(DateTime);
        }

        public override IUtf8ValueConverter<DateTime> GetConverter<DateTime>()
        {
            //return default;
            return (IUtf8ValueConverter<DateTime>)s_dateConverter;
        }
    }
}
