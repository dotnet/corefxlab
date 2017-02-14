// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil
{
    public struct CilLocal : ICilVisitable
    {
        private readonly string _name;
        private readonly string _type;

        public CilLocal(string name, string type)
        {
            _name = name;
            _type = type;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
        }

        public void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}