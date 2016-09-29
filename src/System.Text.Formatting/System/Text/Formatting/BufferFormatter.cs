using System.Buffers;
using System.Collections.Sequences;

namespace System.Text.Formatting
{
    public class BufferFormatter : ITextOutput
    {
        ResizableArray<byte> _buffer;
        EncodingData _encoding;
        ArrayPool<byte> _pool;

        public BufferFormatter(int capacity, EncodingData encoding, ArrayPool<byte> pool = null)
        {
            _pool = pool != null ? pool : ArrayPool<byte>.Shared;
            _encoding = encoding;
            _buffer = new ResizableArray<byte>(_pool.Rent(capacity));
        }

        public int CommitedByteCount => _buffer.Count;

        public void Clear() {
            _buffer._count = 0;
        }

        public ArraySegment<byte> Free => _buffer.Free;
        public ArraySegment<byte> Formatted => _buffer.Items;

        public EncodingData Encoding => _encoding;

        public Span<byte> Buffer => Free.Slice();

        void IOutput.Enlarge(int desiredBufferLength)
        {
            if (desiredBufferLength < 1) desiredBufferLength = 1;
            var doubleCount = _buffer.Free.Count * 2;
            int newSize = desiredBufferLength>doubleCount?desiredBufferLength:doubleCount;
            var newArray = _pool.Rent(newSize + _buffer.Count);
            var oldArray = _buffer.Resize(newArray);
            _pool.Return(oldArray);
        }

        void IOutput.Advance(int bytes)
        {
            _buffer._count += bytes;
            if(_buffer._count > _buffer.Count)
            {
                throw new InvalidOperationException("More bytes commited than returned from FreeBuffer");
            }
        }
    }
}
