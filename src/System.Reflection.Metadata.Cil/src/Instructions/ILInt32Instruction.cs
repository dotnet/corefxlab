using ILDasmLibrary.Visitor;
using System.Reflection.Emit;
using System;

namespace ILDasmLibrary.Instructions
{
    public class ILInt32Instruction : ILNumericValueInstruction<int> , IVisitable
    {
        internal ILInt32Instruction(OpCode opCode, int value, int token, int size)
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
