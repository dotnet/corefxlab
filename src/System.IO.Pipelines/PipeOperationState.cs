// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Threading;

namespace System.IO.Pipelines
{
    internal struct PipeOperationState
    {
        private bool _active;
#if OPERATION_LOCATION_TRACKING
        private string _operationStartLocation;
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Begin(ExceptionResource exception)
        {
            if (_active)
            {
                ThrowHelper.ThrowInvalidOperationException(exception, Location);
            }

            _active = true;

#if OPERATION_LOCATION_TRACKING
            _operationStartLocation = Environment.StackTrace;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void End(ExceptionResource exception)
        {
            if (!_active)
            {
                ThrowHelper.ThrowInvalidOperationException(exception, Location);
            }

            _active = false;
#if OPERATION_LOCATION_TRACKING
            _operationStartLocation = null;
#endif
        }

        public bool IsActive => _active;

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

        public override string ToString()
        {
            return $"{nameof(IsActive)}: {IsActive}";
        }
    }
}
