// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace System.Numerics
{
    // To be replaced once C# natively supports ranges and Range syntax (lower:upper) or (lower..upper) (lower:) or (lower..) etc
    public struct Range
    {
        [DebuggerStepThrough]
        public Range(int lowerBound, int upperBound)
        {
            if (lowerBound > upperBound)
            {
                throw new ArgumentException($"{nameof(upperBound)} must be greater than or equal to {nameof(lowerBound)}", nameof(upperBound));
            }

            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public int LowerBound { get; }
        public int UpperBound { get; }

        public int Length
        {
            get
            {
                return UpperBound - LowerBound + 1;
            }
        }
    }
}
