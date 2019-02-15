// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        public static T Read<T>(ReadOnlySpan<byte> utf8Json, JsonSerializerOptions options = null)
        {
            if (utf8Json == null)
                throw new ArgumentNullException(nameof(utf8Json));

            return (T)ReadInternal(utf8Json, typeof(T), options);
        }

        public static object Read(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerOptions options = null)
        {
            if (utf8Json == null)
                throw new ArgumentNullException(nameof(utf8Json));

            return ReadInternal(utf8Json, returnType, options);
        }

        private static object ReadInternal(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerOptions options)
        {
            if (options == null)
                options = s_defaultSettings;

            var state = new JsonReaderState(options: options.ReaderOptions);
            var reader = new Utf8JsonReader(utf8Json, isFinalBlock: true, state);
            return Read(reader, returnType, options);
        }
    }
}
