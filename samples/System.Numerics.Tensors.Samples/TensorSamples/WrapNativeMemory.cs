using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace TensorSamples
{
    partial class WrapNativeMemory
    {
        public static void RunSample()
        {
            Console.WriteLine($"*** {nameof(WrapNativeMemory)} ***");
            var multTable = GetMultiplicationTable(5);

            Console.WriteLine("Multiplication table:");
            Console.WriteLine(multTable.GetArrayString());

            Console.WriteLine("Sums:");
            for (int row = 0; row < multTable.Dimensions[0]; row++)
            {
                Console.WriteLine(GetRowSum(multTable, row));
            }

            Console.WriteLine("Forcing GC.");
            GC.Collect();
            Console.WriteLine("Remove GC root.");
            multTable = null;
            Console.WriteLine("Forcing GC.");
            GC.Collect();
        }
        
        // The following represent what .NET bindings would look like for an native library that wishes
        // to deal in tensors and allocate them from native code on the managed heap.
        
        public static DenseTensor<double> GetMultiplicationTable(int maxNumber)
        {
            Span<int> dimensions = stackalloc int[2];
            dimensions[0] = dimensions[1] = maxNumber;
            var nativeMemory = new NativeMemory<double>(GetMultTableAllocateNative(maxNumber), maxNumber * maxNumber);
            return new DenseTensor<double>(nativeMemory.Memory, dimensions);
            
        }

        public static unsafe double GetRowSum(DenseTensor<double> tensor, int row)
        {
            fixed(double* dataPtr = &tensor.Buffer.Span.DangerousGetPinnableReference())
            {
                return GetRowSum(dataPtr, tensor.Dimensions.ToArray(), tensor.Rank, row);
            }
        }


        delegate IntPtr AllocatorDelegate(int size);
        [DllImport("TensorSamples.native.dll")]
        extern private static IntPtr GetMultTableAllocateNative(int number);

        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe double GetRowSum(double* data, int[] dimensions, int rank, int row);
    }
}
