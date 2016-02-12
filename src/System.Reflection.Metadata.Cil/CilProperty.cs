using System.Collections.Generic;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Visitor;
using System.Reflection.Metadata.Decoding;
using System.Text;

namespace System.Reflection.Metadata.Cil
{
    public struct CilProperty : ICilVisitable
    {
        private PropertyDefinition _propertyDef;
        private CilReaders _readers;
        private string _name;
        private MethodSignature<CilType> _signature;
        private IEnumerable<CilCustomAttribute> _customAttributes;
        private CilMethodDefinition _getter;
        private CilMethodDefinition _setter;
        private CilConstant _defaultValue;
        private bool _isDefaultValueInitialized;
        private bool _isGetterInitialized;
        private bool _isSetterInitialized;
        private PropertyAccessors _accessors;
        private bool _isSignatureInitialized;
        private CilTypeDefinition _typeDefinition;
        private int _token;

        internal static CilProperty Create(PropertyDefinition propertyDef, int token, ref CilReaders readers, CilTypeDefinition typeDefinition)
        {
            CilProperty property = new CilProperty();
            property._typeDefinition = typeDefinition;
            property._propertyDef = propertyDef;
            property._readers = readers;
            property._isSignatureInitialized = false;
            property._isDefaultValueInitialized = false;
            property._isGetterInitialized = false;
            property._isSetterInitialized = false;
            property._token = token;
            property._accessors = propertyDef.GetAccessors();
            return property;
        }

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        public CilTypeDefinition DeclaringType
        {
            get
            {
                return _typeDefinition;
            }
        }

        public string Name
        {
            get
            {
                return CilDecoder.GetCachedValue(_propertyDef.Name, _readers, ref _name);
            }
        }

        public bool HasGetter
        {
            get
            {
                return !_accessors.Getter.IsNil;
            }
        }

        public bool HasSetter
        {
            get
            {
                return !_accessors.Setter.IsNil;
            }
        }

        public bool HasDefault
        {
            get
            {
                return Attributes.HasFlag(PropertyAttributes.HasDefault);
            }
        }

        public CilConstant DefaultValue
        {
            get
            {
                if (!_isDefaultValueInitialized)
                {
                    if (!HasDefault)
                    {
                        throw new InvalidOperationException("Property doesn't have default value");
                    }
                    _isDefaultValueInitialized = true;
                    _defaultValue = GetDefaultValue();
                }
                return _defaultValue;
            }
        }

        public CilMethodDefinition Getter
        {
            get
            {
                if (!_isGetterInitialized)
                {
                    _isGetterInitialized = true;
                    if (HasGetter)
                    {
                        _getter = CilMethodDefinition.Create(_accessors.Getter, ref _readers, _typeDefinition);
                    }
                }
                return _getter;
            }
        }

        public CilMethodDefinition Setter
        {
            get
            {
                if (!_isSetterInitialized)
                {
                    _isSetterInitialized = true;
                    if (HasSetter)
                    {
                        _setter = CilMethodDefinition.Create(_accessors.Setter, ref _readers, _typeDefinition);
                    }
                }
                return _setter;
            }
        }

        public MethodSignature<CilType> Signature
        {
            get
            {
                if (!_isSignatureInitialized)
                {
                    _isSignatureInitialized = true;
                    _signature = SignatureDecoder.DecodeMethodSignature(_propertyDef.Signature, _readers.Provider);
                }
                return _signature;
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

        public PropertyAttributes Attributes
        {
            get
            {
                return _propertyDef.Attributes;
            }
        }

        public void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetDecodedSignature()
        {
            string attributes = GetAttributesForSignature();
            StringBuilder signature = new StringBuilder();
            if (Signature.Header.IsInstance)
            {
                signature.Append("instance ");
            }
            signature.Append(Signature.ReturnType);
            return string.Format("{0}{1} {2}{3}", attributes, signature.ToString(), Name, CilDecoder.DecodeSignatureParamerTypes(Signature));
        }

        private IEnumerable<CilCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _propertyDef.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }

        private string GetAttributesForSignature()
        {
            if (Attributes.HasFlag(PropertyAttributes.SpecialName))
            {
                return "specialname ";
            }
            if (Attributes.HasFlag(PropertyAttributes.RTSpecialName))
            {
                return "rtspecialname ";
            }
            return string.Empty;
        }

        private CilConstant GetDefaultValue()
        {
            Constant constant = _readers.MdReader.GetConstant(_propertyDef.GetDefaultValue());
            return CilConstant.Create(constant, ref _readers);
        }

    }
}
