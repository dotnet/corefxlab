// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides static methods for creating and working with channels.</summary>
    public static partial class Channel
    {
        /// <summary>Sentinel object used to indicate being done writing.</summary>
        private static readonly Exception s_doneWritingSentinel = new Exception("s_doneWritingSentinel");
        /// <summary>A cached task with a Boolean true result.</summary>
        private static readonly Task<bool> s_trueTask = Task.FromResult(true);
        /// <summary>A cached task with a Boolean false result.</summary>
        private static readonly Task<bool> s_falseTask = Task.FromResult(false);
        /// <summary>Sentinel value to indicate an infinite bound.</summary>
        public const int Unbounded = -1;

        /// <summary>
        /// Creates a buffered channel.  If the specified <paramref name="bufferedCapacity"/> is not <see cref="Unbounded"/>,
        /// the channel may only store up to that number of items; attempts to store more than that will result in writes
        /// being delayed.
        /// </summary>
        /// <typeparam name="T">Specifies the type of data stored in the channel.</typeparam>
        /// <returns>The new channel.</returns>
        public static IChannel<T> Create<T>(int bufferedCapacity = Unbounded, bool singleReaderWriter = false)
        {
            if (bufferedCapacity <= 0 && bufferedCapacity != Unbounded)
                throw new ArgumentOutOfRangeException("bufferedCapacity");

            return bufferedCapacity == Unbounded ?
                singleReaderWriter ? 
                    (IChannel<T>)new SpscUnboundedChannel<T>() : 
                    new UnboundedChannel<T>() :
                new BoundedChannel<T>(bufferedCapacity);
        }

        /// <summary>
        /// Creates an unbuffered channel.  As the resulting channel is unbuffered, readers and writers will not complete
        /// until a corresponding reader or writer is available.
        /// </summary>
        /// <typeparam name="T">Specifies the type of data stored in the channel.</typeparam>
        /// <returns>The new channel.</returns>
        public static IChannel<T> CreateUnbuffered<T>()
        {
            return new UnbufferedChannel<T>();
        }

        /// <summary>Creates a channel for reading <typeparamref name="T"/> instances from the source stream.</summary>
        /// <typeparam name="T">Specifies the type of data to be read.  This must be an unmanaged/primitive type.</typeparam>
        /// <param name="source">The source stream from which to read data.</param>
        /// <returns>A channel that reads elements from the source stream.</returns>
        public static IReadableChannel<T> ReadFromStream<T>(Stream source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (!source.CanRead)
                throw new ArgumentException(SR.ArgumentException_SourceStreamNotReadable, "source");

            return new DeserializationChannel<T>(source);
        }

        /// <summary>Creates a channel for writing <typeparamref name="T"/> instances to the destination stream.</summary>
        /// <typeparam name="T">Specifies the type of data to be written.  This must be an unmanaged/primitive type.</typeparam>
        /// <param name="destination">The destination stream to which to write data.</param>
        /// <returns>A channel that write elements to the destination stream.</returns>
        public static IWritableChannel<T> WriteToStream<T>(Stream destination)
        {
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (!destination.CanWrite)
                throw new ArgumentException(SR.ArgumentException_DestinationStreamNotWritable, "destination");

            return new SerializationChannel<T>(destination);
        }

        /// <summary>Creates a channel for the value in a task.</summary>
        /// <typeparam name="T">Specifies the type of the task's result.</typeparam>
        /// <param name="source">The task.</param>
        /// <returns>A channel that will contain the result from the task.</returns>
        public static IReadableChannel<T> CreateFromTask<T>(Task<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new TaskChannel<T>(source);
        }

        /// <summary>Create a channel that consumes data from the source enumerable.</summary>
        /// <typeparam name="T">Specifies the type of data in the enumerable.</typeparam>
        /// <param name="source">The source enumerable from which to read data.</param>
        /// <returns>A channel that reads data from the source enumerable.</returns>
        public static IReadableChannel<T> CreateFromEnumerable<T>(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new EnumerableChannel<T>(source);
        }

        /// <summary>Creates an observable for a channel.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="source">The channel to be treated as an observable.</param>
        /// <returns>An observable that pulls data from the source.</returns>
        public static IObservable<T> AsObservable<T>(this IReadableChannel<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return (IObservable<T>)s_channelToObservable.GetValue(
                source, 
                s => new ChannelObservable<T>((IReadableChannel<T>)s));
        }

        /// <summary>Table mapping from a channel to the shared observable wrapping it.</summary>
        private static ConditionalWeakTable<object, object> s_channelToObservable = new ConditionalWeakTable<object, object>();

        /// <summary>Creates an observer for a writeable channel.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="target">The channel to be treated as an observer.</param>
        /// <returns>An observer that forwards to the specified channel.</returns>
        public static IObserver<T> AsObserver<T>(this IWritableChannel<T> target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            return new ChannelObserver<T>(target);
        }

        /// <summary>Gets an awaiter that enables directly awaiting a channel to read data from it.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to await and from which to read.</param>
        /// <returns>An awaiter for reading data from the channel.</returns>
        /// <remarks>
        /// Getting the awaiter will initiate a read operation on the channel.
        /// </remarks>
        public static ValueTask<T>.ValueTaskAwaiter GetAwaiter<T>(this IReadableChannel<T> channel)
        {
            if (channel == null)
                throw new ArgumentNullException("channel");

            return new ValueTask<T>.ValueTaskAwaiter(channel.ReadAsync(), continueOnCapturedContext: true);
        }

        /// <summary>Gets an async enumerator of the data in the channel.</summary>
        /// <typeparam name="T">Specifies the type of data being enumerated.</typeparam>
        /// <param name="channel">The channel from which to read data.</param>
        /// <param name="cancellationToken">The cancellation token to use to cancel the asynchronous enumeration.</param>
        /// <returns>The async enumerator.</returns>
        public static IAsyncEnumerator<T> GetAsyncEnumerator<T>(
            this IReadableChannel<T> channel, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (channel == null)
                throw new ArgumentNullException("channel");

            return new AsyncEnumerator<T>(channel, cancellationToken);
        }

        /// <summary>Creates a case-select builder and adds a case for channel reading.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel from which to read.</param>
        /// <param name="action">The action to invoke with data read from the channel.</param>
        /// <returns>This builder.</returns>
        public static CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Action<T> action)
        {
            return new CaseBuilder().CaseRead(channel, action);
        }

        /// <summary>Creates a case-select builder and adds a case for channel reading.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel from which to read.</param>
        /// <param name="func">The asynchronous function to invoke with data read from the channel.</param>
        /// <returns>This builder.</returns>
        public static CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Func<T, Task> func)
        {
            return new CaseBuilder().CaseRead(channel, func);
        }

        /// <summary>Creates a case-select builder and adds a case for channel writing.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to which to write.</param>
        /// <param name="item">The data to write to the channel</param>
        /// <param name="action">The action to invoke after the data has been written.</param>
        /// <returns>This builder.</returns>
        public static CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Action action)
        {
            return new CaseBuilder().CaseWrite(channel, item, action);
        }

        /// <summary>Creates a case-select builder and adds a case for channel writing.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to which to write.</param>
        /// <param name="item">The data to write to the channel</param>
        /// <param name="func">The asynchronous function to invoke after the data has been written.</param>
        /// <returns>This builder.</returns>
        public static CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Func<Task> func)
        {
            return new CaseBuilder().CaseWrite(channel, item, func);
        }

        /// <summary>Completes the specified TaskCompletionSource.</summary>
        /// <param name="tcs">The source to complete.</param>
        /// <param name="error">
        /// The optional exception with which to complete.  
        /// If this is null, the source will be completed successfully.
        /// If this is an OperationCanceledException, it'll be completed with the exception's token.
        /// Otherwise, it'll be completed as faulted with the exception.
        /// </param>
        private static void CompleteWithOptionalError(TaskCompletionSource<VoidResult> tcs, Exception error)
        {
            OperationCanceledException oce = error as OperationCanceledException;
            if (oce != null)
                tcs.TrySetCanceled(oce.CancellationToken);
            else if (error != null && error != s_doneWritingSentinel)
                tcs.TrySetException(error);
            else
                tcs.TrySetResult(default(VoidResult));
        }

        /// <summary>
        /// Given an already faulted or canceled Task, returns a new generic task
        /// with the same failure or cancellation token.
        /// </summary>
        private static async Task<T> PropagateErrorAsync<T>(Task t)
        {
            Debug.Assert(t.IsFaulted || t.IsCanceled);
            await t;
            throw new InvalidOperationException(); // Awaiting should have thrown
        }

        /// <summary>Removes all waiters from the queue, completing each.</summary>
        /// <param name="waiters">The queue of waiters to complete.</param>
        /// <param name="result">The value with which to complete each waiter.</param>
        private static void WakeUpWaiters(SimpleQueue<Reader<bool>> waiters, bool result)
        {
            if (waiters.Count > 0)
                WakeUpWaitersCore(waiters, result); // separated out to streamline inlining
        }

        /// <summary>Core of WakeUpWaiters, separated out for performance due to inlining.</summary>
        private static void WakeUpWaitersCore(SimpleQueue<Reader<bool>> waiters, bool result)
        {
            while (waiters.Count > 0)
            {
                waiters.Dequeue().Success(result);
            }
        }

        /// <summary>Creates an exception detailing concurrent use of a single reader/writer channel.</summary>
        private static Exception CreateSingleReaderWriterMisuseException()
        {
            return new InvalidOperationException(SR.InvalidOperationException_SingleReaderWriterUsedConcurrently).InitializeStackTrace();
        }

        /// <summary>Creates and returns an exception object to indicate that a channel has been closed.</summary>
        private static Exception CreateInvalidCompletionException()
        {
            return new ClosedChannelException().InitializeStackTrace();
        }

        /// <summary>Exception thrown when a channel is used incorrectly after it's been closed.</summary>
        private sealed class ClosedChannelException : InvalidOperationException
        {
            public ClosedChannelException() : base(SR.ClosedChannelException_DefaultMessage) { }
        }

        /// <summary>Initializes the stack trace of an Exception by throwing and catching it.</summary>
        /// <param name="exc">The exception to initialize.</param>
        /// <returns>The same exception.</returns>
        private static Exception InitializeStackTrace(this Exception exc)
        {
            try { throw exc; }
            catch { return exc; }
        }

    }
}
