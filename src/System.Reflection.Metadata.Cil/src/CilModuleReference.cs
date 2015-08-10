using System.Collections.Generic;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil
{
    public struct CilModuleReference : ICilVisitable
    {
        private CilReaders _readers;
        private ModuleReference _moduleReference;
        private string _name;
        private IEnumerable<CilCustomAttribute> _customAttributes;
        private int _token;

        internal static CilModuleReference Create(ModuleReference moduleReference, ref CilReaders readers, int token)
        {
            CilModuleReference reference = new CilModuleReference();
            reference._readers = readers;
            reference._moduleReference = moduleReference;
            reference._token = token;
            return reference;
        }

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        public string Name
        {
            get
            {
                return CilDecoder.GetCachedValue(_moduleReference.Name, _readers, ref _name);
            }
        }

        public IEnumerable<CilCustomAttribute> CustomAttributes
        {
            get
            {
                if(_customAttributes == null)
                {
                    _customAttributes = GetCustomAttributes();
                }
                return _customAttributes;
            }
        }

        public void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

        private IEnumerable<CilCustomAttribute> GetCustomAttributes()
        {
            var handles = _moduleReference.GetCustomAttributes();
            foreach(var handle in handles)
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }
    }
}
