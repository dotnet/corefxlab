// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        /// <summary>Provides a channel around a task.</summary>
        private sealed class TaskChannel<T> : IReadableChannel<T>
        {
            private readonly TaskCompletionSource<VoidResult> _completion = new TaskCompletionSource<VoidResult>(TaskCreationOptions.RunContinuationsAsynchronously);
            private readonly Task<T> _task;

            internal TaskChannel(Task<T> task)
            {
                _task = task;
                switch (task.Status)
                {
                    case TaskStatus.Faulted:
                        _completion.SetException(task.Exception.InnerException);
                        break;
                    case TaskStatus.Canceled:
                        _completion.SetCanceled();
                        break;
                    case TaskStatus.RanToCompletion:
                        // nop
                        break;
                    default:
                        task.ContinueWith((t, s) =>
                        {
                            var tcs = (TaskCompletionSource<VoidResult>)s;
                            bool completed = tcs.TrySetException(t.IsFaulted ?
                                t.Exception.InnerException :
                                CreateInvalidCompletionException());
                            Debug.Assert(completed);
                        }, _completion, 
                            CancellationToken.None, 
                            TaskContinuationOptions.NotOnRanToCompletion | TaskContinuationOptions.ExecuteSynchronously, 
                            TaskScheduler.Default);
                        break;
                }
            }

            public Task Completion => _completion.Task;

            public ValueTask<T> ReadAsync(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));
                }

                switch (_task.Status)
                {
                    case TaskStatus.Faulted:
                        return new ValueTask<T>(_task);

                    case TaskStatus.Canceled:
                        return new ValueTask<T>(Task.FromException<T>(CreateInvalidCompletionException()));

                    case TaskStatus.RanToCompletion:
                        return new ValueTask<T>(TransitionRead() ?
                            _task :
                            Task.FromException<T>(CreateInvalidCompletionException()));

                    default:
                        return new ValueTask<T>(_task.ContinueWith((_, s) =>
                        {
                            TaskChannel<T> thisRef = (TaskChannel<T>)s;
                            return thisRef.ReadAsync(CancellationToken.None).GetAwaiter().GetResult();
                        }, this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default));
                }
            }

            public bool TryRead(out T item)
            {
                if (_task.Status == TaskStatus.RanToCompletion && TransitionRead())
                {
                    item = _task.Result;
                    return true;
                }

                item = default(T);
                return false;
            }

            public Task<bool> WaitToReadAsync(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromCanceled<bool>(cancellationToken);
                }

                switch (_task.Status)
                {
                    case TaskStatus.Faulted:
                    case TaskStatus.Canceled:
                        return s_falseTask;

                    case TaskStatus.RanToCompletion:
                        return _completion.Task.IsCompleted ? s_falseTask : s_trueTask;

                    default:
                        return _task.ContinueWith((_, s) =>
                        {
                            TaskChannel<T> thisRef = (TaskChannel<T>)s;
                            return
                                thisRef._task.Status == TaskStatus.RanToCompletion &&
                                !thisRef._completion.Task.IsCompleted;
                        }, this, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                }
            }

            private bool TransitionRead() => _completion.TrySetResult(default(VoidResult));
        }
    }
}
