using ILDasmLibrary.Visitor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.ILDasm.Decoder;

namespace ILDasmLibrary
{
    public struct ILAssemblyReference: IVisitable
    {
        private Readers _readers;
        private AssemblyReference _assemblyRef;
        private ILAssembly _assemblyDefinition;
        private string _culture;
        private string _name;
        private Version _version;
        private IEnumerable<ILCustomAttribute> _customAttributes;
        private byte[] _hashValue;
        private byte[] _publicKeyOrToken;
        private int _token;

        internal static ILAssemblyReference Create(AssemblyReference assemblyRef, int token, ref Readers readers, ILAssembly assemblyDefinition)
        {
            ILAssemblyReference assembly = new ILAssemblyReference();
            assembly._assemblyRef = assemblyRef;
            assembly._token = token;
            assembly._readers = readers;
            assembly._assemblyDefinition = assemblyDefinition;
            return assembly;
        }

        public ILAssembly AssemblyDefinition
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
                return ILDecoder.GetCachedValue(_assemblyRef.Name, _readers, ref _name);
            }
        }

        public string Culture
        {
            get
            {
                return ILDecoder.GetCachedValue(_assemblyRef.Culture, _readers, ref _culture);
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
            return ILDecoder.GetByteArrayString(HashValue);
        }

        public string GetPublicKeyOrTokenString()
        {
            if (!HasPublicKeyOrToken)
                return string.Empty;
            return ILDecoder.GetByteArrayString(PublicKeyOrToken);
        }

        public string GetFormattedVersion()
        {
            return ILDecoder.GetVersionString(Version);
        }

        public void Accept(IVisitor visitor)
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

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _assemblyRef.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
            }
        }
    }
}
