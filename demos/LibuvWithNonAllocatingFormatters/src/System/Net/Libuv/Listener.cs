
namespace System.Net.Libuv
{
    public abstract class UVListener<TStream> : UVHandle where TStream : UVStream
    {
        static UVInterop.handle_callback Notified = OnNotifiedCallback;

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
            UVException.ThrowIfError(UVInterop.uv_listen(Handle, backlog, Notified));
        }

        static void OnNotifiedCallback(IntPtr handle, int status)
        {
            UVStream stream = null;
            TStream connection = null;
            try {
                var listener = As<UVListener<TStream>>(handle);

                stream = listener.CreateStream();
                connection = stream as TStream;
                UVException.ThrowIfError(UVInterop.uv_accept(listener.Handle, connection.Handle));

                if (listener.ConnectionAccepted != null)
                {
                    listener.ConnectionAccepted(connection);
                }
            }
            catch(Exception e)
            {
                if (stream != null) { stream.Dispose(); }
                if (connection != null) { connection.Dispose(); }
                Environment.FailFast(e.ToString());
            }
        }

        protected abstract UVStream CreateStream();
    }
}
