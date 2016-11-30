using System.IO.Pipelines.Networking.Tls.Internal.OpenSsl;
using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Tls
{
    public class OpenSslSecurityContext : IDisposable
    {
        //Current fixed block size (4k - 1 64 byte cacheline, should be from the pipeline factory in the future
        internal const int BlockSize = 1024 * 4 - 64; 

        private readonly string _hostName;
        private readonly PipelineFactory _pipelineFactory;
        private readonly bool _isServer;
        private InteropKeys.PK12Certificate _certificateInformation;
        private byte[] _alpnSupportedProtocolsBuffer;
        private GCHandle _alpnHandle;
        internal IntPtr SslContext;
        internal Interop.alpn_cb AlpnCallback;
        private ApplicationLayerProtocolIds _alpnSupportedProtocols;

        public OpenSslSecurityContext(PipelineFactory pipelineFactory, string hostName, bool isServer, string pathToPfxFile, string password)
            : this(pipelineFactory, hostName, isServer, pathToPfxFile, password, 0)
        {
        }

        public OpenSslSecurityContext(PipelineFactory pipelineFactory, string hostName, bool isServer, string pathToPfxFile, string password, ApplicationLayerProtocolIds alpnSupportedProtocols)
        {
            if (isServer && string.IsNullOrEmpty(pathToPfxFile))
            {
                throw new ArgumentException("We need a certificate to load if you want to run in server mode");
            }

            InteropCrypto.Init();
            _hostName = hostName;
            _pipelineFactory = pipelineFactory;
            _isServer = isServer;
            _alpnSupportedProtocols = alpnSupportedProtocols;

            if (!string.IsNullOrWhiteSpace(pathToPfxFile))
            {
                InteropBio.BioHandle fileBio = new InteropBio.BioHandle();
                try
                {
                    fileBio = InteropBio.BIO_new_file_read(pathToPfxFile);
                    //Now we pull out the private key, certificate and Authority if they are all there
                    _certificateInformation = new InteropKeys.PK12Certificate(fileBio, password);
                }
                finally
                {
                    fileBio.FreeBio();
                }
            }
            SetupContext();
            SetupAlpn();
        }

        public bool IsServer => _isServer;
        internal InteropKeys.PK12Certificate CertificateInformation => _certificateInformation;
        internal IntPtr AplnBuffer => _alpnHandle.AddrOfPinnedObject();
        internal int AplnBufferLength => _alpnSupportedProtocolsBuffer?.Length ?? 0;

        private unsafe void SetupAlpn()
        {
            if (_alpnSupportedProtocols > 0)
            {
                //We need to get a buffer for the ALPN negotiation and pin it for sending to the lower API
                _alpnSupportedProtocolsBuffer = ApplicationLayerProtocolExtension.GetBufferForProtocolId(_alpnSupportedProtocols, false);
                _alpnHandle = GCHandle.Alloc(_alpnSupportedProtocolsBuffer, GCHandleType.Pinned);
                Interop.SSL_CTX_set_alpn_protos(SslContext, AplnBuffer, (uint)AplnBufferLength);
                if (_isServer)
                {
                    AlpnCallback = alpn_cb;
                    Interop.SSL_CTX_set_alpn_select_cb(this);
                }
            }
        }

        private unsafe Interop.AlpnStatus alpn_cb(IntPtr ssl, out byte* selProto, out byte selProtoLen, byte* inProtos, int inProtosLen, IntPtr arg)
        {
            byte* matchBuffer = (byte*)_alpnHandle.AddrOfPinnedObject();
            for (int i = 0; i < inProtosLen;)
            {
                var len = inProtos[i];
                var s = new Span<byte>(inProtos + i + 1, len);
                for (int x = 0; x < _alpnSupportedProtocolsBuffer.Length;)
                {
                    var len2 = matchBuffer[x];
                    var sCompare = new Span<byte>(matchBuffer + x + 1, len2);
                    if (sCompare.SequenceEqual(s))
                    {
                        selProto = matchBuffer + x + 1;
                        selProtoLen = len2;
                        return Interop.AlpnStatus.SSL_TLSEXT_ERR_OK;
                    }
                    x += len2 + 1;
                }
                i += len + 1;
            }
            selProto = null;
            selProtoLen = 0;
            return Interop.AlpnStatus.SSL_TLSEXT_ERR_NOACK;
        }

        private void SetupContext()
        {
            if (_isServer)
            {
                SslContext = Interop.NewServerContext(Interop.VerifyMode.SSL_VERIFY_NONE);
            }
            else
            {
                SslContext = Interop.NewClientContext(Interop.VerifyMode.SSL_VERIFY_NONE);
            }

            if (_certificateInformation.Handle != IntPtr.Zero)
            {
                Interop.SetKeys(SslContext, _certificateInformation.CertificateHandle, _certificateInformation.PrivateKeyHandle);
            }
        }

        public ISecurePipeline CreateSecurePipeline(IPipelineConnection pipeline)
        {
            var ssl = Interop.SSL_new(SslContext);
            var chan = new SecurePipeline<OpenSslConnectionContext>(pipeline, _pipelineFactory, new OpenSslConnectionContext(this, ssl));
            return chan;
        }

        public void Dispose()
        {
            if (_alpnHandle.IsAllocated)
            {
                _alpnHandle.Free();
            }
            _certificateInformation.Free();
            if (SslContext != IntPtr.Zero)
            {
                Interop.SSL_CTX_free(SslContext);
            }
        }
    }
}
