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
                Base64.Encode(sourceBytes, encodedBytes);

                var encodedText = new Utf8String(encodedBytes).ToString();
                var expectedText = Convert.ToBase64String(testBytes, 0, value + 1);
                Assert.Equal(expectedText, encodedText);

                var decodedBytes = new byte[sourceBytes.Length];
                Base64.Decode(encodedBytes, decodedBytes.Slice());

                for(int i=0; i<decodedBytes.Length; i++) {
                    Assert.Equal(sourceBytes[i], decodedBytes[i]);
                }
            } 
        }
    }
}