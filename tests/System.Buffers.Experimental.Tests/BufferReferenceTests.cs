// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferReferenceUnitTests
    {
        [Fact]
        public void AutoDisposeBufferReferenceTests()
        {
            BufferReferenceTests.TestAutoOwnedBuffer(() => {
                return new AutoDisposeBuffer<byte>(new byte[1024]);
            });
        }

        [Fact]
        public void AutoPooledBufferReferenceTests()
        {
            BufferReferenceTests.TestAutoOwnedBuffer(() => {
                return new AutoPooledBuffer(1024);
            });
        }

        [Fact]
        public void CustomBufferReferenceTests()
        {
            BufferReferenceTests.TestMemoryManager(() => {
                return new CustomBuffer<byte>(512);
            });
        }
    }
}
