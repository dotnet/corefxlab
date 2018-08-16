// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.JsonLab
{
    // Do not change the order of the enum values, since IsSimpleValue relies on it.
    public enum JsonValueType : byte
    {
        Object = 0,
        Array = 1,
        String = 2,
        Number = 3,
        True = 4,
        False = 5,
        Null = 6,
        Unknown = 7
    }
}
