using System.Collections.Generic;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Decoding;

namespace System.Reflection.Metadata.Cil
{
    public struct CilTypeSpecification
    {
        private TypeSpecification _typeSpecification;
        private CilReaders _readers;
        private IEnumerable<CilCustomAttribute> _customAttributes;
        private CilEntity _type;
        private bool _isTypeInitialized;
        private string _signature;
        private byte[] _blob;

        internal static CilTypeSpecification Create(TypeSpecification typeSpecification, ref CilReaders readers)
        {
            CilTypeSpecification type = new CilTypeSpecification();
            type._typeSpecification = typeSpecification;
            type._readers = readers;
            type._isTypeInitialized = false;
            return type;
        }

        public string Signature
        {
            get
            {
                if(_signature == null)
                {
                    BlobReader reader = _readers.MdReader.GetBlobReader(_typeSpecification.Signature);
                    _signature = SignatureDecoder.DecodeType(ref reader, _readers.Provider).ToString();
                }
                return _signature;
            }
        }

        public byte[] Blob
        {
            get
            {
                if(_blob == null)
                {
                    _blob = _readers.MdReader.GetBlobBytes(_typeSpecification.Signature);
                }
                return _blob;
            }
        }

        public CilEntity Type
        {
            get
            {
                if(!_isTypeInitialized){
                    _isTypeInitialized = true;
                    _type = GetTypeEntity();
                }
                return _type;
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

        private CilEntity GetTypeEntity()
        {
            BlobReader blobReader = _readers.MdReader.GetBlobReader(_typeSpecification.Signature);
            SignatureTypeCode typeCode = blobReader.ReadSignatureTypeCode();

            while (typeCode == SignatureTypeCode.GenericTypeInstance)
            {
                typeCode = blobReader.ReadSignatureTypeCode();
            }

            if (typeCode == SignatureTypeCode.TypeHandle)
            {
                Handle handle = blobReader.ReadTypeHandle();
                return CilDecoder.DecodeEntityHandle((EntityHandle)handle, ref _readers);
            }


            return new CilEntity(null, EntityKind.TypeSpecification);
        }

        private IEnumerable<CilCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _typeSpecification.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }
    }
}
