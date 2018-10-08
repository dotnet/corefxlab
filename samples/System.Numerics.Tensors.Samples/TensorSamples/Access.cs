using System;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using System.Text;

namespace TensorSamples
{
    partial class Access
    {
        public static void RunSample()
        {
            Console.WriteLine($"*** {nameof(Access)} ***");

            foreach (var reversedStride in new[] { false, true })
            {
                Console.WriteLine($"{nameof(CompressedSparseTensor<double>)} : {nameof(reversedStride)} = {reversedStride}");
                CompressedSparseTensor<double> tensor = new CompressedSparseTensor<double>(new[] { 3, 5, 4 }, 10, reversedStride);
                tensor[0, 2, 0] = 1.6;
                tensor[1, 0, 3] = 5.3;
                tensor[1, 1, 0] = Math.PI;
                tensor[2, 3, 3] = 867.5309;
                tensor[2, 4, 0] = 19.4231;
                tensor[2, 4, 3] = 10000.1;

                DumpContentsSparse(tensor);
            }


            var arr = new[, ,]
            {
                {
                    {0.0, 1, 2},
                    {3, 4, 5}
                },
                {
                    {6, 7 ,8 },
                    {9, 10 ,11 },
                },
                {
                    {12, 13 ,14 },
                    {15, 16 ,17 },
                },
                {
                    {18, 19 ,20 },
                    {21, 22 ,23 },
                }
            };

            foreach (var reversedStride in new[] { false, true })
            {
                Console.WriteLine($"{nameof(DenseTensor<double>)} : {nameof(reversedStride)} = {reversedStride}");
                var tensor = arr.ToTensor(reversedStride);
                DumpContentsDense(tensor);
            }

        }
        
        // The following represent what .NET bindings would look like for a native library that needs to access Tensors of various layouts
        
        public static unsafe void DumpContentsSparse(CompressedSparseTensor<double> tensor)
        {
            fixed (double* valuesPtr = &MemoryMarshal.GetReference(tensor.Values.Span))
            fixed (int* compressedCountsPtr = &MemoryMarshal.GetReference(tensor.CompressedCounts.Span))
            fixed (int* indicesPtr = &MemoryMarshal.GetReference(tensor.Indices.Span))
            fixed (int* stridesPtr = &MemoryMarshal.GetReference(tensor.Strides))
            {
                DumpContentsSparse(valuesPtr, compressedCountsPtr, indicesPtr, stridesPtr, tensor.Rank, tensor.NonZeroCount);
            }
        }

        public static unsafe void DumpContentsDense(DenseTensor<double> tensor)
        {
            fixed (double* valuesPtr = &MemoryMarshal.GetReference(tensor.Buffer.Span))
            fixed (int* stridesPtr = &MemoryMarshal.GetReference(tensor.Strides))
            {
                DumpContentsDense(valuesPtr, stridesPtr, tensor.Rank, (int)tensor.Length);
            }

        }

        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe void DumpContentsSparse(double* values, int* compressedCounts, int* indices, int* strides, int rank, int valueCount);

        [DllImport("TensorSamples.native.dll")]
        extern private static unsafe void DumpContentsDense(double* values, int* strides, int rank, int length);
    }
}
