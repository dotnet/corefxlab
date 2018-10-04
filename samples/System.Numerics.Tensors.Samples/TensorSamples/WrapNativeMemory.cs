using System;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using System.Text;

namespace TensorSamples
{
    partial class WrapNativeMemory
    {
        public static void RunSample()
        {
            Console.WriteLine($"*** {nameof(WrapNativeMemory)} ***");
            
            Console.WriteLine($"Allocated as part of the operation");
            // in this case we pretend that we cannot know the size of the tensor up front and require the native library to allocate it and tell us the size.
            var multTable = GetMultiplicationTable(5);

            Console.WriteLine("Multiplication table:");
            Console.WriteLine(multTable.GetArrayString());

            Console.WriteLine("Sums:");
            for (int row = 0; row < multTable.Dimensions[0]; row++)
            {
                Console.WriteLine(GetRowSum(multTable, row));
            }
            
            // the memory will be freed when the GC decides to collect the Tensor, we can *try* to force it but no garuntee.
            multTable = null;
            Console.WriteLine("Forcing GC.");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Allocated prior to the operation");
            // in this case we'll assume we can know the size of the buffer up front and manage the lifetime explicitly
            using (var nativeMemory = NativeMemory<double>.Allocate(5 * 5))
            {
                Span<int> dimensions = stackalloc int[2];
                dimensions[0] = dimensions[1] = 5;
                var tensor = new DenseTensor<double>(nativeMemory.Memory, dimensions);

                GetMultiplicationTablePreallocated(5, tensor);

                Console.WriteLine("Sums:");
                for (int row = 0; row < tensor.Dimensions[0]; row++)
                {
                    Console.WriteLine(GetRowSum(tensor, row));
                }
            }
        }

        // The following represent what .NET bindings would look like for a native library that wishes
        // to deal in tensors and allocate them from native code using native memory (for instance 
        // in memory accessible to the graphics card, or in its own buffer pool).

        public static DenseTensor<double> GetMultiplicationTable(int maxNumber)
        {
            Span<int> dimensions = stackalloc int[2];
            dimensions[0] = dimensions[1] = maxNumber;
            var nativeMemory = new NativeMemory<double>(GetMultTableAllocateNative(maxNumber), maxNumber * maxNumber);
            return new DenseTensor<double>(nativeMemory.Memory, dimensions);
            
        }

        public static unsafe double GetRowSum(DenseTensor<double> tensor, int row)
        {
            fixed(double* dataPtr = &MemoryMarshal.GetReference(tensor.Buffer.Span))
            {
                return GetRowSum(dataPtr, tensor.Dimensions.ToArray(), tensor.Rank, row);
            }
        }

        public static unsafe void GetMultiplicationTablePreallocated(int maxNumber, DenseTensor<double> tensor)
        {
            fixed (double* dataPtr = &MemoryMarshal.GetReference(tensor.Buffer.Span))
            {
                GetMultTablePreAllocated(maxNumber, dataPtr, tensor.Buffer.Length);
            }
        }
        
        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe void GetMultTablePreAllocated(int number, double* result, int resultSize);
        
        [DllImport("TensorSamples.native.dll")]
        extern private static IntPtr GetMultTableAllocateNative(int number);

        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe double GetRowSum(double* data, int[] dimensions, int rank, int row);
    }
}
