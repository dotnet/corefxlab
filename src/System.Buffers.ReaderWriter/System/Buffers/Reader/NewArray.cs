// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers
{
    /// <summary>
    /// Simple ArrayPool that just new(s) up arrays
    /// </summary>
    internal sealed class NewArray<T> : ArrayPool<T>
    {
        public override T[] Rent(int minimumLength)
        {
            return new T[minimumLength];
        }

        public override void Return(T[] array, bool clearArray = false)
        {
            // Do nothing
        }
    }
}
