// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

namespace System.Numerics
{
    public partial struct Dec64 : IComparable, IFormattable, IComparable<Dec64>, IEquatable<Dec64>
    {
        public bool Equals(Dec64 other) => _value == other._value;

        public bool Equals(long other)
        {
            return default;
        }

        public override bool Equals(object obj)
        {
            return default;
        }

        public bool Equals(ulong other)
        {
            return default;
        }

        public static int Compare(Dec64 left, Dec64 right)
        {
            return default;
        }

        public int CompareTo(long other)
        {
            return default;
        }

        public int CompareTo(Dec64 other)
        {

            return default;
        }

        public int CompareTo(ulong other)
        {
            return default;
        }

        int System.IComparable.CompareTo(object obj)
        {
            return default;
        }
    }
}
