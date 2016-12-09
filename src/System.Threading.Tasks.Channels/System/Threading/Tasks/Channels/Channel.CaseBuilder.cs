// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        /// <summary>Provides a mutable builder for building up a case-select construct.</summary>
        public sealed class CaseBuilder
        {
            /// <summary>The read/write cases created in this builder.</summary>
            private readonly List<Case> _cases = new List<Case>();
            /// <summary>The default case, or null if there is none.</summary>
            private Case _default;

            /// <summary>Initialize the builder.</summary>
            internal CaseBuilder() { }

            /// <summary>
            /// Adds a channel read case to the builder.  If this case wins due to successfully
            /// being able to read data from the channel, the specified action will be invoked
            /// with the read data.
            /// </summary>
            /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
            /// <param name="channel">The channel from which to read.</param>
            /// <param name="action">The synchronous action to invoke with an element read from the channel.</param>
            /// <returns>This builder.</returns>
            public CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Action<T> action)
            {
                if (channel == null)
                    throw new ArgumentNullException(nameof(channel));
                if (action == null)
                    throw new ArgumentNullException(nameof(action));
                _cases.Add(new SyncReadCase<T>(channel, action));
                return this;
            }

            /// <summary>
            /// Adds a channel read case to the builder.  If this case wins due to successfully
            /// being able to read data from the channel, the specified function will be invoked
            /// with the read data.
            /// </summary>
            /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
            /// <param name="channel">The channel from which to read.</param>
            /// <param name="func">The asynchronous function to invoke with an element read from the channel.</param>
            /// <returns>This builder.</returns>
            public CaseBuilder CaseRead<T>(IReadableChannel<T> channel, Func<T, Task> func)
            {
                if (channel == null)
                    throw new ArgumentNullException(nameof(channel));
                if (func == null)
                    throw new ArgumentNullException(nameof(func));
                _cases.Add(new AsyncReadCase<T>(channel, func));
                return this;
            }

            /// <summary>
            /// Adds a channel write case to the builder.  If this case wins due to successfully
            /// being able to write data to the channel, the specified action will be invoked.
            /// </summary>
            /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
            /// <param name="channel">The channel to which to write.</param>
            /// <param name="item">The data to write to the channel.</param>
            /// <param name="action">The synchronous action to invoke once the write is successful.</param>
            /// <returns>This builder.</returns>
            public CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Action action)
            {
                if (channel == null)
                    throw new ArgumentNullException(nameof(channel));
                if (action == null)
                    throw new ArgumentNullException(nameof(action));
                _cases.Add(new SyncWriteCase<T>(channel, item, action));
                return this;
            }

            /// <summary>
            /// Adds a channel write case to the builder.  If this case wins due to successfully
            /// being able to write data to the channel, the specified action will be invoked.
            /// </summary>
            /// <typeparam name="T">Specifies the type of data in the channel.</typeparam>
            /// <param name="channel">The channel to which to write.</param>
            /// <param name="item">The data to write to the channel.</param>
            /// <param name="func">The asynchronous function to invoke once the write is successful.</param>
            /// <returns>This builder.</returns>
            public CaseBuilder CaseWrite<T>(IWritableChannel<T> channel, T item, Func<Task> func)
            {
                if (channel == null)
                    throw new ArgumentNullException(nameof(channel));
                if (func == null)
                    throw new ArgumentNullException(nameof(func));
                _cases.Add(new AsyncWriteCase<T>(channel, item, func));
                return this;
            }

            /// <summary>
            /// Adds a default case to this builder.  This case will win if no other
            /// cases are satisfied.
            /// </summary>
            /// <param name="action">The action to invoke if no other cases can be satisfied.</param>
            /// <returns>This builder.</returns>
            public CaseBuilder CaseDefault(Action action)
            {
                if (action == null)
                    throw new ArgumentNullException(nameof(action));
                if (_default != null)
                    throw new InvalidOperationException(Properties.Resources.InvalidOperationException_DefaultCaseAlreadyExists);
                _default = new SyncDefaultCase(action);
                return this;
            }

            /// <summary>
            /// Adds a default case to this builder.  This case will win if no other
            /// cases are satisfied.
            /// </summary>
            /// <param name="func">The asynchronous function to invoke if no other cases can be satisfied.</param>
            /// <returns>This builder.</returns>
            public CaseBuilder CaseDefault(Func<Task> func)
            {
                if (func == null)
                    throw new ArgumentNullException(nameof(func));
                if (_default != null)
                    throw new InvalidOperationException(Properties.Resources.InvalidOperationException_DefaultCaseAlreadyExists);
                _default = new AsyncDefaultCase(func);
                return this;
            }

            /// <summary>
            /// Invokes the select operation repeatedly until either the condition returns false,
            /// the channels associated with all of the cases are completed, cancellation is requested,
            /// or a failure occurs.
            /// </summary>
            /// <param name="conditionFunc">
            /// The predicate to invoke to determine whether to continue processing.  It is provided with 
            /// the number of successful selections completed thus far, and returns whether to continue processing.
            /// </param>
            /// <param name="cancellationToken">The cancellation token to use to request cancellation of the operation.</param>
            /// <returns>
            /// A task that represents the asynchronous select operation.
            /// It will complete with the number of cases successfully completed.
            /// </returns>
            public Task<int> SelectUntilAsync(Func<int, bool> conditionFunc, CancellationToken cancellationToken = default(CancellationToken))
            {
                if (conditionFunc == null)
                    throw new ArgumentNullException(nameof(conditionFunc));
                return SelectUntilAsyncCore(conditionFunc, cancellationToken);
            }

            /// <summary>Core of the SelectUntilAsync operation.</summary>
            private async Task<int> SelectUntilAsyncCore(Func<int, bool> condition, CancellationToken cancellationToken)
            {
                // TODO: This can be optimized further, by reusing WaitForReadAsync tasks across iterations
                // rather than getting a new one per iteration, by processing multiple completed-as-true WaitForReadTasks
                // as part of a single iteration, etc.  For now, however, this provides the core functionality.

                int completions = 0;
                while (condition(completions) && await SelectAsync(cancellationToken).ConfigureAwait(false))
                {
                    completions++;
                }
                return completions;
            }

            /// <summary>
            /// Invokes the select operation.  If any cases can be satisfied immediately,
            /// their corresponding actions will be invoked synchronously and the returned
            /// task will complete synchronously.  Otherwise, the first channel that
            /// asynchronously satisifes its case will have its action invoked asynchronously.
            /// </summary>
            /// <param name="cancellationToken">The cancellation token to use to request cancellation of the operation.</param>
            /// <returns>
            /// A task that represents the asynchronous select operation.
            /// It will complete with a result of true if one of the cases was satisfied,
            /// or else a result of false if none of the cases could be satisfied.
            /// </returns>
            public Task<bool> SelectAsync(CancellationToken cancellationToken = default(CancellationToken))
            {
                // First try each case to see if it can be satisfied synchronously
                foreach (Case c in _cases)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return Task.FromCanceled<bool>(cancellationToken);

                    Task<bool> t = c.TryInvokeAsync();
                    if (!t.IsCompleted || t.IsFaulted || (t.Status == TaskStatus.RanToCompletion && t.Result))
                    {
                        // If the task is faulted, then we invoked it's handling code and it failed
                        // so we should return the task to the caller.

                        // If the task succeeded and returned true, then we successfully processed the case
                        // and should return the task to the caller.

                        // And if the task isn't completed, since TryRead/Write are synchronous, 
                        // this must only not have completed synchronously if we successfully "got" 
                        // the case and invoked an asynchronous processing function, so we can
                        // just return the task to represent that.
                        return t;
                    }

                    // Otherwise, the task either returned false or was canceled, in which case
                    // we keep going.
                }

                // No cases could be satisfied.  If there's a default case, use that.
                if (_default != null)
                {
                    return _default.TryInvokeAsync();
                }

                // No default case, and no channels were available.  Run asynchronously.
                return SelectAsyncCore(cancellationToken);
            }

            /// <summary>Core of the SelectAsync operation.</summary>
            private async Task<bool> SelectAsyncCore(CancellationToken cancellationToken)
            {
                // Loop until cancellation occurs or a case is available
                Task<bool>[] tasks = new Task<bool>[_cases.Count];
                while (_cases.Count > 0)
                {
                    // Create a cancellation token that will be canceled either when the incoming token is canceled
                    // or once the await finishes.  When one task from one channel finishes, we don't want to leave
                    // tasks from other channels uncanceled.
                    Task<bool> completedTask;
                    using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, default(CancellationToken)))
                    {
                        for (int i = 0; i < tasks.Length; i++)
                        {
                            tasks[i] = _cases[i].WaitAsync(cts.Token);
                        }

                        completedTask = await Task.WhenAny(tasks).ConfigureAwait(false);
                        await Task.Yield();

                        cts.Cancel();
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    // Get the completed task's result, propagating any exceptions if any occurred
                    Debug.Assert(completedTask.IsCompleted);
                    bool result = completedTask.GetAwaiter().GetResult();

                    // Get the index of the case associated with the completed task
                    int completedIndex = Array.IndexOf(tasks, completedTask);
                    Debug.Assert(completedIndex >= 0);
                    Debug.Assert(completedIndex < _cases.Count);

                    // If the task completed successfully and with a true result,
                    // then we should try to invoke its case and be done if it succeeds.
                    if (result)
                    {
                        if (await _cases[completedIndex].TryInvokeAsync().ConfigureAwait(false))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        // Otherwise, this channel is done and we should remove it so as not
                        // to bother with it anymore.
                        _cases.RemoveAt(completedIndex);
                        tasks = new Task<bool>[_cases.Count];
                    }
                }

                // Ran out of cases
                return false;
            }

            /// <summary>Provides a base class for all cases.</summary>
            private abstract class Case
            {
                /// <summary>Tries to invoke the case.</summary>
                /// <returns>true if the case executed successfully; otherwise, false if it wasn't ready.</returns>
                public abstract Task<bool> TryInvokeAsync();

                /// <summary>Waits for the case to be available.</summary>
                /// <param name="cancellationToken">The cancellation token to use for the operation.</param>
                /// <returns>A task representing the asynchronous wait.</returns>
                public virtual Task<bool> WaitAsync(CancellationToken cancellationToken) { return s_falseTask; }
            }

            /// <summary>Provides the concrete case used for channel reads with synchronous processing.</summary>
            private sealed class SyncReadCase<T> : Case
            {
                private readonly IReadableChannel<T> _channel;
                private readonly Action<T> _action;

                internal SyncReadCase(IReadableChannel<T> channel, Action<T> action)
                {
                    _channel = channel;
                    _action = action;
                }

                public override Task<bool> WaitAsync(CancellationToken cancellationToken)
                {
                    return _channel.WaitToReadAsync(cancellationToken);
                }

                public override Task<bool> TryInvokeAsync()
                {
                    T item;
                    if (_channel.TryRead(out item))
                    {
                        try
                        {
                            _action(item);
                            return s_trueTask;
                        }
                        catch (Exception exc)
                        {
                            return Task.FromException<bool>(exc);
                        }
                    }
                    return s_falseTask;
                }
            }

            /// <summary>Provides the concrete case used for channel reads with asynchronous processing.</summary>
            private sealed class AsyncReadCase<T> : Case
            {
                private readonly IReadableChannel<T> _channel;
                private readonly Func<T, Task> _action;

                internal AsyncReadCase(IReadableChannel<T> channel, Func<T, Task> action)
                {
                    _channel = channel;
                    _action = action;
                }

                public override Task<bool> WaitAsync(CancellationToken cancellationToken)
                {
                    return _channel.WaitToReadAsync(cancellationToken);
                }

                public override Task<bool> TryInvokeAsync()
                {
                    T item;
                    if (_channel.TryRead(out item))
                    {
                        try
                        {
                            return _action(item).ContinueWith(t =>
                            {
                                t.GetAwaiter().GetResult();
                                return true;
                            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                        }
                        catch (Exception exc)
                        {
                            return Task.FromException<bool>(exc);
                        }
                    }
                    return s_falseTask;
                }
            }

            /// <summary>Provides the concrete case used for channel writes with synchronous processing.</summary>
            private sealed class SyncWriteCase<T> : Case
            {
                private readonly IWritableChannel<T> _channel;
                private readonly T _item;
                private readonly Action _action;

                internal SyncWriteCase(IWritableChannel<T> channel, T item, Action action)
                {
                    _channel = channel;
                    _item = item;
                    _action = action;
                }

                public override Task<bool> WaitAsync(CancellationToken cancellationToken)
                {
                    return _channel.WaitToWriteAsync(cancellationToken);
                }

                public override Task<bool> TryInvokeAsync()
                {
                    if (_channel.TryWrite(_item))
                    {
                        try
                        {
                            _action();
                            return s_trueTask;
                        }
                        catch (Exception exc)
                        {
                            return Task.FromException<bool>(exc);
                        }
                    }
                    return s_falseTask;
                }
            }

            /// <summary>Provides the concrete case used for channel writes with asynchronous processing.</summary>
            private sealed class AsyncWriteCase<T> : Case
            {
                private readonly IWritableChannel<T> _channel;
                private readonly T _item;
                private readonly Func<Task> _action;

                internal AsyncWriteCase(IWritableChannel<T> channel, T item, Func<Task> action)
                {
                    _channel = channel;
                    _item = item;
                    _action = action;
                }

                public override Task<bool> WaitAsync(CancellationToken cancellationToken)
                {
                    return _channel.WaitToWriteAsync(cancellationToken);
                }

                public override Task<bool> TryInvokeAsync()
                {
                    if (_channel.TryWrite(_item))
                    {
                        try
                        {
                            return _action().ContinueWith(t =>
                            {
                                t.GetAwaiter().GetResult();
                                return true;
                            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                        }
                        catch (Exception exc)
                        {
                            return Task.FromException<bool>(exc);
                        }
                    }
                    return s_falseTask;
                }
            }

            /// <summary>Provides the concrete case used for synchronous default cases.</summary>
            private sealed class SyncDefaultCase : Case
            {
                private readonly Action _action;

                internal SyncDefaultCase(Action action)
                {
                    _action = action;
                }

                public override Task<bool> TryInvokeAsync()
                {
                    try
                    {
                        _action();
                        return s_trueTask;
                    }
                    catch (Exception exc)
                    {
                        return Task.FromException<bool>(exc);
                    }
                }
            }

            /// <summary>Provides the concrete case used for asynchronous default cases.</summary>
            private sealed class AsyncDefaultCase : Case
            {
                private readonly Func<Task> _action;

                internal AsyncDefaultCase(Func<Task> action)
                {
                    _action = action;
                }

                public override Task<bool> TryInvokeAsync()
                {
                    try
                    {
                        return _action().ContinueWith(t =>
                        {
                            t.GetAwaiter().GetResult();
                            return true;
                        }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                    }
                    catch (Exception exc)
                    {
                        return Task.FromException<bool>(exc);
                    }
                }
            }

        }
    }
}
