using System;
using System.Collections.Generic;
using System.IO.Pipelines.Networking.Tls;
using System.IO.Pipelines.Tests.Internal;
using System.IO.Pipelines.Text.Primitives;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;
using Xunit;

namespace System.IO.Pipelines.Tests
{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    public class TlsFacts
    {
        private static readonly string _certificatePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "data", "TestCert.pfx");
        private static readonly string _certificatePassword = "Test123t";
        private static readonly string _shortTestString = "The quick brown fox jumped over the lazy dog.";

        [WindowsOnlyFact]
        public async Task SspiAplnMatchingProtocol()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var serverContext = new SecurityContext(factory, "CARoot", true, cert, ApplicationLayerProtocolIds.Http11 | ApplicationLayerProtocolIds.Http2OverTls))
            using (var clientContext = new SecurityContext(factory, "CARoot", false, null, ApplicationLayerProtocolIds.Http2OverTls))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var server = serverContext.CreateSecurePipeline(loopback.ServerPipeline))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientPipeline))
                {
                    Echo(server);
                    var proto = await client.PerformHandshakeAsync();
                    Assert.Equal(ApplicationLayerProtocolIds.Http2OverTls, proto);
                }
            }
        }
                
        [NotWindowsFact]
        public async Task OpenSslPipelineAllTheThings()
        {
            using (var factory = new PipelineFactory())
            using (var serverContext = new OpenSslSecurityContext(factory, "test", true, _certificatePath, _certificatePassword))
            using (var clientContext = new OpenSslSecurityContext(factory, "test", false, null, null))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var server = serverContext.CreateSecurePipeline(loopback.ServerPipeline))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientPipeline))
                {
                    Echo(server);
                    await client.PerformHandshakeAsync();
                    var outputBuffer = client.Output.Alloc();
                    outputBuffer.Write(Encoding.UTF8.GetBytes(_shortTestString));
                    await outputBuffer.FlushAsync();

                    //Now check we get the same thing back
                    string resultString;
                    while (true)
                    {
                        var result = await client.Input.ReadAsync();
                        if (result.Buffer.Length >= _shortTestString.Length)
                        {
                            resultString = result.Buffer.GetUtf8String();
                            client.Input.Advance(result.Buffer.End);
                            break;
                        }
                        client.Input.Advance(result.Buffer.Start, result.Buffer.End);
                    }
                    Assert.Equal(_shortTestString, resultString);
                }
            }
        }

        [WindowsOnlyFact]
        public async Task SspiPipelineAllThings()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var serverContext = new SecurityContext(factory, "CARoot", true, cert))
            using (var clientContext = new SecurityContext(factory, "CARoot", false, null))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var server = serverContext.CreateSecurePipeline(loopback.ServerPipeline))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientPipeline))
                {
                    Echo(server);

                    await client.PerformHandshakeAsync();
                    var outputBuffer = client.Output.Alloc();
                    outputBuffer.Write(Encoding.UTF8.GetBytes(_shortTestString));
                    await outputBuffer.FlushAsync();

                    //Now check we get the same thing back
                    string resultString;
                    while (true)
                    {
                        var result = await client.Input.ReadAsync();
                        if (result.Buffer.Length >= _shortTestString.Length)
                        {
                            resultString = result.Buffer.GetUtf8String();
                            client.Input.Advance(result.Buffer.End);
                            break;
                        }
                        client.Input.Advance(result.Buffer.Start, result.Buffer.End);
                    }
                    Assert.Equal(_shortTestString, resultString);
                }
            }
        }

        [WindowsOnlyFact()]
        public async Task SspiPipelineServerStreamClient()
        {
            using (var pipelineFactory = new PipelineFactory())
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var secContext = new SecurityContext(pipelineFactory, "CARoot", true, cert))
            {
                var loopback = new LoopbackPipeline(pipelineFactory);
                using (var server = secContext.CreateSecurePipeline(loopback.ServerPipeline))
                using (var sslStream = new SslStream(loopback.ClientPipeline.GetStream(), false, ValidateServerCertificate, null, EncryptionPolicy.RequireEncryption))
                {
                    Echo(server);

                    await sslStream.AuthenticateAsClientAsync("CARoot");

                    byte[] messsage = Encoding.UTF8.GetBytes(_shortTestString);
                    sslStream.Write(messsage);
                    sslStream.Flush();
                    // Read message from the server.
                    string serverMessage = ReadMessageFromStream(sslStream);
                    Assert.Equal(_shortTestString, serverMessage);
                }
            }
        }

        [WindowsOnlyFact()]
        public async Task SspiStreamServerPipelineClient()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var clientContext = new SecurityContext(factory, "CARoot", false, null))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientPipeline))
                using (var secureServer = new SslStream(loopback.ServerPipeline.GetStream(), false))
                {
                    secureServer.AuthenticateAsServerAsync(cert, false, System.Security.Authentication.SslProtocols.Tls, false);

                    await client.PerformHandshakeAsync();

                    var buff = client.Output.Alloc();
                    buff.Write(Encoding.UTF8.GetBytes(_shortTestString));
                    await buff.FlushAsync();

                    //Check that the server actually got it
                    var tempBuff = new byte[_shortTestString.Length];
                    int totalRead = 0;
                    while (true)
                    {
                        int numberOfBytes = secureServer.Read(tempBuff, totalRead, _shortTestString.Length - totalRead);
                        if (numberOfBytes == -1)
                        {
                            break;
                        }
                        totalRead += numberOfBytes;
                        if (totalRead >= _shortTestString.Length)
                        {
                            break;
                        }
                    }
                    Assert.Equal(_shortTestString, UTF8Encoding.UTF8.GetString(tempBuff));
                }
            }
        }

        [NotWindowsFact]
        public async Task OpenSslPipelineServerStreamClient()
        {
            using (var pipelineFactory = new PipelineFactory())
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var secContext = new OpenSslSecurityContext(pipelineFactory, "CARoot", true, _certificatePath, _certificatePassword))
            {
                var loopback = new LoopbackPipeline(pipelineFactory);
                using (var server = secContext.CreateSecurePipeline(loopback.ServerPipeline))
                using (var sslStream = new SslStream(loopback.ClientPipeline.GetStream(), false, ValidateServerCertificate, null, EncryptionPolicy.RequireEncryption))
                {
                    Echo(server);
                    await sslStream.AuthenticateAsClientAsync("CARoot");
                    byte[] messsage = Encoding.UTF8.GetBytes(_shortTestString);
                    sslStream.Write(messsage);
                    sslStream.Flush();
                    // Read message from the server.
                    string serverMessage = ReadMessageFromStream(sslStream);
                    Assert.Equal(_shortTestString, serverMessage);
                }
            }
        }

        [NotWindowsFact]
        public async Task OpenSslStreamServerPipelineClient()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var pipelineFactory = new PipelineFactory())
            using (var clientContext = new OpenSslSecurityContext(pipelineFactory, "CARoot", false, _certificatePath, _certificatePassword))
            {
                var loopback = new LoopbackPipeline(pipelineFactory);
                using (var secureServer = new SslStream(loopback.ServerPipeline.GetStream(), false))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientPipeline))
                {
                    secureServer.AuthenticateAsServerAsync(cert, false, System.Security.Authentication.SslProtocols.Tls, false);

                    await client.PerformHandshakeAsync();
                    var buff = client.Output.Alloc();
                    buff.Write(Encoding.UTF8.GetBytes(_shortTestString));
                    await buff.FlushAsync();

                    //Check that the server actually got it
                    var tempBuff = new byte[_shortTestString.Length];
                    int totalRead = 0;
                    while (true)
                    {
                        int numberOfBytes = secureServer.Read(tempBuff, totalRead, _shortTestString.Length - totalRead);
                        if (numberOfBytes == -1)
                        {
                            break;
                        }
                        totalRead += numberOfBytes;
                        if (totalRead >= _shortTestString.Length)
                        {
                            break;
                        }
                    }
                    Assert.Equal(_shortTestString, UTF8Encoding.UTF8.GetString(tempBuff));
                }
            }
        }

        private string ReadMessageFromStream(SslStream sslStream)
        {
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                if (messageData.Length == _shortTestString.Length)
                {
                    break;
                }
            } while (bytes != 0);

            return messageData.ToString();
        }

        public bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
            {
                return true;
            }
            return false;
        }

        private async Task Echo(ISecurePipeline pipeline)
        {
            await pipeline.PerformHandshakeAsync();
            try
            {
                while (true)
                {
                    var result = await pipeline.Input.ReadAsync();
                    var request = result.Buffer;

                    if (request.IsEmpty && result.IsCompleted)
                    {
                        pipeline.Input.Advance(request.End);
                        break;
                    }
                    int len = request.Length;
                    var response = pipeline.Output.Alloc();
                    response.Append(request);
                    await response.FlushAsync();
                    pipeline.Input.Advance(request.End);
                }
                pipeline.Input.Complete();
                pipeline.Output.Complete();
            }
            catch (Exception ex)
            {
                pipeline.Input.Complete(ex);
                pipeline.Output.Complete(ex);
            }
        }
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
}
