using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    public interface ISecureContext : IDisposable
    {
        int TrailerSize { get; set; }
        int HeaderSize { get; set; }
        bool ReadyToSend { get; }
        ApplicationProtocols.ProtocolIds NegotiatedProtocol { get; }
        Task ProcessContextMessageAsync(ReadableBuffer readBuffer, IPipelineWriter writePipeline);
        Task ProcessContextMessageAsync(IPipelineWriter writePipeline);
        Task DecryptAsync(ReadableBuffer encryptedData, IPipelineWriter decryptedData);
        Task EncryptAsync(ReadableBuffer unencrypted, IPipelineWriter encryptedData);
        bool IsServer { get; }
        CipherInfo CipherInfo { get; }
    }
}