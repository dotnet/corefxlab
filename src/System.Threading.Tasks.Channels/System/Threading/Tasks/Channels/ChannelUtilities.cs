// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

        /// <summary>Removes all waiters from the queue, completing each.</summary>
        /// <param name="syncObj">Lock held while manipulating <see cref="waiters"/> but not while completing each waiter.</param>
        /// <param name="waiters">The queue of waiters to complete.</param>
        /// <param name="result">The value with which to complete each waiter.</param>
        internal static void WakeUpWaiters(object syncObj, Dequeue<ReaderInteractor<bool>> waiters, bool result)
        {
            while (true)
            {
                ReaderInteractor<bool> r;
                lock (syncObj)
                {
                    if (waiters.Count == 0) return;
                    r = waiters.DequeueHead();
                }
                r.Success(result);
            }
        }

        /// <summary>Removes all interactors from the queue, failing each.</summary>
        /// <param name="interactors">The queue of interactors to complete.</param>
        /// <param name="error">The error with which to complete each interactor.</param>
        internal static void FailInteractors<T>(Dequeue<ReaderInteractor<T>> interactors, Exception error)
        {
            while (interactors.Count > 0)
            {
                interactors.DequeueHead().Fail(error ?? CreateInvalidCompletionException());
            }
        }

        /// <summary>Removes all interactors from the queue, failing each.</summary>
        /// <param name="syncObj">Lock held while manipulating <see cref="interactors"/> but not while completing each interactor.</param>
        /// <param name="interactors">The queue of interactors to complete.</param>
        /// <param name="error">The error with which to complete each interactor.</param>
        internal static void FailInteractors<T>(object syncObj, Dequeue<ReaderInteractor<T>> interactors, Exception error)
        {
            while (true)
            {
                ReaderInteractor<T> interactor;
                lock (syncObj)
                {
                    if (interactors.Count == 0) return;
                    interactor = interactors.DequeueHead();
                }
                interactor.Fail(error ?? CreateInvalidCompletionException());
            }
        }

        /// <summary>Creates an exception detailing concurrent use of a single reader/writer channel.</summary>
        internal static Exception CreateSingleReaderWriterMisuseException() =>
            new InvalidOperationException(Properties.Resources.InvalidOperationException_SingleReaderWriterUsedConcurrently);

        /// <summary>Creates and returns an exception object to indicate that a channel has been closed.</summary>
        internal static Exception CreateInvalidCompletionException() => new ClosedChannelException();
    }
}
