using ILDasmLibrary.Visitor;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILBranchInstruction : ILInstructionWithValue<int>, IVisitable
    {
        internal ILBranchInstruction(OpCode opCode, int value, int ilOffset, int size)
            :base(opCode, value, ilOffset, size)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
