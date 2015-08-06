namespace System.Reflection.Metadata.Cil
{
    public struct CilTypeLayout
    {
        private readonly TypeLayout _layout;

        public CilTypeLayout(TypeLayout layout)
        {
            _layout = layout;
        }

        public int Size
        {
            get { return _layout.Size; }
        }

        public int PackingSize
        {
            get { return _layout.PackingSize; }
        }

        public bool IsDefault
        {
            get { return _layout.IsDefault; }
        }
    }
}
