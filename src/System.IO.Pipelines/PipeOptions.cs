using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public class PipeOptions
    {
        public long MaximumSizeHigh { get; set; }

        public long MaximumSizeLow { get; set; }

        public IScheduler WriterScheduler { get; set; }

        public IScheduler ReaderScheduler { get; set; }
    }
}
