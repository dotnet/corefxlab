using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Instructions;
using System.Reflection.Metadata.Cil.Visitor;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace System.Reflection.Metadata.Cil
{
    /// <summary>
    /// Class representing a method definition in a type whithin an assembly.
    /// </summary>
    public struct CilMethodDefinition : ICilVisitable
    {
        internal CilReaders _readers;
        private MethodDefinition _methodDefinition;
        private CilTypeProvider _provider;
        private MethodBodyBlock _methodBody;
        private CilMethodImport _import;
        private string _name;
        private int _rva;
        private MethodSignature<CilType> _signature;
        private BlobReader _ilReader;
        private ImmutableArray<CilInstruction> _instructions;
        private CilLocal[] _locals;
        private CilParameter[] _parameters;
        private int _token;
        private IEnumerable<CilCustomAttribute> _customAttributes;
        private IEnumerable<string> _genericParameters;
        private CilTypeDefinition _typeDefinition;
        private int _methodDeclarationToken;
        private bool _isIlReaderInitialized;
        private bool _isSignatureInitialized;
        private bool _isImportInitialized;
        private IReadOnlyList<CilExceptionRegion> _exceptionRegions;

        internal static CilMethodDefinition Create(MethodDefinition methodDefinition, int token, ref CilReaders readers, CilTypeDefinition typeDefinition)
        {
            CilMethodDefinition method = new CilMethodDefinition();
            method._methodDefinition = methodDefinition;
            method._token = token;
            method._typeDefinition = typeDefinition;
            method._readers = readers;
            method._provider = readers.Provider;
            method._rva = -1;
            method._methodDeclarationToken = -1;
            method._isIlReaderInitialized = false;
            method._isSignatureInitialized = false;
            method._isImportInitialized = false;
            if(method.RelativeVirtualAddress != 0)
                method._methodBody = method._readers.PEReader.GetMethodBody(method.RelativeVirtualAddress);
            return method;
        }

        internal static CilMethodDefinition Create(MethodDefinitionHandle methodHandle, ref CilReaders readers, CilTypeDefinition typeDefinition)
        {
            MethodDefinition method = readers.MdReader.GetMethodDefinition(methodHandle);
            int token = MetadataTokens.GetToken(methodHandle);
            return Create(method, token, ref readers, typeDefinition);
        }

        #region Internal Properties

        internal MethodBodyBlock MethodBody
        {
            get
            {
                return _methodBody;
            }
        }

        /// <summary>
        /// BlobReader that contains the msil instruction bytes.
        /// </summary>
        internal BlobReader IlReader
        {
            get
            {
                if (!_isIlReaderInitialized)
                {
                    _isIlReaderInitialized = true;
                    _ilReader = MethodBody.GetILReader();
                }
                return _ilReader;
            }
        }

        /// <summary>
        /// Type provider to solve type names and references.
        /// </summary>
        internal CilTypeProvider Provider
        {
            get
            {
                return _provider;
            }
        }

        #endregion

        #region Public APIs
        /// <summary>
        /// Method name
        /// </summary>
        public string Name
        {
            get
            {
                return CilDecoder.GetCachedValue(_methodDefinition.Name, _readers, ref _name);
            }
        }

        public CilTypeDefinition DeclaringType
        {
            get
            {
                return _typeDefinition;
            }
        }

        /// <summary>
        /// Method Relative Virtual Address, if is equal to 0 it is a virtual method and has no body.
        /// </summary>
        public int RelativeVirtualAddress
        {
            get
            {
                if(_rva == -1)
                {
                    _rva = _methodDefinition.RelativeVirtualAddress;
                }
                return _rva;
            }
        }

        /// <summary>
        /// Method Token.
        /// </summary>
        internal int Token
        {
            get
            {
                return _token;
            }
        }

        /// <summary>
        /// Code Size in bytes not including headers.
        /// </summary>
        public int Size
        {
            get
            {
                return IlReader.Length;
            }
        }

        /// <summary>
        /// Method max stack capacity
        /// </summary>
        public int MaxStack
        {
            get
            {
                return MethodBody.MaxStack;
            }
        }

        /// <summary>
        /// Method Signature containing the return type, parameter count and header information.
        /// </summary>
        public MethodSignature<CilType> Signature
        {
            get
            {
                if(!_isSignatureInitialized)
                {
                    _isSignatureInitialized = true;
                    _signature = CilDecoder.DecodeMethodSignature(_methodDefinition, _provider);
                }
                return _signature;
            }
        }

        public bool HasImport
        {
            get
            {
                return Attributes.HasFlag(MethodAttributes.PinvokeImpl);
            }
        }

        public CilMethodImport Import
        {
            get
            {
                if (!_isImportInitialized)
                {
                    _isImportInitialized = true;
                    _import = CilMethodImport.Create(_methodDefinition.GetImport(), ref _readers, _methodDefinition);
                }
                return _import;
            }
        }

        /// <summary>
        /// Boolean to know if it overrides a method definition.
        /// </summary>
        public bool IsImplementation
        {
            get
            {
                return MethodDeclarationToken != 0;
            }
        }

        /// <summary>
        /// Boolean to know if the local variables should be initialized with the "init" prefix on its signature.
        /// </summary>
        public bool LocalVariablesInitialized
        {
            get
            {
                return MethodBody.LocalVariablesInitialized;
            }
        }

        /// <summary>
        /// Boolean to know if the method has a locals declared on its body.
        /// </summary>
        public bool HasLocals
        {
            get
            {
                return !MethodBody.LocalSignature.IsNil;
            }
        }

        /// <summary>
        /// Boolean to know if the current method is the entry point of the assembly.
        /// </summary>
        public bool IsEntryPoint
        {
            get
            {
                return _token == _readers.PEReader.PEHeaders.CorHeader.EntryPointTokenOrRelativeVirtualAddress;
            }
        }

        /// <summary>
        /// Token that represents the token of the method declaration if this method overrides any. 0 if it doesn't override a declaration.
        /// </summary>
        public int MethodDeclarationToken
        {
            get
            {
                if (_methodDeclarationToken == -1)
                {
                    _methodDeclarationToken = _typeDefinition.GetOverridenMethodToken(Token);
                }
                return _methodDeclarationToken;
            }
        }

        /// <summary>
        /// Flags on the method (Calling convention, accesibility flags, etc.
        /// </summary>
        public MethodAttributes Attributes
        {
            get
            {
                return _methodDefinition.Attributes;
            }
        }

        public MethodImplAttributes ImplAttributes
        {
            get
            {
                return _methodDefinition.ImplAttributes;
            }
        }

        /// <summary>
        /// Custom attributes declared on the method body header.
        /// </summary>
        public IEnumerable<CilCustomAttribute> CustomAttributes
        {
            get
            {
                if(_customAttributes == null)
                {
                    _customAttributes = PopulateCustomAttributes();
                }
                return _customAttributes;
            }
        }

        /// <summary>
        /// Exception regions in the method body.
        /// </summary>
        public IReadOnlyList<CilExceptionRegion> ExceptionRegions
        {
            get
            {
                if(_exceptionRegions == null)
                {
                    _exceptionRegions = CilExceptionRegion.CreateRegions(MethodBody.ExceptionRegions);
                }
                return _exceptionRegions;
            }
        }

        /// <summary>
        /// List of instructions that represent the method body.
        /// </summary>
        public ImmutableArray<CilInstruction> Instructions
        {
            get
            {
                if(_instructions == null)
                {
                    _instructions = CilDecoder.DecodeMethodBody(this).ToImmutableArray();
                }
                return _instructions;
            }
        }

        /// <summary>
        /// Parameters that the method take.
        /// </summary>
        public CilParameter[] Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = CilDecoder.DecodeParameters(Signature, _methodDefinition.GetParameters(), ref _readers);
                }
                return _parameters;
            }
        }

        /// <summary>
        /// Locals contained on the method body.
        /// </summary>
        public CilLocal[] Locals
        {
            get
            {
                if (_locals == null)
                {
                    _locals = CilDecoder.DecodeLocalSignature(MethodBody, _readers.MdReader, _provider);
                }
                return _locals;
            }
        }

        /// <summary>
        /// Method generic parameters.
        /// </summary>
        public IEnumerable<string> GenericParameters
        {
            get
            {
                if(_genericParameters == null)
                {
                    _genericParameters = CilDecoder.DecodeGenericParameters(_methodDefinition, this);
                }
                return _genericParameters;
            }
        }

        /// <summary>
        /// Method that given an index returns a local.
        /// 
        /// Exception:
        ///     IndexOutOfBoundsException if the index is greater or equal than the number of locals or less to 0.
        /// </summary>
        /// <param name="index">Index of the local to get</param>
        /// <returns>The local in current index.</returns>
        public CilLocal GetLocal(int index)
        {
            if(index < 0 || index >= Locals.Length)
            {
                throw new IndexOutOfRangeException("Index out of bounds trying to get local");
            }
            return Locals[index];
        }

        /// <summary>
        /// Method that given an index returns a parameter.
        /// 
        /// Exception:
        ///     IndexOutOfBoundsException if the index is greater or equal than the number of locals or less to 0.
        /// </summary>
        /// <param name="index">Index of the parameter to get</param>
        /// <returns>The local in current index.</returns>
        public CilParameter GetParameter(int index)
        {
            if(index < 0 || index >= Parameters.Length)
            {
                throw new IndexOutOfRangeException("Index out of bounds trying to get parameter.");
            }
            return Parameters[index];
        }

        /// <summary>
        /// Method that decodes the method signature as a string with all it's flags, return type, name and parameters.
        /// </summary>
        /// <returns>Returns the method signature as a string</returns>
        public string GetDecodedSignature()
        {
            string attributes = GetAttributesForSignature();
            StringBuilder signature = new StringBuilder();
            if (Signature.Header.IsInstance)
            {
                signature.Append("instance ");
            }
            signature.Append(Signature.ReturnType);
            return String.Format("{0}{1} {2}{3}{4} {5}", attributes, signature.ToString(), Name, GetGenericParametersString(), GetParameterListString(), GetImplementationFlags());
        }
        
        /// <summary>
        /// Method that formats the Relative Virtual Address to it's hexadecimal representation.
        /// </summary>
        /// <returns>String representing the Relative virtual Address in hexadecimal</returns>
        public string GetFormattedRva()
        {
            return string.Format("0x{0:x8}", RelativeVirtualAddress);
        }

        public void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }

        #endregion

        #region Private Methods
        internal IEnumerable<CilParameter> GetOptionalParameters()
        {
            for(int i = 0; i<Parameters.Length; i++)
            {
                var parameter = Parameters[i];
                if (parameter.IsOptional && parameter.HasDefault)
                {
                    yield return parameter;
                }
            }
        }
        private string GetGenericParametersString()
        {
            int i = 0;
            StringBuilder genericParameters = new StringBuilder();
            foreach (var genericParameter in GenericParameters)
            {
                if (i == 0)
                {
                    genericParameters.Append("<");
                }
                genericParameters.Append(genericParameter);
                genericParameters.Append(",");
                i++;
            }
            if (i > 0)
            {
                genericParameters.Length -= 1; //Delete trailing ,
                genericParameters.Append(">");
            }
            return genericParameters.ToString();
        }
        private string GetParameterListString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for(int i = 0; i < Parameters.Length; i++)
            {
                if(i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(Parameters[i].GetParameterSignature());
            }
            sb.Append(")");
            return sb.ToString();
        }
        private IEnumerable<CilCustomAttribute> PopulateCustomAttributes()
        {
            foreach(var handle in _methodDefinition.GetCustomAttributes())
            {
                var attribute = _readers.MdReader.GetCustomAttribute(handle);
                yield return new CilCustomAttribute(attribute, ref _readers);
            }
        }
        private string GetAttributesForSignature()
        {
            return string.Format("{0}{1}", GetAccessibilityFlags(), GetContractFlags());
        }
        private string GetContractFlags()
        {
            StringBuilder sb = new StringBuilder();
            if (Attributes.HasFlag(MethodAttributes.HideBySig))
            {
                sb.Append("hidebysig ");
            }
            if (Attributes.HasFlag(MethodAttributes.Static))
            {
                sb.Append("static ");
            }
            if(Attributes.HasFlag(MethodAttributes.NewSlot))
            {
                sb.Append("newslot ");
            }
            if (Attributes.HasFlag(MethodAttributes.SpecialName))
            {
                sb.Append("specialname ");
            }
            if (Attributes.HasFlag(MethodAttributes.RTSpecialName))
            {
                sb.Append("rtspecialname ");
            }
            if (Attributes.HasFlag(MethodAttributes.Abstract))
            {
                sb.Append("abstract ");
            }
            if (Attributes.HasFlag(MethodAttributes.CheckAccessOnOverride))
            {
                sb.Append("strict ");
            }
            if (Attributes.HasFlag(MethodAttributes.Virtual))
            {
                sb.Append("virtual ");
            }
            if (Attributes.HasFlag(MethodAttributes.Final))
            {
                sb.Append("final ");
            }
            if (Attributes.HasFlag(MethodAttributes.PinvokeImpl))
            {
                sb.Append("pinvokeimpl(");
                sb.Append(Import.GetMethodImportDeclaration());
                sb.Append(") ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// This Method is intended to get the accessibility flags.
        /// Since the enum doesn't have flags values, the smallest values (private, famANDAssem) will always return true.
        /// To solve this we have to check from the greatest through the smallest and the first flag it finds that way we always find the desired value.
        /// </summary>
        /// <returns>
        /// The accesibility flag as a string.
        /// </returns>
        private string GetAccessibilityFlags()
        {
            if (Attributes.HasFlag(MethodAttributes.Public))
            {
                return "public ";
            }
            if (Attributes.HasFlag(MethodAttributes.FamORAssem))
            {
                return "famorassem ";
            }
            if (Attributes.HasFlag(MethodAttributes.Family))
            {
                return "family ";
            }
            if (Attributes.HasFlag(MethodAttributes.Assembly))
            {
                return "assembly ";
            }
            if (Attributes.HasFlag(MethodAttributes.FamANDAssem))
            {
                return "famandassem ";
            }
            if (Attributes.HasFlag(MethodAttributes.Private))
            {
                return "private ";
            }
            return string.Empty;
        }

        private string GetImplementationFlags()
        {
            return string.Format("{0} {1}", GetCodeType(), GetCodeManagement());
        }

        private string GetCodeType()
        {
            if (ImplAttributes.HasFlag(MethodImplAttributes.Runtime))
            {
                return "runtime";
            }
            if (ImplAttributes.HasFlag(MethodImplAttributes.OPTIL))
            {
                return "optil";
            }
            if (ImplAttributes.HasFlag(MethodImplAttributes.Native))
            {
                return "native";
            }
            if (ImplAttributes.HasFlag(MethodImplAttributes.IL))
            {
                return "cil";
            }
            throw new BadImageFormatException("Invalid Code Type flag");
        }

        private string GetCodeManagement()
        {
            if (ImplAttributes.HasFlag(MethodImplAttributes.Managed))
            {
                return "managed";
            }
            if (ImplAttributes.HasFlag(MethodImplAttributes.Unmanaged))
            {
                return "unmanaged";
            }
            throw new BadImageFormatException("Invalid code management flag");
        }
        #endregion
    }
}