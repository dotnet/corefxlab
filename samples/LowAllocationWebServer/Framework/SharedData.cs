using System;
using System.Buffers;
using System.Text;
using System.Text.Formatting;

namespace Microsoft.Net.Http
{
    public struct HttpResponse
    {
        ResponseBuffer _buffer;

        public HttpResponse(ResponseBuffer responseBuffer)
        {
            _buffer = responseBuffer;
        }

        public ResponseFormatter Body { get { return new ResponseFormatter(_buffer, segmentId: 1); } }
        public ResponseFormatter Headers { get { return new ResponseFormatter(_buffer, segmentId: 2); } }
    }

    public struct ResponseFormatter : ITextOutput
    {
        int _segmentId;
        ResponseBuffer _buffer;

        public ResponseFormatter(ResponseBuffer buffer, int segmentId)
        {
            _buffer = buffer;
            this._segmentId = segmentId;
        }

        public int WrittenBytes => _buffer.GetCommitedBytes(_segmentId);

        public EncodingData Encoding => EncodingData.InvariantUtf8;

        public Span<byte> Buffer => _buffer.GetFree(_segmentId).Span;

        public void Advance(int bytes) => _buffer.Commit(_segmentId, bytes);

        public void Enlarge(int desiredBufferLength = 0)
        {
            _buffer.Allocate(desiredBufferLength == 0 ? 1024 : desiredBufferLength, _segmentId);
        }
    }

    // this is a set of buffers used to format the response.
    // it's optimized for cases where the headers and body fit into a single buffer (each)
    // TODO: I am not thrilled by the desing of the type. We should look into this more.
    public class ResponseBuffer : IDisposable
    {
        readonly static ArrayPool<byte> s_pool = ArrayPool<byte>.Shared;

        Segment _first;  // usually used for response line and headers
        Segment _second; // usually used for body
        Segment[] _rest; // usually used when the body is very long
        int _count;

        public int Count { get { return _count; } }

        public Segment this[int index]
        {
            get {
                if (index == 0) return _first;
                if (index == 1) return _second;
                return _rest[index - 2];
            }
        }

        public void Dispose()
        {
            _first.FreeMemory(s_pool);
            _second.FreeMemory(s_pool);
            if (_rest != null) {
                for (int i = 0; i < _count - 2; i++) {
                    _rest[i].FreeMemory(s_pool);
                }
                ArrayPool<Segment>.Shared.Return(_rest);
            }
        }

        public Memory<byte> GetFree(int segmentId)
        {
            var last = GetLast(segmentId);
            if (last == null) return Memory<byte>.Empty;
            return last.Value.Free;
        }

        private Segment? GetLast(int segmentId)
        {
            if (_rest != null) {
                for (int index = _count - 1; index >= 0; index--) {
                    if (_rest[index].Id == segmentId) {
                        return _rest[index];
                    }
                }
            }
            if (_second.Id == segmentId) return _second;
            if (_first.Id == segmentId) return _first;
            return null;
        }

        private int GetLastIndex(int segmentId)
        {
            if (_rest != null) {
                for (int index = _count - 1; index >= 0; index--) {
                    if (_rest[index].Id == segmentId) {
                        return index;
                    }
                }
            }
            if (_second.Id == segmentId) return -1;
            if (_first.Id == segmentId) return -2;
            return -3;
        }

        public void Allocate(int size, int segmentId)
        {
            var newArray = s_pool.Rent(size);
            var newSegment = new Segment(newArray, segmentId);
            if (_count == 0) _first = newSegment;
            else if (_count == 1) _second = newSegment;
            else {
                if (_count == 2 || _rest.Length == _count - 2) {
                    var newSize = _count == 2 ? 4 : _rest.Length << 1;
                    var largerRest = ArrayPool<Segment>.Shared.Rent(newSize);
                    if (_rest != null) {
                        _rest.CopyTo(largerRest, 0);
                        ArrayPool<Segment>.Shared.Return(_rest);
                    }
                    _rest = largerRest;
                }
                _rest[_count - 2] = newSegment;
            }
            _count++;
        }

        public void Commit(int segmentId, int bytes)
        {
            var freeIndex = GetLastIndex(segmentId);
            if (freeIndex < -2) {
                throw new AggregateException("invalid id");
            }

            if (freeIndex == -2) {
                _first.Commit(bytes);
            }
            else if (freeIndex == -1) {
                _second.Commit(bytes);
            }
            else {
                _rest[freeIndex].Commit(bytes);
            }
        }

        internal int GetCommitedBytes(int segmentId)
        {
            int total = 0;
            if (_rest != null) {
                for (int i = 0; i < _count - 2; i++) {
                    if (_rest[i].Id == segmentId) {
                        total += _rest[i].CommitedBytes;
                    }
                }
            }
            if (_first.Id == segmentId) total += _first.CommitedBytes;
            if (_second.Id == segmentId) total += _second.CommitedBytes;
            return total;
        }

        public struct Segment
        {
            internal OwnedMemory<byte> _owned;
            int _commited;
            public int Id;

            public Segment(OwnedMemory<byte> memory, int id)
            {
                _owned = memory;
                _commited = 0;
                Id = id;
            }

            public void Commit(int bytes)
            {
                _commited += bytes;
            }

            public Memory<byte> Commited =>_owned.Memory.Slice(0, _commited); 

            public Memory<byte> Free => _owned.Memory.Slice(_commited);

            public int CommitedBytes => _commited;

            public void FreeMemory(ArrayPool<byte> pool)
            {
                throw new NotImplementedException();
            }
        }
    }
}
