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

        public bool IsCompleted => _exception != null;

        public void TryComplete(Exception exception = null)
        {
#if COMPLETION_LOCATION_TRACKING
            _completionLocation = Environment.StackTrace;
#endif
            // Set the exception object to the exception passed in or a sentinel value
            Interlocked.CompareExchange(ref _exception, exception ?? _completedNoException, null);
        }

        public void ThrowIfFailed()
        {
            if (_exception != null && _exception != _completedNoException)
            {
                throw _exception;
            }
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
    }
}
