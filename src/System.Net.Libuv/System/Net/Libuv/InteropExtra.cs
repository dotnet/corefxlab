using System.Runtime.InteropServices;

namespace System.Net.Libuv
{
    // This is roughly based on LibuvSharp
    static partial class UVInterop
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        [DllImport("uv", CallingConvention = CallingConvention.Cdecl)]
        internal extern static uint uv_version();



        /// <summary>
        /// convert a binary structure containing an IPv4 address to a string.
        /// </summary>
        /// <returns></returns>
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        unsafe internal extern static int uv_udp_getsockname(
            IntPtr handle,IntPtr sockaddr,IntPtr size
        );

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int uv_ip4_name(IntPtr sockaddr, byte[] buffer,IntPtr size);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int uv_ip6_name(IntPtr sockaddr, byte[] buffer,IntPtr size);


        /// <summary>
        /// </summary>
        /// <param name="loop"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        #region uv_udp_init
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_udp_init(IntPtr loop, IntPtr handle);
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="sockaddr"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        #region uv_udp_bind
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_udp_bind(IntPtr handle, ref sockaddr_in sockaddr, uint flags);

        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int uv_udp_bind(IntPtr handle, ref sockaddr_in6 sockaddr, uint flags);
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="req"></param>
        /// <param name="handle"></param>
        /// <param name="bufferList"></param>
        /// <param name="bufferCount"></param>
        /// <param name="socksockaddr"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        #region uv_udp_send
        [DllImport("libuv", EntryPoint = "uv_udp_send", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int uv_udp_send_unix(IntPtr req, IntPtr handle,
            UVBuffer.Windows* bufferList, int bufferCount, ref sockaddr_in socksockaddr, handle_callback callback);
        [DllImport("libuv", EntryPoint = "uv_udp_send", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int uv_udp_send_unix(IntPtr req, IntPtr handle,
            UVBuffer.Windows* bufferList, int bufferCount, ref sockaddr_in6 socksockaddr, handle_callback callback);
        [DllImport("libuv", EntryPoint = "uv_udp_send", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int uv_udp_send_unix(IntPtr req, IntPtr handle,
            UVBuffer.Unix* bufferList, int bufferCount, ref sockaddr_in socksockaddr, handle_callback callback);
        [DllImport("libuv", EntryPoint = "uv_udp_send", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int uv_udp_send_unix(IntPtr req, IntPtr handle,
              UVBuffer.Unix* bufferList, int bufferCount, ref sockaddr_in6 socksockaddr, handle_callback callback);
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="bufferList"></param>
        /// <param name="bufferCount"></param>
        /// <param name="addr"></param>
        /// <returns></returns>
        #region uv_udp_try_send
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int uv_udp_try_send_win(IntPtr handle,
            UVBuffer.Windows* bufferList, int bufferCount, ref sockaddr_in addr);
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int uv_udp_try_send_win(IntPtr handle,
            UVBuffer.Windows* bufferList, int bufferCount, ref sockaddr_in6 addr);
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int uv_udp_try_send_unix(IntPtr handle,
            UVBuffer.Unix* bufferList, int bufferCount, ref sockaddr_in addr);
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern int uv_udp_try_send_unix(IntPtr handle,
            UVBuffer.Unix* bufferList, int bufferCount, ref sockaddr_in6 addr);
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="alloc_callback"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        #region uv_udp_recv_start
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int uv_udp_recv_start(IntPtr handle, 
            alloc_callback_unix alloc_callback, start_receive_callback_unix callback);
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int uv_udp_recv_start(IntPtr handle,
            alloc_callback_win alloc_callback, start_receive_callback_win callback);
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        #region uv_udp_recv_stop
        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
        internal extern static int uv_udp_recv_stop(IntPtr handle);
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="buffer"></param>
        #region alloc_callback
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //internal delegate void alloc_callback_unix(IntPtr data, uint size, out UVBuffer.Unix buffer);
        //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        //internal delegate void alloc_callback_win(IntPtr data, uint size, out UVBuffer.Windows buffer);
        #endregion

        /// <summary>
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="nread"></param>
        /// <param name="buf"></param>
        /// <param name="sockaddr"></param>
        /// <param name="flags"></param>
        #region start_receive_callback
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void start_receive_callback_unix(IntPtr stream, IntPtr size, 
            ref UVBuffer.Unix buffer, IntPtr sockaddr, int flags);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void start_receive_callback_win(IntPtr stream, IntPtr size,
            ref UVBuffer.Windows buffer,IntPtr sockaddr, int flags);
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct sockaddr_storage
    {
        public short sin_family;
        public ushort sin_port;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct uv_udp_t
    {
        /// <summary>
        /// number of bytes queued for sending. 
        /// This field strictly shows how much 
        /// information is currently queued
        /// </summary>
        public IntPtr send_queue_size;

        /// <summary>
        /// number of send requests currently in 
        /// the queue awaiting to be processed.
        /// </summary>
        public IntPtr send_queue_count;
    }
}
