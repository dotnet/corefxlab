using System;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using System.Text;

namespace TensorSamples
{
    class MutateSparse
    {
        public static void RunSample()
        {
            Console.WriteLine($"*** {nameof(MutateSparse)} ***");
            CompressedSparseTensor<double> tensor = new CompressedSparseTensor<double>(new[] { 3, 5, 4 }, 10);

            tensor[0, 2, 0] = 1.6;
            tensor[1, 0, 3] = 5.3;
            tensor[1, 1, 0] = Math.PI;
            tensor[2, 3, 3] = 867.5309;
            tensor[2, 4, 0] = 19.4231;
            tensor[2, 4, 3] = 10000.1;

            Console.WriteLine("Original tensor:");
            Console.WriteLine(tensor.GetArrayString());

            ScalarPowerSparse(tensor, 3);

            Console.WriteLine("Cubed tensor:");
            Console.WriteLine(tensor.GetArrayString());
        }
        
        // The following represent what .NET bindings would look like for a native library that wishes
        // to deal take a sparse tensor and mutate its values     

        public static unsafe double ScalarPowerSparse(CompressedSparseTensor<double> tensor, int exponent)
        {
            fixed (double* valuesPtr = &MemoryMarshal.GetReference(tensor.Values.Span))
            fixed (int* compressedCountsPtr = &MemoryMarshal.GetReference(tensor.CompressedCounts.Span))
            fixed (int* indicesPtr = &MemoryMarshal.GetReference(tensor.Indices.Span))
            {
                return ScalarPowerSparse(valuesPtr, compressedCountsPtr, indicesPtr, tensor.Dimensions.ToArray(), tensor.Rank, tensor.NonZeroCount, exponent);
            }
        }

        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe double ScalarPowerSparse(double* values, int* compressedCounts, int* indices, int[] dimensions, int rank, int valueCount, int power);
    }
}
