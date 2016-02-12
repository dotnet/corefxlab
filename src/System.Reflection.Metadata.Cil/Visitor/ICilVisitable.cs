namespace System.Reflection.Metadata.Cil.Visitor
{
    public interface ICilVisitable
    {
        void Accept(ICilVisitor visitor);
    }
}
