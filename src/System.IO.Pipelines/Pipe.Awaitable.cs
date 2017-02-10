// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    internal partial class Pipe
    {
        private struct Awaitable
        {
            private static readonly Action _awaitableIsCompleted = () => { };
            private static readonly Action _awaitableIsNotCompleted = () => { };

            private int _cancelledState;
            private Action _state;
            private readonly IScheduler _scheduler;

            public Awaitable(IScheduler scheduler, bool completed)
            {
                _cancelledState = CancelledState.NotCancelled;
                _state = completed ? _awaitableIsCompleted : _awaitableIsNotCompleted;
                _scheduler = scheduler;
            }

            public void Resume()
            {
                var awaitableState = Interlocked.Exchange(
                    ref _state,
                    _awaitableIsCompleted);

                if (!ReferenceEquals(awaitableState, _awaitableIsCompleted) &&
                    !ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
                {
                    _scheduler.Schedule(awaitableState);
                }
            }

            public void Reset()
            {
                // Change the state from observed -> not cancelled. We only want to reset the cancelled state if it was observed
                Interlocked.CompareExchange(ref _cancelledState,
                    CancelledState.NotCancelled,
                    CancelledState.CancellationObserved);

                if (_cancelledState != CancelledState.CancellationRequested)
                {
                    Interlocked.CompareExchange(
                        ref _state,
                        _awaitableIsNotCompleted,
                        _awaitableIsCompleted);
                }
            }

            public bool IsCompleted => ReferenceEquals(_state, _awaitableIsCompleted);

            public void OnCompleted(Action continuation, ref Completion completion)
            {
                var awaitableState = Interlocked.CompareExchange(
                    ref _state,
                    continuation,
                    _awaitableIsNotCompleted);

                if (ReferenceEquals(awaitableState, _awaitableIsNotCompleted))
                {
                    return;
                }
                else if (ReferenceEquals(awaitableState, _awaitableIsCompleted))
                {
                    _scheduler.Schedule(continuation);
                }
                else
                {
                    completion.TryComplete(ThrowHelper.GetInvalidOperationException(ExceptionResource.NoConcurrentOperation));

                    Interlocked.Exchange(
                        ref _state,
                        _awaitableIsCompleted);

                    Task.Run(continuation);
                    Task.Run(awaitableState);
                }
            }

            public void Cancel()
            {
                _cancelledState = CancelledState.CancellationRequested;
                Resume();
            }

            public bool ObserveCancelation()
            {
                return Interlocked.CompareExchange(
                    ref _cancelledState,
                    CancelledState.CancellationObserved,
                    CancelledState.CancellationRequested) == CancelledState.CancellationRequested;
            }

            private static class CancelledState
            {
                public static int NotCancelled = 0;
                public static int CancellationRequested = 1;
                public static int CancellationObserved = 2;
            }
        }
    }
}
