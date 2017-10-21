// native.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"



typedef double* (__stdcall *AllocateDoubleArray)(int);

extern "C"  __declspec(dllexport) double* __stdcall GetMultTableAllocateManaged(AllocateDoubleArray allocator, int number)
{
    int size = number * number;
    double* result = allocator(size);

    for (int i = 0; i < number; i++)
    {
        int rowOffset = i * number;
        for (int j = 0; j < number; j++)
        {
            result[rowOffset + j] = (i + 1.0) * (j + 1.0);
        }
    }

    return result;
}

extern "C"  __declspec(dllexport) void __stdcall GetMultTablePreAllocated(int number, double* result, int resultSize)
{
    int size = number * number;

    if (resultSize < size)
    {
        // error
        return;
    }

    for (int i = 0; i < number; i++)
    {
        int rowOffset = i * number;
        for (int j = 0; j < number; j++)
        {
            result[rowOffset + j] = (i + 1.0) * (j + 1.0);
        }
    }

    return;
}

extern "C"  __declspec(dllexport) double __stdcall GetRowSum(double* data, int* dimensions, int rank, int row)
{
    int rowStride = 0;
    for (int i = 1; i < rank; i++)
    {
        rowStride += dimensions[i];
    }

    double* rowStart = data + row * rowStride;
    double sum = 0;
    for (int i = 0; i < rowStride; i++)
    {
        sum += rowStart[i];
    }

    return sum;
}


extern "C"  __declspec(dllexport) void __stdcall ScalarPowerSparse(double* values, int* compressedCounts, int* indices, int* dimensions, int rank, int valueCount, int power)
{
    for (int i = 0; i < valueCount; i++)
    {
        values[i] = pow(values[i], power);
    }
}

extern "C"  __declspec(dllexport) double* __stdcall GetMultTableAllocateNative(int number)
{
    int size = number * number;
    double* result = (double*)malloc(size * sizeof(double));

    for (int i = 0; i < number; i++)
    {
        int rowOffset = i * number;
        for (int j = 0; j < number; j++)
        {
            result[rowOffset + j] = (i + 1.0) * (j + 1.0);
        }
    }

    return result;
}

extern "C"  __declspec(dllexport) void* __stdcall AllocateBuffer(int sizeInBytes)
{
    return malloc(sizeInBytes);
}

extern "C"  __declspec(dllexport) void __stdcall FreeBuffer(void* buffer)
{
    printf("Freeing %p\n", buffer);
    free(buffer);
}


extern "C"  __declspec(dllexport) void __stdcall DumpContentsSparse(double* values, int* compressedCounts, int* indices, int* strides, int rank, int valueCount)
{
    bool isReversed = false;
    if (strides[0] == 1)
    {
        isReversed = true;
    }

    int * valueIndices = new int[rank];

    int compressedIndex = 0;
    for (int i = 0; i < valueCount; i++)
    {
        double value = values[i];
        int remainder = indices[i];

        while (i >= compressedCounts[compressedIndex + 1])
        {
            compressedIndex++;
        }

        int compressedDim = isReversed ? rank - 1 : 0;
        valueIndices[compressedDim] = compressedIndex;
        for (int dim = 1; dim < rank; dim++)
        {
            int actualDim = isReversed ? compressedDim - dim : dim;

            int stride = strides[actualDim];
            valueIndices[actualDim] = remainder / stride;
            remainder = remainder % stride;
        }

        printf("[");
        for (int j = 0; j < rank - 1 ; j++)
        {
            printf("%d, ", valueIndices[j]);
        }
        printf("%d] = %f\n", valueIndices[rank - 1], value);
    }

    delete[] valueIndices;
}


extern "C"  __declspec(dllexport) void __stdcall DumpContentsDense(double* values, int* strides, int rank, int length)
{
    bool isReversed = false;
    if (strides[0] == 1)
    {
        isReversed = true;
    }

    int * valueIndices = new int[rank];

    for (int i = 0; i < length; i++)
    {
        int remainder = i;
        for (int dim = 0; dim < rank; dim++)
        {
            int actualDim = isReversed ? rank - 1 - dim : dim;

            int stride = strides[actualDim];
            valueIndices[actualDim] = remainder / stride;
            remainder = remainder % stride;
        }

        printf("[");
        for (int j = 0; j < rank - 1; j++)
        {
            printf("%d, ", valueIndices[j]);
        }
        printf("%d] = %f\n", valueIndices[rank - 1], values[i]);
    }
}
