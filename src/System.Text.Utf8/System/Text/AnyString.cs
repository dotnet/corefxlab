// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;

// TODO: Choose a much better name for this.

namespace System.Text
{
    /// <summary>
    /// Can hold a <see cref="String"/> or a <see cref="Utf8String"/>.
    /// </summary>
    public struct AnyString : IEnumerable<UnicodeScalar>, IEquatable<AnyString>
    {
        public AnyString(string value) => throw null;

        public AnyString(Utf8String value) => throw null;

        public static bool operator ==(AnyString a, AnyString b) => throw null;

        public static bool operator !=(AnyString a, AnyString b) => throw null;

        public static implicit operator AnyString(string value) => throw null;

        public static implicit operator AnyString(Utf8String value) => throw null;

        public bool Equals(AnyString other) => throw null;

        public override bool Equals(object obj) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public override int GetHashCode() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString() => throw null;

        public Utf8String ToUtf8String() => throw null;

        IEnumerator<UnicodeScalar> IEnumerable<UnicodeScalar>.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<UnicodeScalar>
        {
            public UnicodeScalar Current => throw new NotImplementedException();

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            public void Reset() => throw null;

            object IEnumerator.Current => throw new NotImplementedException();
        }
    }
}
