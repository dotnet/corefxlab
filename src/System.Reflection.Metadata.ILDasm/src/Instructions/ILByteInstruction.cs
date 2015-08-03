using ILDasmLibrary.Visitor;
using System.Reflection.Emit;

namespace ILDasmLibrary.Instructions
{
    public class ILByteInstruction : ILNumericValueInstruction<byte>, IVisitable
    {
        internal ILByteInstruction(OpCode opCode, byte value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override string GetBytes()
        {
            return Value.ToString("X2");
        }
    }
}
