using System;
using Xunit;
namespace System.Drawing.Graphics
{
    class ManagedGDImageScratch
    {
        [Fact]
        public void Test()
        {
            IntPtr imgPtr = DLLImports.gdImageCreate(1, 1);
            System.Console.WriteLine(imgPtr.ToInt64());
            System.Console.WriteLine("hi");
        }
    }
}
