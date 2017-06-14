// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.IO.Pipelines.Networking.Libuv.Interop
{
    /// <summary>
    /// Summary description for UvWriteRequest
    /// </summary>
    public class UvWriteReq : UvRequest
    {
        private readonly static Uv.uv_write_cb _uv_write_cb = (IntPtr ptr, int status) => UvWriteCb(ptr, status);

        private IntPtr _bufs;

        private Action<UvWriteReq, int, object> _callback;
        private object _state;
        private const int BUFFER_COUNT = 4;

        private List<BufferHandle> _handles = new List<BufferHandle>();
        private List<GCHandle> _pins = new List<GCHandle>(BUFFER_COUNT + 1);

        private LibuvAwaitable<UvWriteReq> _awaitable = new LibuvAwaitable<UvWriteReq>();

        public UvWriteReq() : base()
        {
        }

        public void Init(UvLoopHandle loop)
        {
            var requestSize = loop.Libuv.req_size(Uv.RequestType.WRITE);
            var bufferSize = Marshal.SizeOf<Uv.uv_buf_t>() * BUFFER_COUNT;
            CreateMemory(
                loop.Libuv,
                loop.ThreadId,
                requestSize + bufferSize);
            _bufs = handle + requestSize;
        }

        public unsafe LibuvAwaitable<UvWriteReq> WriteAsync(
            UvStreamHandle handle,
            ReadableBuffer buffer)
        {
            Write(handle, buffer, LibuvAwaitable<UvWriteReq>.Callback, _awaitable);
            return _awaitable;
        }

        private unsafe void Write(
            UvStreamHandle handle,
            ReadableBuffer buffer,
            Action<UvWriteReq, int, object> callback,
            object state)
        {
            try
            {
                int nBuffers = 0;
                if (buffer.IsSingleSpan)
                {
                    nBuffers = 1;
                }
                else
                {
                    foreach (var span in buffer)
                    {
                        nBuffers++;
                    }
                }

                // add GCHandle to keeps this SafeHandle alive while request processing
                _pins.Add(GCHandle.Alloc(this, GCHandleType.Normal));

                var pBuffers = (Uv.uv_buf_t*)_bufs;
                if (nBuffers > BUFFER_COUNT)
                {
                    // create and pin buffer array when it's larger than the pre-allocated one
                    var bufArray = new Uv.uv_buf_t[nBuffers];
                    var gcHandle = GCHandle.Alloc(bufArray, GCHandleType.Pinned);
                    _pins.Add(gcHandle);
                    pBuffers = (Uv.uv_buf_t*)gcHandle.AddrOfPinnedObject();
                }

                if (nBuffers == 1)
                {
                    var memory = buffer.First;
                    var memoryHandle = memory.Retain(pin: true);
                    _handles.Add(memoryHandle);
                    pBuffers[0] = Libuv.buf_init((IntPtr)memoryHandle.PinnedPointer, memory.Length);
                }
                else
                {
                    int i = 0;
                    foreach (var memory in buffer)
                    {
                        var memoryHandle = memory.Retain(pin: true);
                        _handles.Add(memoryHandle);
                        pBuffers[i++] = Libuv.buf_init((IntPtr)memoryHandle.PinnedPointer, memory.Length);
                    }
                }

                _callback = callback;
                _state = state;
                _uv.write(this, handle, pBuffers, nBuffers, _uv_write_cb);
            }
            catch
            {
                _callback = null;
                _state = null;
                Unpin(this);
                throw;
            }
        }

        private static void Unpin(UvWriteReq req)
        {
            foreach (var pin in req._pins)
            {
                pin.Free();
            }
            req._pins.Clear();

            foreach (var handle in req._handles)
            {
                handle.Dispose();
            }
            req._handles.Clear();
        }

        private static void UvWriteCb(IntPtr ptr, int status)
        {
            var req = FromIntPtr<UvWriteReq>(ptr);
            Unpin(req);

            var callback = req._callback;
            req._callback = null;

            var state = req._state;
            req._state = null;

            callback(req, status, state);
        }
    }
}
