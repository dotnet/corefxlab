using System;
using System.Collections.Generic;
using System.Text;

namespace System.Numerics
{
    internal static class EmptyArray<T>
    {
        public static readonly T[] Value = new T[0];
    }

    internal static class ArrayUtilities
    {
        public static T[] Empty<T>()
        {
            return EmptyArray<T>.Value;
        }
    }
}
