using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    public interface ISecurePipeline : IPipelineConnection
    {
        Task<ApplicationProtocols.ProtocolIds> HandShakeAsync();
        CipherInfo CipherInfo { get; }
    }
}
