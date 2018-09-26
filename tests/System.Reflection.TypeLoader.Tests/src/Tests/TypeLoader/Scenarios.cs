// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using SampleMetadata;

using Xunit;

namespace System.Reflection.Tests
{
    using Console = global::System.Reflection.Tests.FakeConsole;  // Must be inside namespace for redirect to work properly.

    public static partial class TypeLoaderTests
    {
        [Fact]
        public static void Scenario_GetAssemblyName()
        {
            // Ensure you can do all this without resolving dependencies.
            using (TypeLoader tl = new TypeLoader())
            {
                Assembly a = tl.LoadFromAssemblyPath(typeof(GenericClass1<>).Assembly.Location);
                AssemblyName assemblyName = a.GetName();
                Console.WriteLine(assemblyName.FullName);
            }
        }

        [Fact]
        public static void Scenario_EnumerateDependencies()
        {
            // Ensure you can do all this without resolving dependencies.
            using (TypeLoader tl = new TypeLoader())
            {
                Assembly a = tl.LoadFromAssemblyPath(typeof(GenericClass1<>).Assembly.Location);
                foreach (AssemblyName name in a.GetReferencedAssemblies())
                {
                    Console.WriteLine(name.FullName);
                }
            }
        }

        [Fact]
        public static void Scenario_FindACoreAssembly()
        {
            // Ensure you can do all this without setting a core assembly.
            using (TypeLoader tl = new TypeLoader())
            {
                Assembly[] candidates =
                {
                    tl.LoadFromAssemblyPath(typeof(GenericClass1<>).Assembly.Location),
                    tl.LoadFromAssemblyPath(typeof(object).Assembly.Location),
                };

                foreach (Assembly candidate in candidates)
                {
                    Type objectType = candidate.GetType("System.Object", throwOnError: false);
                    if (objectType != null)
                    {
                        // Found our core assembly. Ensure it's not too late to set the CoreAssemblyName property.
                        tl.CoreAssemblyName = objectType.Assembly.GetName().FullName;
                        return;
                    }
                }

                Assert.True(false, "Did not find a core assembly.");
            }
        }

        [Fact]
        public static void Scenario_EnumerateTypesAndMembers()
        {
            // Ensure you can do all this without resolving dependencies.
            using (TypeLoader tl = new TypeLoader())
            {
                Assembly a = tl.LoadFromAssemblyPath(typeof(GenericClass1<>).Assembly.Location);
                foreach (TypeInfo t in a.DefinedTypes)
                {
                    Console.WriteLine(t.FullName);
                    foreach (ConstructorInfo c in t.DeclaredConstructors)
                    {
                        Console.WriteLine("  " + c.ToString());
                    }

                    foreach (MethodInfo m in t.DeclaredMethods)
                    {
                        Console.WriteLine("  " + m.ToString());
                    }

                    foreach (PropertyInfo p in t.DeclaredProperties)
                    {
                        Console.WriteLine("  " + p.ToString());
                    }

                    foreach (FieldInfo f in t.DeclaredFields)
                    {
                        Console.WriteLine("  " + f.ToString());
                    }

                    foreach (EventInfo e in t.DeclaredEvents)
                    {
                        Console.WriteLine("  " + e.ToString());
                    }
                }
            }
        }
    }

    internal static class FakeConsole
    {
        public static void WriteLine(string s) { }
    }
}


