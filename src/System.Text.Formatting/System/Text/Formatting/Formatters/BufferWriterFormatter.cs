// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;

namespace System.Text.Formatting
{
    public struct BufferWriterFormatter<TOutput> : ITextBufferWriter where TOutput : IBufferWriter<byte>
    {
        TOutput _output;
        SymbolTable _symbolTable;

        public BufferWriterFormatter(TOutput output, SymbolTable symbolTable)
        {
            _output = output;
            _symbolTable = symbolTable;
        }

        public BufferWriterFormatter(TOutput output) : this(output, SymbolTable.InvariantUtf8)
        {
        }

        public SymbolTable SymbolTable => _symbolTable;

        public void Advance(int bytes) => _output.Advance(bytes);

        public Memory<byte> GetMemory(int minimumLength = 0)
        {
            return _output.GetMemory(minimumLength);
        }

        public Span<byte> GetSpan(int minimumLength)
        {
            return _output.GetSpan(minimumLength);
        }
    }
}
