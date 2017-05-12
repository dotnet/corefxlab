// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    internal struct PipeCompletion
    {
        private static readonly ArrayPool<PipeCompletionCallback> CompletionCallbackPool = ArrayPool<PipeCompletionCallback>.Shared;

        private const int InitialCallbacksSize = 1;
        private static readonly Exception _completedNoException = new Exception();

#if COMPLETION_LOCATION_TRACKING
        private string _completionLocation;
#endif
        public Exception Exception { get; set; }

        private PipeCompletionCallback[] _callbacks;
        private int _callbackCount;

        public string Location
        {
            get
            {
#if COMPLETION_LOCATION_TRACKING
                return _completionLocation;
#else
                return null;
#endif
            }
        }

        public bool IsCompleted => Exception != null;

        public bool TryComplete(Exception exception = null)
        {
#if COMPLETION_LOCATION_TRACKING
            _completionLocation = Environment.StackTrace;
#endif
            if (Exception == null)
            {
                // Set the exception object to the exception passed in or a sentinel value
                Exception = exception ?? _completedNoException;
            }
            return _callbackCount > 0;
        }

        public bool AddCallback(Action<Exception, object> callback, object state)
        {
            if (IsCompleted)
            {
                return true;
            }

            if (_callbacks == null)
            {
                _callbacks = CompletionCallbackPool.Rent(InitialCallbacksSize);
            }

            var newIndex = _callbackCount;
            _callbackCount++;

            if (newIndex == _callbacks.Length)
            {
                var newArray = new PipeCompletionCallback[_callbacks.Length * 2];
                Array.Copy(_callbacks, newArray, _callbacks.Length);
                CompletionCallbackPool.Return(_callbacks);
                _callbacks = newArray;
            }
            _callbacks[newIndex].Callback = callback;
            _callbacks[newIndex].State = state;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsCompletedOrThrow()
        {
            if (Exception == null)
            {
                return false;
            }

            if (Exception != _completedNoException)
            {
                ThrowFailed();
            }

            return true;
        }

        public PipeCompletionCallbacks GetCallbacks()
        {
            Debug.Assert(IsCompleted);

            var callbacks = new PipeCompletionCallbacks(CompletionCallbackPool, _callbackCount, Exception, _callbacks);
            _callbacks = null;
            _callbackCount = 0;

            return callbacks;
        }

        public void Reset()
        {
            Debug.Assert(IsCompleted);
            Exception = null;
#if COMPLETION_LOCATION_TRACKING
            _completionLocation = null;
#endif
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ThrowFailed()
        {
            throw Exception;
        }

        public override string ToString()
        {
            return $"{nameof(IsCompleted)}: {IsCompleted}";
        }
    }
}
