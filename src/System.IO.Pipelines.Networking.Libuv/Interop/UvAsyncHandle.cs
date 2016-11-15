// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    public class UvAsyncHandle : UvHandle
    {
        private static readonly Uv.uv_close_cb _destroyMemory = (handle) => DestroyMemory(handle);

        private static readonly Uv.uv_async_cb _uv_async_cb = (handle) => AsyncCb(handle);
        private Action _callback;
        private Action<Action<IntPtr>, IntPtr> _queueCloseHandle;

        public UvAsyncHandle() : base()
        {
        }

        public void Init(UvLoopHandle loop, Action callback, Action<Action<IntPtr>, IntPtr> queueCloseHandle)
        {
            CreateMemory(
                loop.Libuv,
                loop.ThreadId,
                loop.Libuv.handle_size(Uv.HandleType.ASYNC));

            _callback = callback;
            _queueCloseHandle = queueCloseHandle;
            _uv.async_init(loop, this, _uv_async_cb);
        }

        public void Send()
        {
            _uv.async_send(this);
        }

        private static void AsyncCb(IntPtr handle)
        {
            FromIntPtr<UvAsyncHandle>(handle)._callback.Invoke();
        }

        protected override bool ReleaseHandle()
        {
            var memory = handle;
            if (memory != IntPtr.Zero)
            {
                handle = IntPtr.Zero;

                if (Thread.CurrentThread.ManagedThreadId == ThreadId)
                {
                    _uv.close(memory, _destroyMemory);
                }
                else if (_queueCloseHandle != null)
                {
                    // This can be called from the finalizer.
                    // Ensure the closure doesn't reference "this".
                    var uv = _uv;
                    _queueCloseHandle(memory2 => uv.close(memory2, _destroyMemory), memory);
                    uv.unsafe_async_send(memory);
                }
                else
                {
                    Debug.Assert(false, "UvAsyncHandle not initialized with queueCloseHandle action");
                    return false;
                }
            }
            return true;
        }
    }
}
