// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Diagnostics;
using System.Text;

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

        public SpanWriter(Span<byte> buffer, SymbolTable symbols = default(SymbolTable))
        {
            _buffer = buffer;
            _written = 0;
            _symbols = symbols == default(SymbolTable) ? SymbolTable.InvariantUtf8 : symbols;
            Newline = s_defaultNewline;
            Enlarge = null;
        }

        public void WriteBytes<T>(T value, IBufferTransformation transformation = null) where T : IWritable
        {
            var start = Index;
            int written;
            while (!value.TryWrite(Free, out written, default(ParsedFormat)))
            {
                Resize();
            }

            if (transformation != null)
            {
                var status = transformation.Transform(Buffer.Slice(start), written, out written);
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
        }

        public void WriteText<T>(T value, IBufferTransformation transformation = null) where T : IBufferFormattable
        {
            var start = Index;
            int written;
            while (!value.TryFormat(Free, out written, default(ParsedFormat), _symbols))
            {
                Resize();
            }

            if (transformation != null)
            {
                var status = transformation.Transform(Buffer.Slice(start), written, out written);
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
        }

        public void WriteLine(string text, IBufferTransformation transformation = null)
        {
            int start = Index;
            int encoded;
            while (true)
            {
                var status = Buffers.Text.Encodings.Utf16.ToUtf8(text.AsReadOnlySpan().AsBytes(), Free, out var consumed, out encoded);
                if (status == OperationStatus.Done) { _written += encoded; break; }
                if (status == OperationStatus.DestinationTooSmall) { Resize(); continue; }
                if (status == OperationStatus.InvalidData) throw new ArgumentOutOfRangeException(nameof(text));
                Debug.Assert(false, "this (NeedMoreData) should never happen");
            }
            WriteBytes(Newline);

            if (transformation != null)
            {
                encoded = Index - start;
                var status = transformation.Transform(Buffer.Slice(start), encoded, out int transformed);
                if (status == OperationStatus.Done)
                {
                    _written += transformed - encoded; // as this was an in-place transform;
                    return;
                }

                if (status == OperationStatus.InvalidData || status == OperationStatus.NeedMoreData) throw new Exception("invalid value or transformation");

                // if OperationStatus.DestinationTooSmall
                Resize();
                WriteLine(text, transformation);
            }
        }

        public void Write(string text, IBufferTransformation transformation = null)
        {
            int start = Index;
            int encoded;
            while (true)
            {
                var status = Buffers.Text.Encodings.Utf16.ToUtf8(text.AsReadOnlySpan().AsBytes(), Free, out var consumed, out encoded);
                if (status == OperationStatus.Done) { _written += encoded; break; }
                if (status == OperationStatus.DestinationTooSmall) { Resize(); continue; }
                if (status == OperationStatus.InvalidData) throw new ArgumentOutOfRangeException(nameof(text));
                Debug.Assert(false, "this (NeedMoreData) should never happen");
            }

            if (transformation != null)
            {
                var status = transformation.Transform(Buffer.Slice(start), encoded, out int transformed);
                if (status == OperationStatus.Done)
                {
                    _written += transformed - encoded; // as this was an in-place transform;
                    return;
                }

                if (status == OperationStatus.InvalidData || status == OperationStatus.NeedMoreData) throw new Exception("invalid value or transformation");

                // if OperationStatus.DestinationTooSmall
                Resize();
                WriteLine(text, transformation);
            }
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
            if (value.Length == 0) return;
            while (Free.Length < value.Length) Resize();
            value.CopyTo(Free);
            _written += value.Length;
        }

        public void WriteBytes(ReadOnlyMemory<byte> value)
        {
            if (value.Length == 0) return;
            while (Free.Length < value.Length) Resize();
            value.Span.CopyTo(Free);
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

        public override string ToString()
        {
            return Encoding.UTF8.GetString(_buffer.Slice(0, _written).ToArray(), 0, _written);
        }
    }
}
