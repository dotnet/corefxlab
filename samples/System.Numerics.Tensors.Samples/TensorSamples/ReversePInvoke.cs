using System;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using System.Text;

namespace TensorSamples
{
    partial class ReversePInvoke
    {
        public static void RunSample()
        {
            Console.WriteLine($"*** {nameof(ReversePInvoke)} ***");
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
        // to deal in tensors and allocate them on the managed heap through callbacks.
        
        public static DenseTensor<double> GetMultiplicationTable(int maxNumber)
        {
            using (var allocator = new Allocator<double>())
            {
                var bufferPtr = GetMultTableAllocateManaged(allocator.AllocateArray, maxNumber);

                var buffer = allocator.Unpin(bufferPtr);

                var result = new DenseTensor<double>(buffer, new[] { maxNumber, maxNumber });

                return result;
            }
        }

        public static unsafe double GetRowSum(DenseTensor<double> tensor, int row)
        {
            fixed(double* dataPtr = &MemoryMarshal.GetReference(tensor.Buffer.Span))
            {
                return GetRowSum(dataPtr, tensor.Dimensions.ToArray(), tensor.Rank, row);
            }
        }


        delegate IntPtr AllocatorDelegate(int size);
        [DllImport("TensorSamples.native.dll")]
        extern private static IntPtr GetMultTableAllocateManaged(AllocatorDelegate allocator, int number);

        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe double GetRowSum(double* data, int[] dimensions, int rank, int row);
    }
}
