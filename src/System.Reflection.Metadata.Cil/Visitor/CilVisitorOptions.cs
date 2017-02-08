// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Reflection.Metadata.Cil.Visitor
{
    public struct CilVisitorOptions
    {
        private readonly bool _showBytes;

        public CilVisitorOptions(bool showBytes)
        {
            _showBytes = showBytes;
        }

        public bool ShowBytes
        {
            get
            {
                return _showBytes;
            }
        }
    }
}
