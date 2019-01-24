// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace System.Text.CaseFolding
{
    /// <summary>
    /// String comparer with simple case folding.
    /// </summary>
    public class SimpleCaseFoldingStringComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
    {
        // Based on CoreFX StringComparer code

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCaseFoldingStringComparer"/> class.
        /// </summary>
        public SimpleCaseFoldingStringComparer()
        {
        }

        /// <summary>
        /// IComparer.Compare() implementation.
        /// </summary>
        /// <param name="x">Object to compare.</param>
        /// <param name="y">Object to compare.</param>
        /// <returns>
        /// Returns 0 - if equal, -1 - if x &lt; y, +1 - if x &gt; y.
        /// </returns>
        public int Compare(object x, object y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            if (x is string sa && y is string sb)
            {
                return SimpleCaseFolding.CompareUsingSimpleCaseFolding(sa, sb);
            }

            if (x is IComparable ia)
            {
                return ia.CompareTo(y);
            }

            throw new ArgumentException("SR.Argument_ImplementIComparable");
        }

        /// <summary>
        /// IEqualityComparer.Equal() implementation.
        /// </summary>
        /// <param name="x">Object to compare.</param>
        /// <param name="y">Object to compare.</param>
        /// <returns>
        /// Returns true if equal.
        /// </returns>
        public new bool Equals(object x, object y)
        {
            if (x == y)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x is string sa && y is string sb)
            {
                return Equals(sa, sb);
            }

            return x.Equals(y);
        }

        /// <summary>
        /// IEqualityComparer.GetHashCode() implementation.
        /// </summary>
        /// <param name="obj">Object for which to get a hash.</param>
        /// <returns>
        /// Returns a hash code.
        /// </returns>
        public int GetHashCode(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            if (obj is string s)
            {
                return GetHashCodeSimpleCaseFolding(s);
            }

            return obj.GetHashCode();
        }

        private static int GetHashCodeSimpleCaseFolding(string source)
        {
            //Diagnostics.Assert(source != null, "source must not be null");

            // Do not allocate on the stack if string is empty
            if (source.Length == 0)
            {
                return source.GetHashCode();
            }

            char[] borrowedArr = null;
            Span<char> span = source.Length <= 255 ?
                stackalloc char[source.Length] :
                (borrowedArr = ArrayPool<char>.Shared.Rent(source.Length));

            SimpleCaseFolding.SimpleCaseFold(span, source);

            int hash = SCFMarvin.ComputeHash32OrdinalIgnoreCase(span, SCFMarvin.DefaultSeed);

            // Return the borrowed array if necessary.
            if (borrowedArr != null)
            {
                ArrayPool<char>.Shared.Return(borrowedArr);
            }

            return hash;
        }

        // The code come from CoreFX SqlBinary.HashByteArray()
        internal static int HashByteArray(ReadOnlySpan<byte> rgbValue)
        {
            int length = rgbValue.Length;

            if (length <= 0)
            {
                return 0;
            }

            int ulValue = DefaultSeed;
            int ulHi;

            // Size of CRC window (hashing bytes, ssstr, sswstr, numeric)
            const int XcbCrcWindow = 4;

            // const int IntShiftVal = (sizeof ulValue) * (8*sizeof(char)) - XcbCrcWindow;
            const int IntShiftVal = (4 * 8) - XcbCrcWindow;

            for (int i = 0; i < length; i++)
            {
                ulHi = (ulValue >> IntShiftVal) & 0xff;
                ulValue <<= XcbCrcWindow;
                ulValue = ulValue ^ rgbValue[i] ^ ulHi;
            }

            return ulValue;
        }

        private static int DefaultSeed { get; } = GenerateSeed();

        private static int GenerateSeed()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[sizeof(ulong)];
                rng.GetBytes(bytes);
                var hash64 = BitConverter.ToUInt64(bytes, 0);
                return ((int)(hash64 >> 32)) ^ (int)hash64;
            }
        }

        /// <summary>
        /// IComparer&lt;string&gt;.GetHashCode() implementation.
        /// </summary>
        /// <param name="x">Left object to compare.</param>
        /// <param name="y">Right object to compare.</param>
        /// <returns>
        /// Returns 0 - if equal, -1 - if x &lt; y, +1 - if x &gt; y.
        /// </returns>
        public int Compare(string x, string y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            return SimpleCaseFolding.CompareUsingSimpleCaseFolding(x, y);
        }

        /// <summary>
        /// IEqualityComparer&lt;string&gt;.Equals() implementation.
        /// </summary>
        /// <param name="x">Left object to compare.</param>
        /// <param name="y">Right object to compare.</param>
        /// <returns>
        /// Returns true if equal.
        /// </returns>
        public bool Equals(string x, string y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return SimpleCaseFolding.CompareUsingSimpleCaseFolding(x, y) == 0;
        }

        /// <summary>
        /// IEqualityComparer&lt;string&gt;.GetHashCode() implementation.
        /// </summary>
        /// <param name="obj">Object for which to get a hash.</param>
        /// <returns>
        /// Returns a hash code.
        /// </returns>
        public int GetHashCode(string obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return GetHashCodeSimpleCaseFolding(obj);
        }
    }
}
