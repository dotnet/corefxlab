using System.Reflection.Metadata.Cil.Decoder;

namespace System.Reflection.Metadata.Cil
{
    /// <summary>
    /// Struct that represents a parameter object.
    /// </summary>
    public struct CilParameter
    {
        private Parameter _parameter;
        private CilReaders _readers;
        private string _name;
        private string _type;
        private int _sequenceNumber;
        private ParameterAttributes _attributes;
        private bool _isAttributesInitialized;
        private bool _isDefaultValueInitialized;
        private CilConstant _defaultValue;

        internal static CilParameter Create(Parameter parameter, ref CilReaders readers, string type)
        {
            CilParameter param = new CilParameter();
            param._type = type;
            param._readers = readers;
            param._parameter = parameter;
            param._sequenceNumber = -1;
            param._isAttributesInitialized = false;
            param._isDefaultValueInitialized = false;
            return param;
        }

        /// <summary>
        /// Property containing the parameter name.
        /// </summary>
        public string Name
        {
            get
            {
                if(_name == null)
                {
                    _name = CilDecoderHelpers.Instance.NormalizeString(_readers.MdReader.GetString(_parameter.Name));
                }
                return _name;
            }
        }

        /// <summary>
        /// Property containing the parameter type.
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
        }

        public CilConstant DefaultValue
        {
            get
            {
                if(!_isDefaultValueInitialized)
                {
                    if (!HasDefault)
                    {
                        throw new InvalidOperationException("Parameter doesn't have default value");
                    }
                    _isDefaultValueInitialized = true;
                    _defaultValue = GetDefaultValue();
                }
                return _defaultValue;
            }
        }

        public int SequenceNumber
        {
            get
            {
                if(_sequenceNumber == -1)
                {
                    _sequenceNumber = _parameter.SequenceNumber;
                }
                return _sequenceNumber;
            }
        }

        public ParameterAttributes Attributes
        {
            get
            {
                if (!_isAttributesInitialized)
                {
                    _isAttributesInitialized = true;
                    _attributes = _parameter.Attributes;
                }
                return _attributes;
            }
        }

        public bool HasDefault
        {
            get
            {
                return Attributes.HasFlag(ParameterAttributes.HasDefault);
            }
        }

        public bool IsOptional
        {
            get
            {
                return Attributes.HasFlag(ParameterAttributes.Optional);
            }
        }

        public string GetParameterSignature()
        {
            return string.Format("{0}{1} {2}{3}", GetFlags(), Type, GetMarshalAttributes(), Name);
        }

        private string GetMarshalAttributes()
        {
            return string.Empty;
            // TO DO.
            //Need a Marshalling decoder because marshalling descriptor types have diferent specifications.
            //if (!Attributes.HasFlag(ParameterAttributes.HasFieldMarshal))
            //    return string.Empty;
            //StringBuilder sb = new StringBuilder();
            //BlobReader reader = _readers.MdReader.GetBlobReader(_parameter.GetMarshallingDescriptor());
            //var type = SignatureDecoder.DecodeType(ref reader, _readers.Provider);
            //sb.Append("marshal(");
            //sb.Append(type);
            //sb.Append(") ");
            //return sb.ToString();
        }

        private string GetFlags()
        {
            if (Attributes.HasFlag(ParameterAttributes.Optional))
            {
                return "[opt] ";
            }
            if (Attributes.HasFlag(ParameterAttributes.Out))
            {
                return "[out] ";
            }
            if (Attributes.HasFlag(ParameterAttributes.In))
            {
                return "[in] ";
            }
            return string.Empty;
        }

        private CilConstant GetDefaultValue()
        {
            Constant value = _readers.MdReader.GetConstant(_parameter.GetDefaultValue());
            return CilConstant.Create(value, ref _readers);
        }
    }
}
