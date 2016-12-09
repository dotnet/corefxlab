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
