// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.IO.Pipelines
{
    public struct PipeWriterOutput : IOutput
    {
        private readonly IPipeWriter _writer;

        public PipeWriterOutput(IPipeWriter writer) : this()
        {
            _writer = writer;
        }

        public Span<byte> Buffer => _writer.Buffer.Span;

        public void Advance(int bytes)
        {
            _writer.Advance(bytes);
        }

        public void Enlarge(int desiredBufferLength = 0)
        {
            _writer.Ensure(ComputeActualSize(desiredBufferLength));
        }

        private int ComputeActualSize(int desiredBufferLength)
        {
            if (desiredBufferLength < 256) desiredBufferLength = 256;
            if (desiredBufferLength < Buffer.Length) desiredBufferLength = Buffer.Length * 2;
            return desiredBufferLength;
        }
    }
}
