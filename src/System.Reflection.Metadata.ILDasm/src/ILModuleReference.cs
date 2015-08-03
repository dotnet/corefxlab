using ILDasmLibrary.Visitor;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.ILDasm.Decoder;

namespace ILDasmLibrary
{
    public struct ILModuleReference : IVisitable
    {
        private Readers _readers;
        private ModuleReference _moduleReference;
        private string _name;
        private IEnumerable<ILCustomAttribute> _customAttributes;
        private int _token;

        internal static ILModuleReference Create(ModuleReference moduleReference, ref Readers readers, int token)
        {
            ILModuleReference reference = new ILModuleReference();
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
                return ILDecoder.GetCachedValue(_moduleReference.Name, _readers, ref _name);
            }
        }

        public IEnumerable<ILCustomAttribute> CustomAttributes
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

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            var handles = _moduleReference.GetCustomAttributes();
            foreach(var handle in handles)
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
            }
        }
    }
}
