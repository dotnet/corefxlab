using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    public class Idle : UVHandle
    {
        public Idle(UVLoop loop) : base(loop, HandleType.UV_IDLE)
        {
            uv_idle_init(loop.Handle, this.Handle);
        }

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        static extern int uv_idle_init(IntPtr loop, IntPtr idle);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        static extern int uv_idle_start(IntPtr idle, UVInterop.handle_callback callback);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        static extern int uv_idle_stop(IntPtr idle);

        static UVInterop.handle_callback CallbackDelegate = OnCallback;

        unsafe static void OnCallback(IntPtr handle, int status)
        {

        }
        public void Start()
        {
            uv_idle_start(Handle, CallbackDelegate);
        }

        public void Stop()
        {
            uv_idle_stop(Handle);
        }
    }
}
