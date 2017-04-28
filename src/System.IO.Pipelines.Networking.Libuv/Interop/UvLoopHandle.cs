// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    public class UvLoopHandle : UvMemory
    {
        public UvLoopHandle() : base()
        {
        }

        public void Init(Uv uv)
        {
            CreateMemory(
                uv,
                Thread.CurrentThread.ManagedThreadId,
                uv.loop_size());

            _uv.loop_init(this);
        }

        public void Run(int mode = 0)
        {
            _uv.run(this, mode);
        }

        public void Stop()
        {
            _uv.stop(this);
        }

        unsafe protected override bool ReleaseHandle()
        {
            var memory = handle;
            if (memory != IntPtr.Zero)
            {
                // loop_close clears the gcHandlePtr
                var gcHandlePtr = *(IntPtr*)memory;

                _uv.loop_close(this);
                handle = IntPtr.Zero;

                DestroyMemory(memory, gcHandlePtr);
            }

            return true;
        }
    }
}
