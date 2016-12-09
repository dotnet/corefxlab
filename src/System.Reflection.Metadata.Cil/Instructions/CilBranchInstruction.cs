using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public class CilBranchInstruction : CilInstructionWithValue<int>, ICilVisitable
    {
        internal CilBranchInstruction(OpCode opCode, int value, int ilOffset, int size)
            :base(opCode, value, ilOffset, size)
        {
        }

        public override void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
