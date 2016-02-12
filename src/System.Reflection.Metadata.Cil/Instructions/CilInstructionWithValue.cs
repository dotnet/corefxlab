using System.Reflection.Emit;

namespace System.Reflection.Metadata.Cil.Instructions
{
    public abstract class CilInstructionWithValue<T> : CilInstruction
    {
        private T _value;
        private int _token;

        internal CilInstructionWithValue(OpCode opCode, T value, int token, int size)
            : base(opCode, size)
        {
            _value = value;
            _token = token;
        }

        public T Value
        {
            get
            {
                return _value;
            }
        }

        public int Token
        {
            get
            {
                return _token;
            }
        }
    }
}
