// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Runtime.CompilerServices;

namespace System.Numerics.Matrices
{
    internal static class MatrixHelper
    {
        public const double Epsilon = Double.Epsilon * 10;

        /// <summary>
        /// Checks two doubldes for quality using <see cref="Epsilon"/> as the smallest unit of difference.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns>True if the values are within <see cref="Epsilon"/> in value; False otherwise.</returns>
        /// <remarks>see the Precision in Comparisons section of https://msdn.microsoft.com/en-us/library/ya2zha7s(v=vs.110).aspx</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreEqual(double value1, double value2)
        {
            if (Double.IsNaN(value1) || Double.IsNaN(value2))
                return false;

            return value1 == value2
                || (Math.Abs(Math.Abs(value1) - Math.Abs(value2)) < Epsilon);
        }

        /// <summary>
        /// Checks two doubldes for quality using <see cref="Epsilon"/> as the smallest unit of difference.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <returns>False if the values are within <see cref="Epsilon"/> in value; True otherwise.</returns>
        /// <remarks>see the Precision in Comparisons section of https://msdn.microsoft.com/en-us/library/ya2zha7s(v=vs.110).aspx</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NotEqual(double value1, double value2)
        {
            if (Double.IsNaN(value1) || Double.IsNaN(value2))
                return true;

            return value1 != value2
                && !(Math.Abs(Math.Abs(value1) - Math.Abs(value2)) < Epsilon);
        }
    }
}
