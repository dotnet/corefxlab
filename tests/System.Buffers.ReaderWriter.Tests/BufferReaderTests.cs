// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Reader;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public class BufferReaderTests
    {
        private static readonly byte[] s_array;
        private static readonly ReadOnlySequence<byte> s_ros;

        static BufferReaderTests()
        {
            var sections = 100000;
            var section = "1234 ";
            var builder = new StringBuilder(sections * section.Length);
            for (int i = 0; i < sections; i++)
            {
                builder.Append(section);
            }
            s_array = Encoding.UTF8.GetBytes(builder.ToString());
            s_ros = new ReadOnlySequence<byte>(s_array);
        }

        [Fact]
        public void TryParseRos()
        {
            var reader = BufferReader.Create(s_ros);

            while (BufferReaderExtensions.TryParse(ref reader, out int value))
            {
                reader.Advance(1); // advance past the delimiter
                Assert.Equal(1234, value);
            }
        }
    }
}
