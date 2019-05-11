// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.JsonLab.Serialization.Converters;

namespace System.Text.JsonLab.Serialization.Policies
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    internal class JsonArrayConverterAttribute : JsonEnumerableConverterAttribute
    {
        private static readonly JsonEnumerableConverter _converter = new DefaultArrayConverter();
#if BUILDING_INBOX_LIBRARY
        public override JsonEnumerableConverter CreateConverter()
        {
            return _converter;
        }
#else
        internal override JsonEnumerableConverter CreateConverter()
        {
            return _converter;
        }
#endif
    }
}
