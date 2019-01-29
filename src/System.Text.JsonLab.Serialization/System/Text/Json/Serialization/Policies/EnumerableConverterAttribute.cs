// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;

namespace System.Text.Json.Serialization.Policies
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class EnumerableConverterAttribute : Attribute
    {
        public EnumerableConverterAttribute() { }
        public Type EnumerableType { get; protected set; }

        public abstract IEnumerable CreateFromList(Type elementType, IList sourceList);
    }
}
