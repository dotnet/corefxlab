﻿using System.Buffers;

namespace System.Text.Formatting
{
    public class SpanFormatter : IFormatter
    {
        Span<byte> _buffer;
        int _count;

        FormattingData _formattingData;

        public SpanFormatter(Span<byte> buffer, FormattingData formattingData)
        {
            _formattingData = formattingData;
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

        Span<byte> IStream.AvaliableBytes
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

        bool IStream.TryEnsureAvaliable(int minimunByteCount)
        {
            if(_count > _buffer.Length - minimunByteCount) throw new Exception("End of stream");
            return true;

        }

        void IStream.Advance(int bytes)
        {
            _count += bytes;
            if(_count > _buffer.Length)
            {
                throw new InvalidOperationException("More bytes commited than returned from FreeBuffer");
            }
        }
    }
}
