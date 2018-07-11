// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Ecma335;

namespace System.Reflection.Metadata.Cil
{
    public struct CilTypeReference
    {
        /// <summary>
        /// TODO: Resolution Scope to be a valid ILEntity.
        /// </summary>

        private TypeReference _typeReference;
        private CilReaders _readers;
        private string _name;
        private string _namespace;
        private string _fullName;
        private int _token;

        internal static CilTypeReference Create(TypeReference typeReference, ref CilReaders readers, int token)
        {
            CilTypeReference type = new CilTypeReference();
            type._typeReference = typeReference;
            type._readers = readers;
            type._token = token;
            return type;
        }

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        public string FullName
        {
            get
            {
                if(_fullName == null)
                {
                    _fullName = SignatureDecoder.DecodeType(MetadataTokens.TypeReferenceHandle(_token), _readers.Provider, 0).ToString(false);
                }
                return _fullName;
            }
        }

        public string Name
        {
            get
            {
                return CilDecoder.GetCachedValue(_typeReference.Name, _readers, ref _name);
            }
        }

        public string Namespace
        {
            get
            {
                return CilDecoder.GetCachedValue(_typeReference.Namespace, _readers, ref _namespace);
            }
        }

        // TODO ResolutionScope
    }
}
