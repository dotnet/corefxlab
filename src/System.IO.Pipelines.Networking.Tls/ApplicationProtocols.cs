using System;
using System.Collections.Generic;
using System.IO.Pipelines.Networking.Tls.Internal.Sspi;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    public static class ApplicationProtocols
    {
        private static readonly byte[] _http1 = new byte[] { 0x68, 0x74, 0x74, 0x70, 0x2f, 0x31, 0x2e, 0x31 }; //("http/1.1")
        private static readonly byte[] _spdy1 = new byte[] { 0x73, 0x70, 0x64, 0x79, 0x2f, 0x31 }; //("spdy/1")
        private static readonly byte[] _spdy2 = new byte[] { 0x73, 0x70, 0x64, 0x79, 0x2f, 0x32 }; //("spdy/2")
        private static readonly byte[] _spdy3 = new byte[] { 0x73, 0x70, 0x64, 0x79, 0x2f, 0x33 }; //("spdy/3")
        private static readonly byte[] _traversalUsingRelaysAroundNAT = new byte[] { 0x73, 0x74, 0x75, 0x6E, 0x2E, 0x74, 0x75, 0x72, 0x6E }; //("stun.turn")
        private static readonly byte[] _natDiscoveryUsingSessionTraversalUtilitiesforNAT = new byte[] { 0x73, 0x74, 0x75, 0x6E, 0x2E, 0x6e, 0x61, 0x74, 0x2d, 0x64, 0x69, 0x73, 0x63, 0x6f, 0x76, 0x65, 0x72, 0x79 }; //("stun.nat-discovery")
        private static readonly byte[] _http2overTLS = new byte[] { 0x68, 0x32 }; //("h2")
        private static readonly byte[] _http2overTCP = new byte[] { 0x68, 0x32, 0x63 }; //("h2c")
        private static readonly byte[] _webRTCMediaAndData = new byte[] { 0x77, 0x65, 0x62, 0x72, 0x74, 0x63 }; //("webrtc")
        private static readonly byte[] _confidentialWebRTCMediaAndData = new byte[] { 0x63, 0x2d, 0x77, 0x65, 0x62, 0x72, 0x74, 0x63 }; //("c-webrtc")
        private static readonly byte[] _Ftp = new byte[] { 0x66, 0x74, 0x70 }; //("ftp")
        private static readonly byte[][] _allProtocols = new byte[][] { _http1, _spdy1, _spdy2, _spdy3, _traversalUsingRelaysAroundNAT, _natDiscoveryUsingSessionTraversalUtilitiesforNAT, _http2overTLS, _http2overTCP, _webRTCMediaAndData, _confidentialWebRTCMediaAndData, _Ftp };
        private static readonly Array _numberOfProtocols = Enum.GetValues(typeof(ProtocolIds));

        const int _HeaderLength = 10;
        const int _ContentOffset = 4;

        [Flags]
        public enum ProtocolIds
        {
            Http11 = 1,
            Spdy1 = 2,
            Spdy2 = 4,
            Spdy3 = 8,
            TraversalUsingRelaysAroundNAT = 16,
            NatDiscoveryUsingSessionTraversalUtilitiesforNAT = 32,
            Http2overTLS = 64,
            Http2overTCP = 128,
            WebRTCMediaAndData = 256,
            ConfidentialWebRTCMediaAndData = 512,
            Ftp = 1024
        }

        internal enum ApplicaitonProtocolNegotiationExtension
        {
            None = 0,
            NPN,
            ALPN,
        }

        internal enum ApplicationProtocolNegotiationStatus : uint
        {
            None = 0,
            Success,
            SelectedClientOnly
        }

        internal static unsafe ProtocolIds FindNegotiatedProtocol(SSPIHandle context)
        {
            ContextApplicationProtocol protoInfo;
            Interop.QueryContextAttributesW(ref context, ContextAttribute.ApplicationProtocol, out protoInfo);

            if (protoInfo.ProtoNegoStatus != ApplicationProtocolNegotiationStatus.Success)
            {
                throw new InvalidOperationException("Could not negotiate a mutal application protocol");
            }
            return GetNegotiatedProtocol(protoInfo.ProtocolId, protoInfo.ProtocolIdSize);
        }

        public unsafe static byte[] GetBufferForProtocolId(ProtocolIds supportedProtocols, bool withALPNHeader)
        {
            short listLength = 0;
            for (int i = 0; i < _numberOfProtocols.Length; i++)
            {
                if (((int)_numberOfProtocols.GetValue(i) & (int)supportedProtocols) > 0)
                {
                    listLength += (short)(_allProtocols[i].Length + 1);
                }
            }
            byte[] returnValue;
            if (withALPNHeader)
            {
                //Now we know the total length of the list lets create our array of protocols and header
                returnValue = new byte[listLength + _HeaderLength];
                var spa = new Span<byte>(returnValue);
                int length = _HeaderLength + listLength - _ContentOffset;
                spa.Write(length);
                spa = spa.Slice(4);
                spa.Write(ApplicaitonProtocolNegotiationExtension.ALPN);
                spa = spa.Slice(4);
                spa.Write(listLength);
                spa = spa.Slice(2);
                spa = ProtocolList(supportedProtocols, spa);
            }
            else
            {
                //No header just make an array of the correct size
                returnValue = new byte[listLength];
                var spa = new Span<byte>(returnValue);
                ProtocolList(supportedProtocols, spa);
            }
            return returnValue;
        }

        //public static unsafe ProtocolIds ProtocolList()

        private static unsafe Span<byte> ProtocolList(ProtocolIds supportedProtocols, Span<byte> spa)
        {
            for (int i = 0; i < _numberOfProtocols.Length; i++)
            {
                if (((int)_numberOfProtocols.GetValue(i) & (int)supportedProtocols) > 0)
                {
                    var value = _allProtocols[i];
                    spa.Write((byte)value.Length);
                    spa = spa.Slice(1);
                    spa.Set(value);
                    spa = spa.Slice(value.Length);
                }
            }

            return spa;
        }

        internal static unsafe ProtocolIds GetNegotiatedProtocol(byte* protocolId, byte protocolIdSize)
        {
            var matchedValue = new Span<byte>(protocolId, protocolIdSize);
            for (int i = 0; i < _allProtocols.Length; i++)
            {
                var proto = _allProtocols[i];
                if (matchedValue.SequenceEqual(proto))
                {
                    return (ProtocolIds)(1 << i);
                }
            }
            throw new ArgumentOutOfRangeException($"Could not match the negotiated protocol to a valid protocol");
        }
    }
}
