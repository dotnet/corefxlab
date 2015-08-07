using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Visitor;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;

namespace System.Reflection.Metadata.Cil
{
    /// <summary>
    /// Class representing a type definition within an assembly.
    /// </summary>
    public struct CilTypeDefinition : ICilVisitable
    {
        private CilReaders _readers;
        private TypeDefinition _typeDefinition;
        private CilTypeLayout _layout;
        private string _name;
        private string _fullName;
        private string _namespace;
        private IEnumerable<CilMethodDefinition> _methodDefinitions;
        private Dictionary<int, int> _methodImplementationDictionary;
        private int _token;
        private IEnumerable<string> _genericParameters;
        private IEnumerable<CilField> _fieldDefinitions;
        private IEnumerable<CilTypeDefinition> _nestedTypes;
        private IEnumerable<CilCustomAttribute> _customAttributes;
        private IEnumerable<CilProperty> _properties;
        private IEnumerable<CilEventDefinition> _events;
        private CilEntity _baseType;
        private bool _isBaseTypeInitialized;
        private bool _isLayoutInitialized;
        private string _signature;

        internal static CilTypeDefinition Create(TypeDefinition typeDef, ref CilReaders readers, int token)
        {
            CilTypeDefinition type = new CilTypeDefinition();
            type._typeDefinition = typeDef;
            type._token = token;
            type._readers = readers;
            type._isBaseTypeInitialized = false;
            type._isLayoutInitialized = false;
            return type;
        }

        #region Public APIs

        /// <summary>
        /// Type full name
        /// </summary>
        public string FullName
        {
            get
            {
                if(_fullName == null)
                {
                    _fullName = SignatureDecoder.DecodeType(MetadataTokens.TypeDefinitionHandle(_token), _readers.Provider, null).ToString(false);
                }
                return _fullName;
            }
        }

        /// <summary>
        /// Property that contains the type name. 
        /// </summary>
        public string Name
        {
            get
            {
                return CilDecoder.GetCachedValue(_typeDefinition.Name, _readers, ref _name);
            }
        }
        
