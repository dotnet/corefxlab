// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Provides an awaiter that can be created around an existing value or around
    /// another async operation, represented by either a <see cref="Task{TResult}"/>
    /// or an <see cref="IAwaiter{T}"/>. 
    /// </summary>
    /// <typeparam name="TResult">Specifies the type of the result.</typeparam>
    [StructLayout(LayoutKind.Auto)]
    public struct ValueAwaiter<TResult> : IAwaiter<TResult>
    {
        /// <summary>
        /// The wrapped async operation.  Null if <see cref="_result"/> represents the result;
        /// otherwise, an <see cref="IAwaiter{T}"/> or a <see cref="Task{TResult}"/>.
        /// </summary>
        private readonly object _asyncOp;
        /// <summary>The result if <see cref="_asyncOp"/> is null.</summary>
        private readonly TResult _result;

        /// <summary>Initializes the awaiter with the specified result.</summary>
        /// <param name="result">The result to be returned from <see cref="GetResult"/>.</param>
        public ValueAwaiter(TResult result)
        {
            _result = result;
            _asyncOp = null;
        }

        /// <summary>Initializes the awaiter with the specified async operation.</summary>
        /// <param name="awaiter">The awaiter that represents the actual async operation being awaited.</param>
        public ValueAwaiter(IAwaiter<TResult> awaiter)
        {
            _result = default(TResult);
            _asyncOp = awaiter;
        }

        /// <summary>Initializes the awaiter with the specified async operation.</summary>
        /// <param name="task">The task that represents the actual async operation being awaited.</param>
        public ValueAwaiter(Task<TResult> task)
        {
            if (task.Status == TaskStatus.RanToCompletion) // TODO: task.IsCompletedSuccessfully
            {
                _result = task.Result;
                _asyncOp = null;
            }
            else
            {
                _result = default(TResult);
                _asyncOp = task;
            }
        }

        /// <summary>Initializes the awaiter with the specified async operation.</summary>
        /// <param name="task">The task that represents the actual async operation being awaited.</param>
        public ValueAwaiter(ValueTask<TResult> task)
        {
            if (task.IsCompletedSuccessfully)
            {
                _result = task.Result;
                _asyncOp = null;
            }
            else
            {
                _result = default(TResult);
                _asyncOp = task.AsTask();
            }
        }

        /// <summary>Gets whether the awaited operation has completed.</summary>
        public bool IsCompleted
        {
            get
            {
                return _asyncOp == null ? true : IsCompletedCore(_asyncOp);

                bool IsCompletedCore(object asyncOp) =>
                    asyncOp is IAwaiter<TResult> awaiter ?
                        awaiter.IsCompleted :
                        ((Task<TResult>)asyncOp).IsCompleted;
            }
        }

        /// <summary>Gets the result of the completed awaited operation.</summary>
        public TResult GetResult()
        {
            return _asyncOp == null ? _result : GetResultCore(_asyncOp);

            TResult GetResultCore(object asyncOp) =>
                asyncOp is IAwaiter<TResult> awaiter ?
                    awaiter.GetResult() :
                    ((Task<TResult>)asyncOp).GetAwaiter().GetResult();
        }

        /// <summary>Schedules the continuation action that's invoked when the instance completes.</summary>
        /// <param name="continuation">The action to invoke when the operation completes.</param>
        public void OnCompleted(Action continuation)
        {
            switch (_asyncOp)
            {
                case null:
                    Task.CompletedTask.ConfigureAwait(false).GetAwaiter().OnCompleted(continuation);
                    break;

                case IAwaiter<TResult> awaiter:
                    awaiter.OnCompleted(continuation);
                    break;

                default:
                    ((Task<TResult>)_asyncOp).GetAwaiter().OnCompleted(continuation);
                    break;
            }
        }

        /// <summary>Schedules the continuation action that's invoked when the instance completes.</summary>
        /// <param name="continuation">The action to invoke when the operation completes.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            switch (_asyncOp)
            {
                case null:
                    Task.CompletedTask.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(continuation);
                    break;

                case IAwaiter<TResult> awaiter:
                    awaiter.UnsafeOnCompleted(continuation);
                    break;

                default:
                    ((Task<TResult>)_asyncOp).GetAwaiter().UnsafeOnCompleted(continuation);
                    break;
            }
        }
    }
}
