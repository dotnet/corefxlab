using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public class CilStringInstruction : CilInstructionWithValue<string>, ICilVisitable
    {
        private readonly bool _isPrintable;
        internal CilStringInstruction(OpCode opCode,string value, int token, int size, bool isPrintable = true)
            : base(opCode, value, token, size)
        {
            _isPrintable = isPrintable;
        }

        public bool IsPrintable
        {
            get
            {
                return _isPrintable;
            }
        }

        public override void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}
