
namespace System.Reflection.Metadata.Cil.Decoder
{
    public enum CilTokenType
    {
        Module = 0x00,
        TypeReference = 0x01,
        TypeDefinition = 0x02,
        FieldDefinition = 0x04,
        MethodDefinition = 0x06,
        ParameterDefinition = 0x08,
        InterfaceImplementation = 0x09,
        MemberReference = 0x0A,
        CustomAttribute = 0x0C,
        Permission = 0x0E,
        Signature = 0x11,
        Event = 0x14,
        Property = 0x17,
        ModuleReference = 0x1A,
        TypeSpecification = 0x1B,
        Assembly = 0x20,
        AssemblyReference = 0x23,
        File = 0x26,
        ExportedType = 0x27,
        ManifestResource = 0x28,
        GenericParameter = 0x2A,
        MethodSpecification = 0x2B,
        GenericParameterConstraint = 0x2C,
        UserString = 0x70, 
    }
}
