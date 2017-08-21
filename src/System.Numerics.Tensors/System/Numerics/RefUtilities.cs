using System;
using System.Collections.Generic;
using System.Text;

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
