using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    public interface ISecurePipeline : IPipelineConnection
    {
        Task<ApplicationLayerProtocolIds> PerformHandshakeAsync();
        CipherInfo CipherInfo { get; }
    }
}