// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Text.JsonLab.Serialization.Converters;

namespace System.Text.JsonLab.Serialization.Policies
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class JsonEnumerableConverterAttribute : Attribute
    {
#if BUILDING_INBOX_LIBRARY
        public Type EnumerableType { get; protected set; }
        public abstract JsonEnumerableConverter CreateConverter();
#else
        internal Type EnumerableType { get; set; }
        internal abstract JsonEnumerableConverter CreateConverter();
#endif
    }
}
