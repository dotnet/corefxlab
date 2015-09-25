using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    unsafe public abstract class UVStream : UVHandle
    {     
        uv_stream_t* _stream;

        protected UVStream(UVLoop loop, HandleType type) : base(loop, type)
        {
            _stream = (uv_stream_t*)(Handle.ToInt64() + GetSize(HandleType.UV_HANDLE));
        }

        public void ReadStart()
        {
            EnsureNotDisposed();

            // TODO: this buffer management logic does not work for concurrent requests
            if (IsUnix)
            {
                UVException.ThrowIfError(UVInterop.uv_read_start(Handle, UVBuffer.AllocateUnixBuffer, ReadUnix));
            }
            else
            {
                UVException.ThrowIfError(UVInterop.uv_read_start(Handle, UVBuffer.AllocWindowsBuffer, ReadWindows));
            }

        }

        public event Action<Span<byte>> ReadCompleted;
        public event Action<byte[]> ReadAndCopyCompleted;
        public event Action EndOfStream;

        public unsafe void TryWrite(byte[] data)
        {
            Debug.Assert(data != null);
            EnsureNotDisposed();

            fixed (byte* pData = data)
            {
                IntPtr ptrData = (IntPtr)pData;
                if (IsUnix)
                {
                    var buffer = new UnixBufferStruct(ptrData, (uint)data.Length);
                    UVException.ThrowIfError(UVInterop.uv_try_write(Handle, &buffer, 1));
                }
                else
                {
                    var buffer = new WindowsBufferStruct(ptrData, (uint)data.Length);
                    UVException.ThrowIfError(UVInterop.uv_try_write(Handle, &buffer, 1));
                }
            }
        }

        public unsafe void TryWrite(Span<byte> data)
        {
            EnsureNotDisposed();
            GCHandle gcHandle;
            var byteSpan = data.Pin(out gcHandle);

            IntPtr ptrData = (IntPtr)byteSpan.UnsafeBuffer;
            if (IsUnix)
            {
                var buffer = new UnixBufferStruct(ptrData, (uint)data.Length);
                UVException.ThrowIfError(UVInterop.uv_try_write(Handle, &buffer, 1));
            }
            else
            {
                var buffer = new WindowsBufferStruct(ptrData, (uint)data.Length);
                UVException.ThrowIfError(UVInterop.uv_try_write(Handle, &buffer, 1));
            }

            gcHandle.Free(); // TODO: should this be release here or later?
        }

        static bool IsUnix
        {
            get { return false; }
        }

        void OnRead(IntPtr streamPointer, IntPtr bytesAvaliable)
        {
            long bytesRead = bytesAvaliable.ToInt64();
            if (bytesRead == 0)
            {
                return;
            }
            else if (bytesRead < 0)
            {
                var error = UVException.ErrorCodeToError((int)bytesRead);
                if (error == UVError.EOF)
                {
                    OnEndOfStream();
                    base.Dispose();
                }
                else
                {
                    base.Dispose();
                    throw new UVException((int)bytesRead);
                }
            }
            else
            {
                OnReadCompleted(Loop.Pool.Borrow(bytesAvaliable.ToInt32()));
            }
        }

        void OnReadCompleted(byte[] bytesRead)
        {
            if (ReadCompleted != null)
            {
                ReadCompleted(bytesRead);
            }
        }

        void OnReadCompleted(Span<byte> bytesRead)
        {
            if (ReadCompleted != null)
            {
                ReadCompleted(bytesRead);
            }
        }

        static UVInterop.read_callback_unix ReadUnix = OnReadUnix;
        static void OnReadUnix(IntPtr streamPointer, IntPtr size, UnixBufferStruct buffer)
        {
            var stream = As<UVStream>(streamPointer);
            stream.OnRead(streamPointer, size);
        }

        static UVInterop.read_callback_win ReadWindows = OnReadWindows;
        static void OnReadWindows(IntPtr streamPointer, IntPtr size, WindowsBufferStruct buffer)
        {
            var stream = As<UVStream>(streamPointer);
            stream.OnRead(streamPointer, size);
        }

        protected virtual void OnEndOfStream()
        {
            if (EndOfStream != null)
            {
                EndOfStream();
            }
        }

        protected override void Dispose(bool disposing)
        {
            EnsureNotDisposed();

            var request = new CallbackRequest(RequestType.UV_SHUTDOWN);
            request.Callback = (status) =>
            {
                base.Dispose(disposing);
            };
            UVInterop.uv_shutdown(request.Handle, Handle, CallbackRequest.CallbackDelegate);
        }
    }
}
