using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public class TaskRunScheduler : IScheduler
    {
        public static TaskRunScheduler Default = new TaskRunScheduler();

        public void Schedule(Action action)
        {
            Task.Run(action);
        }
    }
}
