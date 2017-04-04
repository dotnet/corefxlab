using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public interface  ISample
    {
        Task Run();
    }
}
