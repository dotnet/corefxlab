using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection.Metadata.Decoding;
using System.Text;

namespace System.Reflection.Metadata.Cil.Decoder
{
    /* 
        TO DO: Change Signature decoder, to decode type with byte indicating whether should have the class or valutype prefix or not.
    */
    public struct CilTypeProvider : ISignatureTypeProvider<CilType>
    {
        private readonly MetadataReader _reader;

        public MetadataReader Reader
        {
            get
            {
                return _reader;
            }
        }

        public CilTypeProvider(MetadataReader reader)
        {
            _reader = reader;
        }

#region Public APIs

        public CilType GetArrayType(CilType elementType, ArrayShape shape)
        {
            elementType.Append("[");
            for(int i = 0; i < shape.Rank; i++)
            {
                int lowerBound = 0;
                if(i < shape.LowerBounds.Length)
                {
                    lowerBound = shape.LowerBounds[i];
                    elementType.Append(lowerBound.ToString());
                    elementType.Append("...");
                }

                if(i < shape.Sizes.Length)
                {
                    elementType.Append((lowerBound + shape.Sizes[i] - 1).ToString());
                }

                if( i < shape.Rank -1)
                {
                    elementType.Append(",");
                }
            }
            elementType.Append("]");
            return elementType;
        }

        public CilType GetByReferenceType(CilType elementType)
        {
            elementType.Append("&");
            return elementType;
        }

        public CilType GetFunctionPointerType(MethodSignature<CilType> signature)
        {
            CilType type = new CilType("method ", false, false);
            type.Append(signature.ReturnType.ToString());
            type.Append("*");
            type.Append(GetParameterList(signature));
            return type;
        }

        public CilType GetGenericInstance(CilType genericType, ImmutableArray<CilType> typeArguments)
        {
            genericType.Append("<");
            for(int i = 0; i < typeArguments.Length; i++)
            {
                var typeArgument = typeArguments[i];
                genericType.Append(typeArgument.ToString());
                if(i < typeArguments.Length - 1)
                {
                    genericType.Append(",");
                }
            }
            genericType.Append(">");
            return genericType;
        }

        public CilType GetGenericMethodParameter(int index)
        {
            var type = new CilType("", false, false);
            type.Append("!!");
            type.Append(index.ToString());
            return type;
        }

        public CilType GetGenericTypeParameter(int index)
        {
            var type = new CilType("", false, false);
            type.Append("!");
            type.Append(index.ToString());
            return type;
        }

        public CilType GetModifiedType(MetadataReader reader, bool isRequired, CilType modifier, CilType unmodifiedType)
        {
            unmodifiedType.Append(isRequired ? " modreq(" : " modopt(");
            unmodifiedType.Append(modifier.ToString());
            unmodifiedType.Append(")");
            return unmodifiedType;
        }

        public CilType GetPinnedType(CilType elementType)
        {
            elementType.Append(" pinned");
            return elementType;
        }

        public CilType GetPointerType(CilType elementType)
        {
            elementType.Append("*");
            return elementType;
        }

        public CilType GetPrimitiveType(PrimitiveTypeCode typeCode)
        {
            string str;
            switch (typeCode)
            {
                case PrimitiveTypeCode.Boolean:
                    str = "bool";
                    break;
                case PrimitiveTypeCode.Byte:
                    str = "uint8";
                    break;
                case PrimitiveTypeCode.SByte:
                    str = "int8";
                    break;
                case PrimitiveTypeCode.Char:
                    str = "char";
                    break;
                case PrimitiveTypeCode.Single:
                    str = "float32";
                    break;
                case PrimitiveTypeCode.Double:
                    str = "float64";
                    break;
                case PrimitiveTypeCode.Int16:
                    str = "int16";
                    break;
                case PrimitiveTypeCode.Int32:
                    str = "int32";
                    break;
                case PrimitiveTypeCode.Int64:
                    str = "int64";
                    break;
                case PrimitiveTypeCode.UInt16:
                    str = "uint16";
                    break;
                case PrimitiveTypeCode.UInt32:
                    str = "uint32";
                    break;
                case PrimitiveTypeCode.UInt64:
                    str = "uint64";
                    break;
                case PrimitiveTypeCode.IntPtr:
                    str = "native int";
                    break;
                case PrimitiveTypeCode.UIntPtr:
                    str = "native uint";
                    break;
                case PrimitiveTypeCode.Object:
                    str = "object";
                    break;
                case PrimitiveTypeCode.String:
                    str = "string";
                    break;
                case PrimitiveTypeCode.TypedReference:
                    str = "typedref";
                    break;
                case PrimitiveTypeCode.Void:
                    str = "void";
                    break;
                default:
                    Debug.Assert(false);
                    throw new ArgumentOutOfRangeException("invalid typeCode");
            }
            return new CilType(str, false, false);
        }

