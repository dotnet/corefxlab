// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class MaterializerTests
    {
        [Fact]
        public static void SimpleObjectWithReflectionMaterializer()
        {
            var options = new JsonSerializerOptions();
            options.ClassMaterializer = JsonClassMaterializer.Reflection;
            SimpleTestClass obj = JsonSerializer.Read<SimpleTestClass>(SimpleTestClass.s_data, options);
            obj.Verify();
        }
    }
}
