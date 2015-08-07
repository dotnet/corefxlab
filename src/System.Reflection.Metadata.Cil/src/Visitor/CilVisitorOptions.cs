namespace System.Reflection.Metadata.Cil.Visitor
{
    public struct CilVisitorOptions
    {
        private readonly bool _showBytes;

        public CilVisitorOptions(bool showBytes)
        {
            _showBytes = showBytes;
        }

        public bool ShowBytes
        {
            get
            {
                return _showBytes;
            }
        }
    }
}
