using Microsoft.Net.Sockets;
using System;
using System.Buffers;
using System.Diagnostics;
using System.Text.Formatting;
using System.Text.Http;
using System.Text.Utf8;
using System.Threading;

namespace Microsoft.Net.Http
{
    public struct HttpResponse
    {
        SharedData _data;

        public HttpResponse(SharedData responseData)
        {
            _data = responseData;
        }

        public ResponseFormatter Body { get { return new ResponseFormatter(_data, 1); } }
        public ResponseFormatter Headers { get { return new ResponseFormatter(_data, 0); } }
    }

    public class SharedData : IDisposable
    {
        ArraySegment<byte>[] _segments = new ArraySegment<byte>[0];
        int[] _ids = new int[0];
        int _count;

        public int Count { get { return _count; } }

        public Tuple<int, Span<byte>> this[int index] {
            get {
                var segment = _segments[index];
                return Tuple.Create(_ids[index], segment.Array.Slice(segment.Offset, segment.Count));
            }
        }

        public void Dispose()
        {
            ArraySegment<byte>[] local;
            lock (_segments) {
                local = _segments;
                _segments = null;
            }

            for(int i=0; i<_count; i++) {
                ArrayPool<byte>.Shared.Return(local[i].Array);
            }
        }

        public Span<byte> GetFree(int id)
        {
            var freeIndex = GetFreeIndex(id);
            if (freeIndex < 0) return Span<byte>.Empty;
            var segment = _segments[freeIndex];
            return segment.Array.Slice(segment.Count, segment.Array.Length - segment.Count);
        }

        private int GetFreeIndex(int id)
        {
            for (int index = _count - 1; index >= 0; index--) {
                if (_ids[index] == id) {
                    return index;
                }
            }
            return -1;
        }

        public void Allocate(int size, int id)
        {
            lock (_segments) {
                if (_count == _segments.Length) {
                    var newSize = Math.Max(4, _segments.Length << 1);
                    // TODO: this SharedData array allocation should be eliminated.
                    // if response has one header buffer and one body buffer, the buffers should be stored in dedicated fields.
                    // otherwise, the arrays should come from the pool
                    var largerSegments = new ArraySegment<byte>[newSize]; 
                    var largerIds = new int[newSize];
                    _segments.CopyTo(largerSegments, 0);
                    _ids.CopyTo(largerIds, 0);
                    _segments = largerSegments;
                    _ids = largerIds;
                }

                _segments[_count] = new ArraySegment<byte>(ArrayPool<byte>.Shared.Rent(size), 0, 0);
                _ids[_count++] = id;
            }
        }

        public void Commit(int id, int bytes)
        {
            var freeIndex = GetFreeIndex(id);
            if (freeIndex < 0) {
                throw new AggregateException("invalid id");
            }
            var segment = _segments[freeIndex];
            _segments[freeIndex] = new ArraySegment<byte>(segment.Array, 0, segment.Count + bytes);
        }

        internal int Commited(int id)
        {
            int total = 0;
            for(int i=0; i<_count; i++) {
                if (_ids[i] == id) {
                    total += _segments[i].Count;
                }
            }
            return total;
        }
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
