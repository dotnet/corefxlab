// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        public static T Read<T>(in ReadOnlySequence<byte> utf8Json, JsonSerializerOptions options = null)
        {
            return (T)Read(utf8Json, typeof(T), options);
        }

        public static object Read(in ReadOnlySequence<byte> utf8Json, Type returnType, JsonSerializerOptions options = null)
        {
            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));

            if (options == null)
                options = s_defaultSettings;

            var state = new JsonReaderState(options: options.ReaderOptions);
            var reader = new Utf8JsonReader(utf8Json, isFinalBlock: true, state);
            return Read(reader, returnType, options);
        }
    }
}
