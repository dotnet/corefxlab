// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Numerics
{
    internal static class RefUtilities
    {
        public static ref T Ternary<T>(bool check, ref T left, ref T right)
        {
            // TODO remove this method and use ternary ref syntax once it is available.
            if (check)
            {
                return ref left;
            }

            return ref right;
        }
    }
}
