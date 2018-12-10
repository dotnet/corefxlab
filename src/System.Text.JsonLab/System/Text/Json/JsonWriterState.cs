// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    public struct JsonWriterState
    {
        internal const int DefaultMaxDepth = JsonConstants.MaxPossibleDepth;

        internal long _bytesWritten;
        internal long _bytesCommitted;
        internal int _maxDepth;
        internal bool _inObject;
        internal bool _isNotPrimitive;
        internal JsonTokenType _tokenType;
        internal JsonWriterOptions _writerOptions;
        internal BitStack _bitStack;

        public long BytesWritten => _bytesWritten;

        public long BytesCommitted => _bytesCommitted;

        public JsonWriterState(int maxDepth = DefaultMaxDepth, JsonWriterOptions options = default)
        {
            //TODO: Use ThrowHelper: GetArgumentException_MaxDepthMustBePositive
            if (maxDepth <= 0)
                throw new ArgumentException("max depth must be positive");

            _bytesWritten = default;
            _bytesCommitted = default;
            _maxDepth = maxDepth;
            _inObject = default;
            _isNotPrimitive = default;
            _tokenType = default;
            _writerOptions = options;

            // Only allocate if the user writes a JSON payload beyond the depth that the _allocationFreeContainer can handle.
            // This way we avoid allocations in the common, default cases, and allocate lazily.
            _bitStack = default;
        }

        public JsonWriterOptions Options => _writerOptions;

        public int MaxDepth => _maxDepth;
    }
}
