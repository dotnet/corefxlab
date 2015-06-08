using System.IO.Buffers;

namespace System.Text.Formatting
{
    public class BufferFormatter : IFormatter
    {
        byte[] _buffer;
        int _count;

        FormattingData _formattingData;
        BufferPool _pool;

        public BufferFormatter(int capacity, FormattingData formattingData, BufferPool pool = null)
        {
            _formattingData = formattingData;
            _count = 0;
            _pool = pool;
            if(_pool == null)
            {
                _pool = BufferPool.Shared;
            }
            _buffer = _pool.RentBuffer(capacity);
        }

        public byte[] Buffer
        {
            get { return _buffer; }
        }
        public int CommitedByteCount
        {
            get { return _count; }
        }

        public void Clear()
        {
            _count = 0;
        }

        Span<byte> IFormatter.FreeBuffer
        {
            get
            {
                return _buffer.Slice(_count);
            }
        }

        FormattingData IFormatter.FormattingData
        {
            get
            {
                return _formattingData;
            }
        }

        void IFormatter.ResizeBuffer()
        {
            _pool.Enlarge(ref _buffer, _buffer.Length * 2);
        }

        void IFormatter.CommitBytes(int bytes)
        {
            _count += bytes;
            if(_count >= _buffer.Length)
            {
                throw new InvalidOperationException("More bytes commited than returned from FreeBuffer");
            }
        }
    }
}
