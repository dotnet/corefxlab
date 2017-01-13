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
        /// If this is null or the DoneWritingSentinel, the source will be completed successfully.
        /// If this is an OperationCanceledException, it'll be completed with the exception's token.
        /// Otherwise, it'll be completed as faulted with the exception.
        /// </param>
        internal static void Complete(TaskCompletionSource<VoidResult> tcs, Exception error = null)
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

        /// <summary>Gets a value task representing an error.</summary>
        /// <typeparam name="T">Specifies the type of the value that would have been returned.</typeparam>
        /// <param name="error">The error.  This may be <see cref="DoneWritingSentinel"/>.</param>
        /// <returns>The failed task.</returns>
        internal static ValueTask<T> GetErrorValueTask<T>(Exception error)
        {
            Debug.Assert(error != null);

            Task<T> t;

            if (error == DoneWritingSentinel)
            {
                t = Task.FromException<T>(CreateInvalidCompletionException());
            }
            else
            {
                OperationCanceledException oce = error as OperationCanceledException;
                t = oce != null ?
                    Task.FromCanceled<T>(oce.CancellationToken.IsCancellationRequested ? oce.CancellationToken : new CancellationToken(true)) :
                    Task.FromException<T>(error);
            }

            return new ValueTask<T>(t);
        }

        /// <summary>Wake up all of the waiters and null out the field.</summary>
        /// <param name="waiters">The waiters.</param>
        /// <param name="result">The value with which to complete each waiter.</param>
        internal static void WakeUpWaiters(ref ReaderInteractor<bool> waiters, bool result)
        {
            ReaderInteractor<bool> w = waiters;
            if (w != null)
            {
                w.Success(result);
                waiters = null;
            }
        }

        /// <summary>Removes all interactors from the queue, failing each.</summary>
        /// <param name="interactors">The queue of interactors to complete.</param>
        /// <param name="error">The error with which to complete each interactor.</param>
        internal static void FailInteractors<T, TInner>(Dequeue<T> interactors, Exception error) where T : Interactor<TInner>
        {
            while (!interactors.IsEmpty)
            {
                interactors.DequeueHead().Fail(error ?? CreateInvalidCompletionException());
            }
        }

        /// <summary>Gets or creates a "waiter" (e.g. WaitForRead/WriteAsync) interactor.</summary>
        /// <param name="waiter">The field storing the waiter interactor.</param>
        /// <param name="runContinuationsAsynchronously">true to force continuations to run asynchronously; otherwise, false.</param>
        /// <param name="cancellationToken">The token to use to cancel the wait.</param>
        internal static Task<bool> GetOrCreateWaiter(ref ReaderInteractor<bool> waiter, bool runContinuationsAsynchronously, CancellationToken cancellationToken)
        {
            // Get the existing waiters interactor.
            ReaderInteractor<bool> w = waiter;

            // If there isn't one, create one.  This explicitly does not include the cancellation token,
            // as we reuse it for any number of waiters that overlap.
            if (w == null)
            {
                waiter = w = ReaderInteractor<bool>.Create(runContinuationsAsynchronously);
            }

            // If the cancellation token can't be canceled, then just return the waiter task.
            // If it can, we need to return a task that will complete when the waiter task does but that can also be canceled.
            // Easiest way to do that is with a cancelable continuation.
            return cancellationToken.CanBeCanceled ?
                w.Task.ContinueWith(t => t.Result, cancellationToken, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default) :
                w.Task;
        }

        /// <summary>Creates and returns an exception object to indicate that a channel has been closed.</summary>
        internal static Exception CreateInvalidCompletionException() => new ClosedChannelException();
    }
}
