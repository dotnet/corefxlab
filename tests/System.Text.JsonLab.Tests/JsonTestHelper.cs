// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace System.Text.JsonLab.Tests
{
    public static class JsonTestHelper
    {
        public static string NewtonsoftReturnStringHelper(TextReader reader)
        {
            var sb = new StringBuilder();
            var json = new Newtonsoft.Json.JsonTextReader(reader);
            while (json.Read())
            {
                if (json.Value != null)
                {
                    sb.Append(json.Value).Append(", ");
                }
            }
            return sb.ToString();
        }
    }
}
