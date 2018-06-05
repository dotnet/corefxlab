// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using Xunit;

namespace System.Polyfill.Tests
{
    public class StreamTests
    {
        [Fact]
        public void StreamRead()
        {
            var buffer = new byte[100];
            for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)i;

            var stream = new MemoryStream(buffer);
            var span = new Span<byte>(new byte[100]);

            var read = stream.Read(span);
            Assert.Equal(buffer.Length, read);
            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.Equal(i, span[i]);
            }
        }
    }
}
