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

extern "C"  __declspec(dllexport) void __stdcall FreeBuffer(void* buffer)
{
    printf("Freeing %p\n", buffer);
    free(buffer);
}
