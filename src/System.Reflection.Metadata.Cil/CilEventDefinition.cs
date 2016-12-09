using System.Collections.Generic;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil
{
    public struct CilEventDefinition : ICilVisitable
    {
        private string _name;
        private CilReaders _readers;
        private EventDefinition _eventDefinition;
        private CilTypeDefinition _typeDefinition;
        private EventAccessors _accessors;
        private CilMethodDefinition _adder;
        private CilMethodDefinition _remover;
        private CilMethodDefinition _raiser;
        private int _token;
        private CilEntity _type;
        private bool _isAdderInitialized;
        private bool _isRemoverInitialized;
        private bool _isRaiserInitialized;
        private IEnumerable<CilCustomAttribute> _customAttributes;
        private bool _isEntityInitialized;

        internal static CilEventDefinition Create(EventDefinition eventDefinition, int token, ref CilReaders readers, CilTypeDefinition declaringType)
        {
            CilEventDefinition eventDef = new CilEventDefinition();
            eventDef._eventDefinition = eventDefinition;
            eventDef._readers = readers;
            eventDef._typeDefinition = declaringType;
            eventDef._accessors = eventDefinition.GetAccessors();
            eventDef._isAdderInitialized = false;
            eventDef._isRemoverInitialized = false;
            eventDef._isRaiserInitialized = false;
            eventDef._isEntityInitialized = false;
            eventDef._token = token;
            return eventDef;
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
                return CilDecoder.GetCachedValue(_eventDefinition.Name, _readers, ref _name);
            }
        }

        public bool HasAdder
        {
            get
            {
                return !_accessors.Adder.IsNil;
            }
        }

        public bool HasRemover
        {
            get
            {
                return !_accessors.Remover.IsNil;
            }
        }

        public bool HasRaiser
        {
            get
            {
                return !_accessors.Raiser.IsNil;
            }
        }

        public CilEntity Type
        {
            get
            {
                if (!_isEntityInitialized)
                {
                    _isEntityInitialized = true;
                    _type = GetEntity();
                }
                return _type;
            }
        }

        public CilMethodDefinition Adder
        {
            get
            {
                if (!_isAdderInitialized)
                {
                    _isAdderInitialized = true;
                    if (HasAdder)
                    {
                        _adder = CilMethodDefinition.Create(_accessors.Adder, ref _readers, _typeDefinition);
                    }
                }
                return _adder;
            }
        }

        public CilMethodDefinition Remover
        {
            get
            {
                if (!_isRemoverInitialized)
                {
                    _isRemoverInitialized = true;
                    if (HasRemover)
                    {
                        _remover = CilMethodDefinition.Create(_accessors.Remover, ref _readers, _typeDefinition);
                    }
                }
                return _remover;
            }
        }

        public CilMethodDefinition Raiser
        {
            get
            {
                if (!_isRaiserInitialized)
                {
                    _isRaiserInitialized = true;
                    if (HasRaiser)
                    {
                        _raiser = CilMethodDefinition.Create(_accessors.Raiser, ref _readers, _typeDefinition);
                    }
                }
                return _raiser;
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

        private IEnumerable<CilCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _eventDefinition.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }

        private CilEntity GetEntity()
        {
            var handle = _eventDefinition.Type;
            return CilDecoder.DecodeEntityHandle(handle, ref _readers);
        }
    }
}
