// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.Text
{
    public static partial class Encoders
    {
        public static class Utf32
        {
            #region Tranforms

            public static readonly ITransformation FromUtf8 = new Utf8ToUtf32Transform();
            public static readonly ITransformation FromUtf16 = new Utf16ToUtf32Transform();

            private sealed class Utf8ToUtf32Transform : ITransformation
            {
                public TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf8(source, destination, out bytesConsumed, out bytesWritten);
            }

            private sealed class Utf16ToUtf32Transform : ITransformation
            {
                public TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf16(source, destination, out bytesConsumed, out bytesWritten);
            }

            #endregion Transforms

            #region Utf-8 to Utf-32 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf8(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                throw new NotImplementedException();
            }

            public static TransformationStatus ConvertFromUtf8(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                throw new NotImplementedException();
            }

            #endregion Utf-8 to Utf-32 conversion

            #region Utf-16 to Utf-32 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf16(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                throw new NotImplementedException();
            }

            public static TransformationStatus ConvertFromUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                throw new NotImplementedException();
            }

            #endregion Utf-16 to Utf-32 conversion
        }
    }
}
