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
            _result = default;
            _asyncOp = awaiter;
        }

        /// <summary>Initializes the awaiter with the specified async operation.</summary>
        /// <param name="task">The task that represents the actual async operation being awaited.</param>
        public ValueAwaiter(Task<TResult> task)
        {
            if (task.Status == TaskStatus.RanToCompletion)
            {
                _result = task.Result;
                _asyncOp = null;
            }
            else
            {
                _result = default;
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
                _result = default;
                _asyncOp = task.AsTask();
            }
        }

        /// <summary>Gets whether the awaited operation has completed.</summary>
        public bool IsCompleted
        {
            get
            {
                if (_asyncOp == null)
                {
                    return true;
                }

                IAwaiter<TResult> awaiter = _asyncOp as IAwaiter<TResult>;
                if (awaiter != null)
                {
                    return awaiter.IsCompleted;
                }

                Task<TResult> task = (Task<TResult>)_asyncOp;
                return task.IsCompleted;
            }
        }

        /// <summary>Gets the result of the completed awaited operation.</summary>
        public TResult GetResult()
        {
            if (_asyncOp == null)
            {
                return _result;
            }

            IAwaiter<TResult> awaiter = _asyncOp as IAwaiter<TResult>;
            if (awaiter != null)
            {
                return awaiter.GetResult();
            }

            Task<TResult> task = (Task<TResult>)_asyncOp;
            return task.GetAwaiter().GetResult();
        }

        /// <summary>Schedules the continuation action that's invoked when the instance completes.</summary>
        /// <param name="continuation">The action to invoke when the operation completes.</param>
        public void OnCompleted(Action continuation)
        {
            if (_asyncOp == null)
            {
                Task.Run(continuation);
                return;
            }

            IAwaiter<TResult> awaiter = _asyncOp as IAwaiter<TResult>;
            if (awaiter != null)
            {
                awaiter.OnCompleted(continuation);
                return;
            }

            Task<TResult> task = (Task<TResult>)_asyncOp;
            task.GetAwaiter().OnCompleted(continuation);
        }

        /// <summary>Schedules the continuation action that's invoked when the instance completes.</summary>
        /// <param name="continuation">The action to invoke when the operation completes.</param>
        public void UnsafeOnCompleted(Action continuation)
        {
            if (_asyncOp == null)
            {
                Task.Run(continuation);
                return;
            }

            IAwaiter<TResult> awaiter = _asyncOp as IAwaiter<TResult>;
            if (awaiter != null)
            {
                awaiter.UnsafeOnCompleted(continuation);
                return;
            }

            Task<TResult> task = (Task<TResult>)_asyncOp;
            task.GetAwaiter().UnsafeOnCompleted(continuation);
        }
    }
}
