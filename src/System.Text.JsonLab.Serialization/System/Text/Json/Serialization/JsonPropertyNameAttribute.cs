// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.JsonLab.Serialization.Policies;

namespace System.Text.JsonLab.Serialization
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonPropertyNameAttribute : JsonPropertyNamePolicyAttribute
    {
        public JsonPropertyNameAttribute() { }

        public JsonPropertyNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string Read(string value)
        {
            return value;
        }

        public override string Write(string value)
        {
            return Name;
        }
    }
}
