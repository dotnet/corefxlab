using ILDasmLibrary.Visitor;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILInstructionWithNoValue :ILInstruction, IVisitable
    {
        internal ILInstructionWithNoValue(OpCode opCode, int size)
            : base(opCode, size)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
       
    }
}
