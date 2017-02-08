// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Metadata.Cil.Visitor;
using System.Reflection.Emit;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public class CilInt32Instruction : CilNumericValueInstruction<int> , ICilVisitable
    {
        internal CilInt32Instruction(OpCode opCode, int value, int token, int size)
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
