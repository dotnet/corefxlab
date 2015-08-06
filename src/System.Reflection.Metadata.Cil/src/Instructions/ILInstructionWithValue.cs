using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public abstract class ILInstructionWithValue<T> : ILInstruction
    {
        private T _value;
        private int _token;

        internal ILInstructionWithValue(OpCode opCode, T value, int token, int size)
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
