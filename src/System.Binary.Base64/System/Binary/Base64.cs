// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Binary.Base64
{
    public sealed class Base64
    {
        public static readonly Base64Encoder Encoder = Base64Encoder.Instance;
        public static readonly Base64Decoder Decoder = Base64Decoder.Instance;
    }
}
