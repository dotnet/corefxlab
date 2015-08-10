using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public class CilSwitchInstruction : CilInstructionWithValue<uint>, ICilVisitable
    {
        private readonly int[] _jumps;
        private readonly int _ilOffset;
        internal CilSwitchInstruction(OpCode opCode, int ilOffset, int[] jumps, int token, uint value, int size)
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

        public override void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
