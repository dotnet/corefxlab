// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Libuv.Interop;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.IO.Pipelines.Networking.Libuv
{
    public class LibuvAwaitable<TRequest> : ICriticalNotifyCompletion where TRequest : UvRequest
    {
        private readonly static Action CALLBACK_RAN = () => { };

        private Action _callback;

        private Exception _exception;

        private int _status;

        public static Action<TRequest, int, object> Callback = (req, status, state) =>
        {
            var awaitable = (LibuvAwaitable<TRequest>)state;

            Exception exception;
            req.Libuv.Check(status, out exception);
            awaitable._exception = exception;
            awaitable._status = status;

            var continuation = Interlocked.Exchange(ref awaitable._callback, CALLBACK_RAN);

            continuation?.Invoke();
        };

        public LibuvAwaitable<TRequest> GetAwaiter() => this;
        public bool IsCompleted => _callback == CALLBACK_RAN;

        public int GetResult()
        {
            var exception = _exception;
            var status = _status;

            // Reset the awaitable state
            _exception = null;
            _status = 0;
            _callback = null;

            if (exception != null)
            {
                throw exception;
            }

            return status;
        }

        public void OnCompleted(Action continuation)
        {
            if (_callback == CALLBACK_RAN ||
                Interlocked.CompareExchange(ref _callback, continuation, null) == CALLBACK_RAN)
            {
                Task.Run(continuation);
            }
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            OnCompleted(continuation);
        }
    }
}