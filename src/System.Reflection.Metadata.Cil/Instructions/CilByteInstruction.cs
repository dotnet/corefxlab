// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public class CilByteInstruction : CilNumericValueInstruction<byte>, ICilVisitable
    {
        internal CilByteInstruction(OpCode opCode, byte value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        public override void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override string GetBytes()
        {
            return Value.ToString("X2");
        }
    }
}
