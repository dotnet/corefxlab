// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Sequences;

namespace System.Text.Formatting
{
    public static class SequenceFormatterExtensions
    {
        public static SequenceFormatter<TSequence> CreateFormatter<TSequence>(this TSequence sequence, SymbolTable symbolTable = null) where TSequence : ISequence<Memory<byte>>
        {
            return new SequenceFormatter<TSequence>(sequence, symbolTable);
        }
    }

    public class SequenceFormatter<TSequence> : ITextOutput where TSequence : ISequence<Memory<byte>>
    {
        ISequence<Memory<byte>> _buffers;
        SymbolTable _symbolTable;

        Position _currentPosition = default;
        int _currentWrittenBytes;
        Position _previousPosition = Position.Infinity;
        int _previousWrittenBytes;
        int _totalWritten;

        public SequenceFormatter(TSequence buffers, SymbolTable symbolTable)
        {
            _symbolTable = symbolTable;
            _buffers = buffers;
            _previousWrittenBytes = -1;
        }

        Span<byte> IOutput.Buffer
        {
            get {
                return Current.Span.Slice(_currentWrittenBytes);
            }
        }

        private Memory<byte> Current {
            get {
                if (!_buffers.TryGet(ref _currentPosition, out Memory<byte> result, advance: false)) { throw new InvalidOperationException(); }
                return result;
            }
        }
        private Memory<byte> Previous
        {
            get {
                if (!_buffers.TryGet(ref _previousPosition, out Memory<byte> result, advance: false)) { throw new InvalidOperationException(); }
                return result;
            }
        }
        private bool NeedShift => _previousWrittenBytes != -1;

        SymbolTable ITextOutput.SymbolTable => _symbolTable;

        public int TotalWritten => _totalWritten;

        void IOutput.Enlarge(int desiredBufferLength)
        {
            if (NeedShift) throw new NotImplementedException("need to allocate temp array");

            _previousPosition = _currentPosition;
            _previousWrittenBytes = _currentWrittenBytes;

            if (!_buffers.TryGet(ref _currentPosition, out Memory<byte> span))
            {
                throw new InvalidOperationException();
            }
            _currentWrittenBytes = 0;
        }

        void IOutput.Advance(int bytes)
        {
            var current = Current;
            if (NeedShift) {
                var previous = Previous;
                var spaceInPrevious = previous.Length - _previousWrittenBytes;
                if(spaceInPrevious < bytes) {
                    current.Slice(0, spaceInPrevious).Span.CopyTo(previous.Span.Slice(_previousWrittenBytes));
                    current.Slice(spaceInPrevious, bytes - spaceInPrevious).Span.CopyTo(current.Span);
                    _previousWrittenBytes = -1;
                    _currentWrittenBytes = bytes - spaceInPrevious;
                }
                else {
                    current.Slice(0, bytes).Span.CopyTo(previous.Span.Slice(_previousWrittenBytes));
                    _currentPosition = _previousPosition;
                    _currentWrittenBytes = _previousWrittenBytes + bytes;
                }

            }
            else {
                if (current.Length - _currentWrittenBytes < bytes) throw new NotImplementedException();
                _currentWrittenBytes += bytes;
            }

            _totalWritten += bytes;
        }
    }
}
