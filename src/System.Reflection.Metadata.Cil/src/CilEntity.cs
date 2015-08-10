namespace System.Reflection.Metadata.Cil
{
    public enum EntityKind
    {
        TypeReference,
        TypeDefinition,
        TypeSpecification
    }

    public struct CilEntity
    {
        private object _entity;
        private EntityKind _kind;

        internal CilEntity(object entity, EntityKind kind)
        {
            _entity = entity;
            _kind = kind;
        }

        public object Entity
        {
            get
            {
                return _entity;
            }
        }

        public EntityKind Kind
        {
            get
            {
                return _kind;
            }
        }
    }
}
