// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferReferenceUnitTests
    {
        [Fact]
        public void OwnedArrayReferenceTests()
        {
            BufferReferenceTests.TestMemoryManager(() => {
                return new CustomMemoryForTest<byte>(new byte[1024]);
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
