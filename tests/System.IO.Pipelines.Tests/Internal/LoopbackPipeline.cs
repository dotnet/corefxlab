using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Tests.Internal
{
    public class LoopbackPipeline
    {
        IPipelineConnection _clientPipeline;
        IPipelineConnection _serverPipeline;

        public LoopbackPipeline(PipelineFactory factory)
        {
            var backPipeline1 = factory.Create();
            var backPipeline2 = factory.Create();

            _clientPipeline = new TestPipeline(backPipeline1, backPipeline2);
            _serverPipeline = new TestPipeline(backPipeline2, backPipeline1);
        }

        public IPipelineConnection ServerPipeline => _serverPipeline;
        public IPipelineConnection ClientPipeline => _clientPipeline;

        class TestPipeline : IPipelineConnection
        {
            Pipe _inPipeline;
            Pipe _outPipeline;

            public TestPipeline(Pipe inPipeline, Pipe outPipeline)
            {
                _inPipeline = inPipeline;
                _outPipeline = outPipeline;
            }

            public IPipelineReader Input => _inPipeline;
            public IPipelineWriter Output => _outPipeline;
            public void Dispose()
            {
            }
        }
    }
}
