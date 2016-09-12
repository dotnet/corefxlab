﻿using System;
using System.Buffers;

namespace System.Text.Formatting
{
    public class MultispanFormatter : IFormatter
    {
        Multispan<byte> _buffer;
        int _segmentSize;
        Span<byte> _lastFull;
        FormattingData _formattingData;

        public MultispanFormatter(Multispan<byte> buffer, int segmentSize, FormattingData formattingData)
        {
            _formattingData = formattingData;
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

        Span<byte> IStream.AvaliableBytes
        {
            get
            {
                return _lastFull.Slice(_buffer.Last.Length);
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
            var newSize = _segmentSize;
            if(newSize < minimunByteCount) newSize = minimunByteCount;
            var index = _buffer.AppendNewSegment(newSize);
            _lastFull = _buffer.Last;
            _buffer.ResizeSegment(index, 0);

            return true;
        }

        void IStream.Advance(int bytes)
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
