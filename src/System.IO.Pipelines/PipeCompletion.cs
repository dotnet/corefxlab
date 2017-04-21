// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace System.IO.Pipelines
{
    internal struct PipeCompletion
    {
        private static readonly Exception _completedNoException = new Exception();

        private Exception _exception;
#if COMPLETION_LOCATION_TRACKING
        private string _completionLocation;
#endif
        private Action<Exception> _callback;

        public bool IsCompleted => _exception != null;

        public Action TryComplete(Exception exception = null)
        {
#if COMPLETION_LOCATION_TRACKING
            _completionLocation = Environment.StackTrace;
#endif
            if (_exception != null)
            {
                // Set the exception object to the exception passed in or a sentinel value
                _exception = exception ?? _completedNoException;

                var callback = _callback;
                _callback = null;

                // TODO: Allocation
                return () => callback(exception);
            }

            return null;
        }

        public void AttachCallback(Action<Exception> callback)
        {
            if (_callback == null)
            {
                _callback = callback;
            }
            else
            {
                var oldCallback = _callback;
                _callback = exception =>
                {
                    oldCallback(exception);
                    callback(exception);
                };
            }
        }

        public bool IsCompletedOrThrow()
        {
            if (_exception != null)
            {
                if (_exception != _completedNoException)
                {
                    ThrowFailed();
                }
                return true;
            }
            return false;
        }

        private void ThrowFailed()
        {
            throw _exception;
        }

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

        public override string ToString()
        {
            return $"{nameof(IsCompleted)}: {IsCompleted}";
        }
    }
}
