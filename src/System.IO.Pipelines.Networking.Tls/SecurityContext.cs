using System.IO.Pipelines.Networking.Tls.Internal.Sspi;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace System.IO.Pipelines.Networking.Tls
{
    public class SecurityContext : IDisposable
    {
        internal const ContextFlags RequiredFlags =
            ContextFlags.ReplayDetect | ContextFlags.SequenceDetect | ContextFlags.Confidentiality |
            ContextFlags.AllocateMemory;

        internal const ContextFlags ServerRequiredFlags = RequiredFlags | ContextFlags.AcceptStream;
        //Current fixed block size (4k - 1 64 byte cacheline, should be from the pipeline factory in the future
        internal const int BlockSize = 1024 * 4 - 64;
        internal const SslProtocols SupportedProtocols = SslProtocols.Tls;
        private const string SecurityPackage = "Microsoft Unified Security Protocol Provider";
        //Maximum sized as defined by the IETF for TLS frames
        public const int MaxStackAllocSize = 16 * 1024;

        private readonly X509Certificate _serverCertificate;
        private SSPIHandle _credsHandle;
        private readonly bool _isServer;
        private readonly string _hostName;
        private readonly byte[] _alpnSupportedProtocols;
        private GCHandle _alpnHandle;
        private readonly SecurityBuffer _alpnBuffer;
        private readonly PipelineFactory _pipelineFactory;

        /// <summary>
        /// Loads up SSPI and sets up the credentials handle in memory ready to authenticate TLS connections
        /// </summary>
        /// <param name="factory">The pipeline factory that will be used to allocate input and output pipelines for the secure pipelines</param>
        /// <param name="hostName">The name of the host that will be sent to the other parties, for a server this should be the name on the certificate. For clients this can be left blank or the name on a client cert</param>
        /// <param name="isServer">Used to denote if you are going to be negotiating incoming or outgoing Tls connections</param>
        /// <param name="serverCert">This is the in memory representation of the certificate used for the PKI exchange and authentication</param>
        public SecurityContext(PipelineFactory factory, string hostName, bool isServer, X509Certificate serverCert)
            : this(factory, hostName, isServer, serverCert, 0)
        {
        }

        /// <summary>
        /// Loads up SSPI and sets up the credentials handle in memory ready to authenticate TLS connections
        /// </summary>
        /// <param name="factory">The pipeline factory that will be used to allocate input and output pipelines for the secure channels</param>
        /// <param name="hostName">The name of the host that will be sent to the other parties, for a server this should be the name on the certificate. For clients this can be left blank or the name on a client cert</param>
        /// <param name="isServer">Used to denote if you are going to be negotiating incoming or outgoing Tls connections</param>
        /// <param name="serverCert">This is the in memory representation of the certificate used for the PKI exchange and authentication</param>
        /// <param name="alpnSupportedProtocols">This is the protocols that are supported and that will be negotiated with on the otherside, if a protocol can't be negotiated then the handshake will fail</param>
        public unsafe SecurityContext(PipelineFactory factory, string hostName, bool isServer,
            X509Certificate serverCert, ApplicationProtocols.ProtocolIds alpnSupportedProtocols)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException(nameof(hostName));
            }
            _hostName = hostName;
            _pipelineFactory = factory;
            _serverCertificate = serverCert;
            _isServer = isServer;
            CreateAuthentication();
            if (alpnSupportedProtocols > 0)
            {
                //We need to get a buffer for the ALPN negotiation and pin it for sending to the lower API
                _alpnSupportedProtocols = ApplicationProtocols.GetBufferForProtocolId(alpnSupportedProtocols, true);
                _alpnHandle = GCHandle.Alloc(_alpnSupportedProtocols, GCHandleType.Pinned);
                _alpnBuffer = new SecurityBuffer((void*) _alpnHandle.AddrOfPinnedObject(),
                    _alpnSupportedProtocols.Length, SecurityBufferType.ApplicationProtocols);
            }
        }

        internal SSPIHandle CredentialsHandle => _credsHandle;
        internal bool AplnRequired => _alpnSupportedProtocols != null;
        internal SecurityBuffer AplnBuffer => _alpnBuffer;
        internal string HostName => _hostName;
        public bool IsServer => _isServer;

        private unsafe void CreateAuthentication()
        {
            int numberOfPackages;
            SecPkgInfo* secPointer = null;
            try
            {
                //Load the available security packages and look for the Unified pack from MS that supplies TLS support
                if (Interop.EnumerateSecurityPackagesW(out numberOfPackages, out secPointer) != 0)
                {
                    throw new InvalidOperationException("Unable to enumerate security packages");
                }
                for (var i = 0; i < numberOfPackages; i++)
                {
                    var package = secPointer[i];
                    var name = Marshal.PtrToStringUni(package.Name);
                    if (name != SecurityPackage)
                    {
                        continue;
                    }
                    //The correct security package is available
                    GetCredentials();
                    return;
                }
                throw new InvalidOperationException($"Unable to find the security package named {SecurityPackage}");
            }
            finally
            {
                if (secPointer != null)
                {
                    Interop.FreeContextBuffer((IntPtr) secPointer);
                }
            }
        }

        private unsafe void GetCredentials()
        {
            CredentialUse direction;
            CredentialFlags flags;
            if (_isServer)
            {
                direction = CredentialUse.Inbound;
                flags = CredentialFlags.UseStrongCrypto | CredentialFlags.SendAuxRecord;
            }
            else
            {
                direction = CredentialUse.Outbound;
                flags = CredentialFlags.ValidateManual | CredentialFlags.NoDefaultCred | CredentialFlags.SendAuxRecord |
                        CredentialFlags.UseStrongCrypto;
            }

            var creds = new SecureCredential()
            {
                rootStore = IntPtr.Zero,
                phMappers = IntPtr.Zero,
                palgSupportedAlgs = IntPtr.Zero,
                cMappers = 0,
                cSupportedAlgs = 0,
                dwSessionLifespan = 0,
                reserved = 0,
                dwMinimumCipherStrength = 0, //this is required to force encryption
                dwMaximumCipherStrength = 0,
                version = SecureCredential.CurrentVersion,
                dwFlags = flags,
                certContextArray = IntPtr.Zero,
                cCreds = 0
            };
            IntPtr certPointer;
            if (_isServer)
            {
                creds.grbitEnabledProtocols = Interop.ServerProtocolMask;
                certPointer = _serverCertificate.Handle;
                //pointer to the pointer
                IntPtr certPointerPointer = new IntPtr(&certPointer);
                creds.certContextArray = certPointerPointer;
                creds.cCreds = 1;
            }
            else
            {
                creds.grbitEnabledProtocols = Interop.ClientProtocolMask;
            }

            long timestamp = 0;
            SecurityStatus code = Interop.AcquireCredentialsHandleW(null, SecurityPackage, (int) direction
                , null, ref creds, null, null, ref _credsHandle, out timestamp);

            if (code != SecurityStatus.OK)
            {
                throw new InvalidOperationException($"Could not acquire the credentials with return code {code}");
            }
        }

        public ISecurePipeline CreateSecurePipeline(IPipelineConnection pipeline)
        {
            var chan = new SecurePipeline<SecureConnectionContext>(pipeline, _pipelineFactory,
                new SecureConnectionContext(this));
            return chan;
        }

        public void Dispose()
        {
            if (_credsHandle.IsValid)
            {
                Interop.FreeCredentialsHandle(ref _credsHandle);
                _credsHandle = new SSPIHandle();
            }
            if (_alpnHandle.IsAllocated)
            {
                _alpnHandle.Free();
            }
            GC.SuppressFinalize(this);
        }

        ~SecurityContext()
        {
            Dispose();
        }
    }
}