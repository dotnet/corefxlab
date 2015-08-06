using ILDasmLibrary;
using ILDasmLibrary.Instructions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata.Decoding;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace System.Reflection.Metadata.ILDasm.Decoder
{
    public struct ILDecoder
    {
        #region Public APIs

        /// <summary>
        /// Method that given a token defines if it is a type reference token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type reference false if not</returns>
        public static bool IsTypeReference(int token)
        {
            return GetTokenType(token) == CILTokenType.TypeReference;
        }

        /// <summary>
        /// Method that given a token defines if it is a type definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type definition false if not</returns>
        public static bool IsTypeDefinition(int token)
        {
            return GetTokenType(token) == CILTokenType.TypeDefinition;
        }

        /// <summary>
        /// Method that given a token defines if it is a user string token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a user string false if not</returns>
        public static bool IsUserString(int token)
        {
            return GetTokenType(token) == CILTokenType.UserString;
        }

        /// <summary>
        /// Method that given a token defines if it is a member reference token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a member reference false if not</returns>
        public static bool IsMemberReference(int token)
        {
            return GetTokenType(token) == CILTokenType.MemberReference;
        }

        /// <summary>
        /// Method that given a token defines if it is a method specification token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a method specification false if not</returns>
        public static bool IsMethodSpecification(int token)
        {
            return GetTokenType(token) == CILTokenType.MethodSpecification;
        }

        /// <summary>
        /// Method that given a token defines if it is a method definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a method definition false if not</returns>
        public static bool IsMethodDefinition(int token)
        {
            return GetTokenType(token) == CILTokenType.MethodDefinition;
        }

        /// <summary>
        /// Method that given a token defines if it is a field definition token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a field definition false if not</returns>
        public static bool IsFieldDefinition(int token)
        {
            return GetTokenType(token) == CILTokenType.FieldDefinition;
        }

        /// <summary>
        /// Method that given a token defines if it is a type specification token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a type specification false if not</returns>
        public static bool IsTypeSpecification(int token)
        {
            return GetTokenType(token) == CILTokenType.TypeSpecification;
        }

        /// <summary>
        /// Method that given a token defines if it is a standalone signature token.
        /// </summary>
        /// <param name="token">token to solve</param>
        /// <returns>true if is a standalone signature false if not</returns>
        public static bool IsStandaloneSignature(int token)
        {
            return GetTokenType(token) == CILTokenType.Signature;
        }

        public static string GetByteArrayString(byte[] array)
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

        

        public static string GetVersionString(Version version)
        {
            int build = version.Build;
            int revision = version.Revision;
            if (build == -1) build = 0;
            if (revision == -1) revision = 0;
            return String.Format("{0}:{1}:{2}:{3}", version.Major.ToString(), version.Minor.ToString(), build.ToString(), revision.ToString());
        }

        public static string DecodeSignatureParamerTypes(MethodSignature<ILType> Signature)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            var types = Signature.ParameterTypes;
            for (int i = 0; i < types.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(types[i].ToString());
            }
            sb.Append(")");
            return sb.ToString();
        }

        internal static ILEntity DecodeEntityHandle(EntityHandle handle, ref Readers _readers)
        {
            if (handle.Kind == HandleKind.TypeDefinition)
            {
                TypeDefinition definition = _readers.MdReader.GetTypeDefinition((TypeDefinitionHandle)handle);
                int token = MetadataTokens.GetToken(handle);
                ILTypeDefinition type = ILTypeDefinition.Create(definition, ref _readers, token);
                return new ILEntity(type, EntityKind.TypeDefinition);
            }
            if (handle.Kind == HandleKind.TypeSpecification)
            {
                TypeSpecification specification = _readers.MdReader.GetTypeSpecification((TypeSpecificationHandle)handle);
                ILTypeSpecification type = ILTypeSpecification.Create(specification, ref _readers);
                return new ILEntity(type, EntityKind.TypeSpecification);
            }
            if (handle.Kind == HandleKind.TypeReference)
            {
                TypeReference reference = _readers.MdReader.GetTypeReference((TypeReferenceHandle)handle);
                int token = MetadataTokens.GetToken(handle);
                ILTypeReference type = ILTypeReference.Create(reference, ref _readers, token);
                return new ILEntity(type, EntityKind.TypeReference);
            }
            throw new BadImageFormatException("Event definition type must be either type reference, type definition or type specification");
        }

        public static MethodSignature<ILType> DecodeMethodSignature(MethodDefinition _methodDefinition, ILTypeProvider _provider)
        {
            return SignatureDecoder.DecodeMethodSignature(_methodDefinition.Signature, _provider);
        }

        public static IEnumerable<ILInstruction> DecodeMethodBody(ILMethodDefinition _methodDefinition)
        {
            return DecodeMethodBody(_methodDefinition.IlReader, _methodDefinition._readers.MdReader, _methodDefinition.Provider, _methodDefinition);
        }

        #endregion

        #region Private and internal helpers

        internal static bool HasArgument(OpCode opCode)
        {
            bool isLoad = opCode == OpCodes.Ldarg || opCode == OpCodes.Ldarga || opCode == OpCodes.Ldarga_S || opCode == OpCodes.Ldarg_S;
            bool isStore = opCode == OpCodes.Starg_S || opCode == OpCodes.Starg;
            return isLoad || isStore;
        }

        private static IEnumerable<ILInstruction> DecodeMethodBody(BlobReader ilReader, MetadataReader mdReader, ILTypeProvider provider, ILMethodDefinition _methodDefinition)
        {
            ilReader.Reset();
            int intOperand;
            ushort shortOperand;
            int ilOffset = 0;
            ILInstruction instruction = null;
            while (ilReader.Offset < ilReader.Length)
            {
                OpCode opCode;
                int expectedSize;
                var _byte = ilReader.ReadByte();
                /*If the byte read is 0xfe it means is a two byte instruction, 
                so since it is going to read the second byte to get the actual
                instruction it has to check that the offset is still less than the length.*/
                if (_byte == 0xfe && ilReader.Offset < ilReader.Length)
                {
                    opCode = ILDecoderHelpers.Instance.twoByteOpCodes[ilReader.ReadByte()];
                    expectedSize = 2;
                }
                else
                {
                    opCode = ILDecoderHelpers.Instance.oneByteOpCodes[_byte];
                    expectedSize = 1;
                }
                switch (opCode.OperandType)
                {
                    //The instruction size is the expected size (1 or 2 depending if it is a one or two byte instruction) + the size of the operand.
                    case OperandType.InlineField:
                        intOperand = ilReader.ReadInt32();
                        string fieldInfo = GetFieldInformation(mdReader, intOperand, provider);
                        instruction = new ILStringInstruction(opCode, fieldInfo, intOperand, expectedSize + (int)CILInstructionSize.Int32);
                        break;
                    case OperandType.InlineString:
                        intOperand = ilReader.ReadInt32();
                        bool isPrintable;
                        string str = GetArgumentString(mdReader, intOperand, out isPrintable);
                        instruction = new ILStringInstruction(opCode, str, intOperand, expectedSize + (int)CILInstructionSize.Int32, isPrintable);
                        break;
                    case OperandType.InlineMethod:
                        intOperand = ilReader.ReadInt32();
                        string methodCall = SolveMethodName(mdReader, intOperand, provider);
                        instruction = new ILStringInstruction(opCode, methodCall, intOperand, expectedSize + (int)CILInstructionSize.Int32);
                        break;
                    case OperandType.InlineType:
                        intOperand = ilReader.ReadInt32();
                        string type = GetTypeInformation(mdReader, intOperand, provider);
                        instruction = new ILStringInstruction(opCode, type, intOperand, expectedSize + (int)CILInstructionSize.Int32);
                        break;
                    case OperandType.InlineTok:
                        intOperand = ilReader.ReadInt32();
                        string tokenType = GetInlineTokenType(mdReader, intOperand, provider);
                        instruction = new ILStringInstruction(opCode, tokenType, intOperand, expectedSize + (int)CILInstructionSize.Int32);
                        break;
                    case OperandType.InlineI:
                        instruction = new ILInt32Instruction(opCode, ilReader.ReadInt32(), -1, expectedSize + (int)CILInstructionSize.Int32);
                        break;
                    case OperandType.InlineI8:
                        instruction = new ILInt64Instruction(opCode, ilReader.ReadInt64(), -1, expectedSize + (int)CILInstructionSize.Int64);
                        break;
                    case OperandType.InlineR:
                        instruction = new ILDoubleInstruction(opCode, ilReader.ReadDouble(), -1, expectedSize + (int)CILInstructionSize.Double);
                        break;
                    case OperandType.InlineSwitch:
                        instruction = CreateSwitchInstruction(ref ilReader, expectedSize, ilOffset, opCode);
                        break;
                    case OperandType.ShortInlineBrTarget:
                        instruction = new ILInt16BranchInstruction(opCode, ilReader.ReadSByte(), ilOffset, expectedSize + (int)CILInstructionSize.Byte);
                        break;
                    case OperandType.InlineBrTarget:
                        instruction = new ILBranchInstruction(opCode, ilReader.ReadInt32(), ilOffset, expectedSize + (int)CILInstructionSize.Int32);
                        break;
                    case OperandType.ShortInlineI:
                        instruction = new ILByteInstruction(opCode, ilReader.ReadByte(), -1, expectedSize + (int)CILInstructionSize.Byte);
                        break;
                    case OperandType.ShortInlineR:
                        instruction = new ILSingleInstruction(opCode, ilReader.ReadSingle(), -1, expectedSize + (int)CILInstructionSize.Single);
                        break;
                    case OperandType.InlineNone:
                        instruction = new ILInstructionWithNoValue(opCode, expectedSize);
                        break;
                    case OperandType.ShortInlineVar:
                        byte token = ilReader.ReadByte();
                        instruction = new ILInt16VariableInstruction(opCode, GetVariableName(opCode, token, _methodDefinition), token, expectedSize + (int)CILInstructionSize.Byte);
                        break;
                    case OperandType.InlineVar:
                        shortOperand = ilReader.ReadUInt16();
                        instruction = new ILVariableInstruction(opCode, GetVariableName(opCode, shortOperand, _methodDefinition), shortOperand, expectedSize + (int)CILInstructionSize.Int16);
                        break;
                    case OperandType.InlineSig:
                        intOperand = ilReader.ReadInt32();
                        instruction = new ILStringInstruction(opCode, GetSignature(mdReader, intOperand, provider), intOperand, expectedSize + (int)CILInstructionSize.Int32);
                        break;
                    default:
                        break;
                }
                ilOffset += instruction.Size;
                yield return instruction;
            }
        }

        internal static ILLocal[] DecodeLocalSignature(MethodBodyBlock methodBody, MetadataReader mdReader, ILTypeProvider provider)
        {
            if (methodBody.LocalSignature.IsNil)
            {
                return new ILLocal[0];
            }
            ImmutableArray<ILType> localTypes = SignatureDecoder.DecodeLocalSignature(methodBody.LocalSignature, provider);
            ILLocal[] locals = new ILLocal[localTypes.Length];
            for (int i = 0; i < localTypes.Length; i++)
            {
                string name = "V_" + i;
                locals[i] = new ILLocal(name, localTypes[i].ToString());
            }
            return locals;
        }

        internal static ILParameter[] DecodeParameters(MethodSignature<ILType> signature, ParameterHandleCollection parameters, ref Readers readers)
        {
            ImmutableArray<ILType> types = signature.ParameterTypes;
            int requiredCount = Math.Min(signature.RequiredParameterCount, types.Length);
            if (requiredCount == 0)
            {
                return new ILParameter[0];
            }
            ILParameter[] result = new ILParameter[requiredCount];
            for (int i = 0; i < requiredCount; i++)
            {
                var parameter = readers.MdReader.GetParameter(parameters.ElementAt(i));
                result[i] = ILParameter.Create(parameter, ref readers, types[i].ToString());
            }
            return result;
        }

        internal static IEnumerable<string> DecodeGenericParameters(MethodDefinition _methodDefinition, ILMethodDefinition method)
        {
            int count = method.Signature.GenericParameterCount;
            foreach (var handle in _methodDefinition.GetGenericParameters())
            {
                var parameter = method._readers.MdReader.GetGenericParameter(handle);
                yield return method._readers.MdReader.GetString(parameter.Name);
            }
        }

        internal static ILType DecodeType(EntityHandle type, ILTypeProvider provider)
        {
            return SignatureDecoder.DecodeType(type, provider, null);
        }

        private static string GetSignature(MetadataReader mdReader, int intOperand, ILTypeProvider provider)
        {
            if (IsStandaloneSignature(intOperand))
            {
                var handle = MetadataTokens.StandaloneSignatureHandle(intOperand);
                var standaloneSignature = mdReader.GetStandaloneSignature(handle);
                var signature = SignatureDecoder.DecodeMethodSignature(standaloneSignature.Signature, provider);
                return string.Format("{0}{1}", GetMethodReturnType(signature), provider.GetParameterList(signature));
            }
            throw new ArgumentException("Get signature invalid token");
        }

        private static string GetVariableName(OpCode opCode, int token, ILMethodDefinition _methodDefinition)
        {
            if (HasArgument(opCode))
            {
                if (_methodDefinition.Signature.Header.IsInstance)
                {
                    token--; //the first parameter is "this".
                }
                return _methodDefinition.GetParameter(token).Name;
            }
            return _methodDefinition.GetLocal(token).Name;
            
        }

        private static string GetInlineTokenType(MetadataReader mdReader, int intOperand, ILTypeProvider provider)
        {
            if (IsMethodDefinition(intOperand) || IsMethodSpecification(intOperand) || IsMemberReference(intOperand))
            {
                return "method " + SolveMethodName(mdReader, intOperand, provider);
            }
            if (IsFieldDefinition(intOperand))
            {
                return "field " + GetFieldInformation(mdReader, intOperand, provider);
            }
            return GetTypeInformation(mdReader, intOperand, provider);
        }

        private static string GetTypeInformation(MetadataReader mdReader, int intOperand, ILTypeProvider provider)
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

        private static ILInstruction CreateSwitchInstruction(ref BlobReader ilReader, int expectedSize, int ilOffset, OpCode opCode)
        {
            uint caseNumber = ilReader.ReadUInt32();
            int[] jumps = new int[caseNumber];
            for (int i = 0; i < caseNumber; i++)
            {
                jumps[i] = ilReader.ReadInt32();
            }
            int size = (int)CILInstructionSize.Int32 + expectedSize;
            size += (int)caseNumber * (int)CILInstructionSize.Int32;
            return new ILSwitchInstruction(opCode, ilOffset, jumps, (int)caseNumber, caseNumber, size);
        }

        private static string GetArgumentString(MetadataReader mdReader, int intOperand, out bool isPrintable)
        {
            if (IsUserString(intOperand))
            {
                UserStringHandle usrStr = MetadataTokens.UserStringHandle(intOperand);
                var str = mdReader.GetUserString(usrStr);
                str = ProcessAndNormalizeString(str, out isPrintable);
                return str;
            }
            throw new NotImplementedException("Argument String");
        }

        private static string ProcessAndNormalizeString(string str, out bool isPrintable)
        {
            foreach (char c in str)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category == UnicodeCategory.Control || category == UnicodeCategory.OtherNotAssigned || category == UnicodeCategory.OtherSymbol || c == '"')
                {
                    byte[] _bytes = Encoding.Unicode.GetBytes(str);
                    isPrintable = false;
                    return string.Format("bytearray{0}", GetByteArrayString(_bytes));
                }
            }

            isPrintable = true;
            return str;
        }

        private static string GetMethodReturnType(MethodSignature<ILType> signature)
        {
            string returnTypeStr = signature.ReturnType.ToString();
            return signature.Header.IsInstance ? "instance " + returnTypeStr : returnTypeStr;
        }

        private static string GetMemberRef(MetadataReader mdReader, int token, ILTypeProvider provider, string genericParameterSignature = "")
        {
            var refHandle = MetadataTokens.MemberReferenceHandle(token);
            var reference = mdReader.GetMemberReference(refHandle);
            var parentToken = MetadataTokens.GetToken(reference.Parent);
            string type;
            if (IsTypeSpecification(parentToken))
            {
                var typeSpecificationHandle = MetadataTokens.TypeSpecificationHandle(parentToken);
                var typeSpecification = mdReader.GetTypeSpecification(typeSpecificationHandle);
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
                MethodSignature<ILType> signature = SignatureDecoder.DecodeMethodSignature(reference.Signature, provider);
                signatureValue = GetMethodReturnType(signature);
                parameters = provider.GetParameterList(signature);
                return String.Format("{0} {1}::{2}{3}{4}", signatureValue, type, GetString(mdReader, reference.Name), genericParameterSignature,parameters);
            }
            signatureValue = SignatureDecoder.DecodeFieldSignature(reference.Signature, provider).ToString();
            return String.Format("{0} {1}::{2}{3}", signatureValue, type, GetString(mdReader, reference.Name), parameters);
        }

        internal static string SolveMethodName(MetadataReader mdReader, int token, ILTypeProvider provider)
        {
            string genericParameters = string.Empty;
            if (IsMethodSpecification(token))
            {
                var methodHandle = MetadataTokens.MethodSpecificationHandle(token);
                var methodSpec = mdReader.GetMethodSpecification(methodHandle);
                token = MetadataTokens.GetToken(methodSpec.Method);
                genericParameters = GetGenericParametersSignature(methodSpec, provider);
            }
            if (IsMemberReference(token))
            {
                return GetMemberRef(mdReader, token, provider, genericParameters);
            }
            var handle = MetadataTokens.MethodDefinitionHandle(token);
            var definition = mdReader.GetMethodDefinition(handle);
            var parent = definition.GetDeclaringType();
            MethodSignature<ILType> signature = SignatureDecoder.DecodeMethodSignature(definition.Signature, provider);
            var returnType = GetMethodReturnType(signature);
            var parameters = provider.GetParameterList(signature);
            var parentType = SignatureDecoder.DecodeType(parent, provider, null);
            return string.Format("{0} {1}::{2}{3}{4}",returnType, parentType.ToString(false), GetString(mdReader, definition.Name), genericParameters, parameters);
        }

        private static string GetGenericParametersSignature(MethodSpecification methodSpec, ILTypeProvider provider)
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

        private static string GetFieldInformation(MetadataReader mdReader, int intOperand, ILTypeProvider provider)
        {
            if(IsMemberReference(intOperand))
            {
                return GetMemberRef(mdReader, intOperand, provider);
            }
            var handle = MetadataTokens.FieldDefinitionHandle(intOperand);
            var definition = mdReader.GetFieldDefinition(handle);
            var typeHandle = definition.GetDeclaringType();
            var typeSignature = SignatureDecoder.DecodeType(typeHandle, provider, null);
            var signature = SignatureDecoder.DecodeFieldSignature(definition.Signature, provider);
            return String.Format("{0} {1}::{2}", signature.ToString(), typeSignature.ToString(false), GetString(mdReader, definition.Name));
        }

        private static string GetString(MetadataReader mdReader, StringHandle handle)
        {
            return ILDecoderHelpers.Instance.NormalizeString(mdReader.GetString(handle));
        }

        private static CILTokenType GetTokenType(int token)
        {
            return unchecked((CILTokenType)(token >> 24));
        }

        private static char HexadecimalNibble(byte b)
        {
            return (char)(b >= 0 && b <= 9 ? '0' + b : 'A' + (b - 10));
        }

        internal static string DecodeOverridenMethodName(MetadataReader mdReader, int token, ILTypeProvider provider)
        {
            var handle = MetadataTokens.MethodDefinitionHandle(token);
            var definition = mdReader.GetMethodDefinition(handle);
            var parent = definition.GetDeclaringType();
            MethodSignature<ILType> signature = SignatureDecoder.DecodeMethodSignature(definition.Signature, provider);
            var parentType = SignatureDecoder.DecodeType(parent, provider, null);
            return string.Format("{0}::{1}", parentType.ToString(false), GetString(mdReader, definition.Name));
        }

        internal static string GetCachedValue(StringHandle value, Readers _readers, ref string storage)
        {
            if (storage != null)
            {
                return storage;
            }
            storage = _readers.MdReader.GetString(value);
            return storage;
        }

        #endregion
    }
}
