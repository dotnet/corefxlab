// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    /// <summary>
    /// Base class for IL instructions.
    /// </summary>
    public abstract class CilInstruction : ICilVisitable
    {
        private OpCode _opCode;
        private int _size;

        internal CilInstruction(OpCode opCode, int size)
        {
            _opCode = opCode;
            _size = size;
        }

        public OpCode opCode
        {
            get
            {
                return _opCode;
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }
        }
        
        abstract public void Accept(ICilVisitor visitor);
    }
}
