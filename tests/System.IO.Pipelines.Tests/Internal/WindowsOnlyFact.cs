using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests.Internal
{
    public class WindowsOnlyFact : FactAttribute
    {
        public WindowsOnlyFact()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Skip = $"Ignored on {RuntimeInformation.OSDescription}";
            }
        }
    }
}
