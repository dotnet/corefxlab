using ILDasmLibrary.Visitor;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata.ILDasm.Decoder;
using System.Text;

namespace ILDasmLibrary
{
    /* 
        TO DO: Decode Custom attribute with custom attribute decoder to have custom attribute named arguments.
    */
    public struct ILCustomAttribute : IVisitable
    {
        private Readers _readers;
        private readonly CustomAttribute _attribute;
        private string _constructor;
        private string _parent;
        private byte[] _value;

        internal ILCustomAttribute(CustomAttribute attribute, ref Readers readers)
        {
            _attribute = attribute;
            _readers = readers;
            _parent = null;
            _constructor = null;
            _value = null;
        }

        public string Parent
        {
            get
            {
                if(_parent == null)
                {
                    SignatureDecoder.DecodeType(_attribute.Parent, _readers.Provider, null).ToString();
                }
                return _parent;
            }
        }
        
        public string Constructor
        {
            get
            {
                if(_constructor == null)
                {
                    _constructor = ILDecoder.SolveMethodName(_readers.MdReader, MetadataTokens.GetToken(_attribute.Constructor), _readers.Provider);
                }
                return _constructor;
            }
        }

        public byte[] Value
        {
            get
            {
                if(_value == null)
                {
                    if (_attribute.Value.IsNil)
                    {
                        _value = new byte[0];
                        return _value;
                    }
                    _value = _readers.MdReader.GetBlobBytes(_attribute.Value);
                }
                return _value;
            }
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetValueString()
        {
            return ILDecoder.GetByteArrayString(Value);
        }
    }
}
