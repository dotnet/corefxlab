// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab.Serialization.Policies
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class JsonValueConverterAttribute : Attribute
    {
        public Type PropertyType { get; protected set; }

#if BUILDING_INBOX_LIBRARY
        public abstract JsonValueConverter<TValue> GetConverter<TValue>();

        public virtual JsonValueConverter<object> GetConverter()
        {
            return GetConverter<object>();
        }
#else
        internal abstract JsonValueConverter<TValue> GetConverter<TValue>();

        internal virtual JsonValueConverter<object> GetConverter()
        {
            return GetConverter<object>();
        }
#endif
    }
}
