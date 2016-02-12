using System.Collections.Generic;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Visitor;
using System.Reflection.Metadata.Decoding;
using System.Text;

namespace System.Reflection.Metadata.Cil
{
    public struct CilField : ICilVisitable
    {
        private FieldDefinition _fieldDefinition;
        private CilTypeDefinition _typeDefinition;
        private CilConstant _defaultValue;
        private IEnumerable<CilCustomAttribute> _customAttributes;
        private CilReaders _readers;
        private string _name;
        private string _type;
        private string _signature;
        private int _token;
        private bool _isDefaultInitialized;

        internal static CilField Create(FieldDefinition fieldDefinition, int token, ref CilReaders readers, CilTypeDefinition typeDefinition)
        {
            CilField field = new CilField();
            field._fieldDefinition = fieldDefinition;
            field._token = token;
            field._readers = readers;
            field._typeDefinition = typeDefinition;
            field._isDefaultInitialized = false;
            return field;
        }

        public string Name
        {
            get
            {
                return CilDecoder.GetCachedValue(_fieldDefinition.Name, _readers, ref _name);
            }
        }

        public string Type
        {
            get
            {
                if(_type == null)
                {
                    _type = SignatureDecoder.DecodeFieldSignature(_fieldDefinition.Signature, _readers.Provider).ToString();
                }
                return _type;
            }
        }

        public FieldAttributes Attributes
        {
            get
            {
                return _fieldDefinition.Attributes;
            }
        }

        public string Signature
        {
            get
            {
                if(_signature == null)
                {
                    _signature = GetSignature();
                }
                return _signature;
            }
        }

        public CilConstant DefaultValue
        {
            get
            {
                if (!HasDefault)
                {
                    throw new InvalidOperationException("Field doesn't have default value");
                }
                if (!_isDefaultInitialized)
                {
                    _isDefaultInitialized = true;
                    _defaultValue = GetDefault();
                }
                return _defaultValue;
            }
        }

        public bool HasDefault
        {
            get
            {
                return Attributes.HasFlag(FieldAttributes.HasDefault);
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

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        private IEnumerable<CilCustomAttribute> GetCustomAttributes()
        {
            var handles = _fieldDefinition.GetCustomAttributes();
            foreach(var handle in handles)
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }

        private string GetSignature()
        {
            return string.Format("{0}{1} {2}{3} {4}", GetAccessibilityFlags(), GetContractFlags(), GetMarshalAttributes(), Type ,Name);
        }

        private CilConstant GetDefault()
        {
            Constant value = _readers.MdReader.GetConstant(_fieldDefinition.GetDefaultValue());
            return CilConstant.Create(value, ref _readers);
        }

        private string GetMarshalAttributes()
        {
            return string.Empty;
            // TO DO.
            //Need a Marshalling decoder because marshalling descriptor types have diferent specifications.
            //if (!Attributes.HasFlag(FieldAttributes.HasFieldMarshal))
            //    return string.Empty;
            //StringBuilder sb = new StringBuilder();
            //BlobReader reader = _readers.MdReader.GetBlobReader(_fieldDefinition.GetMarshallingDescriptor());
            //var type = SignatureDecoder.DecodeType(ref reader, _readers.Provider);
            //sb.Append("marshal(");
            //sb.Append(type);
            //sb.Append(") ");
            //return sb.ToString();
        }

        private string GetContractFlags()
        {
            StringBuilder sb = new StringBuilder();
            if (Attributes.HasFlag(FieldAttributes.Static))
            {
                sb.Append("static ");
            }
            if (Attributes.HasFlag(FieldAttributes.InitOnly))
            {
                sb.Append("initonly ");
            }
            if (Attributes.HasFlag(FieldAttributes.Literal))
            {
                sb.Append("literal ");
            }
            if (Attributes.HasFlag(FieldAttributes.NotSerialized))
            {
                sb.Append("notserialized ");
            }
            if (Attributes.HasFlag(FieldAttributes.RTSpecialName))
            {
                sb.Append("rtspecialname ");
            }
            if (Attributes.HasFlag(FieldAttributes.SpecialName))
            {
                sb.Append("specialname ");
            }
            if(sb.Length > 0)
                sb.Length--; //remove trailing space;
            return sb.ToString();
        }

        private string GetAccessibilityFlags()
        {
            if (Attributes.HasFlag(FieldAttributes.Public))
            {                      
                return "public ";  
            }                      
            if (Attributes.HasFlag(FieldAttributes.FamORAssem))
            {
                return "famorassem ";
            }
            if (Attributes.HasFlag(FieldAttributes.Family))
            {
                return "family ";
            }
            if (Attributes.HasFlag(FieldAttributes.Assembly))
            {
                return "assembly ";
            }
            if (Attributes.HasFlag(FieldAttributes.FamANDAssem))
            {
                return "famandassem ";
            }
            if (Attributes.HasFlag(FieldAttributes.Private))
            {
                return "private ";
            }
            return string.Empty;
        }
    }
}
