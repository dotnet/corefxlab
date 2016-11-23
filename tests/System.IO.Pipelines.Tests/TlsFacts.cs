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

        [WindowsOnlyFact()]
        public async Task SspiAplnMatchingProtocol()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var serverContext = new SecurityContext(factory, "CARoot", true, cert, ApplicationProtocols.ProtocolIds.Http11 | ApplicationProtocols.ProtocolIds.Http2OverTls))
            using (var clientContext = new SecurityContext(factory, "CARoot", false, null, ApplicationProtocols.ProtocolIds.Http2OverTls))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var server = serverContext.CreateSecurePipeline(loopback.ServerChannel))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientChannel))
                {
                    Echo(server);
                    var proto = await client.ShakeHandsAsync();
                    Assert.Equal(ApplicationProtocols.ProtocolIds.Http2OverTls, proto);
                }
            }
        }

        [WindowsOnlyFact]
        public async Task OpenSslAsServerSspiAsClientAplnMatchingProtocol()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var serverContext = new OpenSslSecurityContext(factory, "test", true, _certificatePath, _certificatePassword, ApplicationProtocols.ProtocolIds.Http11 | ApplicationProtocols.ProtocolIds.Http2OverTls))
            using (var clientContext = new SecurityContext(factory, "CARoot", false, cert, ApplicationProtocols.ProtocolIds.Http2OverTls))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var server = serverContext.CreateSecurePipeline(loopback.ServerChannel))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientChannel))
                {
                    Echo(server);
                    var proto = await client.ShakeHandsAsync();
                    Assert.Equal(ApplicationProtocols.ProtocolIds.Http2OverTls, proto);
                }
            }
        }

        [WindowsOnlyFact]
        public async Task SspiAsServerOpenSslAsClientAplnMatchingProtocol()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var clientContext = new OpenSslSecurityContext(factory, "test", false, _certificatePath, _certificatePassword, ApplicationProtocols.ProtocolIds.Http11 | ApplicationProtocols.ProtocolIds.Http2OverTls))
            using (var serverContext = new SecurityContext(factory, "CARoot", true, cert, ApplicationProtocols.ProtocolIds.Http2OverTls))
            {
                var loopback = new LoopbackPipeline(factory);
                Echo(serverContext.CreateSecurePipeline(loopback.ServerChannel));
                var client = clientContext.CreateSecurePipeline(loopback.ClientChannel);
                var proto = await client.ShakeHandsAsync();
                Assert.Equal(ApplicationProtocols.ProtocolIds.Http2OverTls, proto);
            }
        }

        [WindowsOnlyFact]
        public async Task OpenSslAndSspiChannelAllTheThings()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var clientContext = new OpenSslSecurityContext(factory, "test", false, _certificatePath, _certificatePassword))
            using (var serverContext = new SecurityContext(factory, "CARoot", true, cert))
            {
                var loopback = new LoopbackPipeline(factory);
                Echo(serverContext.CreateSecurePipeline(loopback.ServerChannel));
                var client = clientContext.CreateSecurePipeline(loopback.ClientChannel);
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

        [Fact]
        public async Task OpenSslChannelAllTheThings()
        {
            using (var factory = new PipelineFactory())
            using (var serverContext = new OpenSslSecurityContext(factory, "test", true, _certificatePath, _certificatePassword))
            using (var clientContext = new OpenSslSecurityContext(factory, "test", false, null, null))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var server = serverContext.CreateSecurePipeline(loopback.ServerChannel))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientChannel))
                {
                    Echo(server);
                    await client.ShakeHandsAsync();
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
        public async Task SspiChannelAllThings()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var serverContext = new SecurityContext(factory, "CARoot", true, cert))
            using (var clientContext = new SecurityContext(factory, "CARoot", false, null))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var server = serverContext.CreateSecurePipeline(loopback.ServerChannel))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientChannel))
                {
                    Echo(server);

                    await client.ShakeHandsAsync();
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
        public async Task SspiChannelServerStreamClient()
        {
            using (var channelFactory = new PipelineFactory())
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var secContext = new SecurityContext(channelFactory, "CARoot", true, cert))
            {
                var loopback = new LoopbackPipeline(channelFactory);
                using (var server = secContext.CreateSecurePipeline(loopback.ServerChannel))
                using (var sslStream = new SslStream(loopback.ClientChannel.GetStream(), false, ValidateServerCertificate, null, EncryptionPolicy.RequireEncryption))
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
        public async Task SspiStreamServerChannelClient()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var factory = new PipelineFactory())
            using (var clientContext = new SecurityContext(factory, "CARoot", false, null))
            {
                var loopback = new LoopbackPipeline(factory);
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientChannel))
                using (var secureServer = new SslStream(loopback.ServerChannel.GetStream(), false))
                {
                    secureServer.AuthenticateAsServerAsync(cert, false, System.Security.Authentication.SslProtocols.Tls, false);

                    await client.ShakeHandsAsync();

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

        [WindowsOnlyFact]
        public async Task OpenSslChannelServerStreamClient()
        {
            using (var channelFactory = new PipelineFactory())
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var secContext = new OpenSslSecurityContext(channelFactory, "CARoot", true, _certificatePath, _certificatePassword))
            {
                var loopback = new LoopbackPipeline(channelFactory);
                using (var server = secContext.CreateSecurePipeline(loopback.ServerChannel))
                using (var sslStream = new SslStream(loopback.ClientChannel.GetStream(), false, ValidateServerCertificate, null, EncryptionPolicy.RequireEncryption))
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

        [WindowsOnlyFact]
        public async Task OpenSslStreamServerChannelClient()
        {
            using (var cert = new X509Certificate(_certificatePath, _certificatePassword))
            using (var channelFactory = new PipelineFactory())
            using (var clientContext = new OpenSslSecurityContext(channelFactory, "CARoot", false, _certificatePath, _certificatePassword))
            {
                var loopback = new LoopbackPipeline(channelFactory);
                using (var secureServer = new SslStream(loopback.ServerChannel.GetStream(), false))
                using (var client = clientContext.CreateSecurePipeline(loopback.ClientChannel))
                {
                    secureServer.AuthenticateAsServerAsync(cert, false, System.Security.Authentication.SslProtocols.Tls, false);

                    await client.ShakeHandsAsync();
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

        private async Task Echo(ISecurePipeline channel)
        {
            await channel.ShakeHandsAsync();
            try
            {
                while (true)
                {
                    var result = await channel.Input.ReadAsync();
                    var request = result.Buffer;

                    if (request.IsEmpty && result.IsCompleted)
                    {
                        channel.Input.Advance(request.End);
                        break;
                    }
                    int len = request.Length;
                    var response = channel.Output.Alloc();
                    response.Append(request);
                    await response.FlushAsync();
                    channel.Input.Advance(request.End);
                }
                channel.Input.Complete();
                channel.Output.Complete();
            }
            catch (Exception ex)
            {
                channel.Input.Complete(ex);
                channel.Output.Complete(ex);
            }
        }
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
}
