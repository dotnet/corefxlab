using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata.ILDasm.Decoder;

namespace ILDasmLibrary
{
    public struct ILTypeReference
    {
        /// <summary>
        /// TODO: Resolution Scope to be a valid ILEntity.
        /// </summary>

        private TypeReference _typeReference;
        private Readers _readers;
        private string _name;
        private string _namespace;
        private ILEntity _resolutionScope;
        private string _fullName;
        private int _token;

        internal static ILTypeReference Create(TypeReference typeReference, ref Readers readers, int token)
        {
            ILTypeReference type = new ILTypeReference();
            type._typeReference = typeReference;
            type._readers = readers;
            type._token = token;
            return type;
        }

        internal int Token
        {
            get
            {
                return Token;
            }
        }

        public string FullName
        {
            get
            {
                if(_fullName == null)
                {
                    _fullName = SignatureDecoder.DecodeType(MetadataTokens.TypeReferenceHandle(_token), _readers.Provider, null).ToString(false);
                }
                return _fullName;
            }
        }

        public string Name
        {
            get
            {
                return ILDecoder.GetCachedValue(_typeReference.Name, _readers, ref _name);
            }
        }

        public string Namespace
        {
            get
            {
                return ILDecoder.GetCachedValue(_typeReference.Namespace, _readers, ref _namespace);
            }
        }

        // TODO ResolutionScope
    }
}
