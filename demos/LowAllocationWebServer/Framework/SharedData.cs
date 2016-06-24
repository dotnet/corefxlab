using System;
using System.Buffers;
using System.Text;
using System.Text.Formatting;

namespace Microsoft.Net.Http
{
    // this is a set of buffers used to format the response.
    // it's optimized for cases where the headers and body fit into a single buffer (each)
    // TODO: I am not thrilled by the desing of the type. We should look into this more.
    public class SharedData : IDisposable
    {
        readonly static ArrayPool<byte> s_pool = ArrayPool<byte>.Shared;

        Segment _first;
        Segment _second;
        Segment[] _rest;
        int _count;

        public int Count { get { return _count; } }

        public Segment this[int index] {
            get {
                if (index == 0) return _first;
                if (index == 1) return _second;
                return _rest[index - 2];
            }
        }

        public void Dispose()
        {
            if (_first.Array.Array != null) s_pool.Return(_first.Array.Array);
            if (_second.Array.Array != null) s_pool.Return(_second.Array.Array);
            if (_rest != null) {
                for (int i = 0; i < _count - 2; i++) {
                    if (_rest[i].Array.Array != null) s_pool.Return(_rest[i].Array.Array);
                }
                ArrayPool<Segment>.Shared.Return(_rest);
            }
        }

        public Span<byte> GetFree(int id)
        {
            var last = GetLast(id);
            if (last == null) return Span<byte>.Empty;
            return last.Value.Free;
        }

        private Segment? GetLast(int id)
        {
            if (_rest != null) {
                for (int index = _count - 1; index >= 0; index--) {
                    if (_rest[index].Id == id) {
                        return _rest[index];
                    }
                }
            }
            if (_second.Id == id) return _second;
            if (_first.Id == id) return _first;
            return null;
        }

        private int GetLastIndex(int id)
        {
            if (_rest != null) {
                for (int index = _count - 1; index >= 0; index--) {
                    if (_rest[index].Id == id) {
                        return index;
                    }
                }
            }
            if (_second.Id == id) return -1;
            if (_first.Id == id) return -2;
            return -3;
        }

        public void Allocate(int size, int id)
        {
            var newArray = new ArraySegment<byte>(s_pool.Rent(size), 0, 0);
            var newSegment = new Segment(newArray, id);
            if (_count == 0) _first = newSegment;
            else if (_count == 1) _second = newSegment;
            else {
                if (_count == 2 || _rest.Length == _count - 2) {
                    var newSize = _count==2?4:_rest.Length << 1;
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

        public void Commit(int id, int bytes)
        {
            var freeIndex = GetLastIndex(id);
            if (freeIndex < -2) {
                throw new AggregateException("invalid id");
            }

            if (freeIndex == -2) {
                var segment = _first.Array;
                _first.Array = new ArraySegment<byte>(segment.Array, 0, segment.Count + bytes);
            }
            else if (freeIndex == -1) {
                var segment = _second.Array;
                _second.Array = new ArraySegment<byte>(segment.Array, 0, segment.Count + bytes);
            }
            else {
                var segment = _rest[freeIndex].Array;
                _rest[freeIndex].Array = new ArraySegment<byte>(segment.Array, 0, segment.Count + bytes);
            }
        }

        internal int Commited(int id)
        {
            int total = 0;
            if (_rest != null) {
                for (int i = 0; i < _count - 2; i++) {
                    if (_rest[i].Id == id) {
                        total += _rest[i].Array.Count;
                    }
                }
            }
            if (_first.Id == id) total += _first.Array.Count;
            if (_second.Id == id) total += _second.Array.Count;
            return total;
        }

        public struct Segment
        {
            internal ArraySegment<byte> Array;
            public int Id;

            public Segment(ArraySegment<byte> array, int id)
            {
                Array = array;
                Id = id;
            }

            public Span<byte> Commited
            {
                get { return Array.Array.Slice(Array.Offset, Array.Count); }
            }

            public Span<byte> Free
            {
                get {
                    var start = Array.Offset + Array.Count;
                    return Array.Array.Slice(start, Array.Array.Length - start);
                }
            }
        }
    }

    public struct HttpResponse
    {
        SharedData _data;

        public HttpResponse(SharedData responseData)
        {
            _data = responseData;
        }

        public ResponseFormatter Body { get { return new ResponseFormatter(_data, 1); } }
        public ResponseFormatter Headers { get { return new ResponseFormatter(_data, 2); } }
    }

    public struct ResponseFormatter : IFormatter
    {
        int _order;
        SharedData _data;

        public ResponseFormatter(SharedData data, int order)
        {
            _data = data;
            _order = order;
        }

        public int CommitedBytes { get { return _data.Commited(_order); } }

        public FormattingData FormattingData {
            get {
                return FormattingData.InvariantUtf8;
            }
        }

        public Span<byte> FreeBuffer {
            get {
                var lastFree = _data.GetFree(_order);
                return lastFree;
            }
        }

        public void CommitBytes(int bytes)
        {
            _data.Commit(_order, bytes);
        }

        public void ResizeBuffer()
        {
            _data.Allocate(1024, _order);
        }
    }
}
