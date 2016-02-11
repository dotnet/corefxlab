using System.Reflection.Metadata.Cil.Visitor;

namespace System.Reflection.Metadata.Cil
{
    public struct CilLocal : ICilVisitable
    {
        private readonly string _name;
        private readonly string _type;

        public CilLocal(string name, string type)
        {
            _name = name;
            _type = type;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }
        }

        public void Accept(ICilVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}