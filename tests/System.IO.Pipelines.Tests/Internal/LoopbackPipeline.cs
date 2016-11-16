using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Tests.Internal
{
    public class LoopbackPipeline
    {
        IPipelineConnection _clientChannel;
        IPipelineConnection _serverChannel;

        public LoopbackPipeline(PipelineFactory factory)
        {
            var backChannel1 = factory.Create();
            var backChannel2 = factory.Create();

            _clientChannel = new TestChannel(backChannel1, backChannel2);
            _serverChannel = new TestChannel(backChannel2, backChannel1);
        }

        public IPipelineConnection ServerChannel => _serverChannel;
        public IPipelineConnection ClientChannel => _clientChannel;

        class TestChannel : IPipelineConnection
        {
            PipelineReaderWriter _inChannel;
            PipelineReaderWriter _outChannel;

            public TestChannel(PipelineReaderWriter inChannel, PipelineReaderWriter outChannel)
            {
                _inChannel = inChannel;
                _outChannel = outChannel;
            }

            public IPipelineReader Input => _inChannel;
            public IPipelineWriter Output => _outChannel;
            public void Dispose()
            {
            }
        }
    }
}
