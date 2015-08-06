using ILDasmLibrary.Visitor;
using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    public class ILInt16BranchInstruction : ILInstructionWithValue<sbyte>, IVisitable
    {
        internal ILInt16BranchInstruction(OpCode opCode, sbyte value, int ilOffset, int size)
            :base(opCode, value, ilOffset, size)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        
    }
}
