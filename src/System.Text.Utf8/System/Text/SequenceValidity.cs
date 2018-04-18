// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text
{
    /// <summary>
    /// Represents the validity of a sequence returned by <see cref="UnicodeReader.PeekFirstScalar"/>.
    /// </summary>
    public enum SequenceValidity
    {
        /// <summary>
        /// The input buffer was empty.
        /// </summary>
        Empty = 0,

        /// <summary>
        /// The input buffer contained a valid sequence.
        /// </summary>
        ValidSequence = 1,

        /// <summary>
        /// The input buffer contained an invalid or incomplete sequence.
        /// </summary>
        InvalidSequence = 2,

        Incomplete = 3,
    }
}
