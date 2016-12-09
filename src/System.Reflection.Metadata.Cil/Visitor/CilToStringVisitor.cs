using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Cil.Decoder;
using System.Reflection.Metadata.Cil.Instructions;
using System.Text;

namespace System.Reflection.Metadata.Cil.Visitor
{
    public class CilToStringVisitor : ICilVisitor
    {
        private readonly CilVisitorOptions _options;
        private readonly TextWriter _writer;
        private string _indent = "  ";
        private int _indentation = 0;

        public CilToStringVisitor(CilVisitorOptions options, TextWriter writer)
        {
            _options = options;
            _writer = writer;
        }

        private void Indent()
        {
            _indentation++;
        }

        private void Unindent()
        {
            _indentation--;
        }

        private void WriteIndentation()
        {
            for (int i = 0; i < _indentation; i++)
            {
                _writer.Write(_indent);
            }
        }

        public TextWriter Writer
        {
            get
            {
                return _writer;
            }
        }

        public void Visit(CilAssembly assembly)
        {
            _writer.WriteLine();
            _writer.WriteLine("//  Copyright (c) Microsoft Corporation.  All rights reserved.");
            _writer.WriteLine();
            _writer.WriteLine();
            _writer.WriteLine();

            foreach(var moduleRef in assembly.ModuleReferences)
            {
                moduleRef.Accept(this);
            }

            foreach (var assemblyRef in assembly.AssemblyReferences)
            {
                assemblyRef.Accept(this);
            }

            WriteAssemblyDefinition(assembly);

            _writer.WriteLine();
            _writer.WriteLine("// =============== CLASS MEMBERS DECLARATION ===================");
            _writer.WriteLine();

            foreach (var type in assembly.TypeDefinitions)
            {
                if (type.Token == 33554433) continue; //skipping <Module> type definition for emitting correct IL.
                type.Accept(this);
            }
        }

        public void Visit(CilAssemblyReference assemblyReference)
        {
            WriteIndentation();
            _writer.Write(".assembly extern ");
            if (_options.ShowBytes)
                _writer.Write(string.Format("/* {0:X8} */ ", assemblyReference.Token));
            _writer.WriteLine(assemblyReference.Name);
            _writer.WriteLine('{');
            Indent();
            if (assemblyReference.HasPublicKeyOrToken)
            {
                WriteIndentation();
                _writer.Write(".publickeytoken = ");
                _writer.WriteLine(assemblyReference.GetPublicKeyOrTokenString());
            }

            if (assemblyReference.HasCulture)
            {
                WriteIndentation();
                _writer.Write(".locale ");
                _writer.WriteLine("'{0}'", assemblyReference.Culture);
            }

            if (assemblyReference.HasHashValue)
            {
                WriteIndentation();
                _writer.Write(".hash = ");
                _writer.WriteLine(assemblyReference.GetHashValueString());
            }

            WriteIndentation();
            _writer.WriteLine(string.Format(".ver {0}", assemblyReference.GetFormattedVersion()));

            foreach(var attribute in assemblyReference.CustomAttributes)
            {
                attribute.Accept(this);
            }

            Unindent();
            WriteIndentation();
            _writer.WriteLine('}');
        }

        public void Visit(CilModuleReference moduleReference)
        {
            WriteIndentation();
            _writer.Write(".module extern");
            _writer.Write(" ");
            _writer.Write(moduleReference.Name);
            if (_options.ShowBytes)
            {
                _writer.Write(string.Format(" /* {0:X8} */", moduleReference.Token));
            }
            _writer.WriteLine();
        }

