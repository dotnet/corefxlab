using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Cil.Instructions;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace System.Reflection.Metadata.Cil.Decoder
{
    public struct CilDecoder
    {
        #region Public APIs

        /// <summary>
        /// Method that given a token defines if it is a type reference token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type reference false if not</returns>
        public static bool IsTypeReference(int token)
        {
            return GetTokenType(token) == CilTokenType.TypeReference;
        }

        /// <summary>
        /// Method that given a token defines if it is a type definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type definition false if not</returns>
        public static bool IsTypeDefinition(int token)
        {
            return GetTokenType(token) == CilTokenType.TypeDefinition;
        }

        /// <summary>
        /// Method that given a token defines if it is a user string token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a user string false if not</returns>
        public static bool IsUserString(int token)
        {
            return GetTokenType(token) == CilTokenType.UserString;
        }

        /// <summary>
        /// Method that given a token defines if it is a member reference token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a member reference false if not</returns>
        public static bool IsMemberReference(int token)
        {
            return GetTokenType(token) == CilTokenType.MemberReference;
        }

        /// <summary>
        /// Method that given a token defines if it is a method specification token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a method specification false if not</returns>
        public static bool IsMethodSpecification(int token)
        {
            return GetTokenType(token) == CilTokenType.MethodSpecification;
        }

        /// <summary>
        /// Method that given a token defines if it is a method definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a method definition false if not</returns>
        public static bool IsMethodDefinition(int token)
        {
            return GetTokenType(token) == CilTokenType.MethodDefinition;
        }

        /// <summary>
        /// Method that given a token defines if it is a field definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a field definition false if not</returns>
        public static bool IsFieldDefinition(int token)
        {
            return GetTokenType(token) == CilTokenType.FieldDefinition;
        }

        /// <summary>
        /// Method that given a token defines if it is a type specification token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type specification false if not</returns>
        public static bool IsTypeSpecification(int token)
        {
            return GetTokenType(token) == CilTokenType.TypeSpecification;
        }

        /// <summary>
        /// Method that given a token defines if it is a standalone signature token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a standalone signature false if not</returns>
        public static bool IsStandaloneSignature(int token)
        {
            return GetTokenType(token) == CilTokenType.Signature;
        }

        public static string CreateByteArrayString(byte[] array)
        {
            if (array.Length == 0) return "()";
            char[] chars = new char[3 + 3 * array.Length]; //3 equals for parenthesis and first space, 3 for 1 (space) + 2(byte).
            int i = 0;
            chars[i++] = '(';
            chars[i++] = ' ';
            foreach(byte b in array)
            {
                chars[i++] = HexadecimalNibble((byte)(b >> 4)); //get every byte char in Hex X2 representation.
                chars[i++] = HexadecimalNibble((byte)(b & 0xF));
                chars[i++] = ' ';
            }

            chars[i++] = ')';
            return new string(chars);
        }

        

        public static string CreateVersionString(Version version)
        {
            int build = version.Build;
            int revision = version.Revision;
            if (build == -1) build = 0;
            if (revision == -1) revision = 0;
            return String.Format("{0}:{1}:{2}:{3}", version.Major.ToString(), version.Minor.ToString(), build.ToString(), revision.ToString());
        }

        public static string DecodeSignatureParamerTypes(MethodSignature<CilType> signature)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            var types = signature.ParameterTypes;
            for (int i = 0; i < types.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(',');
                }
                sb.Append(types[i].ToString());
            }
            sb.Append(')');
            return sb.ToString();
        }

        internal static CilEntity DecodeEntityHandle(EntityHandle handle, ref CilReaders readers)
        {
            if (handle.Kind == HandleKind.TypeDefinition)
            {
                TypeDefinition definition = readers.MdReader.GetTypeDefinition((TypeDefinitionHandle)handle);
                int token = MetadataTokens.GetToken(handle);
                CilTypeDefinition type = CilTypeDefinition.Create(definition, ref readers, token);
                return new CilEntity(type, EntityKind.TypeDefinition);
            }
            if (handle.Kind == HandleKind.TypeSpecification)
            {
                TypeSpecification specification = readers.MdReader.GetTypeSpecification((TypeSpecificationHandle)handle);
                CilTypeSpecification type = CilTypeSpecification.Create(specification, ref readers);
                return new CilEntity(type, EntityKind.TypeSpecification);
            }
            if (handle.Kind == HandleKind.TypeReference)
            {
                TypeReference reference = readers.MdReader.GetTypeReference((TypeReferenceHandle)handle);
                int token = MetadataTokens.GetToken(handle);
                CilTypeReference type = CilTypeReference.Create(reference, ref readers, token);
                return new CilEntity(type, EntityKind.TypeReference);
            }
            throw new BadImageFormatException("Event definition type must be either type reference, type definition or type specification");
        }

        public static MethodSignature<CilType> DecodeMethodSignature(MethodDefinition methodDefinition, CilTypeProvider provider)
        {
            return SignatureDecoder.DecodeMethodSignature(methodDefinition.Signature, provider);
        }

        public static IEnumerable<CilInstruction> DecodeMethodBody(CilMethodDefinition methodDefinition)
        {
            return DecodeMethodBody(methodDefinition.IlReader, methodDefinition._readers.MdReader, methodDefinition.Provider, methodDefinition);
        }

        #endregion

        #region Private and internal helpers

        internal static bool HasArgument(OpCode opCode)
        {
            bool isLoad = opCode == OpCodes.Ldarg || opCode == OpCodes.Ldarga || opCode == OpCodes.Ldarga_S || opCode == OpCodes.Ldarg_S;
            bool isStore = opCode == OpCodes.Starg_S || opCode == OpCodes.Starg;
            return isLoad || isStore;
        }

        private static IEnumerable<CilInstruction> DecodeMethodBody(BlobReader ilReader, MetadataReader metadataReader, CilTypeProvider provider, CilMethodDefinition methodDefinition)
        {
            ilReader.Reset();
            int intOperand;
            ushort shortOperand;
            int ilOffset = 0;
            CilInstruction instruction = null;
            while (ilReader.Offset < ilReader.Length)
            {
                OpCode opCode;
                int expectedSize;
                byte _byte = ilReader.ReadByte();
                /*If the byte read is 0xfe it means is a two byte instruction, 
                so since it is going to read the second byte to get the actual
                instruction it has to check that the offset is still less than the length.*/
                if (_byte == 0xfe && ilReader.Offset < ilReader.Length)
                {
                    opCode = CilDecoderHelpers.Instance.twoByteOpCodes[ilReader.ReadByte()];
                    expectedSize = 2;
                }
                else
                {
                    opCode = CilDecoderHelpers.Instance.oneByteOpCodes[_byte];
                    expectedSize = 1;
                }
                switch (opCode.OperandType)
                {
                    //The instruction size is the expected size (1 or 2 depending if it is a one or two byte instruction) + the size of the operand.
                    case OperandType.InlineField:
                        intOperand = ilReader.ReadInt32();
                        string fieldInfo = GetFieldInformation(metadataReader, intOperand, provider);
                        instruction = new CilStringInstruction(opCode, fieldInfo, intOperand, expectedSize + (int)CilInstructionSize.Int32);
                        break;
                    case OperandType.InlineString:
                        intOperand = ilReader.ReadInt32();
                        bool isPrintable;
                        string str = GetArgumentString(metadataReader, intOperand, out isPrintable);
                        instruction = new CilStringInstruction(opCode, str, intOperand, expectedSize + (int)CilInstructionSize.Int32, isPrintable);
                        break;
                    case OperandType.InlineMethod:
                        intOperand = ilReader.ReadInt32();
                        string methodCall = SolveMethodName(metadataReader, intOperand, provider);
                        instruction = new CilStringInstruction(opCode, methodCall, intOperand, expectedSize + (int)CilInstructionSize.Int32);
                        break;
                    case OperandType.InlineType:
                        intOperand = ilReader.ReadInt32();
                        string type = GetTypeInformation(metadataReader, intOperand, provider);
                        instruction = new CilStringInstruction(opCode, type, intOperand, expectedSize + (int)CilInstructionSize.Int32);
                        break;
                    case OperandType.InlineTok:
                        intOperand = ilReader.ReadInt32();
                        string tokenType = GetInlineTokenType(metadataReader, intOperand, provider);
                        instruction = new CilStringInstruction(opCode, tokenType, intOperand, expectedSize + (int)CilInstructionSize.Int32);
                        break;
                    case OperandType.InlineI:
                        instruction = new CilInt32Instruction(opCode, ilReader.ReadInt32(), -1, expectedSize + (int)CilInstructionSize.Int32);
                        break;
                    case OperandType.InlineI8:
                        instruction = new CilInt64Instruction(opCode, ilReader.ReadInt64(), -1, expectedSize + (int)CilInstructionSize.Int64);
                        break;
                    case OperandType.InlineR:
                        instruction = new CilDoubleInstruction(opCode, ilReader.ReadDouble(), -1, expectedSize + (int)CilInstructionSize.Double);
                        break;
                    case OperandType.InlineSwitch:
                        instruction = CreateSwitchInstruction(ref ilReader, expectedSize, ilOffset, opCode);
                        break;
                    case OperandType.ShortInlineBrTarget:
                        instruction = new CilInt16BranchInstruction(opCode, ilReader.ReadSByte(), ilOffset, expectedSize + (int)CilInstructionSize.Byte);
                        break;
                    case OperandType.InlineBrTarget:
                        instruction = new CilBranchInstruction(opCode, ilReader.ReadInt32(), ilOffset, expectedSize + (int)CilInstructionSize.Int32);
                        break;
                    case OperandType.ShortInlineI:
                        instruction = new CilByteInstruction(opCode, ilReader.ReadByte(), -1, expectedSize + (int)CilInstructionSize.Byte);
                        break;
                    case OperandType.ShortInlineR:
                        instruction = new CilSingleInstruction(opCode, ilReader.ReadSingle(), -1, expectedSize + (int)CilInstructionSize.Single);
                        break;
                    case OperandType.InlineNone:
                        instruction = new CilInstructionWithNoValue(opCode, expectedSize);
                        break;
                    case OperandType.ShortInlineVar:
                        byte token = ilReader.ReadByte();
                        instruction = new CilInt16VariableInstruction(opCode, GetVariableName(opCode, token, methodDefinition), token, expectedSize + (int)CilInstructionSize.Byte);
                        break;
                    case OperandType.InlineVar:
                        shortOperand = ilReader.ReadUInt16();
                        instruction = new CilVariableInstruction(opCode, GetVariableName(opCode, shortOperand, methodDefinition), shortOperand, expectedSize + (int)CilInstructionSize.Int16);
                        break;
                    case OperandType.InlineSig:
                        intOperand = ilReader.ReadInt32();
                        instruction = new CilStringInstruction(opCode, GetSignature(metadataReader, intOperand, provider), intOperand, expectedSize + (int)CilInstructionSize.Int32);
                        break;
                    default:
                        break;
                }
                ilOffset += instruction.Size;
                yield return instruction;
            }
        }

        internal static CilLocal[] DecodeLocalSignature(MethodBodyBlock methodBody, MetadataReader metadataReader, CilTypeProvider provider)
        {
            if (methodBody.LocalSignature.IsNil)
            {
                return new CilLocal[0];
            }
            ImmutableArray<CilType> localTypes = SignatureDecoder.DecodeLocalSignature(methodBody.LocalSignature, provider);
            CilLocal[] locals = new CilLocal[localTypes.Length];
            for (int i = 0; i < localTypes.Length; i++)
            {
                string name = "V_" + i;
                locals[i] = new CilLocal(name, localTypes[i].ToString());
            }
            return locals;
        }

        internal static CilParameter[] DecodeParameters(MethodSignature<CilType> signature, ParameterHandleCollection parameters, ref CilReaders readers)
        {
            ImmutableArray<CilType> types = signature.ParameterTypes;
            int requiredCount = Math.Min(signature.RequiredParameterCount, types.Length);
            if (requiredCount == 0)
            {
                return new CilParameter[0];
            }
            CilParameter[] result = new CilParameter[requiredCount];
            for (int i = 0; i < requiredCount; i++)
            {
                var parameter = readers.MdReader.GetParameter(parameters.ElementAt(i));
                result[i] = CilParameter.Create(parameter, ref readers, types[i].ToString());
            }
            return result;
        }

        internal static IEnumerable<string> DecodeGenericParameters(MethodDefinition methodDefinition, CilMethodDefinition method)
        {
            foreach (var handle in methodDefinition.GetGenericParameters())
            {
                var parameter = method._readers.MdReader.GetGenericParameter(handle);
                yield return method._readers.MdReader.GetString(parameter.Name);
            }
        }

        internal static CilType DecodeType(EntityHandle type, CilTypeProvider provider)
        {
            return SignatureDecoder.DecodeType(type, provider, null);
        }

        private static string GetSignature(MetadataReader metadataReader, int intOperand, CilTypeProvider provider)
        {
            if (IsStandaloneSignature(intOperand))
            {
                var handle = MetadataTokens.StandaloneSignatureHandle(intOperand);
                var standaloneSignature = metadataReader.GetStandaloneSignature(handle);
                var signature = SignatureDecoder.DecodeMethodSignature(standaloneSignature.Signature, provider);
                return string.Format("{0}{1}", GetMethodReturnType(signature), provider.GetParameterList(signature));
            }
            throw new ArgumentException("Get signature invalid token");
        }

        private static string GetVariableName(OpCode opCode, int token, CilMethodDefinition methodDefinition)
        {
            if (HasArgument(opCode))
            {
                if (methodDefinition.Signature.Header.IsInstance)
                {
                    token--; //the first parameter is "this".
                }
                return methodDefinition.GetParameter(token).Name;
            }
            return methodDefinition.GetLocal(token).Name;
            
        }

        private static string GetInlineTokenType(MetadataReader metadataReader, int intOperand, CilTypeProvider provider)
        {
            if (IsMethodDefinition(intOperand) || IsMethodSpecification(intOperand) || IsMemberReference(intOperand))
            {
                return "method " + SolveMethodName(metadataReader, intOperand, provider);
            }
            if (IsFieldDefinition(intOperand))
            {
                return "field " + GetFieldInformation(metadataReader, intOperand, provider);
            }
            return GetTypeInformation(metadataReader, intOperand, provider);
        }

        private static string GetTypeInformation(MetadataReader metadataReader, int intOperand, CilTypeProvider provider)
        {
            if (IsTypeReference(intOperand))
            {
                var refHandle = MetadataTokens.TypeReferenceHandle(intOperand);
                return SignatureDecoder.DecodeType(refHandle, provider, null).ToString();
            }
            if (IsTypeSpecification(intOperand))
            {
                var typeHandle = MetadataTokens.TypeSpecificationHandle(intOperand);
                return SignatureDecoder.DecodeType(typeHandle, provider, null).ToString();
            }
            var defHandle = MetadataTokens.TypeDefinitionHandle(intOperand);
            return SignatureDecoder.DecodeType(defHandle, provider, null).ToString();
        }

        private static CilInstruction CreateSwitchInstruction(ref BlobReader ilReader, int expectedSize, int ilOffset, OpCode opCode)
        {
            uint caseNumber = ilReader.ReadUInt32();
            int[] jumps = new int[caseNumber];
            for (int i = 0; i < caseNumber; i++)
            {
                jumps[i] = ilReader.ReadInt32();
            }
            int size = (int)CilInstructionSize.Int32 + expectedSize;
            size += (int)caseNumber * (int)CilInstructionSize.Int32;
            return new CilSwitchInstruction(opCode, ilOffset, jumps, (int)caseNumber, caseNumber, size);
        }

        private static string GetArgumentString(MetadataReader metadataReader, int intOperand, out bool isPrintable)
        {
            if (IsUserString(intOperand))
            {
                UserStringHandle usrStr = MetadataTokens.UserStringHandle(intOperand);
                var str = metadataReader.GetUserString(usrStr);
                str = ProcessAndNormalizeString(str, out isPrintable);
                return str;
            }
            throw new ArgumentException("Invalid argument, must be a user string metadata token.");
        }

        private static string ProcessAndNormalizeString(string str, out bool isPrintable)
        {
            foreach (char c in str)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category == UnicodeCategory.Control || category == UnicodeCategory.OtherNotAssigned || category == UnicodeCategory.OtherSymbol || c == '"')
                {
                    byte[] bytes = Encoding.Unicode.GetBytes(str);
                    isPrintable = false;
                    return string.Format("bytearray{0}", CreateByteArrayString(bytes));
                }
            }

            isPrintable = true;
            return str;
        }

        private static string GetMethodReturnType(MethodSignature<CilType> signature)
        {
            string returnTypeStr = signature.ReturnType.ToString();
            return signature.Header.IsInstance ? "instance " + returnTypeStr : returnTypeStr;
        }

        private static string GetMemberRef(MetadataReader metadataReader, int token, CilTypeProvider provider, string genericParameterSignature = "")
        {
            var refHandle = MetadataTokens.MemberReferenceHandle(token);
            var reference = metadataReader.GetMemberReference(refHandle);
            var parentToken = MetadataTokens.GetToken(reference.Parent);
            string type;
            if (IsTypeSpecification(parentToken))
            {
                var typeSpecificationHandle = MetadataTokens.TypeSpecificationHandle(parentToken);
                type = SignatureDecoder.DecodeType(typeSpecificationHandle, provider, null).ToString();
            }
            else
            {
                var parentHandle = MetadataTokens.TypeReferenceHandle(parentToken);
                type = SignatureDecoder.DecodeType(parentHandle, provider, null).ToString(false);
            }
            string signatureValue;
            string parameters = string.Empty;
            if (reference.GetKind() == MemberReferenceKind.Method)
            {
                MethodSignature<CilType> signature = SignatureDecoder.DecodeMethodSignature(reference.Signature, provider);
                signatureValue = GetMethodReturnType(signature);
                parameters = provider.GetParameterList(signature);
                return String.Format("{0} {1}::{2}{3}{4}", signatureValue, type, GetString(metadataReader, reference.Name), genericParameterSignature,parameters);
            }
            signatureValue = SignatureDecoder.DecodeFieldSignature(reference.Signature, provider).ToString();
            return String.Format("{0} {1}::{2}{3}", signatureValue, type, GetString(metadataReader, reference.Name), parameters);
        }

        internal static string SolveMethodName(MetadataReader metadataReader, int token, CilTypeProvider provider)
        {
            string genericParameters = string.Empty;
            if (IsMethodSpecification(token))
            {
                var methodHandle = MetadataTokens.MethodSpecificationHandle(token);
                var methodSpec = metadataReader.GetMethodSpecification(methodHandle);
                token = MetadataTokens.GetToken(methodSpec.Method);
                genericParameters = GetGenericParametersSignature(methodSpec, provider);
            }
            if (IsMemberReference(token))
            {
                return GetMemberRef(metadataReader, token, provider, genericParameters);
            }
            var handle = MetadataTokens.MethodDefinitionHandle(token);
            var definition = metadataReader.GetMethodDefinition(handle);
            var parent = definition.GetDeclaringType();
            MethodSignature<CilType> signature = SignatureDecoder.DecodeMethodSignature(definition.Signature, provider);
            var returnType = GetMethodReturnType(signature);
            var parameters = provider.GetParameterList(signature);
            var parentType = SignatureDecoder.DecodeType(parent, provider, null);
            return string.Format("{0} {1}::{2}{3}{4}",returnType, parentType.ToString(false), GetString(metadataReader, definition.Name), genericParameters, parameters);
        }

        private static string GetGenericParametersSignature(MethodSpecification methodSpec, CilTypeProvider provider)
        {
            var genericParameters = SignatureDecoder.DecodeMethodSpecificationSignature(methodSpec.Signature, provider);
            StringBuilder sb = new StringBuilder();
            int i;
            for(i = 0; i < genericParameters.Length; i++)
            {
                if(i == 0)
                {
                    sb.Append("<");
                }
                sb.Append(genericParameters[i]);
                sb.Append(",");
            }
            if(i > 0)
            {
                sb.Length--;
                sb.Append(">");
            }
            return sb.ToString();
        }

        private static string GetFieldInformation(MetadataReader metadataReader, int intOperand, CilTypeProvider provider)
        {
            if(IsMemberReference(intOperand))
            {
                return GetMemberRef(metadataReader, intOperand, provider);
            }
            var handle = MetadataTokens.FieldDefinitionHandle(intOperand);
            var definition = metadataReader.GetFieldDefinition(handle);
            var typeHandle = definition.GetDeclaringType();
            var typeSignature = SignatureDecoder.DecodeType(typeHandle, provider, null);
            var signature = SignatureDecoder.DecodeFieldSignature(definition.Signature, provider);
            return String.Format("{0} {1}::{2}", signature.ToString(), typeSignature.ToString(false), GetString(metadataReader, definition.Name));
        }

        private static string GetString(MetadataReader metadataReader, StringHandle handle)
        {
            return CilDecoderHelpers.Instance.NormalizeString(metadataReader.GetString(handle));
        }

        private static CilTokenType GetTokenType(int token)
        {
            return unchecked((CilTokenType)(token >> 24));
        }

        private static char HexadecimalNibble(byte b)
        {
            return (char)(b >= 0 && b <= 9 ? '0' + b : 'A' + (b - 10));
        }

        internal static string DecodeOverridenMethodName(MetadataReader metadataReader, int token, CilTypeProvider provider)
        {
            var handle = MetadataTokens.MethodDefinitionHandle(token);
            var definition = metadataReader.GetMethodDefinition(handle);
            var parent = definition.GetDeclaringType();
            var parentType = SignatureDecoder.DecodeType(parent, provider, null);
            return string.Format("{0}::{1}", parentType.ToString(false), GetString(metadataReader, definition.Name));
        }

        internal static string GetCachedValue(StringHandle value, CilReaders readers, ref string storage)
        {
            if (storage != null)
            {
                return storage;
            }
            storage = readers.MdReader.GetString(value);
            return storage;
        }

        #endregion
    }
}
