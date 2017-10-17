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
            ReversePInvoke.RunSample();

            MutateSparse.RunSample();

            WrapNativeMemory.RunSample();
        }
    }
}