        public void Visit(CilTypeDefinition typeDefinition)
        {
            WriteIndentation();
            _writer.Write(".class ");
            if (_options.ShowBytes)
                _writer.Write("/* {0:X8} */ ", typeDefinition.Token);
            if (typeDefinition.IsNested)
            {
                _writer.Write(typeDefinition.Name);
            }
            else
            {
                _writer.Write(typeDefinition.FullName);
            }
            if (typeDefinition.HasBaseType)
            {
                _writer.Write(" extends ");
                WriteEntityType(typeDefinition.BaseType);
            }
            _writer.WriteLine();
            WriteIndentation();
            _writer.WriteLine('{');
            Indent();
            if (!typeDefinition.Layout.IsDefault)
            {
                WriteIndentation();
                _writer.WriteLine(string.Format(".pack {0}", typeDefinition.Layout.PackingSize));
                WriteIndentation();
                _writer.WriteLine(string.Format(".size {0}", typeDefinition.Layout.Size));
            }

            foreach (var attribute in typeDefinition.CustomAttributes)
            {
                attribute.Accept(this);
            }

            foreach (var nestedType in typeDefinition.NestedTypes)
            {
                nestedType.Accept(this);
            }

            foreach(var field in typeDefinition.FieldDefinitions)
            {
                field.Accept(this);
            }

            foreach (var method in typeDefinition.MethodDefinitions)
            {
                method.Accept(this);
            }

            foreach(var eventDef in typeDefinition.Events)
            {
                eventDef.Accept(this);
            }

            foreach (var property in typeDefinition.Properties)
            {
                property.Accept(this);
            }

            Unindent();
            WriteIndentation();
            _writer.Write('}');
            _writer.WriteLine(string.Format(" // end of class {0}", typeDefinition.FullName));
            _writer.WriteLine();
        }

        public void Visit(CilMethodDefinition methodDefinition)
        {
            WriteMethodDefinition(methodDefinition);
            Indent();
            WriteMethodHeader(methodDefinition);
            int ilOffset = 0;
            int instructionIndex = 0;
            int lastRegionIndex = 0;
            if (methodDefinition.RelativeVirtualAddress != 0)
                WriteMethodBody(methodDefinition, methodDefinition.ExceptionRegions, ref instructionIndex, ilOffset, lastRegionIndex, out lastRegionIndex);
            Unindent();
            WriteIndentation();
            _writer.Write('}');
            _writer.WriteLine(string.Format(" // end of method {0}", methodDefinition.Name));
            _writer.WriteLine();
        }

        public void Visit(CilField field)
        {
            WriteIndentation();
            _writer.Write(".field ");
            if (_options.ShowBytes)
            {
                _writer.Write(string.Format("/* {0:X8} */ ", field.Token));
            }
            _writer.Write(field.Signature);

            if (field.HasDefault)
            {
                _writer.Write(string.Format(" = {0}",field.DefaultValue.GetValueString()));
            }
            _writer.WriteLine();

            foreach(var attribute in field.CustomAttributes)
            {
                attribute.Accept(this);
            }
        }

        public void Visit(CilLocal local)
        {
            _writer.Write(string.Format("{0} {1}", local.Type, local.Name));
        }

        public void Visit(CilCustomAttribute attribute)
        {
            WriteIndentation();
            _writer.WriteLine(string.Format(".custom {0} = {1}", attribute.Constructor, attribute.GetValueString()));
        }

        public void Visit(CilProperty property)
        {
            WritePropertyHeader(property);
            WriteIndentation();
            _writer.WriteLine('{');
            WritePropertyBody(property);
            WriteIndentation();
            _writer.WriteLine('}');
            _writer.WriteLine();
        }

        public void Visit(CilEventDefinition eventDef)
        {
            WriteIndentation();
            _writer.Write(".event ");
            if (_options.ShowBytes)
            {
                _writer.Write(string.Format("/* {0:X8} */ ", eventDef.Token));
            }

            WriteEntityType(eventDef.Type);
            _writer.Write(' ');
            _writer.WriteLine(eventDef.Name);
            WriteIndentation();
            _writer.WriteLine('{');
            Indent();
            WriteEventBody(eventDef);
            Unindent();
            WriteIndentation();
            _writer.WriteLine('}');
            _writer.WriteLine();
        }

