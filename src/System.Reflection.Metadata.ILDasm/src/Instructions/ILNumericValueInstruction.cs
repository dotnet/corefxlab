using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    public abstract class ILNumericValueInstruction<T> : ILInstructionWithValue<T>
    {
        private string _bytes;
        internal ILNumericValueInstruction(OpCode opCode, T value, int token, int size)
            :base(opCode, value, token, size)
        {
        }

        public string Bytes
        {
            get
            {
                if(_bytes == null)
                {
                    _bytes = GetBytes();
                }
                return _bytes;
            }
        }

        protected abstract string GetBytes();
    }
}
