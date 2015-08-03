using ILDasmLibrary.Visitor;
using System;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILDoubleInstruction : ILNumericValueInstruction<double>, IVisitable
    {
        internal ILDoubleInstruction(OpCode opCode, double value, int token, int size)
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
