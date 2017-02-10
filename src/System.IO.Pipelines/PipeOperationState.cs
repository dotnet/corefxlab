// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace System.IO.Pipelines
{
    internal struct PipeOperationState
    {
        private int _state;
#if OPERATION_LOCATION_TRACKING
        private string _operationStartLocation;
#endif

        public void Begin(ExceptionResource exception)
        {
            var success =  Interlocked.Exchange(ref _state, State.Active) == State.NotActive;
            if (!success)
            {
                ThrowHelper.ThrowInvalidOperationException(exception, Location);
            }
#if OPERATION_LOCATION_TRACKING
            _operationStartLocation = Environment.StackTrace;
#endif
        }

        public void End(ExceptionResource exception)
        {
            var success = Interlocked.Exchange(ref _state, State.NotActive) == State.Active;
            if (!success)
            {
                ThrowHelper.ThrowInvalidOperationException(exception, Location);
            }
#if OPERATION_LOCATION_TRACKING
            _operationStartLocation = null;
#endif
        }

        public bool IsActive => _state == State.Active;

        public string Location
        {
            get
            {
#if OPERATION_LOCATION_TRACKING
                return _operationStartLocation;
#else
                return null;
#endif
            }
        }

        private static class State
        {
            public static int NotActive = 0;
            public static int Active = 1;
        }
    }
}
