using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    unsafe public abstract class UVHandle : IDisposable
    {
        IntPtr _handle;
        UVLoop _loop;

        GCHandle _gcHandle { get; set; }

        protected UVHandle(UVLoop loop, HandleType handleType)
        {
            Debug.Assert(loop != null);
            _loop = loop;
            _handle = Allocate(handleType);

            _gcHandle = GCHandle.Alloc(this);
            Data = GCHandle.ToIntPtr(_gcHandle);
        }

        public UVLoop Loop
        {
            get
            {
                return _loop;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public HandleType Type
        {
            get { return HandlePointer->type; }
        }

        #region Lifetime management
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!IsDisposing && !IsDisposed)
            {
                UVInterop.uv_close(_handle, CloseCallback);
            }
        }

        bool IsDisposed
        {
            get {
                return _handle == IntPtr.Zero;
            }
        }

        public bool IsDisposing
        {
            get { 
                if (IsDisposed) { return false; }
                return UVInterop.uv_is_closing(_handle) != 0;
            }
        }

        static UVInterop.close_callback CloseCallback = OnClose;

        static void OnClose(IntPtr handlePointer)
        {
            var handle = UVHandle.As<UVHandle>(handlePointer);
            handle.Free(handlePointer);
        }

        static IntPtr Allocate(HandleType type)
        {
            var byteCount = GetSize(type);
            return Marshal.AllocHGlobal(byteCount);
        }

        void Free(IntPtr nativeHandle)
        {
            if (_handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeHandle);

                _handle = IntPtr.Zero;

                if (_gcHandle.IsAllocated)
                {
                    _gcHandle.Free();
                }
            }
        }

        protected void EnsureNotDisposed()
        {
            if (_handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException(GetType().ToString(), "handle was closed");
            }
        }
        #endregion

        protected static int GetSize(HandleType type)
        {
            return UVInterop.uv_handle_size(type);
        }

        internal static T As<T>(IntPtr handle)
        {
            var data = ((uv_handle_t*)handle)->data;
            return (T)GCHandle.FromIntPtr(data).Target;
        }

        uv_handle_t* HandlePointer
        {
            get
            {
                EnsureNotDisposed();
                return (uv_handle_t*)_handle;
            }
        }

        IntPtr Data
        {
            get
            {
                return HandlePointer->data;
            }
            set
            {
                HandlePointer->data = value;
            }
        }

        public enum HandleType : int
        {
            UV_UNKNOWN_HANDLE = 0,
            UV_ASYNC,
            UV_CHECK,
            UV_FS_EVENT,
            UV_FS_POLL,
            UV_HANDLE,
            UV_IDLE,
            UV_NAMED_PIPE,
            UV_POLL,
            UV_PREPARE,
            UV_PROCESS,
            UV_STREAM,
            UV_TCP,
            UV_TIMER,
            UV_TTY,
            UV_UDP,
            UV_SIGNAL,
            UV_FILE,
            UV_HANDLE_TYPE_PRIVATE,
            UV_HANDLE_TYPE_MAX,
        }
    }
}
