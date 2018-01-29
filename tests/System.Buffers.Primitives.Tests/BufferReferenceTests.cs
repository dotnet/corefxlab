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
                return new CustomMemoryForTest<byte>(new byte[1024]);
            });
        }

        [Fact]
        public void PooledBufferReferenceTests()
        {
            BufferReferenceTests.TestOwnedBuffer(() => {
                return MemoryPool<byte>.Shared.Rent(1000);
            });
        }

        [Fact]
        public void ArrayBufferReferenceTests()
        {
            BufferReferenceTests.TestBuffer(() => {
                return (Memory<byte>)new byte[1024];
            });
        }

        [Fact]
        public void EmptyBufferReferenceTests()
        {
            BufferReferenceTests.TestBuffer(() => {
                return Memory<byte>.Empty;
            });
        }

        [Fact]
        public void EmptyReadOnlyBufferReferenceTests()
        {
            BufferReferenceTests.TestBuffer(() => {
                return ReadOnlyMemory<byte>.Empty;
            });
        }
    }
}
