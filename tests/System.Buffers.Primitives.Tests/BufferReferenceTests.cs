// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferReferenceUnitTests
    {
        [Fact]
        public void OwnedArrayReferenceTests()
        {
            BufferReferenceTests.Run(() => { 
                return new OwnedArray<byte>(1024);
            });
        }

        [Fact]
        public void AutoDisposeBufferReferenceTests()
        {
            BufferReferenceTests.Run(() => {
                return new AutoDisposeBuffer<byte>(new byte[1024]);
            });
        }

        [Fact]
        public void AutoPooledBufferReferenceTests()
        {
            BufferReferenceTests.Run(() => {
                return new AutoPooledBuffer(1024);
            });
        }

        [Fact]
        public void CustomBufferReferenceTests()
        {
            BufferReferenceTests.Run(() => {
                return new CustomBuffer();
            });
        }
    }
}
