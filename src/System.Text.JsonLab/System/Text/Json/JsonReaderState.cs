// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Text.JsonLab
{
    public struct JsonReaderState
    {
        internal ulong _containerMask;
        internal int _depth;
        internal bool _inObject;
        internal Stack<bool> _stack;
        internal JsonTokenType _tokenType;
        internal int _lineNumber;
        internal int _position;

        public bool IsDefault
            => _containerMask == default &&
            _depth == default &&
            _inObject == default &&
            _stack == null &&
            _tokenType == default &&
            _lineNumber == default &&
            _position == default;
    }
}
