// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers.Adapters;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public partial class Utf8BufferReaderTests
    {
        [Theory]
        [InlineData("hello world\uF8D0")]
        [InlineData("\uF8D0")]
        public void ReadAndPeek(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var reader = new Utf8BufferReader(buffer);
            int index = 0;
            while(true){   
                var peeked = (char)reader.Peek();
                var read = reader.Read();
                if(read==-1) break;
                Assert.Equal(str[index++], (char)read);
                Assert.Equal((char)read, (char)peeked);
            }

            reader = new Utf8BufferReader(buffer);
            Assert.Equal(str, reader.ReadToEnd());

            reader = new Utf8BufferReader(buffer);
            Assert.Equal(str, reader.ReadToEndAsync().Result);
        }
    }
}