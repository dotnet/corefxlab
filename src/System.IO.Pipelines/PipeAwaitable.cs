// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    internal struct PipeAwaitable
    {
        private static readonly Action _awaitableIsCompleted = () => { };
        private static readonly Action _awaitableIsNotCompleted = () => { };

        private CancelledState _cancelledState;
        private Action _state;

        public PipeAwaitable(bool completed)
        {
            _cancelledState = CancelledState.NotCancelled;
            _state = completed ? _awaitableIsCompleted : _awaitableIsNotCompleted;
        }

        public Action Complete()
        {
            var awaitableState = _state;
            _state = _awaitableIsCompleted;

            if (!ReferenceEquals(awaitableState, _awaitableIsCompleted) &&
                !ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
            {
                return awaitableState;
            }
            return null;
        }

        public void Reset()
        {
            if (IsCompletedSuccessfully)
            {
                _state = _awaitableIsNotCompleted;
            }

            // Change the state from observed -> not cancelled. We only want to reset the cancelled state if it was observed
            if (_cancelledState == CancelledState.CancellationObserved)
            {
                _cancelledState = CancelledState.NotCancelled;
            }
        }

        public bool IsCompleted => ReferenceEquals(_state, _awaitableIsCompleted);

        public bool IsCompletedSuccessfully => IsCompleted && _cancelledState != CancelledState.CancellationRequested;

        public Action OnCompleted(Action continuation, ref PipeCompletion completion)
        {
            var awaitableState = _state;
            if (_state == _awaitableIsNotCompleted)
            {
                _state = continuation;
            }
            
            if (ReferenceEquals(awaitableState, _awaitableIsCompleted))
            {
                return continuation;
            }
            else if (!ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
            {
                completion.TryComplete(ThrowHelper.GetInvalidOperationException(ExceptionResource.NoConcurrentOperation));

                _state = _awaitableIsCompleted;

                Task.Run(continuation);
                Task.Run(awaitableState);
            }

            return null;
        }

        public Action Cancel()
        {
            _cancelledState = CancelledState.CancellationRequested;
            return Complete();
        }

        public bool ObserveCancelation()
        {
            if (_cancelledState == CancelledState.CancellationRequested)
            {
                _cancelledState = CancelledState.CancellationObserved;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"CancelledState: {_cancelledState}, {nameof(IsCompleted)}: {IsCompleted}";
        }

        private enum CancelledState
        {
            NotCancelled = 0,
            CancellationRequested = 1,
            CancellationObserved = 2
        }
    }
}
