// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.Buffers.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public class AsciiCasingTests
    {
        [Theory]
        [InlineData(0, 128, OperationStatus.Done, 128)]
        [InlineData(0, 129, OperationStatus.InvalidData, 128)]
        public void ToUpper(int from, int to, OperationStatus expectedStatus, int expectedProcessed)
        {
            var buffer = Create(from, to);
            var copy = new byte[buffer.Length];
            buffer.AsSpan().CopyTo(copy);
            var output = new byte[buffer.Length];

            var status = TextEncodings.Ascii.ToUpper(buffer, output, out int processedBytes);
            Assert.Equal(expectedStatus, status);
            Assert.Equal(expectedProcessed, processedBytes);

            var clr = Encoding.ASCII.GetString(copy, 0, processedBytes).ToUpperInvariant();
            var transformed = Encoding.ASCII.GetString(output, 0, processedBytes);

            Assert.Equal(clr, transformed);
        }

        [Theory]
        [InlineData(0, 128, OperationStatus.Done, 128)]
        [InlineData(0, 129, OperationStatus.InvalidData, 128)]
        public void ToUpperInPlace(int from, int to, OperationStatus expectedStatus, int expectedProcessed)
        {
            var buffer = Create(from, to);
            var copy = new byte[buffer.Length];
            buffer.AsSpan().CopyTo(copy);

            var status = TextEncodings.Ascii.ToUpperInPlace(buffer, out int processedBytes);
            Assert.Equal(expectedStatus, status);
            Assert.Equal(expectedProcessed, processedBytes);

            var clr = Encoding.ASCII.GetString(copy, 0, processedBytes).ToUpperInvariant();
            var transformed = Encoding.ASCII.GetString(buffer, 0, processedBytes);
        }

        [Theory]
        [InlineData(0, 128, OperationStatus.Done, 128)]
        [InlineData(0, 129, OperationStatus.InvalidData, 128)]
        public void ToLower(int from, int to, OperationStatus expectedStatus, int expectedProcessed)
        {
            var buffer = Create(from, to);
            var copy = new byte[buffer.Length];
            buffer.AsSpan().CopyTo(copy);
            var output = new byte[buffer.Length];

            var status = TextEncodings.Ascii.ToLower(buffer, output, out int processedBytes);
            Assert.Equal(expectedStatus, status);
            Assert.Equal(expectedProcessed, processedBytes);

            var clr = Encoding.ASCII.GetString(copy, 0, processedBytes).ToLowerInvariant();
            var transformed = Encoding.ASCII.GetString(output, 0, processedBytes);

            Assert.Equal(clr, transformed);
        }

        [Theory]
        [InlineData(0, 128, OperationStatus.Done, 128)]
        [InlineData(0, 129, OperationStatus.InvalidData, 128)]
        public void ToLowerInPlace(int from, int to, OperationStatus expectedStatus, int expectedProcessed)
        {
            var buffer = Create(from, to);
            var copy = new byte[buffer.Length];
            buffer.AsSpan().CopyTo(copy);

            var status = TextEncodings.Ascii.ToLowerInPlace(buffer, out int processedBytes);
            Assert.Equal(expectedStatus, status);
            Assert.Equal(expectedProcessed, processedBytes);

            var clr = Encoding.ASCII.GetString(copy, 0, processedBytes).ToLowerInvariant();
            var transformed = Encoding.ASCII.GetString(buffer, 0, processedBytes);

            Assert.Equal(clr, transformed);
        }

        static byte[] Create(int from, int to)
        {
            var buffer = new byte[to - from];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(i + from);
            }
            return buffer;
        }
    }
}
