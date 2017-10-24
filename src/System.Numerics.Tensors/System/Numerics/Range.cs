using System;
using System.Collections.Generic;
using System.Text;

namespace System.Numerics
{
    // To be replaced once C# natively supports ranges and Range syntax (lower:upper) or (lower..upper) (lower:) or (lower..) etc
    public struct Range
    {
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
