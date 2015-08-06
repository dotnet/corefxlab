using System.Reflection.Metadata.Cil.Instructions;

namespace System.Reflection.Metadata.Cil.Visitor
{
    public interface ICilVisitor
    {
        void Visit(CilAssembly assembly);
        void Visit(CilAssemblyReference assemblyReference);
        void Visit(CilModuleReference moduleReference);
        void Visit(CilTypeDefinition typeDefinition);
        void Visit(CilMethodDefinition methodDefinition);
        void Visit(CilField field);
        void Visit(CilLocal local);
        void Visit(CilCustomAttribute attribute);
        void Visit(CilProperty property);
        void Visit(CilEventDefinition eventDef);
        void Visit(CilBranchInstruction branchInstruction);
        void Visit(CilByteInstruction byteInstruction);
        void Visit(CilDoubleInstruction doubleInstruction);
        void Visit(CilSingleInstruction floatInstruction);
        void Visit(CilInstructionWithNoValue instruction);
        void Visit(CilInt32Instruction intInstruction);
        void Visit(CilInt64Instruction longInstruction);
        void Visit(CilInt16BranchInstruction shortBranchInstruction);
        void Visit(CilInt16VariableInstruction shortVariableInstruction);
        void Visit(CilStringInstruction stringInstruction);
        void Visit(CilSwitchInstruction switchInstruction);
        void Visit(CilVariableInstruction variableInstruction);
    }
}
