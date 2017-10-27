// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.IO;
using System.Text;
using System.Buffers.Text;
using System.Threading.Tasks;

namespace System.Buffers.Adapters {
    public sealed class Utf8BufferReader : TextReader
    {
        ReadOnlyMemory<byte> _buffer;
        int _index;

        public Utf8BufferReader(ReadOnlyMemory<byte> buffer)
        {
            _buffer = buffer;
            _index = 0;
        }

        // TODO: Peek and Read need to be optimized. 
        public override int Read()
        {
            if (_index >= _buffer.Length) return -1;
            var utf8Unread = _buffer.Span.Slice(_index);
            char result = default(char);
            unsafe
            {
                var destination = new Span<char>(&result, 1).AsBytes();
                if (Encodings.Utf8.ToUtf16(utf8Unread, destination, out int consumed, out int written) == OperationStatus.InvalidData)
                {
                    throw new Exception("invalid UTF8 byte at " + _index.ToString());
                }
                _index += consumed;
            }
            return result;
        }

        public override int Peek()
        {
            if (_index >= _buffer.Length) return -1;
            var utf8Unread = _buffer.Span.Slice(_index);
            char result = default(char);
            unsafe
            {
                var destination = new Span<char>(&result, 1).AsBytes();
                if (Encodings.Utf8.ToUtf16(utf8Unread, destination, out int consumed, out int written) == OperationStatus.InvalidData)
                {
                    throw new Exception("invalid UTF8 byte at " + _index.ToString());
                }
            }
            return result;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var utf8Unread = _buffer.Span.Slice(_index);
            var utf16Buffer = buffer.AsSpan().Slice(index, count);
            if(Encodings.Utf8.ToUtf16(utf8Unread, utf16Buffer.AsBytes(), out int bytesConsumed, out int bytesWritten)== OperationStatus.InvalidData)
            {
                throw new Exception("invalid UTF8 byte at " + _index.ToString());
            }
            _index += bytesConsumed;
            return bytesWritten / sizeof(char);
        }

        public override string ReadToEnd()
        {
            var utf8Unread = _buffer.Span.Slice(_index);
            _index = _buffer.Length;
            return Encodings.Utf8.ToString(utf8Unread);
        }

        public override string ReadLine()
        {
            var utf8Unread = _buffer.Span.Slice(_index);
            var indexOfNewline = utf8Unread.IndexOf(s_newline);
            if (indexOfNewline < 0) return ReadToEnd();
            _index += indexOfNewline + s_newline.Length;
            return Encodings.Utf8.ToString(utf8Unread.Slice(0, indexOfNewline));
        }

        public override int ReadBlock(char[] buffer, int index, int count) => Read(buffer, index, count);
        public override Task<string> ReadLineAsync() => Task.FromResult(ReadLine());
        public override Task<string> ReadToEndAsync() => Task.FromResult(ReadToEnd());
        public override Task<int> ReadBlockAsync(char[] buffer, int index, int count) => Task.FromResult(ReadBlock(buffer, index, count));
        public override Task<int> ReadAsync(char[] buffer, int index, int count) => Task.FromResult(Read(buffer, index, count));

        static byte[] s_newline = Encoding.UTF8.GetBytes(Environment.NewLine);

        protected override void Dispose(bool disposing)
        {
            _buffer = default(ReadOnlyMemory<byte>);
            _index = 0;
        }
    }
}
