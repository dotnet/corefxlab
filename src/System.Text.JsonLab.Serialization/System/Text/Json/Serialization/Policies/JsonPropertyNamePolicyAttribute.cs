// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization.Policies
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class JsonPropertyNamePolicyAttribute : Attribute
    {
        public abstract string Read(string value);
        public abstract string Write(string value);
    }
}
