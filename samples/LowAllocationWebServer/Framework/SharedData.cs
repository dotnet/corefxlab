using System;
using System.Buffers;
using System.Collections.Sequences;
using System.Text;
using System.Text.Formatting;

namespace Microsoft.Net.Http
{
    public struct HttpResponse : IDisposable
    {
        LinkedBuffer _headers;
        LinkedBuffer _body;

        public HttpResponse(int size)
        {
            _headers = new LinkedBuffer();
            _headers.Allocate(size / 2);
            _body = new LinkedBuffer();
            _body.Allocate(size / 2);
        }

        public int? Length => _body.CommitedBytes + _headers.CommitedBytes;

        public LinkedBuffer Headers => _headers;


        public LinkedBuffer Body => _body;

        public void Dispose()
        {
            _headers.Dispose();
            _body.Dispose();
        }
    }

    public struct ResponseFormatter : ITextOutput
    {
        LinkedBuffer _buffer;

        public ResponseFormatter(LinkedBuffer buffer)
        {
            _buffer = buffer;
        }

        public int WrittenBytes => _buffer.CommitedBytes;

        public EncodingData Encoding => EncodingData.InvariantUtf8;

        public Span<byte> Buffer => _buffer.Free.Span;

        public void Advance(int bytes) => _buffer.Commit(bytes);

        public void Enlarge(int desiredBufferLength = 0)
        {
            _buffer.Allocate(desiredBufferLength == 0 ? 1024 : desiredBufferLength);
        }
    }

    public class LinkedBuffer : ISequence<Memory<byte>>
    {
        Segment _head;
        Segment _last;
        int _commited;

        public int CommitedBytes => _commited;

        public Memory<byte> Free => _last.Free;

        public int? Length => _commited;

        internal void Allocate(int bytes)
        {
            var newSegment = new Segment(new byte[1024], _last);
            if (_head == null) _head = newSegment;
            _last = newSegment;
        }

        internal void Commit(int bytes)
        {
            _commited += bytes;
            _last.Commit(bytes);
        }

        internal void Dispose()
        {
            _head.Dispose();
        }

        public bool TryGet(ref Position position, out Memory<byte> item, bool advance = false)
        {
            if (position == Position.First) {
                item = _head.Commited;
                if (advance) {
                    if(_head._next == null) {
                        position = Position.AfterLast;
                    }
                    else {
                        position.ObjectPosition = _head._next;
                    }
                }
                return true;
            }

            if (position == Position.AfterLast) {
                item = Memory<byte>.Empty;
                return false;
            }

            var segment = position.ObjectPosition as Segment;
            item = segment.Memory;
            if (advance) {
                if(segment._next == null) {
                    position = Position.AfterLast;
                }
                else {
                    position.ObjectPosition = segment._next;
                }
            }
            return true;
        }

        public SequenceEnumerator<Memory<byte>> GetEnumerator()
        {
            return new SequenceEnumerator<Memory<byte>>(this);
        }
    }

    public class Segment : OwnedPinnedArray<byte>
    {
        int _commited;
        internal Segment _next;

        public Segment(byte[] array, Segment previous = null) : base(array)
        {
            if (previous != null) {
                previous._next = this;
            }
        }

        public void Commit(int bytes)
        {
            _commited += bytes;
        }

        public Memory<byte> Commited => Memory.Slice(0, _commited);

        public Memory<byte> Free => Memory.Slice(_commited);
    }
}
