using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILDasmLibrary
{
    public enum EntityKind
    {
        TypeReference,
        TypeDefinition,
        TypeSpecification
    }

    public struct ILEntity
    {
        private object _entity;
        private EntityKind _kind;

        internal ILEntity(object entity, EntityKind kind)
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
