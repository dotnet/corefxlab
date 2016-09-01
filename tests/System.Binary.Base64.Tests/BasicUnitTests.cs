// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;
using System.Binary;
using System.Collections.Generic;
using System.Text.Utf8;

namespace System.Binary.Tests
{
    public class Base64Tests
    {
        [Fact]
        public void BasicEncodingDecoding()
        {
            var list = new List<byte>();
            for(int value=0; value < 256; value++) {
                list.Add((byte)value);
            }
            var testBytes = list.ToArray();

            for (int value = 0; value < 256; value++) {

                var sourceBytes = testBytes.Slice(0, value + 1);
                var encodedBytes = new byte[Base64.ComputeEncodedLength(sourceBytes.Length)].Slice();
                var encodedBytesCount = Base64.Encode(sourceBytes, encodedBytes);
                Assert.Equal(encodedBytes.Length, encodedBytesCount);

                var encodedText = new Utf8String(encodedBytes).ToString();
                var expectedText = Convert.ToBase64String(testBytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                var decodedBytes = new byte[sourceBytes.Length];
                var decodedByteCount = Base64.Decode(encodedBytes, decodedBytes.Slice());
                Assert.Equal(sourceBytes.Length, decodedByteCount);

                for (int i=0; i<decodedBytes.Length; i++) {
                    Assert.Equal(sourceBytes[i], decodedBytes[i]);
                }
            } 
        }

        [Fact]
        public void DecodeInPlace()
        {
            var list = new List<byte>();
            for (int value = 0; value < 256; value++) {
                list.Add((byte)value);
            }
            var testBytes = list.ToArray();

            for (int value = 0; value < 256; value++) {
                var sourceBytes = testBytes.Slice(0, value + 1);
                var buffer = new byte[Base64.ComputeEncodedLength(sourceBytes.Length)];
                var bufferSlice = buffer.Slice();

                Base64.Encode(sourceBytes, bufferSlice);

                var encodedText = new Utf8String(bufferSlice).ToString();
                var expectedText = Convert.ToBase64String(testBytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                var decodedByteCount = Base64.DecodeInPlace(bufferSlice);
                Assert.Equal(sourceBytes.Length, decodedByteCount);

                for (int i = 0; i < decodedByteCount; i++) {
                    Assert.Equal(sourceBytes[i], buffer[i]);
                }
            }
        }
    }
}