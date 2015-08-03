using ILDasmLibrary.Visitor;
using System.Reflection.Emit;
using System.Text;

namespace ILDasmLibrary.Instructions
{
    /// <summary>
    /// Base class for IL instructions.
    /// </summary>
    public abstract class ILInstruction : IVisitable
    {
        private OpCode _opCode;
        private int _size;

        internal ILInstruction(OpCode opCode, int size)
        {
            _opCode = opCode;
            _size = size;
        }

        public OpCode opCode
        {
            get
            {
                return _opCode;
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }
        }
        
        abstract public void Accept(IVisitor visitor);
    }
}
