// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks.Channels
{
    /// <summary>Provides extension methods for manipulating channel instances.</summary>
    public static class ChannelExtensions
    {
        /// <summary>Table mapping from a channel to the shared observable wrapping it.</summary>
        private static readonly ConditionalWeakTable<object, object> s_channelToObservable = new ConditionalWeakTable<object, object>();

        /// <summary>Creates an observable for a channel.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="source">The channel to be treated as an observable.</param>
        /// <returns>An observable that pulls data from the source.</returns>
        public static IObservable<T> AsObservable<T>(this IReadableChannel<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return (IObservable<T>)s_channelToObservable.GetValue(
                source, 
                s => new Channel.ChannelObservable<T>((IReadableChannel<T>)s));
        }

        /// <summary>Creates an observer for a writeable channel.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="target">The channel to be treated as an observer.</param>
        /// <returns>An observer that forwards to the specified channel.</returns>
        public static IObserver<T> AsObserver<T>(this IWritableChannel<T> target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            return new Channel.ChannelObserver<T>(target);
        }

        /// <summary>Gets an awaiter that enables directly awaiting a channel to read data from it.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to await and from which to read.</param>
        /// <returns>An awaiter for reading data from the channel.</returns>
        /// <remarks>
        /// Getting the awaiter will initiate a read operation on the channel.
        /// </remarks>
        public static ValueTaskAwaiter<T> GetAwaiter<T>(this IReadableChannel<T> channel)
        {
            // No explicit null check.  await'ing something that's null can produce a NullReferenceException.
            return channel.ReadAsync().GetAwaiter();
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
                throw new ArgumentNullException(nameof(channel));

            return new Channel.AsyncEnumerator<T>(channel, cancellationToken);
        }

        /// <summary>Creates a case-select builder and adds a case for channel reading.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel from which to read.</param>
        /// <param name="action">The action to invoke with data read from the channel.</param>
        /// <returns>This builder.</returns>
        public static Channel.CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Action<T> action) => new Channel.CaseBuilder().CaseRead(channel, action);

        /// <summary>Creates a case-select builder and adds a case for channel reading.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel from which to read.</param>
        /// <param name="func">The asynchronous function to invoke with data read from the channel.</param>
        /// <returns>This builder.</returns>
        public static Channel.CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Func<T, Task> func) => new Channel.CaseBuilder().CaseRead(channel, func);

        /// <summary>Creates a case-select builder and adds a case for channel writing.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to which to write.</param>
        /// <param name="item">The data to write to the channel</param>
        /// <param name="action">The action to invoke after the data has been written.</param>
        /// <returns>This builder.</returns>
        public static Channel.CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Action action) => new Channel.CaseBuilder().CaseWrite(channel, item, action);

        /// <summary>Creates a case-select builder and adds a case for channel writing.</summary>
        /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
        /// <param name="channel">The channel to which to write.</param>
        /// <param name="item">The data to write to the channel</param>
        /// <param name="func">The asynchronous function to invoke after the data has been written.</param>
        /// <returns>This builder.</returns>
        public static Channel.CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Func<Task> func) => new Channel.CaseBuilder().CaseWrite(channel, item, func);

        /// <summary>Mark the channel as being complete, meaning no more items will be written to it.</summary>
        /// <param name="channel">The channel to mark as complete.</param>
        /// <param name="error">Optional Exception indicating a failure that's causing the channel to complete.</param>
        /// <exception cref="InvalidOperationException">The channel has already been marked as complete.</exception>
        public static void Complete<T>(this IWritableChannel<T> channel, Exception error = null)
        {
            if (channel == null)
                throw new ArgumentNullException(nameof(channel));
            if (!channel.TryComplete(error))
                throw ChannelUtilities.CreateInvalidCompletionException();
        }
    }
}
