// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using System.Threading;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Simple awaitable gate - intended to synchronize a single producer with a single consumer to ensure the producer doesn't
    /// produce until the consumer is ready. Similar to a <see cref="TaskCompletionSource{TResult}"/> but reusable so we don't have
    /// to keep allocating new ones every time.
    /// </summary>
    /// <remarks>
    /// The gate can be in one of two states: "Open", indicating that an await will immediately return and "Closed", meaning that an await
    /// will block until the gate is opened. The gate is initially "Closed" and can be opened by a call to <see cref="Open"/>. Upon the completion
    /// of an await, it will automatically return to the "Closed" state (this is done in the <see cref="GetResult"/> call that is injected by the
    /// compiler's async/await logic).
    /// </remarks>
    internal class Gate : ICriticalNotifyCompletion
    {
        private static readonly Action _gateIsOpen = () => {};

        private volatile Action _gateState;

        /// <summary>
        /// Returns a boolean indicating if the gate is "open"
        /// </summary>
        public bool IsCompleted => ReferenceEquals(_gateState, _gateIsOpen);

        public void UnsafeOnCompleted(Action continuation) => OnCompleted(continuation);

        public void OnCompleted(Action continuation)
        {
            // If we're already completed, call the continuation immediately
            if (_gateState == _gateIsOpen)
            {
                continuation();
            }
            else
            {
                // Otherwise, if the current continuation is null, atomically store the new continuation in the field and return the old value
                var previous = Interlocked.CompareExchange(ref _gateState, continuation, null);
                if (previous == _gateIsOpen)
                {
                    // It got completed in the time between the previous the method and the cmpexch.
                    // So call the continuation (the value of _continuation will remain _completed because cmpexch is atomic,
                    // so we didn't accidentally replace it).
                    continuation();
                }
            }
        }

        /// <summary>
        /// Resets the gate to continue blocking the waiter. This is called immediately after awaiting the signal.
        /// </summary>
        public void GetResult()
        {
            // Clear the active continuation to "reset" the state of this event
            Interlocked.Exchange(ref _gateState, null);
        }

        /// <summary>
        /// Set the gate to allow the waiter to continue.
        /// </summary>
        public void Open()
        {
            // Set the stored continuation value to a sentinel that indicates the state is completed, then call the previous value.
            var completion = Interlocked.Exchange(ref _gateState, _gateIsOpen);
            if (completion != _gateIsOpen)
            {
                completion?.Invoke();
            }
        }

        public Gate GetAwaiter() => this;
    }
}
