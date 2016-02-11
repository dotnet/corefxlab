using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    unsafe internal class UVRequest : IDisposable
    {
        IntPtr _handle;
        uv_req_t* _requestPointer;

        public UVRequest(RequestType type) 
        {
            var size = UVInterop.uv_req_size(type);
            _handle = Marshal.AllocHGlobal(size);
            _requestPointer = (uv_req_t*)_handle;

            GCHandle = GCHandle.Alloc(this, GCHandleType.Normal);
        }

        public IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Data != IntPtr.Zero)
            {
                if (GCHandle.IsAllocated)
                {
                    GCHandle.Free();
                }
                Data = IntPtr.Zero;
            }

            if (_handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_handle);
                _handle = IntPtr.Zero;
            }
        }

        public IntPtr Data
        {
            get
            {
                return _requestPointer->data;
            }
            set
            {
                _requestPointer->data = value;
            }
        }

        public RequestType Type
        {
            get
            {
                return _requestPointer->type;
            }
            set
            {
                _requestPointer->type = value;
            }
        }

        GCHandle GCHandle
        {
            get
            {
                return GCHandle.FromIntPtr(Data);
            }
            set
            {
                Data = GCHandle.ToIntPtr(value);
            }
        }
    }

    internal class CallbackRequest : UVRequest
    {
        public CallbackRequest(RequestType type) : base(type)
        {}

        public Action<int> Callback { get; set; }

        public static UVInterop.handle_callback CallbackDelegate = OnCallback;

        unsafe static void OnCallback(IntPtr handle, int status)
        {
            uv_req_t* uvRequest = (uv_req_t*)handle.ToPointer();
            var request = GCHandle.FromIntPtr(uvRequest->data).Target as CallbackRequest;
            if (request == null)
            {
                Environment.FailFast("invalid callback");
            }
            else
            {
                request.Callback(status);
                request.Dispose();
            }
        }
    }

    internal class DisposeRequest : UVRequest
    {
        UVHandle _handle;

        public DisposeRequest(UVHandle handle) : base(RequestType.UV_SHUTDOWN)
        {
            _handle = handle;
            UVInterop.uv_shutdown(Handle, _handle.Handle, CallbackDelegate);
        }

        static UVInterop.handle_callback CallbackDelegate = OnCallback;

        unsafe static void OnCallback(IntPtr handle, int status)
        {
            uv_req_t* uvRequest = (uv_req_t*)handle.ToPointer();
            var request = GCHandle.FromIntPtr(uvRequest->data).Target as DisposeRequest;
            if (request == null)
            {
                Environment.FailFast("invalid callback");
            }
            else
            {
                request._handle.Dispose();
                request.Dispose();
            }
        }
    }
}
