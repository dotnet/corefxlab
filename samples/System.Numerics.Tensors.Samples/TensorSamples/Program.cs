using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace TensorSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            PreAllocate.RunSample();

            ReversePInvoke.RunSample();

            MutateSparse.RunSample();

            WrapNativeMemory.RunSample();

            Access.RunSample();
        }
    }
}
