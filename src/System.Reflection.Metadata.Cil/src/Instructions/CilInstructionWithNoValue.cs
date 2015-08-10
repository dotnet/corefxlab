using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public class CilInstructionWithNoValue :CilInstruction, ICilVisitable
    {
        internal CilInstructionWithNoValue(OpCode opCode, int size)
            : base(opCode, size)
        {
        }

        public override void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }
       
    }
}
