using System;
using System.Buffers;
using System.Collections.Sequences;
using System.Text;
using System.Text.Formatting;
using System.Text.Http;

namespace Microsoft.Net.Http
{
    //public class HttpResponse : IDisposable
    //{
    //    ReadWriteBytes _buffers;
    //    int _written;
    //    OwnedArray<byte> _memory;
    //    int _bodyStart;
    //    bool _chunkWritten;

    //    public HttpResponse(int size)
    //    {
    //        var array = ArrayPool<byte>.Shared.Rent(size);
    //        _memory = new OwnedArray<byte>(array);
    //        _buffers = new ReadWriteBytes(_memory.Memory);
    //    }

    //    public ReadWriteBytes Buffers => _buffers;

    //    public int WrittenBytes => _written;

    //    public Memory<byte> Free => _buffers.First.Slice(_written);
    //    public Memory<byte> Written => _buffers.First.Slice(0, _written);

    //    public void Dispose()
    //    {
    //        var array = _memory.Array;
    //        _memory.Dispose();
    //        ArrayPool<byte>.Shared.Return(array);
    //        _buffers = ReadWriteBytes.Empty;
    //        _memory = null;
    //    }

    //    internal void Advance(int bytes)
    //    {
    //        _written += bytes;
    //    }

    //    public void EndOfHeaders()
    //    {
    //        _bodyStart = _written;
    //        _written += ChunkBufferSize;
    //    }

    //    public Memory<byte> Headers => Written.Slice(0, _bodyStart);

    //    public Memory<byte> Body {
    //        get {
    //            if(!_chunkWritten)
    //            {
    //                WriteChunkBuffer();

    //                var formatter = new ResponseFormatter(this);
    //                formatter.Append('\r');
    //                formatter.Append('\n');
    //                formatter.Append(0);
    //                formatter.Append('\r');
    //                formatter.Append('\n');
    //                formatter.Append('\r');
    //                formatter.Append('\n');
    //            }
    //            var len = _written - _bodyStart;
    //            return Written.Slice(_bodyStart, len);
    //        }
    //   }

    //    void WriteChunkBuffer()
    //    {
    //        var bodySize = BodySize;
    //        var buffer = ChunkBuffer.Span;
    //        if (!PrimitiveFormatter.TryFormat(bodySize, buffer, out var written, 'X', EncodingData.InvariantUtf8))
    //        {
    //            throw new Exception("cannot format chunk length");
    //        }

    //        for(int i=written - 1; i>=0; i--)
    //        {
    //            buffer[ChunkBufferSize - written + i - 2] = buffer[i];
    //        }

    //        buffer[ChunkBufferSize - 2] = 13;
    //        buffer[ChunkBufferSize - 1] = 10;

    //        _bodyStart += (ChunkBufferSize - (written + 2)); 
    //        _chunkWritten = true;
    //    }
    //    Memory<byte> ChunkBuffer => Written.Slice(_bodyStart, ChunkBufferSize);
    //    int BodySize => _written - _bodyStart - ChunkBufferSize;
    //    const int ChunkBufferSize = 10;
    //}

    //public struct ResponseFormatter : ITextOutput
    //{
    //    HttpResponse _response;

    //    public ResponseFormatter(HttpResponse response)
    //    {
    //        _response = response;
    //    }

    //    public EncodingData Encoding => EncodingData.InvariantUtf8;

    //    public Span<byte> Buffer => _response.Free.Span;

    //    public void Advance(int bytes)
    //    {
    //        _response.Advance(bytes);
    //    }

    //    public void Enlarge(int desiredBufferLength = 0)
    //    {
    //        throw new NotImplementedException("resizing not implemented");
    //    }

    //    public void AppendEoh()
    //    {
    //        this.AppendHttpNewLine();
    //        _response.EndOfHeaders();
    //    }
    //}
}
