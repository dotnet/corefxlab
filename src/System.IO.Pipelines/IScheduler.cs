using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public interface IScheduler
    {
        void Schedule(Action action);
    }
}
