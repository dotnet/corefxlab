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
        public ArraySegment<byte> Written => _buffer.Full;

        public EncodingData Encoding => _encoding;

        public Span<byte> Buffer => Free.Slice();

        void IOutput.Enlarge(int desiredBufferLength)
        {
            if (desiredBufferLength < 1) desiredBufferLength = 1;
            var doubleCount = _buffer.Count * 2;
            int newSize = doubleCount + desiredBufferLength;

            try {
                var newArray = _pool.Rent(newSize);
                var oldArray = _buffer.Resize(newArray);
                _pool.Return(oldArray);
            }
            catch {
                throw new Exception(string.Format("{0} {1} {2}", desiredBufferLength, _buffer.Count, newSize));
                throw;
            }

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
