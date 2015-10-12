using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    public class UVLoop // TODO: implement IDisposable to shut down the loop
    {
        static UVLoop s_default;

        IntPtr _handle;
        UVBuffer _pool;

        public static UVLoop Default
        {
            get
            {
                if (s_default == null)
                {
                    s_default = new UVLoop(UVInterop.uv_default_loop(), UVBuffer.Default);
                }
                return s_default;
            }
        }

        public UVLoop()
        {
            _pool = UVBuffer.Default;
            var size = UVInterop.uv_loop_size().ToInt32();
            var loopHandle = Marshal.AllocHGlobal(size); // this needs to be deallocated
            UVException.ThrowIfError(UVInterop.uv_loop_init(loopHandle));
            _handle = loopHandle;
        }

        UVLoop(IntPtr handle, UVBuffer pool) 
        {
            _handle = handle;
            _pool = pool;
        }

        public IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        internal UVBuffer Pool { get { return _pool; } }

        public bool IsAlive
        {
            get
            {
                return UVInterop.uv_loop_alive(_handle) != 0;
            }
        }

        public void Run()
        {
            UVInterop.uv_run(_handle, uv_run_mode.UV_RUN_DEFAULT);
        }

        public void Stop()
        {
            UVInterop.uv_stop(_handle);
        }
    }
}