        public CilType GetSZArrayType(CilType elementType)
        {
            elementType.Append("[]");
            return elementType;
        }

        public CilType GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, SignatureTypeHandleCode code)
        {
            bool _isValueType = (code == SignatureTypeHandleCode.Class);
            bool _isClass = (code == SignatureTypeHandleCode.ValueType);

            CilType type = new CilType(GetFullName(Reader.GetTypeDefinition(handle)), _isValueType, _isClass);
            return type;
        }

        public CilType GetTypeFromReference(MetadataReader reader, TypeReferenceHandle handle, SignatureTypeHandleCode code)
        {
            bool _isValueType = (code == SignatureTypeHandleCode.Class);
            bool _isClass = (code == SignatureTypeHandleCode.ValueType);

            CilType type = new CilType(GetFullName(Reader.GetTypeReference(handle)), _isValueType, _isClass);
            return type;
        }

        public CilType GetTypeFromSpecification(MetadataReader reader, TypeSpecificationHandle handle, SignatureTypeHandleCode code)
        {
            return Reader.GetTypeSpecification(handle).DecodeSignature(this);
        }

        public string GetParameterList(MethodSignature<CilType> signature, ParameterHandleCollection? parameters = null)
        {
            ImmutableArray<CilType> types = signature.ParameterTypes;
            if (types.IsEmpty)
            {
                return "()";
            }
            int requiredCount = Math.Min(signature.RequiredParameterCount, types.Length);
            string[] parameterNames = GetParameterNames(parameters, requiredCount);
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            int i = 0;
            for (; i < requiredCount; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(types[i].ToString());
                if (parameterNames != null)
                {
                    sb.AppendFormat(" {0}", CilDecoderHelpers.Instance.NormalizeString(parameterNames[i]));
                }
            }

            if (i < types.Length)
            {
                sb.Append("...,");
            }
            for (; i < types.Length; i++)
            {
                sb.Append(types[i].ToString());
                if(i < types.Length -1)
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }

#endregion

#region Private helper methods

        internal string[] GetParameterNames(ParameterHandleCollection? parameters, int requiredCount)
        {
            if(parameters == null || requiredCount == 0)
            {
                return null;
            }
            string[] parameterNames = new string[requiredCount];
            foreach(var handle in parameters)
            {
                Parameter parameter = Reader.GetParameter(handle);
                if(parameter.SequenceNumber > 0 && parameter.SequenceNumber <= requiredCount)
                {
                    parameterNames[parameter.SequenceNumber - 1] = Reader.GetString(parameter.Name);
                }
            }
            return parameterNames;
        }

        private string GetName(TypeReference reference)
        {
            if (reference.Namespace.IsNil)
            {
                return Reader.GetString(reference.Name);
            }
            return String.Format("{0}.{1}", Reader.GetString(reference.Namespace), Reader.GetString(reference.Name));
        }

        private string GetName(TypeDefinition type)
        {
            if (type.Namespace.IsNil)
            {
                return Reader.GetString(type.Name);
            }
            return String.Format("{0}.{1}", Reader.GetString(type.Namespace), Reader.GetString(type.Name));
        }

        private string GetFullName(TypeDefinitionHandle handle)
        {
            return GetFullName(Reader.GetTypeDefinition(handle));
        }

        private string GetFullName(TypeDefinition type)
        {
            var declaringType = type.GetDeclaringType();

            if (declaringType.IsNil)
            {
                return GetName(type);
            }
            return String.Format("{0}/{1}", GetFullName(declaringType), GetName(type));
        }

        private string GetFullName(TypeReferenceHandle handle)
        {
            return GetFullName(Reader.GetTypeReference(handle));
        }

        private string GetFullName(TypeReference reference)
        {
            Handle resolutionScope = reference.ResolutionScope;
            string name = GetName(reference);
            switch (resolutionScope.Kind)
            {
                case HandleKind.ModuleReference:
                    return String.Format("[.module {0}]{1}", Reader.GetString(Reader.GetModuleReference((ModuleReferenceHandle)resolutionScope).Name), name);
                case HandleKind.AssemblyReference:
                    return String.Format("[{0}]{1}", Reader.GetString(Reader.GetAssemblyReference((AssemblyReferenceHandle)resolutionScope).Name), name);
                case HandleKind.TypeReference:
                    return String.Format("{0}/{1}", GetFullName((TypeReferenceHandle)resolutionScope), name);
                case HandleKind.TypeSpecification:
                    throw new ArgumentException("Check this type");
                default:
                    return name;
            }
        }

#endregion
    }
}
