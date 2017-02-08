// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public class CilInt16BranchInstruction : CilInstructionWithValue<sbyte>, ICilVisitable
    {
        internal CilInt16BranchInstruction(OpCode opCode, sbyte value, int ilOffset, int size)
            :base(opCode, value, ilOffset, size)
        {
        }

        public override void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }
        
    }
}
