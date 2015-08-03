using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.ILDasm.Decoder;

namespace ILDasmLibrary
{
    public struct ILTypeSpecification
    {
        private TypeSpecification _typeSpecification;
        private Readers _readers;
        private IEnumerable<ILCustomAttribute> _customAttributes;
        private ILEntity _type;
        private bool _isTypeInitialized;
        private string _signature;
        private byte[] _blob;

        internal static ILTypeSpecification Create(TypeSpecification typeSpecification, ref Readers readers)
        {
            ILTypeSpecification type = new ILTypeSpecification();
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

        public ILEntity Type
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

        private ILEntity GetTypeEntity()
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
                return ILDecoder.DecodeEntityHandle((EntityHandle)handle, ref _readers);
            }


            return new ILEntity(null, EntityKind.TypeSpecification);
        }

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _typeSpecification.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
            }
        }
    }
}
