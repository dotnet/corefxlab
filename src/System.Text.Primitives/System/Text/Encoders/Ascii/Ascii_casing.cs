// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace System.Buffers
{
    public static partial class Encodings
    {
        public static partial class Ascii
        {
            static readonly byte[] s_toLower = new byte[128];
            static readonly byte[] s_toUpper = new byte[128];

            static Ascii()
            {
                for (int i = 0; i < s_toLower.Length; i++)
                {
                    s_toLower[i] = (byte)char.ToLowerInvariant(((char)i));
                    s_toUpper[i] = (byte)char.ToUpperInvariant(((char)i));
                }
            }

            public static OperationStatus ToLowerInPlace(Span<byte> ascii, out int bytesChanged)
            {
                for (bytesChanged = 0; bytesChanged < ascii.Length; bytesChanged++)
                {
                    byte next = ascii[bytesChanged];
                    if (next > 127)
                    {
                        return OperationStatus.InvalidData;
                    }
                    ascii[bytesChanged] = s_toLower[next];
                }
                return OperationStatus.Done;
            }

            public static OperationStatus ToLower(ReadOnlySpan<byte> input, Span<byte> output, out int processedBytes)
            {
                int min = input.Length < output.Length ? input.Length : output.Length;
                for (processedBytes = 0; processedBytes < min; processedBytes++)
                {
                    byte next = input[processedBytes];
                    if (next > 127) return OperationStatus.InvalidData;
                    output[processedBytes] = s_toLower[next];
                }
                return OperationStatus.Done;
            }

            public static OperationStatus ToUpperInPlace(Span<byte> ascii, out int bytesChanged)
            {
                for (bytesChanged = 0; bytesChanged < ascii.Length; bytesChanged++)
                {
                    byte next = ascii[bytesChanged];
                    if (next > 127) return OperationStatus.InvalidData;
                    ascii[bytesChanged] = s_toUpper[next];
                }
                return OperationStatus.Done;
            }

            public static OperationStatus ToUpper(ReadOnlySpan<byte> input, Span<byte> output, out int processedBytes)
            {
                int min = input.Length < output.Length ? input.Length : output.Length;
                for (processedBytes = 0; processedBytes < min; processedBytes++)
                {
                    byte next = input[processedBytes];
                    if (next > 127) return OperationStatus.InvalidData;
                    output[processedBytes] = s_toUpper[next];
                }
                return OperationStatus.Done;
            }
        }
    }
}
