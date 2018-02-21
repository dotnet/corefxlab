// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Threading.Tasks;
using System.Text.Formatting;
using System.Buffers.Text;

namespace System.IO.Pipelines.Text.Primitives
{
    public class PipelineTextBufferWriter : ITextBufferWriter
    {
        private readonly PipeWriter _writer;

        public PipelineTextBufferWriter(PipeWriter writer, SymbolTable symbolTable)
        {
            _writer = writer;
            SymbolTable = symbolTable;
        }

        public SymbolTable SymbolTable { get; }

        public void Advance(int bytes)
        {
            _writer.Advance(bytes);
        }

        public Memory<byte> GetMemory(int desiredFreeBytesHint = 0)
        {
            return _writer.GetMemory(desiredFreeBytesHint);
        }

        public Span<byte> GetSpan(int minimumLength) => GetMemory(minimumLength).Span;

        public int MaxBufferSize => _writer.MaxBufferSize;

        public void Write(Span<byte> data)
        {
            _writer.Write(data);
        }

        public async Task FlushAsync()
        {
            await _writer.FlushAsync();
        }
    }
}
