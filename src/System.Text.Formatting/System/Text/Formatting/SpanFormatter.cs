using System.Buffers;

namespace System.Text.Formatting
{
    public class SpanFormatter : ITextOutput
    {
        Span<byte> _buffer;
        int _count;

        EncodingData _encoding;

        public SpanFormatter(Span<byte> buffer, EncodingData encoding)
        {
            _encoding = encoding;
            _count = 0;
            _buffer = buffer;
        }

        public int CommitedByteCount
        {
            get { return _count; }
        }

        public void Clear()
        {
            _count = 0;
        }

        Span<byte> IOutput.Buffer
        {
            get
            {
                return _buffer.Slice(_count);
            }
        }

        void IOutput.Enlarge(int desiredBufferLength)
        {
            throw new InvalidOperationException("cannot resize fixed size buffers.");
        }

        void IOutput.Advance(int bytes)
        {
            _count += bytes;
            if(_count > _buffer.Length)
            {
                throw new InvalidOperationException("More bytes commited than returned from FreeBuffer");
            }
        }

        EncodingData ITextOutput.Encoding
        {
            get {
                return _encoding;
            }
        }
    }
}
