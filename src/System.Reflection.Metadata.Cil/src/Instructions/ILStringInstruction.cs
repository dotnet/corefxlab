using ILDasmLibrary.Visitor;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILStringInstruction : ILInstructionWithValue<string>, IVisitable
    {
        private readonly bool _isPrintable;
        internal ILStringInstruction(OpCode opCode,string value, int token, int size, bool isPrintable = true)
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

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}
