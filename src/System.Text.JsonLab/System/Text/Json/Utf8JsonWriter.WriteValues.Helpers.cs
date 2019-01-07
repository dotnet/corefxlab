// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace System.Text.JsonLab
{
    public ref partial struct Utf8JsonWriter2
    {
        private void ValidateWritingValue()
        {
            if (_inObject)
            {
                Debug.Assert(_tokenType != JsonTokenType.None && _tokenType != JsonTokenType.StartArray);
                JsonThrowHelper.ThrowJsonWriterException(_tokenType);    //TODO: Add resource message
            }
            else
            {
                if (!_isNotPrimitive && _tokenType != JsonTokenType.None)
                {
                    JsonThrowHelper.ThrowJsonWriterException(_tokenType);    //TODO: Add resource message
                }
            }
        }
    }
}
