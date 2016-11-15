// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    public abstract class UvStreamHandle : UvHandle
    {
        private readonly static Uv.uv_connection_cb _uv_connection_cb = (handle, status) => UvConnectionCb(handle, status);
        // Ref and out lamda params must be explicitly typed
        private readonly static Uv.uv_alloc_cb _uv_alloc_cb = (IntPtr handle, int suggested_size, out Uv.uv_buf_t buf) => UvAllocCb(handle, suggested_size, out buf);
        private readonly static Uv.uv_read_cb _uv_read_cb = (IntPtr handle, int status, ref Uv.uv_buf_t buf) => UvReadCb(handle, status, ref buf);

        private Action<UvStreamHandle, int, Exception, object> _listenCallback;
        private object _listenState;
        private GCHandle _listenVitality;

        private Func<UvStreamHandle, int, object, Uv.uv_buf_t> _allocCallback;
        private Action<UvStreamHandle, int, object> _readCallback;
        private object _readState;
        private GCHandle _readVitality;

        protected UvStreamHandle() : base()
        {
        }

        protected override bool ReleaseHandle()
        {
            if (_listenVitality.IsAllocated)
            {
                _listenVitality.Free();
            }
            if (_readVitality.IsAllocated)
            {
                _readVitality.Free();
            }
            return base.ReleaseHandle();
        }

        public void Listen(int backlog, Action<UvStreamHandle, int, Exception, object> callback, object state)
        {
            if (_listenVitality.IsAllocated)
            {
                throw new InvalidOperationException("TODO: Listen may not be called more than once");
            }
            try
            {
                _listenCallback = callback;
                _listenState = state;
                _listenVitality = GCHandle.Alloc(this, GCHandleType.Normal);
                _uv.listen(this, backlog, _uv_connection_cb);
            }
            catch
            {
                _listenCallback = null;
                _listenState = null;
                if (_listenVitality.IsAllocated)
                {
                    _listenVitality.Free();
                }
                throw;
            }
        }

        public void Accept(UvStreamHandle handle)
        {
            _uv.accept(this, handle);
        }

        public void ReadStart(
            Func<UvStreamHandle, int, object, Uv.uv_buf_t> allocCallback,
            Action<UvStreamHandle, int, object> readCallback,
            object state)
        {
            if (_readVitality.IsAllocated)
            {
                throw new InvalidOperationException("TODO: ReadStop must be called before ReadStart may be called again");
            }

            try
            {
                _allocCallback = allocCallback;
                _readCallback = readCallback;
                _readState = state;
                _readVitality = GCHandle.Alloc(this, GCHandleType.Normal);
                _uv.read_start(this, _uv_alloc_cb, _uv_read_cb);
            }
            catch
            {
                _allocCallback = null;
                _readCallback = null;
                _readState = null;
                if (_readVitality.IsAllocated)
                {
                    _readVitality.Free();
                }
                throw;
            }
        }

        // UvStreamHandle.ReadStop() should be idempotent to match uv_read_stop()
        public void ReadStop()
        {
            if (_readVitality.IsAllocated)
            {
                _readVitality.Free();
            }
            _allocCallback = null;
            _readCallback = null;
            _readState = null;
            _uv.read_stop(this);
        }

        public int TryWrite(Uv.uv_buf_t buf)
        {
            return _uv.try_write(this, new[] { buf }, 1);
        }

        private static void UvConnectionCb(IntPtr handle, int status)
        {
            var stream = FromIntPtr<UvStreamHandle>(handle);

            Exception error;
            stream.Libuv.Check(status, out error);
            stream._listenCallback(stream, status, error, stream._listenState);
        }

        private static void UvAllocCb(IntPtr handle, int suggested_size, out Uv.uv_buf_t buf)
        {
            var stream = FromIntPtr<UvStreamHandle>(handle);
            try
            {
                buf = stream._allocCallback(stream, suggested_size, stream._readState);
            }
            catch
            {
                buf = stream.Libuv.buf_init(IntPtr.Zero, 0);
                throw;
            }
        }

        private static void UvReadCb(IntPtr handle, int status, ref Uv.uv_buf_t buf)
        {
            var stream = FromIntPtr<UvStreamHandle>(handle);
            stream._readCallback(stream, status, stream._readState);
        }

    }
}
