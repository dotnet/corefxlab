// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Threading
{
    public abstract class Scheduler
    {
        private protected static readonly Action<object> ScheduleAction = o => ((Action)o)();

        public static Scheduler TaskRun { get; } = new TaskRunScheduler();
        public static Scheduler Inline { get; } = new InlineScheduler();

        public abstract void Schedule(Action<object> action, object state);

        public virtual void Schedule(Action action) => Schedule(ScheduleAction, action);
    }
}
