// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    internal struct PipeOperationState
    {
        private State _state;
#if OPERATION_LOCATION_TRACKING
        private string _operationStartLocation;
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Begin()
        {
            // Inactive and Tenative are allowed
            if (_state == State.Active)
            {
                ThrowHelper.ThrowInvalidOperationException_AlreadyReading();
            }

            _state = State.Active;

#if OPERATION_LOCATION_TRACKING
            _operationStartLocation = Environment.StackTrace;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BeginTentative()
        {
            // Inactive and Tenative are allowed
            if (_state == State.Active)
            {
                ThrowHelper.ThrowInvalidOperationException_AlreadyReading();
            }

            _state = State.Tentative;

#if OPERATION_LOCATION_TRACKING
            _operationStartLocation = Environment.StackTrace;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void End()
        {
            if (_state == State.Inactive)
            {
                ThrowHelper.CreateInvalidOperationException_NoReadToComplete();
            }

            _state = State.Inactive;
#if OPERATION_LOCATION_TRACKING
            _operationStartLocation = null;
#endif
        }

        public bool IsActive => _state == State.Active;
        public bool IsStarted => _state > State.Inactive;
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
            return $"State: {_state}";
        }
    }

    internal enum State: byte
    {
        Inactive = 1,
        Active = 2,
        Tentative = 3
    }
}
