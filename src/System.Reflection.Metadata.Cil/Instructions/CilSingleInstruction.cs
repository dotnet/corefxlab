using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public class CilSingleInstruction : CilNumericValueInstruction<float>, ICilVisitable
    {
        internal CilSingleInstruction(OpCode opCode, float value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        protected override string GetBytes()
        {
            var data = BitConverter.GetBytes(Value);
            return BitConverter.ToString(data).Replace("-", string.Empty);
        }

        public override void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}
