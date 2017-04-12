// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    internal interface IKnown
    {
        void AddReference();
        void Release();
    }
}
