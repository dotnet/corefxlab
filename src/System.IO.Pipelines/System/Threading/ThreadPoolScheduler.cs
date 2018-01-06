// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace System.Threading
{
    internal sealed class ThreadPoolScheduler : Scheduler
    {
        public override void Schedule(Action action)
        {
#if NETCOREAPP2_1
            // Queue to low contention local ThreadPool queue; rather than global queue as per Task
            Threading.ThreadPool.QueueUserWorkItem(_actionAsTask, action, preferLocal: true);
#elif NETSTANDARD2_0
            Threading.ThreadPool.QueueUserWorkItem(_actionAsTask, action);
#else
            Task.Factory.StartNew(action);
#endif
        }

        public override void Schedule(Action<object> action, object state)
        {
#if NETCOREAPP2_1
            // Queue to low contention local ThreadPool queue; rather than global queue as per Task
            Threading.ThreadPool.QueueUserWorkItem(_actionObjectAsTask, new ActionObjectAsTask(action, state), preferLocal: true);
#elif NETSTANDARD2_0
            Threading.ThreadPool.QueueUserWorkItem(_actionObjectAsTask, new ActionObjectAsTask(action, state));
#else
            Task.Factory.StartNew(action, state);
#endif
        }

#if NETCOREAPP2_1 || NETSTANDARD2_0
        // Catches only the exception into a failed Task, so the fire-and-forget action 
        // can be queued directly to the ThreadPool without the extra overhead of as Task
        private readonly static WaitCallback _actionAsTask = state =>
        {
            try
            {
                ((Action)state)();
            }
            catch (Exception ex)
            {
                // Create faulted Task for the TaskScheulder to handle exception
                // rather than letting it escape onto the ThreadPool and crashing the process
                Task.FromException(ex);
            }
        };

        private readonly static WaitCallback _actionObjectAsTask = state => ((ActionObjectAsTask)state).Run();

        private sealed class ActionObjectAsTask
        {
            private Action<object> _action;
            private object _state;

            public ActionObjectAsTask(Action<object> action, object state)
            {
                _action = action;
                _state = state;
            }

            // Catches only the exception into a failed Task, so the fire-and-forget action 
            // can be queued directly to the ThreadPool without the extra overhead of as Task
            public void Run()
            {
                try
                {
                    _action(_state);
                }
                catch (Exception ex)
                {
                    // Create faulted Task for the TaskScheulder to handle exception
                    // rather than letting it escape onto the ThreadPool and crashing the process
                    Task.FromException(ex);
                }
            }
        }
#endif
    }
}
