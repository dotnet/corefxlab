// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;

namespace System.Threading
{
    internal sealed class TaskRunScheduler : Scheduler
    {
        public override void Schedule(Action action)
        {
#if NETCOREAPP2_1
            ThreadPool.QueueUserWorkItem(_actionAsTask, action, preferLocal: true);
#elif NETSTANDARD2_0
            ThreadPool.QueueUserWorkItem(_actionAsTask, action);
#else
            Task.Factory.StartNew(ScheduleAction, action);
#endif
        }

        public override void Schedule(Action<object> action, object state)
        {
#if NETCOREAPP2_1
            ThreadPool.QueueUserWorkItem(_actionObjectAsTask, new ActionObjectAsTask(action, state), preferLocal: true);
#elif NETSTANDARD2_0
            ThreadPool.QueueUserWorkItem(_actionObjectAsTask, new ActionObjectAsTask(action, state));
#else
            Task.Factory.StartNew(action, state);
#endif
        }

#if NETCOREAPP2_1 || NETSTANDARD2_0
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
