// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

public struct Range
{
    public Range(int index, int length)
    {
        Index = index;
        Length = length;
    }
    public readonly int Index;
    public readonly int Length;

    public void Deconstruct(out int index, out int length)
    {
        index = Index;
        length = Length;
    }
}