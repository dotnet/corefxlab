using ILDasmLibrary.Visitor;
using System;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILInt64Instruction : ILNumericValueInstruction<long>, IVisitable
    {
        internal ILInt64Instruction(OpCode opCode, long value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        protected override string GetBytes()
        {
            var data = BitConverter.GetBytes(Value);
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