        public void Visit(CilSingleInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Bytes, instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            if (float.IsNaN(instruction.Value) || float.IsInfinity(instruction.Value))
            {
                var data = BitConverter.GetBytes(instruction.Value);
                _writer.Write('(');
                _writer.Write(BitConverter.ToString(data).Replace('-', ' '));
                _writer.WriteLine(')');
                return;
            }

            if(instruction.Value == 0.0)
            {
                var bytes = BitConverter.GetBytes(instruction.Value);
                if(bytes[bytes.Length-1] == 128)
                {
                    _writer.WriteLine("-0.0");
                    return;
                }
                _writer.WriteLine("0.0");
                return;
            }
            _writer.Write(instruction.Value.ToString());
            if (instruction.Value % 10 == 0)
            {
                _writer.Write('.');
            }
            _writer.WriteLine();
        }

        public void Visit(CilInt32Instruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Token.ToString("X8"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(instruction.Value.ToString());
        }

        public void Visit(CilInt16BranchInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Value.ToString("X2"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(string.Format("IL_{0:x4}", (instruction.Token + instruction.Value + instruction.Size)));
        }

        public void Visit(CilStringInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                int tok = instruction.Token;
                uint RIDMask = (1 << 24) - 1;
                string tokenValue = string.Format("({0:X2}){1:X6}", (tok >> 24), (tok & RIDMask));
                WriteBytes(tokenValue, instruction);
            }
            _writer.Write(string.Format("{0,-13}", instruction.opCode));
            if (instruction.Token >> 24 == 0x70)
            {
                if (instruction.IsPrintable)
                {
                    _writer.WriteLine(string.Format("\"{0}\"", instruction.Value));
                    return;
                }
                _writer.WriteLine(string.Format("{0}", instruction.Value));
                return;
            }
            _writer.WriteLine(instruction.Value);
        }

        public void Visit(CilVariableInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Token.ToString("X4"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(instruction.Value);
        }

        public void Visit(CilSwitchInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                _writer.Write(string.Format("/* {0:X2,-4} | ", instruction.opCode.Value));
                string value = string.Format("{0:X2}000000", (int)instruction.Value);
                _writer.Write(string.Format("{0,-16} */ ", value));
                _writer.Write(string.Format("{0,-10}", instruction.opCode));
                _writer.Write('(');
                for (int i = 0; i < instruction.Token; i++)
                {
                    _writer.WriteLine();
                    WriteIndentation();
                    _writer.Write(string.Format("{0,12} {1,-4} | ", "/*", ""));
                    value = string.Format("{0:X2}000000", instruction.Jumps[i]);
                    _writer.Write(string.Format("{0,-16} */ ", value));
                    _writer.Write(string.Format("{0,11}", ' '));
                    _writer.Write(string.Format("IL_{0:x4}", (instruction.IlOffset + instruction.Size + instruction.Jumps[i])));
                    if (i < instruction.Token - 1)
                    {
                        _writer.Write(',');
                    }
                }
                _writer.WriteLine(')');
                return;
            }
            _writer.Write(string.Format("{0,-10}", instruction.opCode));
            _writer.Write('(');
            for (int i = 0; i < instruction.Token; i++)
            {
                _writer.WriteLine();
                WriteIndentation();
                _writer.Write(string.Format("{0,-21}", ""));
                _writer.Write(string.Format("IL_{0:x4}", (instruction.IlOffset + instruction.Size + instruction.Jumps[i])));
                if (i < instruction.Token - 1)
                {
                    _writer.Write(',');
                }
            }
            _writer.WriteLine(')');
        }

        public void Visit(CilInt16VariableInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Token.ToString("X2"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(instruction.Value);
        }

        public void Visit(CilInt64Instruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Bytes, instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(string.Format("0x{0:x}", instruction.Value));
        }

        public void Visit(CilInstructionWithNoValue instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(string.Empty, instruction);
            }
            _writer.WriteLine(string.Format("{0}", instruction.opCode));
        }

        public void Visit(CilDoubleInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Bytes, instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            if (double.IsNaN(instruction.Value) || double.IsInfinity(instruction.Value))
            {
                var data = BitConverter.GetBytes(instruction.Value);
                _writer.Write('(');
                _writer.Write(BitConverter.ToString(data).Replace('-', ' '));
                _writer.WriteLine(')');
                return;
            }

            if (instruction.Value == 0.0)
            {
                var bytes = BitConverter.GetBytes(instruction.Value);
                if (bytes[bytes.Length - 1] == 128)
                {
                    _writer.WriteLine("-0.0");
                    return;
                }
                _writer.WriteLine("0.0");
                return;
            }

            _writer.Write(instruction.Value.ToString());
            if (instruction.Value % 10 == 0)
            {
                _writer.Write('.');
            }
            _writer.WriteLine();
        }

        public void Visit(CilByteInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Token.ToString("X8"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(instruction.Value.ToString());
        }

        public void Visit(CilBranchInstruction instruction)
        {
            if (_options.ShowBytes)
            {
                WriteBytes(instruction.Value.ToString("X4"), instruction);
            }
            _writer.Write(string.Format("{0,-11}", instruction.opCode));
            _writer.WriteLine(string.Format("IL_{0:x4}", (instruction.Token + instruction.Value + instruction.Size)));
        }

        private void WriteAssemblyDefinition(CilAssembly assembly)
        {
            WriteIndentation();
            _writer.Write(".assembly ");
            _writer.Write(assembly.Flags);
            _writer.WriteLine(assembly.Name);
            WriteIndentation();
            _writer.WriteLine('{');
            Indent();

            foreach (var attribute in assembly.CustomAttributes)
            {
                attribute.Accept(this);
            }
            _writer.WriteLine();
            _writer.WriteLine();

            WriteIndentation();
            _writer.Write(".hash algorithm ");
            _writer.WriteLine(assembly.GetFormattedHashAlgorithm());

            if (assembly.HasPublicKey)
            {
                WriteIndentation();
                _writer.Write(".publickey = ");
                _writer.WriteLine(assembly.GetPublicKeyString());
            }

            if (assembly.HasCulture)
            {
                WriteIndentation();
                _writer.Write(".locale ");
                _writer.WriteLine("'{0}'", assembly.Culture);
            }

            WriteIndentation();
            _writer.WriteLine(string.Format(".ver {0}", assembly.GetFormattedVersion()));
            Unindent();
            WriteIndentation();
            _writer.WriteLine('}');
            WriteIndentation();
            _writer.Write(".module ");
            _writer.WriteLine(assembly.ModuleDefinition.Name);
            WriteIndentation();
            _writer.WriteLine(string.Format(".imagebase 0x{0:X8}", assembly.HeaderOptions.ImageBase));
            WriteIndentation();
            _writer.WriteLine(string.Format(".file alignment 0x{0:X8}", assembly.HeaderOptions.FileAlignment));
            WriteIndentation();
            _writer.WriteLine(string.Format(".stackreserve 0x{0:X8}", assembly.HeaderOptions.StackReserve));
            WriteIndentation();
            _writer.WriteLine(string.Format(".subsystem 0x{0:X}", assembly.HeaderOptions.SubSystem));
            WriteIndentation();
            _writer.WriteLine(string.Format(".corflags 0x{0:X}", assembly.HeaderOptions.Corflags));
        }
        private void WriteEventBody(CilEventDefinition eventDef)
        {
            if (eventDef.HasAdder)
            {
                WriteIndentation();
                _writer.Write(".addon ");
                WritePropertyOrEventAccessor(eventDef.Adder);
            }

            if (eventDef.HasRemover)
            {
                WriteIndentation();
                _writer.Write(".removeon ");
                WritePropertyOrEventAccessor(eventDef.Remover);
            }

            if (eventDef.HasRaiser)
            {
                WriteIndentation();
                _writer.Write(".fire ");
                WritePropertyOrEventAccessor(eventDef.Raiser);
            }
        }

        private void WriteMethodDefinition(CilMethodDefinition methodDefinition)
        {
            WriteIndentation();
            _writer.Write(".method ");
            if (_options.ShowBytes)
                _writer.Write(string.Format("/* {0:X8} */ ", methodDefinition.Token));
            _writer.WriteLine(methodDefinition.GetDecodedSignature());
            WriteIndentation();
            _writer.WriteLine('{');
        }

        private void WriteMethodHeader(CilMethodDefinition methodDefinition)
        {
            WriteCustomAttributes(methodDefinition);
            if (methodDefinition.RelativeVirtualAddress == 0)
            {
                return;
            }
            
            foreach(var parameter in methodDefinition.GetOptionalParameters())
            {
                WriteIndentation();
                _writer.Write(".param ");
                _writer.WriteLine(string.Format("[{0}] = {1}", parameter.SequenceNumber, parameter.DefaultValue.GetValueString()));
            }

            if (methodDefinition.IsImplementation)
            {
                WriteOverridenMethod(methodDefinition);
            }

            if (methodDefinition.IsEntryPoint)
            {
                WriteIndentation();
                _writer.WriteLine(".entrypoint");
            }

            WriteIndentation();
            _writer.WriteLine(string.Format("// code size {0,8} (0x{0:x})", methodDefinition.Size));
            WriteIndentation();
            _writer.WriteLine(string.Format(".maxstack {0,2}", methodDefinition.MaxStack));

            if (methodDefinition.HasLocals)
            {
                WriteLocals(methodDefinition);
            }
        }

        private void WriteLocals(CilMethodDefinition methodDefinition)
        {
            WriteIndentation();
            _writer.Write(".locals");

            if (methodDefinition.LocalVariablesInitialized)
            {
                _writer.Write(" init");
            }

            int i = 0;
            var locals = methodDefinition.Locals;
            _writer.Write('(');
            foreach (var local in locals)
            {
                if (i > 0)
                {
                    WriteIndentation();
                    _writer.Write(string.Format("{0,13}", ""));
                }
                _writer.Write(string.Format("[{0}] ", i));
                local.Accept(this);
                if (i < locals.Length - 1)
                {
                    _writer.WriteLine(',');
                }
                i++;
            }
            _writer.WriteLine(')');
        }

        private void WriteOverridenMethod(CilMethodDefinition methodDefinition)
        {
            WriteIndentation();
            _writer.Write(".override ");
            int token = methodDefinition.MethodDeclarationToken;
            if (CilDecoder.IsMemberReference(token))
            {
                _writer.Write("method ");
                _writer.Write(CilDecoder.SolveMethodName(methodDefinition._readers.MdReader, token, methodDefinition.Provider));
                if (_options.ShowBytes)
                    _writer.Write(string.Format(" /* {0:X8} */", token));
                _writer.WriteLine();
                return;
            }
            _writer.Write(CilDecoder.DecodeOverridenMethodName(methodDefinition._readers.MdReader, token, methodDefinition.Provider));
            if (_options.ShowBytes)
                _writer.Write(string.Format(" /* {0:X8} */", token));
            _writer.WriteLine();
        }

        private void WriteCustomAttributes(CilMethodDefinition methodDefinition)
        {
            foreach (var attribute in methodDefinition.CustomAttributes)
            {
                attribute.Accept(this);
            }
        }

        private int WriteMethodBody(CilMethodDefinition methodDefinition, IReadOnlyList<CilExceptionRegion> exceptionRegions, ref int instructionIndex, int ilOffset, int regionIndex, out int nextRegionIndex)
        {
            int lastRegionIndex = regionIndex - 1;
            var instructions = methodDefinition.Instructions;
            for (; instructionIndex < instructions.Length; instructionIndex++)
            {
                var instruction = instructions[instructionIndex];
                if (EndFilterRegion(exceptionRegions, lastRegionIndex, ilOffset))
                {
                    Unindent();
                    WriteIndentation();
                    _writer.WriteLine("} // end filter");
                    WriteIndentation();
                    _writer.WriteLine("{ // handler");
                    Indent();
                }

                if (StartRegion(exceptionRegions, regionIndex, ilOffset))
                {
                    var region = exceptionRegions[regionIndex];
                    WriteIndentation();
                    _writer.WriteLine(region.ToString(methodDefinition.Provider));
                    WriteIndentation();
                    _writer.WriteLine('{');
                    Indent();
                    ilOffset = WriteMethodBody(methodDefinition, exceptionRegions, ref instructionIndex, ilOffset, regionIndex + 1, out regionIndex);
                    Unindent();
                    WriteIndentation();
                    _writer.Write('}');
                    _writer.WriteLine(string.Format(" // end {0}", (region.Kind == HandlerKind.Try ? ".try" : "handler")));
                }

                else
                {
                    WriteIndentation();
                    _writer.Write(string.Format("IL_{0:x4}:", ilOffset));
                    _writer.Write(_indent);
                    instruction.Accept(this);
                    ilOffset += instruction.Size;
                }

                if (EndRegion(exceptionRegions, lastRegionIndex, ilOffset))
                {
                    break;
                }
            }

            nextRegionIndex = regionIndex;
            return ilOffset;
        }

        private void WritePropertyBody(CilProperty property)
        {
            Indent();
            foreach (var attribute in property.CustomAttributes)
            {
                attribute.Accept(this);
            }
            if (property.HasGetter)
            {
                WriteIndentation();
                _writer.Write(".get ");
                WritePropertyOrEventAccessor(property.Getter);
            }

            if (property.HasSetter)
            {
                WriteIndentation();
                _writer.Write(".set ");
                WritePropertyOrEventAccessor(property.Setter);
            }

            Unindent();
        }

        private void WritePropertyOrEventAccessor(CilMethodDefinition accessor)
        {
            int i = 0;
            StringBuilder genericParameters = new StringBuilder();
            foreach (var genericParameter in accessor.GenericParameters)
            {
                if (i == 0)
                {
                    genericParameters.Append('<');
                }
                genericParameters.Append(genericParameter);
                genericParameters.Append(',');
                i++;
            }

            if (i > 0)
            {
                genericParameters.Length -= 1; //Delete trailing ,
                genericParameters.Append('>');
            }

            if (accessor.Signature.Header.IsInstance)
            {
                _writer.Write("instance ");
            }
            _writer.Write(string.Format("{0} {1}::{2}{3}{4}", accessor.Signature.ReturnType, accessor.DeclaringType.FullName, accessor.Name, genericParameters.ToString(), CilDecoder.DecodeSignatureParamerTypes(accessor.Signature)));
            _writer.WriteLine();
        }

        private void WritePropertyHeader(CilProperty property)
        {
            WriteIndentation();
            _writer.Write(".property ");
            if (_options.ShowBytes)
                _writer.Write("/* {0:X8} */ ", property.Token);
            _writer.WriteLine(property.GetDecodedSignature());
            if (property.HasDefault)
            {
                WriteIndentation();
                _writer.WriteLine(string.Format(" = {0}", property.DefaultValue.GetValueString()));
            }
        }

        private void WriteEntityType(CilEntity type)
        {
            if (type.Kind == EntityKind.TypeDefinition)
            {
                _writer.Write(((CilTypeDefinition)type.Entity).FullName);
            }
            else if (type.Kind == EntityKind.TypeReference)
            {
                _writer.Write(((CilTypeReference)type.Entity).FullName);
            }
            else
            {
                _writer.Write(((CilTypeSpecification)type.Entity).Signature);
            }
        }

        private static bool EndFilterRegion(IReadOnlyList<CilExceptionRegion> exceptionRegions, int lastRegionIndex, int ilOffset)
        {
            return lastRegionIndex >= 0 && exceptionRegions[lastRegionIndex].Kind == HandlerKind.Filter && exceptionRegions[lastRegionIndex].StartOffset == ilOffset;
        }

        private static bool EndRegion(IReadOnlyList<CilExceptionRegion> exceptionRegions, int regionIndex, int ilOffset)
        {
            return exceptionRegions != null && regionIndex >= 0 && exceptionRegions[regionIndex].EndOffset == ilOffset;
        }

        private static bool StartRegion(IReadOnlyList<CilExceptionRegion> exceptionRegions, int regionIndex, int ilOffset)
        {
            return exceptionRegions != null && regionIndex < exceptionRegions.Count &&
                (exceptionRegions[regionIndex].StartOffset == ilOffset || exceptionRegions[regionIndex].FilterHandlerStart == ilOffset);
        }

        private void WriteBytes(string bytes, CilInstruction instruction)
        {
            _writer.Write(string.Format("/* {0:X2,-4} | {1,-16} */ ", instruction.opCode.Value, bytes));
        }
    }
}
