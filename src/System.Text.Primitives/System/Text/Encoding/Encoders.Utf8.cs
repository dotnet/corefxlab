// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.Text
{
    public static partial class Encoders
    {
        public static class Utf8
        {
            #region Tranforms

            public static readonly ITransformation FromUtf16 = new Utf16ToUtf8Transform();
            public static readonly ITransformation FromUtf32 = new Utf32ToUtf8Transform();

            private sealed class Utf16ToUtf8Transform : ITransformation
            {
                public TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf16(source, destination, out bytesConsumed, out bytesWritten);
            }

            private sealed class Utf32ToUtf8Transform : ITransformation
            {
                public TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf32(source, destination, out bytesConsumed, out bytesWritten);
            }

            #endregion Transforms

            #region Utf-16 to Utf-8 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf16(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                throw new NotImplementedException();
            }

            public static TransformationStatus ConvertFromUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                throw new NotImplementedException();
            }

            #endregion Utf-16 to Utf-8 conversion

            #region Utf-32 to Utf-8 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf32(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                throw new NotImplementedException();
            }

            public static TransformationStatus ConvertFromUtf32(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                throw new NotImplementedException();
            }

            #endregion Utf-32 to Utf-8 conversion
        }
    }
}
