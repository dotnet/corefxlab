// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Diagnostics;
using System.Text.Utf8;

namespace System.Buffers.Text
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

        public void WriteBytes<T>(T value, StandardFormat format = default, IBufferTransformation transformation = null) where T : IWritable
        {
            int written;
            while (!value.TryWrite(Free, out written, format))
            {
                Resize();
            }

            if (transformation == null)
            {
                _written += written;
                return;
            }

            if (!TryApplyTransformation(transformation, written)) {
                Resize();
                WriteBytes(value, format, transformation);
            }
        }

        public void WriteBytes<T>(T value, StandardFormat format, ReadOnlySpan<IBufferTransformation> transformations) where T : IWritable
        {
            int written;
            while (!value.TryWrite(Free, out written, format))
            {
                Resize();
            }

            if (!TryApplyTransformations(transformations, written))
            {
                Resize();
                WriteBytes(value, format, transformations);
            }
        }

        public void WriteText<T>(T value, StandardFormat format = default, IBufferTransformation transformation = null) where T : IBufferFormattable
        {
            int written;
            while (!value.TryFormat(Free, out written, format, _symbols))
            {
                Resize();
            }

            if (transformation == null)
            {
                _written += written;
                return;
            }

            if (!TryApplyTransformation(transformation, written))
            {
                Resize();
                WriteText(value, format, transformation);
            }
        }

        public void WriteText<T>(T value, StandardFormat format, ReadOnlySpan<IBufferTransformation> transformations) where T : IBufferFormattable
        {
            int written;
            while (!value.TryFormat(Free, out written, format, _symbols))
            {
                Resize();
            }

            if (!TryApplyTransformations(transformations, written))
            {
                Resize();
                WriteText(value, format, transformations);
            }
        }

        public void WriteLine(string text, IBufferTransformation transformation = null)
        {
            int encoded = WriteCore(text);
            encoded += CopyBytesAt(encoded, Newline);
        
            if(transformation == null)
            {
                _written += encoded;
                return;
            }

            if (!TryApplyTransformation(transformation, encoded)) {
                Resize();
                WriteLine(text, transformation);
            }
        }

        public void WriteLine(string text, ReadOnlySpan<IBufferTransformation> transformations)
        {
            int encoded = WriteCore(text);
            encoded += CopyBytesAt(encoded, Newline);

            if (!TryApplyTransformations(transformations, encoded))
            {
                Resize();
                WriteLine(text, transformations);
            }
        }

        public void Write(string text, IBufferTransformation transformation = null)
        {
            int encoded = WriteCore(text);

            if (transformation == null)
            {
                _written += encoded;
                return;
            }

            if (!TryApplyTransformation(transformation, encoded)) {
                Resize();
                Write(text, transformation);
            }
        }

        public void Write(string text, ReadOnlySpan<IBufferTransformation> transformations)
        {
            int encoded = WriteCore(text);

            if (!TryApplyTransformations(transformations, encoded))
            {
                Resize();
                Write(text, transformations);
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

            if (!TryApplyTransformation(transformation, encoded)) {
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

            if (!TryApplyTransformation(transformation, encoded))
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

        public void Write(DateTime date, StandardFormat format)
        {
            int bytesWritten;
            while (!CustomFormatter.TryFormat(date, Free, out bytesWritten, format, _symbols)) Resize();
            _written += bytesWritten;
        }
        public void WriteLine(DateTime date, StandardFormat format)
        {
            int bytesWritten;
            while (!CustomFormatter.TryFormat(date, Free, out bytesWritten, format, _symbols)) Resize();
            _written += bytesWritten;
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
            if (Enlarge == null) throw new BufferTooSmallException();
            var newBuffer = Enlarge(_buffer.Length + 1);
            if (newBuffer.Length <= _buffer.Length) throw new Exception("Enlarge delegate created too small buffer");
            _buffer.CopyTo(newBuffer.Span);
            _buffer = newBuffer.Span;
        }

        private bool TryApplyTransformation(IBufferTransformation transformation, int dataLength)
        {
            Debug.Assert(transformation != null);
            var status = transformation.Transform(Buffer.Slice(Index), dataLength, out dataLength);
            if (status == OperationStatus.Done)
            {
                _written += dataLength;
                return true;
            }

            if (status == OperationStatus.InvalidData || status == OperationStatus.NeedMoreData) throw new Exception("invalid value or transformation");

            return true;
        }

        private bool TryApplyTransformations(ReadOnlySpan<IBufferTransformation> transformations, int dataLength)
        {
            var buffer = Free;
            for (int i = 0; i < transformations.Length; i++)
            {
                var status = transformations[i].Transform(buffer, dataLength, out dataLength);
                if (status == OperationStatus.Done) continue;
                if (status == OperationStatus.DestinationTooSmall) return false;
                throw new Exception("invalid value or transformation");
            }

            _written += dataLength;
            return true;
        }

        private int CopyBytesAt(int offset, ReadOnlyMemory<byte> value)
        {
            while (!value.Span.TryCopyTo(Free.Slice(offset))) Resize();
            return value.Length;
        }

        private int WriteCore(string text)
        {
            if (_symbols == SymbolTable.InvariantUtf8)
            {
                while (true)
                {
                    var status = Buffers.Text.Encodings.Utf16.ToUtf8(text.AsReadOnlySpan().AsBytes(), Free, out var consumed, out var encoded);
                    if (status == OperationStatus.Done) { return encoded; }
                    if (status == OperationStatus.DestinationTooSmall) { Resize(); continue; }
                    if (status == OperationStatus.InvalidData) throw new ArgumentOutOfRangeException(nameof(text));
                    Debug.Assert(false, "this (NeedMoreData) should never happen");
                }
            }
            else
            {
                int encoded;
                while (!_symbols.TryEncode(text.AsReadOnlySpan(), Free, out var consumed, out encoded)) Resize();
                return encoded;
            }
        }

        public override string ToString()
        {
            return Encodings.Utf8.ToString(_buffer.Slice(0, _written));
        }

        public class BufferTooSmallException : Exception {
            public BufferTooSmallException() : base("Buffer is too small") { }
        }
    }
}
