// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
