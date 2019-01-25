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
        private volatile JsonMemberBasedClassMaterializer _classMaterializerStrategy;
        private JsonClassMaterializer _classMaterializer;
        private int _defaultBufferSize = 16 * 1024;
        private int _maxDepth = 64;
        private bool _hasRuntimeCustomAttributes;

        private static readonly Dictionary<ICustomAttributeProvider, object[]> s_reflectionAttributes = new Dictionary<ICustomAttributeProvider, object[]>();
        private readonly Lazy<Dictionary<ICustomAttributeProvider, List<Attribute>>> _runtimeAttributes = new Lazy<Dictionary<ICustomAttributeProvider, List<Attribute>>>();

        private static readonly ConcurrentDictionary<Type, JsonClassInfo> s_classes = new ConcurrentDictionary<Type, JsonClassInfo>();
        private readonly ConcurrentDictionary<Type, JsonClassInfo> _local_classes = new ConcurrentDictionary<Type, JsonClassInfo>();

        //todo: exception once used on all globals

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

        public JsonReaderOptions ReaderOptions { get; set; }

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

        //public static void AddAttribute(ICustomAttributeProvider type, Attribute attribute)
        //{
        //    if (type == null)
        //        throw new ArgumentException(nameof(type));

        //    if (attribute == null)
        //        throw new ArgumentNullException(nameof(attribute));

        //    if (!_runtimeAttributes.Value.TryGetValue(type, out List<Attribute> attributes))
        //    {
        //        _runtimeAttributes.Value.Add(type, attributes = new List<Attribute>());
        //    }

        //    attributes.Add(attribute);
        //    _hasRuntimeCustomAttributes = true;
        //}

        public void AddAttribute(ICustomAttributeProvider type, Attribute attribute)
        {
            if (type == null)
                throw new ArgumentException(nameof(type));

            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            if (!_runtimeAttributes.Value.TryGetValue(type, out List<Attribute> attributes))
            {
                _runtimeAttributes.Value.Add(type, attributes = new List<Attribute>());
            }

            attributes.Add(attribute);
            _hasRuntimeCustomAttributes = true;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>(ICustomAttributeProvider type, bool inherit = false) where TAttribute:Attribute
        {
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
                        _classMaterializerStrategy = new JsonDefaultMaterializer(this);
                    }
                    else if (ClassMaterializer == JsonClassMaterializer.ReflectionEmit)
                    {
                        _classMaterializerStrategy = new JsonReflectionEmitMaterializer();
                    }
                    else if (ClassMaterializer == JsonClassMaterializer.Reflection)
                    {
                        _classMaterializerStrategy = new JsonReflectionMaterializer();
                    }
                    else
                    {
                        throw new InvalidOperationException("todo");
                    }
                }

                return _classMaterializerStrategy;
            }
            set
            {
                _classMaterializerStrategy = value;
            }
        }

        /// <summary>
        /// Determine if Reflection.Emit is supported with try\catch in this single location to reduce subsequent overhead.
        /// </summary>
        private class JsonDefaultMaterializer : JsonMemberBasedClassMaterializer
        {
            private JsonConverterSettings _settings;

            public JsonDefaultMaterializer(JsonConverterSettings settings)
            {
                _settings = settings;
            }

            public override JsonClassInfo.ConstructorDelegate CreateConstructor(Type type)
            {
                try
                {
                    var testMaterializer = new JsonReflectionEmitMaterializer();
                    JsonClassInfo.ConstructorDelegate value = testMaterializer.CreateConstructor(type);
                    _settings._classMaterializerStrategy = testMaterializer;
                    return value;
                }
                catch (NotSupportedException)
                {
                    var fallbackMaterializer = new JsonReflectionMaterializer();
                    _settings._classMaterializerStrategy = fallbackMaterializer;
                    return fallbackMaterializer.CreateConstructor(type);
                }
            }

            public override JsonPropertyInfo<TValue>.GetterDelegate CreateGetter<TValue>(PropertyInfo propertyInfo)
            {
                // Should never get here as we create constructors first
                Debug.Fail("Should never get here");
                return default;
            }

            public override JsonPropertyInfo<TValue>.SetterDelegate CreateSetter<TValue>(PropertyInfo propertyInfo)
            {
                // Should never get here as we create constructors first
                Debug.Fail("Should never get here");
                return default;
            }
        }
    }
}
