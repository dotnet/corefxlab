using System.Collections.Generic;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil
{
    public struct CilAssemblyReference: ICilVisitable
    {
        private CilReaders _readers;
        private AssemblyReference _assemblyRef;
        private CilAssembly _assemblyDefinition;
        private string _culture;
        private string _name;
        private Version _version;
        private IEnumerable<CilCustomAttribute> _customAttributes;
        private byte[] _hashValue;
        private byte[] _publicKeyOrToken;
        private int _token;

        internal static CilAssemblyReference Create(AssemblyReference assemblyRef, int token, ref CilReaders readers, CilAssembly assemblyDefinition)
        {
            CilAssemblyReference assembly = new CilAssemblyReference();
            assembly._assemblyRef = assemblyRef;
            assembly._token = token;
            assembly._readers = readers;
            assembly._assemblyDefinition = assemblyDefinition;
            return assembly;
        }

        public CilAssembly AssemblyDefinition
        {
            get
            {
                return _assemblyDefinition;
            }
        }
        public string Name
        {
            get
            {
                return CilDecoder.GetCachedValue(_assemblyRef.Name, _readers, ref _name);
            }
        }

        public string Culture
        {
            get
            {
                return CilDecoder.GetCachedValue(_assemblyRef.Culture, _readers, ref _culture);
            }
        }

        public Version Version
        {
            get
            {
                if (_version == null)
                {
                    _version = _assemblyRef.Version;
                }
                return _version;
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

        public bool HasHashValue
        {
            get
            {
                return HashValue.Length != 0;
            }
        }

        public bool HasPublicKeyOrToken
        {
            get
            {
                return PublicKeyOrToken.Length != 0;
            }
        }

        public bool HasCulture
        {
            get
            {
                return !_assemblyRef.Culture.IsNil;
            }
        }

        public AssemblyFlags Flags
        {
            get
            {
                return _assemblyRef.Flags;
            }
        }

        public byte[] HashValue
        {
            get
            {
                if(_hashValue == null)
                {
                    _hashValue = _readers.MdReader.GetBlobBytes(_assemblyRef.HashValue);
                }
                return _hashValue;
            }
        }

        public byte[] PublicKeyOrToken
        {
            get
            {
                if(_publicKeyOrToken == null)
                {
                    _publicKeyOrToken = _readers.MdReader.GetBlobBytes(_assemblyRef.PublicKeyOrToken);
                }
                return _publicKeyOrToken;
            }
        }

        public string GetHashValueString()
        {
            if (!HasHashValue)
                return string.Empty;
            return CilDecoder.CreateByteArrayString(HashValue);
        }

        public string GetPublicKeyOrTokenString()
        {
            if (!HasPublicKeyOrToken)
                return string.Empty;
            return CilDecoder.CreateByteArrayString(PublicKeyOrToken);
        }

        public string GetFormattedVersion()
        {
            return CilDecoder.CreateVersionString(Version);
        }

        public void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        private IEnumerable<CilCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _assemblyRef.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }
    }
}
