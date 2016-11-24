using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    public interface ISecurePipeline : IPipelineConnection
    {
        Task<ApplicationProtocols.ProtocolIds> PerformHandshakeAsync();
        CipherInfo CipherInfo { get; }
    }
}