using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.IO.Pipelines.Networking.Sockets.Internal
{
    /// <summary>
    /// Very lightweight awaitable gate - intended for use in high-volume single-producer/single-consumer
    /// scenario, in particular targeting the bridge between async IO operations
    /// and the async method that is pumping the read/write queue. A key consideration is that
    /// no objects (in particular Task/TaskCompletionSource) are allocated even in the await case. Instead,
    /// a custom awaiter is provided. Works like a <see cref="ManualResetEvent "/> - the <see cref="Reset"/> method must
    /// be called between operations.
    /// </summary>
    internal class Signal : ICriticalNotifyCompletion
    {
        private readonly ContinuationMode _continuationMode;

        private Action _continuation;
        private static readonly Action _completedSentinel = delegate { };

        public Signal(ContinuationMode continuationMode = ContinuationMode.Synchronous)
        {
            _continuationMode = continuationMode;
        }

        public bool IsCompleted => ReferenceEquals(_completedSentinel, Volatile.Read(ref _continuation));

        private object SyncLock => this;

        public Signal GetAwaiter() => this;

        public void GetResult() { }

        public void UnsafeOnCompleted(Action continuation) => OnCompleted(continuation);

        public void OnCompleted(Action continuation)
        {
            if (continuation != null)
            {
                var oldValue = Interlocked.CompareExchange(ref _continuation, continuation, null);

                if (ReferenceEquals(oldValue, _completedSentinel))
                {
                    // already complete; calback sync
                    continuation.Invoke();
                }
                else if (oldValue != null)
                {
                    ThrowMultipleCallbacksNotSupported();
                }
            }
        }
        private static void ThrowMultipleCallbacksNotSupported()
        {
            throw new NotSupportedException("Multiple callbacks via Signal.OnCompleted are not supported");
        }


        public void Reset()
        {
            Volatile.Write(ref _continuation, null);
        }

        public void Set()
        {
            Action continuation = Interlocked.Exchange(ref _continuation, _completedSentinel);

            if (continuation != null && !ReferenceEquals(continuation, _completedSentinel))
            {
                switch (_continuationMode)
                {
                    case ContinuationMode.Synchronous:
                        continuation.Invoke();
                        break;
                    case ContinuationMode.ThreadPool:
                        ThreadPool.QueueUserWorkItem(state => ((Action)state).Invoke(), continuation);
                        break;
                }
            }
        }

        // utility method for people who don't feel comfortable with `await obj;` and prefer `await obj.WaitAsync();`
        internal Signal WaitAsync() => this;
    }

}
