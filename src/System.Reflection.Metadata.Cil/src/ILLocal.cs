using ILDasmLibrary.Visitor;

namespace ILDasmLibrary
{
    public struct ILLocal : IVisitable
    {
        private readonly string _name;
        private readonly string _type;

        public ILLocal(string name, string type)
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

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}