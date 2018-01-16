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

        public void Advance(int bytes)
        {
            _writableBuffer.Advance(bytes);
        }

        public Memory<byte> GetMemory(int minimumLength = 0)
        {
            _writableBuffer.Ensure(minimumLength == 0 ? 2048 : minimumLength);
            return _writableBuffer.Buffer;
        }

        public Span<byte> GetSpan(int minimumLength) => GetMemory(minimumLength).Span;

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
