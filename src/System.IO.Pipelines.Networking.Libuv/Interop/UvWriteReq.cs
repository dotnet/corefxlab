// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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
        private PreservedBuffer _buffer;
        private object _state;
        private const int BUFFER_COUNT = 4;

        private List<GCHandle> _pins = new List<GCHandle>(BUFFER_COUNT + 1);

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

        public unsafe void Write(
            UvStreamHandle handle,
            ReadableBuffer buffer,
            Action<UvWriteReq, int, object> callback,
            object state)
        {
            try
            {
                // Preserve the buffer for the async call
                _buffer = buffer.Preserve();
                buffer = _buffer.Buffer;

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
                    void* pointer;
                    if (memory.TryGetPointer(out pointer))
                    {
                        pBuffers[0] = Libuv.buf_init((IntPtr)pointer, memory.Length);
                    }
                    else
                    {
                        throw new InvalidOperationException("Memory needs to be pinned");
                    }
                }
                else
                {
                    int i = 0;
                    void* pointer;
                    foreach (var memory in buffer)
                    {
                        if (memory.TryGetPointer(out pointer))
                        {
                            pBuffers[i++] = Libuv.buf_init((IntPtr)pointer, memory.Length);
                        }
                        else
                        {
                            throw new InvalidOperationException("Memory needs to be pinned");
                        }
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
                _buffer.Dispose();
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
        }

        private static void UvWriteCb(IntPtr ptr, int status)
        {
            var req = FromIntPtr<UvWriteReq>(ptr);
            Unpin(req);

            req._buffer.Dispose();

            var callback = req._callback;
            req._callback = null;

            var state = req._state;
            req._state = null;

            callback(req, status, state);
        }
    }
}