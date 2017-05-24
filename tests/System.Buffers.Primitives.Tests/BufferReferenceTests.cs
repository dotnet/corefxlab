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
            BufferReferenceTests.TestOwnedBuffer(() => {
                return (OwnedBuffer<byte>)new byte[1024];
            });
        }

        [Fact]
        public void PooledBufferReferenceTests()
        {
            BufferReferenceTests.TestOwnedBuffer(() => {
                return BufferPool.Default.Rent(1000);
            });
        }

        [Fact]
        public void ArrayBufferReferenceTests()
        {
            BufferReferenceTests.TestBuffer(() => {
                return (Buffer<byte>)new byte[1024];
            });
        }

        [Fact]
        public void EmptyBufferReferenceTests()
        {
            BufferReferenceTests.TestBuffer(() => {
                return Buffer<byte>.Empty;
            });
        }

        [Fact]
        public void EmptyReadOnlyBufferReferenceTests()
        {
            BufferReferenceTests.TestBuffer(() => {
                return ReadOnlyBuffer<byte>.Empty;
            });
        }
    }
}
