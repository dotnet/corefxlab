// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Diagnostics;
using System.Text;
using System.Text.Utf8;

namespace System.Buffers
{
    public ref struct SpanWriter
    {
        Span<byte> _buffer;
        int _written;
        SymbolTable _symbols;

        public ReadOnlyMemory<byte> Newline;
        public Func<int, Memory<byte>> Enlarge;

        static ReadOnlyMemory<byte> s_defaultNewline = new byte[] { (byte)'\n' };

        public SpanWriter(Span<byte> buffer, SymbolTable symbols = default)
        {
            _buffer = buffer;
            _written = 0;
            _symbols = symbols == default(SymbolTable) ? SymbolTable.InvariantUtf8 : symbols;
            Newline = s_defaultNewline;
            Enlarge = null;
        }

        public void WriteBytes<T>(T value, IBufferTransformation transformation = null) where T : IWritable
        {
            int written;
            while (!value.TryWrite(Free, out written, default))
            {
                Resize();
            }

            if (transformation == null)
            {
                _written += written;
                return;
            }

            var status = transformation.Transform(Free, written, out written);
            if (status == OperationStatus.Done)
            {
                _written += written;
                return;
            }

            if (status == OperationStatus.InvalidData || status == OperationStatus.NeedMoreData) throw new Exception("invalid value or transformation");
            // if OperationStatus.DestinationTooSmall
            Resize();
            WriteBytes(value, transformation);  
        }

        public void WriteText<T>(T value, IBufferTransformation transformation = null) where T : IBufferFormattable
        {
            int written;
            while (!value.TryFormat(Free, out written, default(ParsedFormat), _symbols))
            {
                Resize();
            }

            if (transformation == null)
            {
                _written += written;
                return;
            }

            var status = transformation.Transform(Free, written, out written);
            if (status == OperationStatus.Done)
            {
                _written += written;
                return;
            }

            if (status == OperationStatus.InvalidData || status == OperationStatus.NeedMoreData) throw new Exception("invalid value or transformation");
            // if OperationStatus.DestinationTooSmall
            Resize();
            WriteText(value, transformation);
            
        }

        public void WriteLine(string text, IBufferTransformation transformation = null)
        {
            int encoded;
            if (_symbols == SymbolTable.InvariantUtf8)
            {
                while (true)
                {
                    var status = Buffers.Text.Encodings.Utf16.ToUtf8(text.AsReadOnlySpan().AsBytes(), Free, out var consumed, out encoded);
                    if (status == OperationStatus.Done) { break; }
                    if (status == OperationStatus.DestinationTooSmall) { Resize(); continue; }
                    if (status == OperationStatus.InvalidData) throw new ArgumentOutOfRangeException(nameof(text));
                    Debug.Assert(false, "this (NeedMoreData) should never happen");
                }
            }
            else
            {
                while (!_symbols.TryEncode(text.AsReadOnlySpan(), Free, out var consumed, out encoded)) Resize();
            }

            encoded += CopyBytesAt(encoded, Newline);
        
            if(transformation == null)
            {
                _written += encoded;
                return;
            }

            if (!TryApplyTransformation(Index, encoded, transformation)) {
                Resize();
                WriteLine(text, transformation);
            }
        }

        public void Write(string text, IBufferTransformation transformation = null)
        {
            int encoded;

            if (_symbols == SymbolTable.InvariantUtf8)
            {
                while (true)
                {
                    var status = Buffers.Text.Encodings.Utf16.ToUtf8(text.AsReadOnlySpan().AsBytes(), Free, out var consumed, out encoded);
                    if (status == OperationStatus.Done) { break; }
                    if (status == OperationStatus.DestinationTooSmall) { Resize(); continue; }
                    if (status == OperationStatus.InvalidData) throw new ArgumentOutOfRangeException(nameof(text));
                    Debug.Assert(false, "this (NeedMoreData) should never happen");
                }
            }
            else
            {
                while (!_symbols.TryEncode(text.AsReadOnlySpan(), Free, out var consumed, out encoded)) Resize();
            }

            if (transformation == null)
            {
                _written += encoded;
                return;
            }

            if (!TryApplyTransformation(Index, encoded, transformation)) {
                Resize();
                Write(text, transformation);
            }
        }

        public void Write(Utf8Span text, IBufferTransformation transformation = null)
        {
            int encoded = text.Bytes.Length;
            if (_symbols == SymbolTable.InvariantUtf8)
                while (!text.Bytes.TryCopyTo(Free)) Resize();     
            else
                while (!_symbols.TryEncode(text.Bytes, Free, out var consumed, out encoded)) Resize();

            if (transformation == null)
            {
                _written += encoded;
                return;
            }

            if (!TryApplyTransformation(Index, encoded, transformation)) {
                Resize();
                Write(text, transformation);
            }
        }

        public void WriteLine(Utf8Span text, IBufferTransformation transformation = null)
        {
            int encoded = text.Bytes.Length;
            if (_symbols == SymbolTable.InvariantUtf8)
                while (!text.Bytes.TryCopyTo(Free)) Resize();
            else
                while (!_symbols.TryEncode(text.Bytes, Free, out var consumed, out encoded)) Resize();

            encoded += CopyBytesAt(encoded, Newline);

            if (transformation == null)
            {
                _written += encoded;
                return;
            }

            if (!TryApplyTransformation(Index, encoded, transformation))
            {
                Resize();
                Write(text, transformation);
            }
        }

        public void Write(Utf8String text, IBufferTransformation transformation = null)
        {
            Write((Utf8Span)text, transformation);
        }

        public void WriteLine(Utf8String text, IBufferTransformation transformation = null)
        {
            WriteLine((Utf8Span)text, transformation);
        }

        public void Write(char character)
        {
            if (character > 127) throw new NotImplementedException();
            if (Free.Length < 1) Resize();
            Free[0] = (byte)character;
            _written++;
        }

        public void Write(DateTime date, ParsedFormat format)
        {
            int written;
            while (!date.TryFormat(Free, out written, format, _symbols)) Resize();
            _written += written;
        }
        public void WriteLine(DateTime date, ParsedFormat format)
        {
            int written;
            while (!date.TryFormat(Free, out written, format, _symbols)) Resize();
            _written += written;
            WriteBytes(Newline);
        }

        public void WriteBytes(ReadOnlySpan<byte> value)
        {
            while (!value.TryCopyTo(Free)) Resize();
            _written += value.Length; 
        }

        public void WriteBytes(ReadOnlyMemory<byte> value)
        {
            while (!value.Span.TryCopyTo(Free)) Resize();
            _written += value.Length; 
        }

        public Span<byte> Free => _buffer.Slice(_written);
        public Span<byte> Written => _buffer.Slice(0, _written);
        public Span<byte> Buffer => _buffer;

        public int Index
        {
            get { return _written; }
            set {
                while (value > _buffer.Length) Resize();
                _written = value;
            }
        }

        private void Resize()
        {
            if (Enlarge == null) throw new Exception("buffer too small");
            var newBuffer = Enlarge(_buffer.Length + 1);
            if (newBuffer.Length <= _buffer.Length) throw new Exception("Enlarge delegate created too small buffer");
            _buffer.CopyTo(newBuffer.Span);
            _buffer = newBuffer.Span;
        }

        private bool TryApplyTransformation(int index, int length, IBufferTransformation transformation)
        {
            Debug.Assert(transformation != null);
            var status = transformation.Transform(Buffer.Slice(index), length, out int transformed);
            if (status == OperationStatus.Done)
            {
                _written += transformed;
                return true;
            }

            if (status == OperationStatus.InvalidData || status == OperationStatus.NeedMoreData) throw new Exception("invalid value or transformation");
            
            return true;
        }

        private bool TryApplyTransformations(int index, int length, ReadOnlySpan<IBufferTransformation> transformations)
        {
            var buffer = Buffer.Slice(index);
            for (int i = 0; i < transformations.Length; i++)
            {
                var status = transformations[i].Transform(buffer, length, out length);
                if (status == OperationStatus.Done) continue;
                if (status == OperationStatus.DestinationTooSmall) return false;
                throw new Exception("invalid value or transformation");
            }

            _written += length;
            return true;
        }

        private int CopyBytesAt(int offset, ReadOnlyMemory<byte> value)
        {
            while (!value.Span.TryCopyTo(Free.Slice(offset))) Resize();
            return value.Length;
        }

        public override string ToString()
        {
            return Encodings.Utf8.ToString(_buffer.Slice(0, _written));
        }
    }
}
