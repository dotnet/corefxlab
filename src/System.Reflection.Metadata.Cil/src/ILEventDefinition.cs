using ILDasmLibrary.Visitor;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.ILDasm.Decoder;

namespace ILDasmLibrary
{
    public struct ILEventDefinition : IVisitable
    {
        private string _name;
        private Readers _readers;
        private EventDefinition _eventDefinition;
        private ILTypeDefinition _typeDefinition;
        private EventAccessors _accessors;
        private ILMethodDefinition _adder;
        private ILMethodDefinition _remover;
        private ILMethodDefinition _raiser;
        private int _token;
        private ILEntity _type;
        private bool _isAdderInitialized;
        private bool _isRemoverInitialized;
        private bool _isRaiserInitialized;
        private IEnumerable<ILCustomAttribute> _customAttributes;
        private bool _isEntityInitialized;

        internal static ILEventDefinition Create(EventDefinition eventDefinition, int token, ref Readers readers, ILTypeDefinition declaringType)
        {
            ILEventDefinition eventDef = new ILEventDefinition();
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
                return ILDecoder.GetCachedValue(_eventDefinition.Name, _readers, ref _name);
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

        public ILEntity Type
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

        public ILMethodDefinition Adder
        {
            get
            {
                if (!_isAdderInitialized)
                {
                    _isAdderInitialized = true;
                    if (HasAdder)
                    {
                        _adder = ILMethodDefinition.Create(_accessors.Adder, ref _readers, _typeDefinition);
                    }
                }
                return _adder;
            }
        }

        public ILMethodDefinition Remover
        {
            get
            {
                if (!_isRemoverInitialized)
                {
                    _isRemoverInitialized = true;
                    if (HasRemover)
                    {
                        _remover = ILMethodDefinition.Create(_accessors.Remover, ref _readers, _typeDefinition);
                    }
                }
                return _remover;
            }
        }

        public ILMethodDefinition Raiser
        {
            get
            {
                if (!_isRaiserInitialized)
                {
                    _isRaiserInitialized = true;
                    if (HasRaiser)
                    {
                        _raiser = ILMethodDefinition.Create(_accessors.Raiser, ref _readers, _typeDefinition);
                    }
                }
                return _raiser;
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

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        private IEnumerable<ILCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _eventDefinition.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new ILCustomAttribute(attribute, ref _readers);
            }
        }

        private ILEntity GetEntity()
        {
            var handle = _eventDefinition.Type;
            return ILDecoder.DecodeEntityHandle(handle, ref _readers);
        }
    }
}
