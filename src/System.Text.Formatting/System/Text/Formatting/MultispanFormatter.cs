using System.Buffers;

namespace System.Text.Formatting
{
    public class MultispanFormatter : ITextOutput
    {
        Multispan<byte> _buffer;
        int _segmentSize;
        Span<byte> _lastFull;
        EncodingData _encoding;

        public MultispanFormatter(Multispan<byte> buffer, int segmentSize, EncodingData encoding)
        {
            _encoding = encoding;
            _segmentSize = segmentSize;
            _buffer = buffer;
            int index = _buffer.AppendNewSegment(_segmentSize); // TODO: is this the right thing to do? Should Multispan be resilient to empty segment list?
            _lastFull = _buffer.Last;
            _buffer.ResizeSegment(index, 0);
        }

        public Multispan<byte> Multispan
        {
            get { return _buffer; }
        }

        Span<byte> IOutput.Buffer
        {
            get
            {
                return _lastFull.Slice(_buffer.Last.Length);
            }
        }

        EncodingData ITextOutput.Encoding
        {
            get {
                return _encoding;
            }
        }

        void IOutput.Enlarge(int desiredBufferLength)
        {
            var newSize = _segmentSize;
            if(desiredBufferLength != 0){
                newSize = desiredBufferLength;
            }
            var index = _buffer.AppendNewSegment(newSize);
            _lastFull = _buffer.Last;
            _buffer.ResizeSegment(index, 0);
        }

        void IOutput.Advance(int bytes)
        {
            var lastSegmentCommited = _buffer.Last.Length + bytes;
            if(lastSegmentCommited > _lastFull.Length)
            {
                throw new InvalidOperationException("More bytes commited than returned from FreeBuffer");
            }
            _buffer.ResizeSegment(_buffer.Count - 1, lastSegmentCommited);
        }
    }
}
