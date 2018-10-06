using System;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using System.Text;

namespace TensorSamples
{
    partial class PreAllocate
    {
        public static void RunSample()
        {
            Console.WriteLine($"*** {nameof(PreAllocate)} ***");
            var multTable = GetMultiplicationTable(5);

            Console.WriteLine("Multiplication table:");
            Console.WriteLine(multTable.GetArrayString());

            Console.WriteLine("Sums:");
            for (int row = 0; row < multTable.Dimensions[0]; row++)
            {
                Console.WriteLine(GetRowSum(multTable, row));
            }
        }
        
        // The following represent what .NET bindings would look like for a native library that wishes
        // to deal in tensors where the size of the tensor is known before calling the API and can be allocated
        // by the caller.
        
        public static unsafe DenseTensor<double> GetMultiplicationTable(int maxNumber)
        {
            Span<int> dimensions = stackalloc int[2];
            dimensions[0] = dimensions[1] = maxNumber;

            var result = new DenseTensor<double>(dimensions);
            
            fixed (double* dataPtr = &MemoryMarshal.GetReference(result.Buffer.Span))
            {
                GetMultTablePreAllocated(maxNumber, dataPtr, result.Buffer.Length);
            }

            return result;
        }

        public static unsafe double GetRowSum(DenseTensor<double> tensor, int row)
        {
            fixed(double* dataPtr = &MemoryMarshal.GetReference(tensor.Buffer.Span))
            {
                return GetRowSum(dataPtr, tensor.Dimensions.ToArray(), tensor.Rank, row);
            }
        }
        
        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe void GetMultTablePreAllocated(int number, double* result, int resultSize);

        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe double GetRowSum(double* data, int[] dimensions, int rank, int row);
    }
}
