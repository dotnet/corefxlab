// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public class TaskRunScheduler : IScheduler
    {
        public static TaskRunScheduler Default = new TaskRunScheduler();

        public void Schedule(Action<object> action, object state)
        {
            Task.Factory.StartNew(action, state);
        }
    }
}
