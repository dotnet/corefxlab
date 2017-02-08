// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Reflection.Metadata.Cil
{
    public struct CilTypeLayout
    {
        private readonly TypeLayout _layout;

        public CilTypeLayout(TypeLayout layout)
        {
            _layout = layout;
        }

        public int Size
        {
            get { return _layout.Size; }
        }

        public int PackingSize
        {
            get { return _layout.PackingSize; }
        }

        public bool IsDefault
        {
            get { return _layout.IsDefault; }
        }
    }
}
