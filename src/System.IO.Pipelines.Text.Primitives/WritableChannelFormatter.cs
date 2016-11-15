using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Formatting;
using System.Text;

namespace System.IO.Pipelines.Text.Primitives
{
    public class WritableChannelFormatter : ITextOutput
    {
        private readonly IPipelineWriter _channel;
        private WritableBuffer _writableBuffer;
        private bool _needAlloc = true;

        public WritableChannelFormatter(IPipelineWriter channel, EncodingData encoding)
        {
            _channel = channel;
            Encoding = encoding;
        }

        public EncodingData Encoding { get; }

        public Span<byte> Buffer
        {
            get
            {
                EnsureBuffer();

                return _writableBuffer.Memory.Span;
            }
        }

        public void Advance(int bytes)
        {
            _writableBuffer.Advance(bytes);
        }

        public void Enlarge(int desiredFreeBytesHint = 0)
        {
            _writableBuffer.Ensure(desiredFreeBytesHint == 0 ? 2048 : desiredFreeBytesHint);
        }

        public void Write(Span<byte> data)
        {
            EnsureBuffer();
            _writableBuffer.Write(data);
        }

        public async Task FlushAsync()
        {
            await _writableBuffer.FlushAsync();
            _needAlloc = true;
        }

        private void EnsureBuffer()
        {
            if (_needAlloc)
            {
                _writableBuffer = _channel.Alloc();
                _needAlloc = false;
            }
        }
    }
}
