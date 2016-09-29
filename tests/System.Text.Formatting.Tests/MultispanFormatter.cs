// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Globalization;
using System.IO;
using System.Text.Formatting;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        [Fact]
        public void MultispanFormatterBasics()
        {
            var data = new Multispan<byte>();
            data.AppendNewSegment(10);
            data.AppendNewSegment(10);
            data.AppendNewSegment(10);
            data.AppendNewSegment(10);

            var formatter = new SequenceFormatter(data, EncodingData.InvariantUtf8);
            formatter.Append(new string('x', 10));
            formatter.Append(new string('x', 8));
            formatter.Append(new string('x', 8));
            formatter.Append(new string('x', 5));
            formatter.Append(new string('x', 5));

            var bytesWritten = formatter.TotalWritten;
            Assert.Equal(36, bytesWritten);

            foreach(var slice in data) {
                for(int i=0; i<slice.Length; i++) {
                    if (bytesWritten == 0) return; 
                    Assert.Equal((byte)'x', slice[i]);
                    bytesWritten--;
                }
            }
        }
    }
}
