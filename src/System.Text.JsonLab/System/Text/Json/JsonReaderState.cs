// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Text.JsonLab
{
    public struct JsonReaderState
    {
        internal ulong _containerMask;
        internal int _currentDepth;
        internal bool _inObject;
        internal Stack<byte> _stack;
        internal JsonTokenType _tokenType;
        internal long _lineNumber;
        internal long _position;
        internal bool _isSingleValue;
        internal SequencePosition _sequencePosition;
        internal long _bytesConsumed;

        public SequencePosition Position => _sequencePosition;
        public long BytesConsumed => _bytesConsumed;

        internal bool IsDefault
            => _containerMask == default &&
            _currentDepth == default &&
            _inObject == default &&
            _stack == null &&
            _tokenType == default &&
            _sequencePosition.GetObject() == null &&
            _bytesConsumed == default &&
            //_lineNumber == default && // the default _lineNumber is 1, we want IsDefault to return true for default(JsonReaderState)
            //_isSingleValue == default && // the default _isSingleValue is true, we want IsDefault to return true for default(JsonReaderState)
            _position == default;
    }
}