        /// <summary>
        /// Property containing the namespace name. 
        /// </summary>
        public string Namespace
        {
            get
            {
                return CilDecoder.GetCachedValue(_typeDefinition.Namespace, _readers ,ref _namespace);
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

        public bool IsGeneric
        {
            get
            {
                return GenericParameters.Count() != 0;
            }
        }

        public bool IsNested
        {
            get
            {
                return !_typeDefinition.GetDeclaringType().IsNil;
            }
        }

        public bool IsInterface
        {
            get
            {
                return _typeDefinition.BaseType.IsNil;
            }
        }

        public bool HasBaseType
        {
            get
            {
                return !_typeDefinition.BaseType.IsNil;
            }
        }

        public CilEntity BaseType
        {
            get
            {
                if (IsInterface) throw new InvalidOperationException("The type definition is an interface, they don't have a base type");
                if(!_isBaseTypeInitialized)
                {
                    _isBaseTypeInitialized = true;
                    _baseType = CilDecoder.DecodeEntityHandle(_typeDefinition.BaseType, ref _readers);
                }
                return _baseType;
            }
        }

        public TypeAttributes Attributes
        {
            get
            {
                return _typeDefinition.Attributes;
            }
        }

        public CilTypeLayout Layout
        {
            get
            {
                if (!_isLayoutInitialized)
                {
                    _isLayoutInitialized = true;
                    _layout = new CilTypeLayout(_typeDefinition.GetLayout());
                }
                return _layout;
            }
        }

        public IEnumerable<InterfaceImplementation> InterfaceImplementations
        {
            get
            {
                throw new NotImplementedException("not implemented Interface Impl on Type Def");
            }
        }
        
        public IEnumerable<CilEventDefinition> Events
        {
            get
            {
                if(_events == null)
                {
                    _events = GetEvents();
                }
                return _events;
            }
        }

        public IEnumerable<CilProperty> Properties
        {
            get
            {
                if(_properties == null)
                {
                    _properties = GetProperties();
                }
                return _properties;
            }
        }

        public IEnumerable<CilTypeDefinition> NestedTypes
        {
            get
            {
                if(_nestedTypes == null)
                {
                    _nestedTypes = GetNestedTypes();
                }
                return _nestedTypes;
            }
        }

        public IEnumerable<string> GenericParameters
        {
            get
            {
                if(_genericParameters == null)
                {
                    _genericParameters = GetGenericParameters();
                }
                return _genericParameters;
            }
        }
        
        /// <summary>
        /// Property containing all the method definitions within a type. 
        /// </summary>
        public IEnumerable<CilMethodDefinition> MethodDefinitions
        {
            get
            {
                if (_methodDefinitions == null)
                {
                    _methodDefinitions = GetMethodDefinitions();
                }
                return _methodDefinitions;
            }
        }

        public IEnumerable<CilField> FieldDefinitions
        {
            get
            {
                if (_fieldDefinitions == null)
                {
                    _fieldDefinitions = GetFieldDefinitions();
                }
                return _fieldDefinitions;
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

        /// <summary>
        /// Method that returns the token of a method declaration given the method token that overrides it. Returns 0 if the token doesn't represent an overriden method.
        /// </summary>
        /// <param name="methodBodyToken">Token of the method body that overrides a declaration.</param>
        /// <returns>token of the method declaration, 0 if there is no overriding of that method.</returns>
        public int GetOverridenMethodToken(int methodBodyToken)
        {
            int result = 0;
            MethodImplementationDictionary.TryGetValue(methodBodyToken, out result);
            return result;
        }

        public void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

        #endregion

        #region Private Methods
        private IEnumerable<CilMethodDefinition> GetMethodDefinitions()
        {
            var handles = _typeDefinition.GetMethods();
            foreach (var handle in handles)
            {
                var method = _readers.MdReader.GetMethodDefinition(handle);
                yield return CilMethodDefinition.Create(method, MetadataTokens.GetToken(handle), ref _readers, this);
            }
        }
        private void PopulateMethodImplementationDictionary()
        {
            var implementations = _typeDefinition.GetMethodImplementations();
            Dictionary<int, int> dictionary = new Dictionary<int, int>(implementations.Count);
            foreach (var implementationHandle in implementations)
            {
                var implementation = _readers.MdReader.GetMethodImplementation(implementationHandle);
                int declarationToken = MetadataTokens.GetToken(implementation.MethodDeclaration);
                int bodyToken = MetadataTokens.GetToken(implementation.MethodBody);
                dictionary.Add(bodyToken, declarationToken);
            }
            _methodImplementationDictionary = dictionary;
        }
        private IEnumerable<string> GetGenericParameters()
        {
            foreach(var handle in _typeDefinition.GetGenericParameters())
            {
                var parameter = _readers.MdReader.GetGenericParameter(handle);
                yield return _readers.MdReader.GetString(parameter.Name);
            }
        }
        private IEnumerable<CilField> GetFieldDefinitions()
        {
            foreach(var handle in _typeDefinition.GetFields())
            {
                var field = _readers.MdReader.GetFieldDefinition(handle);
                var token = MetadataTokens.GetToken(handle);
                yield return CilField.Create(field, token, ref _readers, this);
            }
        }
        private IEnumerable<CilTypeDefinition> GetNestedTypes()
        {
            foreach(var handle in _typeDefinition.GetNestedTypes())
            {
                if (handle.IsNil)
                {
                    continue;
                }
                var typeDefinition = _readers.MdReader.GetTypeDefinition(handle);
                yield return Create(typeDefinition, ref _readers, MetadataTokens.GetToken(handle));
            }
        }
        private IEnumerable<CilCustomAttribute> GetCustomAttributes()
        {
            foreach(var handle in _typeDefinition.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }
        private IEnumerable<CilProperty> GetProperties()
        {
            foreach(var handle in _typeDefinition.GetProperties())
            {
                var property = _readers.MdReader.GetPropertyDefinition(handle);
                int token = MetadataTokens.GetToken(handle);
                yield return CilProperty.Create(property, token,ref _readers, this);
            }
        }
        private IEnumerable<CilEventDefinition> GetEvents()
        {
            foreach (var handle in _typeDefinition.GetEvents())
            {
                var eventDef = _readers.MdReader.GetEventDefinition(handle);
                yield return CilEventDefinition.Create(eventDef, MetadataTokens.GetToken(handle), ref _readers, this);
            }
        }
        private string GetSignature()
        {
            return string.Empty;
        }

        #endregion

        #region Internal Members

        internal Dictionary<int, int> MethodImplementationDictionary
        {
            get
            {
                if(_methodImplementationDictionary == null)
                {
                   PopulateMethodImplementationDictionary();
                }
                return _methodImplementationDictionary;
            }
        }

        internal int Token
        {
            get
            {
                return _token;
            }
        }

        #endregion
    }
}
