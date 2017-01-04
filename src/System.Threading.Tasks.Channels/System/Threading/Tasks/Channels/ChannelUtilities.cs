// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides internal helper methods for implementing channels.</summary>
    internal static class ChannelUtilities
    {
        /// <summary>Sentinel object used to indicate being done writing.</summary>
        internal static readonly Exception DoneWritingSentinel = new Exception(nameof(DoneWritingSentinel));
        /// <summary>A cached task with a Boolean true result.</summary>
        internal static readonly Task<bool> TrueTask = Task.FromResult(true);
        /// <summary>A cached task with a Boolean false result.</summary>
        internal static readonly Task<bool> FalseTask = Task.FromResult(false);

        /// <summary>Completes the specified TaskCompletionSource.</summary>
        /// <param name="tcs">The source to complete.</param>
        /// <param name="error">
        /// The optional exception with which to complete.  
        /// If this is null, the source will be completed successfully.
        /// If this is an OperationCanceledException, it'll be completed with the exception's token.
        /// Otherwise, it'll be completed as faulted with the exception.
        /// </param>
        internal static void CompleteWithOptionalError(TaskCompletionSource<VoidResult> tcs, Exception error)
        {
            OperationCanceledException oce = error as OperationCanceledException;
            if (oce != null)
            {
                tcs.TrySetCanceled(oce.CancellationToken);
            }
            else if (error != null && error != DoneWritingSentinel)
            {
                tcs.TrySetException(error);
            }
            else
            {
                tcs.TrySetResult(default(VoidResult));
            }
        }

        /// <summary>
        /// Given an already faulted or canceled Task, returns a new generic task
        /// with the same failure or cancellation token.
        /// </summary>
        internal static async Task<T> PropagateErrorAsync<T>(Task t)
        {
            Debug.Assert(t.IsFaulted || t.IsCanceled, $"Expected Faulted or Canceled, got {t.Status}");
            await t;
            throw new InvalidOperationException(); // Awaiting should have thrown
        }

        /// <summary>Removes all waiters from the queue, completing each.</summary>
        /// <param name="waiters">The queue of waiters to complete.</param>
        /// <param name="result">The value with which to complete each waiter.</param>
        internal static void WakeUpWaiters(Dequeue<ReaderInteractor<bool>> waiters, bool result)
        {
            if (waiters.Count > 0)
            {
                WakeUpWaitersCore(waiters, result); // separated out to streamline inlining
            }
        }

        /// <summary>Core of ChannelUtilities.WakeUpWaiters, separated out for performance due to inlining.</summary>
        internal static void WakeUpWaitersCore(Dequeue<ReaderInteractor<bool>> waiters, bool result)
        {
            while (waiters.Count > 0)
            {
                waiters.DequeueHead().Success(result);
            }
        }

        /// <summary>Creates an exception detailing concurrent use of a single reader/writer channel.</summary>
        internal static Exception CreateSingleReaderWriterMisuseException() =>
            new InvalidOperationException(Properties.Resources.InvalidOperationException_SingleReaderWriterUsedConcurrently);

        /// <summary>Creates and returns an exception object to indicate that a channel has been closed.</summary>
        internal static Exception CreateInvalidCompletionException() => new ClosedChannelException();
    }
}
