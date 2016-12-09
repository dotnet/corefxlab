using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil.Instructions
{
    /// <summary>
    /// Base class for IL instructions.
    /// </summary>
    public abstract class CilInstruction : ICilVisitable
    {
        private OpCode _opCode;
        private int _size;

        internal CilInstruction(OpCode opCode, int size)
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
        
        abstract public void Accept(ICilVisitor visitor);
    }
}
