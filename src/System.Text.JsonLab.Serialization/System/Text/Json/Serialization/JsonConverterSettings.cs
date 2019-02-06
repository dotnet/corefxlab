// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace System.Text.Json.Serialization
{
    public class JsonConverterSettings
    {
        private const int DefaultBufferSizeInternal = 16 * 1024;

        private volatile JsonMemberBasedClassMaterializer _classMaterializerStrategy;
        private JsonClassMaterializer _classMaterializer;
        private int _defaultBufferSize = DefaultBufferSizeInternal;
        private int _maxDepth = 64;
        private bool _hasRuntimeCustomAttributes;

        private static readonly ConcurrentDictionary<ICustomAttributeProvider, object[]> s_reflectionAttributes = new ConcurrentDictionary<ICustomAttributeProvider, object[]>();
        private readonly Lazy<ConcurrentDictionary<ICustomAttributeProvider, List<Attribute>>> _runtimeAttributes = new Lazy<ConcurrentDictionary<ICustomAttributeProvider, List<Attribute>>>();

        private static readonly ConcurrentDictionary<Type, JsonClassInfo> s_classes = new ConcurrentDictionary<Type, JsonClassInfo>();
        private readonly ConcurrentDictionary<Type, JsonClassInfo> _local_classes = new ConcurrentDictionary<Type, JsonClassInfo>();

        //todo: throw exception if we try to add attributes once (de)serialization occurred.

        internal JsonClassInfo GetOrAddClass(Type classType)
        {
            JsonClassInfo result;

            // Once custom attributes have been used, cache classes locally.
            if (_hasRuntimeCustomAttributes)
            {
                if (!_local_classes.TryGetValue(classType, out result))
                {
                    result = _local_classes.GetOrAdd(classType, new JsonClassInfo(classType, this));
                }
            }
            else
            {
                if (!s_classes.TryGetValue(classType, out result))
                {
                    result = s_classes.GetOrAdd(classType, new JsonClassInfo(classType, this));
                }
            }

            return result;
        }

        // Todo: add these options to the JsonConverterSettings and then internally map to JsonReaderOptions\JsonWriterOptions
        internal JsonReaderOptions ReaderOptions { get; set; }
        internal JsonWriterOptions WriterOptions { get; set; }

        public JsonClassMaterializer ClassMaterializer
        {
            get
            {
                return _classMaterializer;
            }
            set
            {
                if (_classMaterializer != JsonClassMaterializer.Default && value != _classMaterializer)
                {
                    throw new InvalidOperationException("Can't change materializer");
                }

                _classMaterializer = value;
            }
        }

        public int DefaultBufferSize
        {
            get
            {
                return _defaultBufferSize;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("todo");
                }

                _defaultBufferSize = value;
            }
        }

        public int MaxDepth
        {
            get
            {
                return _maxDepth;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("todo");
                }

                _maxDepth = value;
            }
        }

        public void AddAttribute(ICustomAttributeProvider type, Attribute attribute)
        {
            if (type == null)
                throw new ArgumentException(nameof(type));

            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            if (!_runtimeAttributes.Value.TryGetValue(type, out List<Attribute> attributes))
            {
                _runtimeAttributes.Value.TryAdd(type, attributes = new List<Attribute>());
                // Failure in TryAdd is OK since another thread just finished the same operation.
            }

            attributes.Add(attribute);
            _hasRuntimeCustomAttributes = true;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(ICustomAttributeProvider type, bool inherit = false) where TAttribute:Attribute
        {
            if (type == null)
                throw new ArgumentException(nameof(type));

            IEnumerable<TAttribute> attributes = Enumerable.Empty<TAttribute>();

            if (_runtimeAttributes.IsValueCreated)
            {
                ICustomAttributeProvider baseType = type;
                do
                {
                    _runtimeAttributes.Value.TryGetValue(baseType, out List<Attribute> allRuntimeAttributes);
                    if (allRuntimeAttributes != null)
                    {
                        attributes = attributes.Concat(allRuntimeAttributes.OfType<TAttribute>());
                    }
                    if (inherit)
                    {
                        baseType = (baseType as Type)?.BaseType;
                    }
                    else
                    {
                        baseType = null;
                    }
                }
                while (baseType != null && (Type)baseType != typeof(object));
            }

            if (!s_reflectionAttributes.TryGetValue(type, out object[] allReflectionAttributes))
            {
                allReflectionAttributes = type.GetCustomAttributes(inherit: inherit);
                s_reflectionAttributes.TryAdd(type, allReflectionAttributes);
                // Failure in TryAdd is OK since another thread just finished the same operation.
            }

            return attributes.Concat(allReflectionAttributes.OfType<TAttribute>());
        }

        internal JsonMemberBasedClassMaterializer ClassMaterializerStrategy
        {
            get
            {
                if (_classMaterializerStrategy == null)
                {
                    if (ClassMaterializer == JsonClassMaterializer.Default)
                    {
#if BUILDING_INBOX_LIBRARY
                        _classMaterializerStrategy = new JsonReflectionEmitMaterializer();
#else
                        _classMaterializerStrategy = new JsonReflectionMaterializer();
#endif
                    }
                    else if (ClassMaterializer == JsonClassMaterializer.ReflectionEmit)
                    {
#if BUILDING_INBOX_LIBRARY
                        _classMaterializerStrategy = new JsonReflectionEmitMaterializer();
#else
                        throw new PlatformNotSupportedException("TODO: IL Emit is not supported on .NET Standard 2.0.");
#endif
                    }
                    else if (ClassMaterializer == JsonClassMaterializer.Reflection)
                    {
                        _classMaterializerStrategy = new JsonReflectionMaterializer();
                    }
                    else
                    {
                        throw new InvalidOperationException("todo: invalid option");
                    }
                }

                return _classMaterializerStrategy;
            }
            set
            {
                _classMaterializerStrategy = value;
            }
        }
    }
}
