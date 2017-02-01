using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public class PipeOptions
    {
        public int MaximumSizeHigh { get; set; }

        public int MaximumSizeLow { get; set; }

        public IScheduler Scheduler { get; set; }
    }
}
