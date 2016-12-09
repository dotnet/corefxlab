// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    public class UvPipeHandle : UvStreamHandle
    {
        public UvPipeHandle() : base()
        {
        }

        public void Init(UvLoopHandle loop, Action<Action<IntPtr>, IntPtr> queueCloseHandle, bool ipc = false)
        {
            CreateHandle(
                loop.Libuv, 
                loop.ThreadId,
                loop.Libuv.handle_size(Uv.HandleType.NAMED_PIPE), queueCloseHandle);

            _uv.pipe_init(loop, this, ipc);
        }

        public void Bind(string name)
        {
            _uv.pipe_bind(this, name);
        }

        public int PendingCount()
        {
            return _uv.pipe_pending_count(this);
        }
    }
}
