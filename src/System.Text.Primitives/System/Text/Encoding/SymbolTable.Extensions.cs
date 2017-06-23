// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text
{
    public static class SymbolTableExtensions
    {
        public static bool TryEncode(this SymbolTable symbolTable, ReadOnlySpan<char> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            ReadOnlySpan<byte> srcBytes = source.AsBytes();

            if (symbolTable == SymbolTable.InvariantUtf16)
                return TryEncodeUtf16(srcBytes, destination, out bytesConsumed, out bytesWritten);

            const int BufferSize = 256;

            int srcLength = srcBytes.Length;
            if (srcLength <= 0)
            {
                bytesConsumed = bytesWritten = 0;
                return true;
            }

            Span<byte> temp;
            unsafe
            {
                byte* pTemp = stackalloc byte[BufferSize];
                temp = new Span<byte>(pTemp, BufferSize);
            }

            bytesWritten = 0;
            bytesConsumed = 0;
            while (srcLength > bytesConsumed)
            {
                var status = Encoders.Utf8.ConvertFromUtf16(srcBytes, temp, out int consumed, out int written);
                if (status == Buffers.TransformationStatus.InvalidData)
                    goto ExitFailed;

                srcBytes = srcBytes.Slice(consumed);
                bytesConsumed += consumed;

                if (!symbolTable.TryEncode(temp.Slice(0, written), destination, out consumed, out written))
                    goto ExitFailed;

                destination = destination.Slice(written);
                bytesWritten += written;
            }

            return true;

        ExitFailed:
            return false;
        }

        private static bool TryEncodeUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            // NOTE: There is no validation of this UTF-16 encoding. A caller is expected to do any validation on their own if
            //       they don't trust the data.
            bytesConsumed = source.Length;
            bytesWritten = destination.Length;
            if (bytesConsumed > bytesWritten)
            {
                source = source.Slice(0, bytesWritten);
                bytesConsumed = bytesWritten;
            }
            else
            {
                bytesWritten = bytesConsumed;
            }

            source.CopyTo(destination);
            return true;
        }
    }
}
