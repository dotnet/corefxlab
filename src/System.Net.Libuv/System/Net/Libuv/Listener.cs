
namespace System.Net.Libuv
{
    public abstract class UVListener<TStream> : UVHandle where TStream : UVStream
    {
        static UVInterop.handle_callback Notified = OnNotified;

        public event Action<TStream> ConnectionAccepted;

        public int DefaultBacklog { get; set; }

        protected UVListener(UVLoop loop, HandleType type) : base(loop, type)
        {
            DefaultBacklog = 128;
        }

        public void Listen()
        {
            Listen(DefaultBacklog);
        }

        public void Listen(int backlog)
        {
            UVInterop.uv_listen(Handle, backlog, Notified);
        }

        static void OnNotified(IntPtr handle, int status)
        {
            var listener = As<UVListener<TStream>>(handle);

            var stream = listener.CreateStream();
            var connection = stream as TStream;
            try
            {
                UVException.ThrowIfError(UVInterop.uv_accept(listener.Handle, connection.Handle));
            }
            catch(Exception e)
            {
                stream.Dispose();
                connection.Dispose();
                Environment.FailFast(e.ToString());
            }

            if (listener.ConnectionAccepted != null)
            {
                listener.ConnectionAccepted(connection);
            }
        }

        protected abstract UVStream CreateStream();
    }
}
