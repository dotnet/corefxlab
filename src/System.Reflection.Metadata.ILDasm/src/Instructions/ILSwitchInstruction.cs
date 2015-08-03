using ILDasmLibrary.Visitor;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public class ILSwitchInstruction : ILInstructionWithValue<uint>, IVisitable
    {
        private readonly int[] _jumps;
        private readonly int _ilOffset;
        internal ILSwitchInstruction(OpCode opCode, int ilOffset, int[] jumps, int token, uint value, int size)
            :base(opCode,value,token,size)
        {
            _jumps = jumps;
            _ilOffset = ilOffset;
        }

        public int[] Jumps
        {
            get
            {
                return _jumps;
            }
        }

        public int IlOffset
        {
            get
            {
                return _ilOffset;
            }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
