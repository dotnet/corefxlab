using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Networking.Tls
{
    /// <summary>
    /// Used when chopping the incoming stream into the correct frames
    /// </summary>
    internal enum TlsFrameType
    {
        ChangeCipherSpec = 20,
        Alert = 21,
        Handshake = 22,
        AppData = 23,
        Invalid = -1,
        Incomplete = 0
    }
}
