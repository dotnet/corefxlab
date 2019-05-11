﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab.Serialization
{
    public static partial class JsonSerializer
    {
        public static TValue Parse<TValue>(ReadOnlySpan<byte> utf8Json, JsonSerializerOptions options = null)
        {
            if (utf8Json == null)
                throw new ArgumentNullException(nameof(utf8Json));

            return (TValue)ParseCore(utf8Json, typeof(TValue), options);
        }

        public static object Parse(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerOptions options = null)
        {
            if (utf8Json == null)
                throw new ArgumentNullException(nameof(utf8Json));

            return ParseCore(utf8Json, returnType, options);
        }

        private static object ParseCore(ReadOnlySpan<byte> utf8Json, Type returnType, JsonSerializerOptions options)
        {
            if (options == null)
                options = s_defaultSettings;

            var state = new JsonReaderState(options: options.ReaderOptions);
            var reader = new Utf8JsonReader(utf8Json, isFinalBlock: true, state);
            return ReadCore(reader, returnType, options);
        }
    }
}
