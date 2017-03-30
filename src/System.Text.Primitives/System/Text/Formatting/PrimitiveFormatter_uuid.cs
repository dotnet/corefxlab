// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Text 
{
    public static partial class PrimitiveFormatter
    {
        public static bool TryFormat(this Guid value, Span<byte> buffer, out int bytesWritten, TextFormat format = default(TextFormat), TextEncoder encoder = null)
        {
            encoder = encoder == null ? TextEncoder.Utf8 : encoder;

            if (encoder.IsInvariantUtf8)
                return InvariantUtf8UuidFormatter.TryFormat(value, buffer, out bytesWritten, format);
            else if (encoder.IsInvariantUtf16)
                return InvariantUtf16UuidFormatter.TryFormat(value, buffer, out bytesWritten, format);
            else
                throw new NotImplementedException();
        }
    }
}
