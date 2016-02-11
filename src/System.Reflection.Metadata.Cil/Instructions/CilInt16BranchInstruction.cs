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
