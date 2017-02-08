// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.Text.Formatting
{
    public struct OutputFormatter<TOutput> : ITextOutput where TOutput : IOutput
    {
        TOutput _output;
        EncodingData _encoding;

        public OutputFormatter(TOutput output, EncodingData encoding)
        {
            _output = output;
            _encoding = encoding;
        }

        public OutputFormatter(TOutput output) : this(output, EncodingData.InvariantUtf8)
        {
        }

        public Span<byte> Buffer => _output.Buffer;

        public EncodingData Encoding => _encoding;

        public void Advance(int bytes) => _output.Advance(bytes);

        public void Enlarge(int desiredBufferLength = 0) => _output.Enlarge(desiredBufferLength);
    }
}
