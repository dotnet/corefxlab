// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        [Fact]
        public async void StreamReadAsync()
        {
            var buffer = new byte[100];
            for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)i;
            var stream = new MemoryStream(buffer);

            buffer = new byte[100];
            var memory = new Memory<byte>(buffer);
            int read = await stream.ReadAsync(memory);

            Assert.Equal(buffer.Length, read);
            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.Equal(i, buffer[i]);
            }
        }

        [Fact]
        public void StreamWrite()
        {
            var span = new Span<byte>(new byte[100]);
            for (int i = 0; i < span.Length; i++) span[i] = (byte)i;

            var buffer = new byte[1000];
            var stream = new MemoryStream(buffer);

            stream.Write(span);
            for (int i = 0; i < span.Length; i++)
            {
                Assert.Equal(i, buffer[i]);
            }
        }

        [Fact]
        public async void StreamWriteAsync()
        {
            var array = new byte[100];
            var memory = new Memory<byte>(array);
            for (int i = 0; i < array.Length; i++) array[i] = (byte)i;

            var buffer = new byte[100];
            var stream = new MemoryStream(buffer);
            await stream.WriteAsync(memory);

            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.Equal(i, buffer[i]);
            }
        }
    }
}
