// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;

namespace System.Binary.Base64
{
    public static partial class Base64
    {
        public static readonly Transformation Encoder = new ToBase64();
        public static readonly Transformation Decoder = new FromBase64();
    }
}
