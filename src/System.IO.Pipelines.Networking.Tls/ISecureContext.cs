using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    internal interface ISecureContext : IDisposable
    {
        int TrailerSize { get; set; }
        int HeaderSize { get; set; }
        bool ReadyToSend { get; }
        ApplicationLayerProtocolIds NegotiatedProtocol { get; }
        Task ProcessContextMessageAsync(ReadableBuffer readBuffer, IPipelineWriter writer);
        Task ProcessContextMessageAsync(IPipelineWriter writer);
        Task DecryptAsync(ReadableBuffer encryptedData, IPipelineWriter decryptedPipeline);
        Task EncryptAsync(ReadableBuffer unencryptedData, IPipelineWriter encryptedPipeline);
        bool IsServer { get; }
        CipherInfo CipherInfo { get; }
    }
}