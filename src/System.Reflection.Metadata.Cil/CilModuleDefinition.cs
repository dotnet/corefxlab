namespace System.Reflection.Metadata.Cil
{
    public struct CilModuleDefinition
    {
        private CilReaders _readers;
        private ModuleDefinition _moduleDefinition;
        private int _generation;
        private string _name;
        private bool _isMvidIsInitialized;
        private bool _isGenerationIdIsInitialized;
        private bool _isBaseGenerationIdIsInitialized;
        private Guid _mvid;
        private Guid _generationId;
        private Guid _baseGenerationId;

        internal static CilModuleDefinition Create(ModuleDefinition definition, ref CilReaders readers)
        {
            CilModuleDefinition module = new CilModuleDefinition();
            module._moduleDefinition = definition;
            module._readers = readers;
            module._generation = -1;
            module._isMvidIsInitialized = false;
            module._isGenerationIdIsInitialized = false;
            module._isBaseGenerationIdIsInitialized = false;
            return module;
        }

        public string Name
        {
            get
            {
                if (_name == null)
                {
                    _name = _readers.MdReader.GetString(_moduleDefinition.Name);
                }
                return _name;
            }
        }

        public int Generation
        {
            get
            {
                if(_generation == -1)
                {
                    _generation = _moduleDefinition.Generation;
                }
                return _generation;
            }
        }

        public Guid Mvid
        {
            get
            {
                if (!_isMvidIsInitialized)
                {
                    _isMvidIsInitialized = true;
                    _mvid = _readers.MdReader.GetGuid(_moduleDefinition.Mvid);
                }
                return _mvid;
            }
        }

        public Guid GenerationId
        {
            get
            {
                if (!_isGenerationIdIsInitialized)
                {
                    _isGenerationIdIsInitialized = true;
                    _generationId = _readers.MdReader.GetGuid(_moduleDefinition.GenerationId);
                }
                return _generationId;
            }
        }

        public Guid BaseGenerationId
        {
            get
            {
                if (!_isBaseGenerationIdIsInitialized)
                {
                    _isBaseGenerationIdIsInitialized = true;
                    _baseGenerationId = _readers.MdReader.GetGuid(_moduleDefinition.BaseGenerationId);
                }
                return _baseGenerationId;
            }
        }
    }
}
