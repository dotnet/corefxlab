// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using System.Text.Formatting;
using System.Buffers.Text;

namespace System.IO.Pipelines.Text.Primitives
{
    public class PipelineTextOutput : ITextOutput
    {
        private readonly IPipeWriter _writer;
        private WritableBuffer _writableBuffer;
        private bool _needAlloc = true;

        public PipelineTextOutput(IPipeWriter writer, SymbolTable symbolTable)
        {
            _writer = writer;
            SymbolTable = symbolTable;
        }

        public SymbolTable SymbolTable { get; }

        public Span<byte> Buffer
        {
            get
            {
                EnsureBuffer();

                return _writableBuffer.Buffer.Span;
            }
        }

        public void Advance(int bytes)
        {
            _writableBuffer.Advance(bytes);
        }

        public void Enlarge(int desiredFreeBytesHint = 0)
        {
            _writableBuffer.Ensure(desiredFreeBytesHint == 0 ? 2048 : desiredFreeBytesHint);
        }

        public void Write(Span<byte> data)
        {
            EnsureBuffer();
            _writableBuffer.Write(data);
        }

        public async Task FlushAsync()
        {
            await _writableBuffer.FlushAsync();
            _needAlloc = true;
        }

        private void EnsureBuffer()
        {
            if (_needAlloc)
            {
                _writableBuffer = _writer.Alloc();
                _needAlloc = false;
            }
        }
    }
}
