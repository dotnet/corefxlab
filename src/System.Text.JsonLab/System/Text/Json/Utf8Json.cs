// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;

using static System.Text.JsonLab.JsonThrowHelper;

namespace System.Text.JsonLab
{
    public partial class Utf8Json
    {
        public Reader GetReader(ReadOnlySequence<byte> jsonData, bool isFinalBlock = true) => new Reader(this, jsonData, isFinalBlock);

        public Reader GetReader(ReadOnlySpan<byte> jsonData, bool isFinalBlock = true) => new Reader(this, jsonData, isFinalBlock);

        // We are using a ulong to represent our nested state, so we can only go 64 levels deep.
        internal const int StackFreeMaxDepth = sizeof(ulong) * 8;

        internal JsonReaderState _state;

        private JsonReaderOptions _readerOptions;
        private int _maxDepth;

        public Utf8Json()
        {
            _state = default;
            _state._lineNumber = 1;
            _state._isSingleValue = true;

            _readerOptions = new JsonReaderOptions(JsonReaderOptions.CommentHandling.Default);
            _maxDepth = StackFreeMaxDepth;
        }

        public Utf8Json(JsonReaderState initialState)
        {
            _state = initialState;

            if (_state._lineNumber < 1)
                ThrowArgumentException("Invalid line number. Line number must be >= 1.");

            if (_state._currentDepth < 0)
                ThrowArgumentException("Invalid depth. Depth must be positive.");

            if (_state._position < 0)
                ThrowArgumentException("Invalid position. Position must be positive.");

            _readerOptions = new JsonReaderOptions(JsonReaderOptions.CommentHandling.Default);
            _maxDepth = StackFreeMaxDepth;
        }

        public JsonReaderState CurrentState => _state;

        public JsonReaderOptions Options
        {
            get
            {
                return _readerOptions;
            }
            set
            {
                _readerOptions = value;
                if (_readerOptions._commentHandling == JsonReaderOptions.CommentHandling.AllowComments && _state._stack == null)
                    _state._stack = new Stack<byte>();
            }
        }

        public int MaxDepth
        {
            get
            {
                return _maxDepth;
            }
            set
            {
                if (value <= 0)
                    ThrowArgumentException("Max depth must be positive.");
                _maxDepth = value;
                if (_maxDepth > StackFreeMaxDepth)
                    _state._stack = new Stack<byte>();
            }
        }
    }
}
