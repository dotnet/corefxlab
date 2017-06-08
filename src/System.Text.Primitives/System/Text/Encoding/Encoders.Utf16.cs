// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.Text
{
    public static partial class Encoders
    {
        public static class Utf16
        {
            #region Tranforms

            public static readonly ITransformation FromUtf8 = new Utf8ToUtf16Transform();
            public static readonly ITransformation FromUtf32 = new Utf32ToUtf16Transform();

            private sealed class Utf8ToUtf16Transform : ITransformation
            {
                public TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf8(source, destination, out bytesConsumed, out bytesWritten);
            }

            private sealed class Utf32ToUtf16Transform : ITransformation
            {
                public TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf32(source, destination, out bytesConsumed, out bytesWritten);
            }

            #endregion Transforms

            #region Utf-8 to Utf-16 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf8(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                throw new NotImplementedException();
            }

            public static TransformationStatus ConvertFromUtf8(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                throw new NotImplementedException();
            }

            #endregion Utf-8 to Utf-16 conversion

            #region Utf-32 to Utf-16 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf32(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                throw new NotImplementedException();
            }

            public static TransformationStatus ConvertFromUtf32(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                throw new NotImplementedException();
            }

            #endregion Utf-32 to Utf-16 conversion
        }
    }
}
