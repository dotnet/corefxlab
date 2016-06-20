using System.Buffers;
using System.Text;

namespace System.Text.Formatting
{
    public class BufferFormatter : IFormatter
    {
        byte[] _buffer;
        int _count;

        FormattingData _formattingData;
        ArrayPool<byte> _pool;

        public BufferFormatter(int capacity, FormattingData formattingData, ArrayPool<byte> pool = null)
        {
            _formattingData = formattingData;
            _count = 0;
            _pool = pool;
            if(_pool == null)
            {
                _pool = ArrayPool<byte>.Shared;
            }
            _buffer = _pool.Rent(capacity);
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
                return new Span<byte>(_buffer, _count, _buffer.Length - _count);
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
            var temp = _buffer;
            _buffer = _pool.Rent(_buffer.Length * 2);
            Array.Copy(temp, 0, _buffer, 0, _count);
            _pool.Return(temp);
        }

        void IFormatter.CommitBytes(int bytes)
        {
            _count += bytes;
            if(_count > _buffer.Length)
            {
                throw new InvalidOperationException("More bytes commited than returned from FreeBuffer");
            }
        }
    }
}
