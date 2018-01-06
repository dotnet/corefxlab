// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Threading
{
    public abstract class Scheduler
    {
        private static TaskRunScheduler _taskRunScheduler = new TaskRunScheduler();
        private static InlineScheduler _inlineScheduler = new InlineScheduler();

        public static Scheduler TaskRun => _taskRunScheduler;
        public static Scheduler Inline => _inlineScheduler;
        
        public abstract void Schedule(Action action);
        public abstract void Schedule(Action<object> action, object state);
    }
}
