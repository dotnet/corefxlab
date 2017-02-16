// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        [Fact]
        public void SequenceFormatterBasics()
        {
            var list = new ArrayList<Memory<byte>>();
            list.Add(new byte[10]);
            list.Add(new byte[10]);
            list.Add(new byte[10]);
            list.Add(new byte[10]);

            var formatter = list.CreateFormatter(TextEncoder.Utf8);
            formatter.Append(new string('x', 10));
            formatter.Append(new string('x', 8));
            formatter.Append(new string('x', 8));
            formatter.Append(new string('x', 5));
            formatter.Append(new string('x', 5));

            var bytesWritten = formatter.TotalWritten;
            Assert.Equal(36, bytesWritten);

            foreach(var slice in list) {
                for(int i=0; i<slice.Length; i++) {
                    if (bytesWritten == 0) return; 
                    Assert.Equal((byte)'x', slice.Span[i]);
                    bytesWritten--;
                }
            }
        }
    }
}
