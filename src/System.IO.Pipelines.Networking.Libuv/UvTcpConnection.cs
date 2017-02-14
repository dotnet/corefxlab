// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Libuv.Interop;

namespace System.IO.Pipelines.Networking.Libuv
{
    public class UvTcpConnection : IPipeConnection
    {
        private const int EOF = -4095;

        private static readonly Action<UvStreamHandle, int, object> _readCallback = ReadCallback;
        private static readonly Func<UvStreamHandle, int, object, Uv.uv_buf_t> _allocCallback = AllocCallback;

        protected readonly IPipe _input;
        protected readonly IPipe _output;
        private readonly UvThread _thread;
        private readonly UvTcpHandle _handle;
        private volatile bool _stopping;

        private Task _sendingTask;
        private WritableBuffer? _inputBuffer;

        public UvTcpConnection(UvThread thread, UvTcpHandle handle)
        {
            _thread = thread;
            _handle = handle;

            _input = _thread.PipeFactory.Create(new PipeOptions
            {
                // ReaderScheduler = TaskRunScheduler.Default, // execute user code on the thread pool
                // ReaderScheduler = thread,
                WriterScheduler = thread // resume from back pressure on the uv thread
            });

            _output = _thread.PipeFactory.Create(new PipeOptions
            {
                // WriterScheduler = TaskRunScheduler.Default, // Execute the flush callback on the thread pool
                // WriterScheduler = thread,
                ReaderScheduler = thread // user code will dispatch back to the uv thread for writes,
            });

            StartReading();
            _sendingTask = ProcessWrites();
        }

        public async Task DisposeAsync()
        {
            // Dispose on the thread pool so we don't return on the libuv thread
            await Task.Factory.StartNew(state => ((UvTcpConnection)state).DisposeAsyncCore(), this);
        }

        private async Task DisposeAsyncCore()
        {
            if (!_stopping)
            {
                _stopping = true;
                _output.Reader.CancelPendingRead();

                await _sendingTask.ConfigureAwait(false);

                _output.Writer.Complete();
                _input.Reader.Complete();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            DisposeAsync().GetAwaiter().GetResult();
        }

        public IPipeWriter Output => _output.Writer;

        public IPipeReader Input => _input.Reader;

        private async Task ProcessWrites()
        {
            try
            {
                while (!_stopping)
                {
                    var result = await _output.Reader.ReadAsync();
                    var buffer = result.Buffer;

                    try
                    {
                        if (buffer.IsEmpty && result.IsCompleted)
                        {
                            break;
                        }

                        if (!buffer.IsEmpty)
                        {
                            await WriteAsync(buffer);
                        }
                    }
                    finally
                    {
                        _output.Reader.Advance(buffer.End);
                    }
                }
            }
            catch (Exception ex)
            {
                _output.Reader.Complete(ex);
            }
            finally
            {
                _output.Reader.Complete();

                _handle.Dispose();

                // We'll never call the callback after disposing the handle
                _input.Writer.Complete();
            }
        }

        private async Task WriteAsync(ReadableBuffer buffer)
        {
            var writeReq = _thread.WriteReqPool.Allocate();

            try
            {
                await writeReq.WriteAsync(_handle, buffer);

            }
            finally
            {
                _thread.WriteReqPool.Return(writeReq);
            }
        }

        private void StartReading()
        {
            _handle.ReadStart(_allocCallback, _readCallback, this);
        }

        private static void ReadCallback(UvStreamHandle handle, int status, object state)
        {
            ((UvTcpConnection)state).OnRead(handle, status);
        }

        private async void OnRead(UvStreamHandle handle, int status)
        {
            if (status == 0)
            {
                // A zero status does not indicate an error or connection end. It indicates
                // there is no data to be read right now.
                // See the note at http://docs.libuv.org/en/v1.x/stream.html#c.uv_read_cb.
                _inputBuffer?.Commit();
                _inputBuffer = null;
                return;
            }

            var normalRead = status > 0;
            var normalDone = status == EOF;
            var errorDone = !(normalDone || normalRead);
            var readCount = normalRead ? status : 0;

            if (!normalRead)
            {
                handle.ReadStop();
            }

            IOException error = null;
            if (errorDone)
            {
                Exception uvError;
                handle.Libuv.Check(status, out uvError);
                error = new IOException(uvError.Message, uvError);

                _inputBuffer?.Commit();

                // REVIEW: Should we treat ECONNRESET as an error?
                // Ignore the error for now
                _input.Writer.Complete();
            }
            else
            {
                var inputBuffer = _inputBuffer.Value;
                _inputBuffer = null;

                inputBuffer.Advance(readCount);
                inputBuffer.Commit();

                // Flush if there was data
                if (readCount > 0)
                {
                    var awaitable = inputBuffer.FlushAsync();

                    if (!awaitable.IsCompleted)
                    {
                        // If there's back pressure
                        handle.ReadStop();

                        // Resume reading when the awaitable completes
                        if (await awaitable)
                        {
                            StartReading();
                        }
                        else
                        {
                            // We're done writing, the reading is gone
                            _input.Writer.Complete();
                        }
                    }
                }
            }

            if (normalDone)
            {
                _input.Writer.Complete();
            }
        }

        private static Uv.uv_buf_t AllocCallback(UvStreamHandle handle, int status, object state)
        {
            return ((UvTcpConnection)state).OnAlloc(handle, status);
        }

        private unsafe Uv.uv_buf_t OnAlloc(UvStreamHandle handle, int status)
        {
            var inputBuffer = _input.Writer.Alloc(2048);

            _inputBuffer = inputBuffer;

            void* pointer;
            if (!inputBuffer.Memory.TryGetPointer(out pointer))
            {
                throw new InvalidOperationException("Pointer must be pinned");
            }


            return handle.Libuv.buf_init((IntPtr)pointer, inputBuffer.Memory.Length);
        }
    }
}
