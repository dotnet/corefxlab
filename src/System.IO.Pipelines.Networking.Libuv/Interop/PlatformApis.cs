// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    public static class PlatformApis
    {
        static PlatformApis()
        {
            IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            IsDarwin = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool IsWindows { get; }

        public static bool IsDarwin { get; }
    }
}
