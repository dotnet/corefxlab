using System.Net.Sockets;

namespace System.IO.Pipelines.Networking.Sockets.Internal
{
    internal static class SocketExtensions
    {
        /// <summary>
        /// Note that this presumes that args.UserToken is already a Signal, and that args.Completed
        /// knows to call .Set on the Signal
        /// </summary>
        public static Signal ReceiveSignalAsync(this Socket socket, SocketAsyncEventArgs args)
        {
            var signal = (Signal)args.UserToken;
            signal.Reset();
            if (!socket.ReceiveAsync(args))
            {   // mark it as already complete (probably an error)
                signal.Set();
            }
            return signal;
        }

        /// <summary>
        /// Note that this presumes that args.UserToken is already a Signal, and that args.Completed
        /// knows to call .Set on the Signal
        /// </summary>
        public static Signal SendSignalAsync(this Socket socket, SocketAsyncEventArgs args)
        {
            var signal = (Signal)args.UserToken;
            signal.Reset();
            if (!socket.SendAsync(args))
            {   // mark it as already complete (probably an error)
                signal.Set();
            }
            return signal;
        }
    }
}
