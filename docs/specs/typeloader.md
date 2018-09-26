# System.Reflection.TypeLoader

## Introduction

.Net Core does not support the ReflectionOnlyLoad feature of NETFX. ReflectionOnlyLoad was a feature for inspecting managed assemblies using the familiar Reflection api (`Type`, `MethodInfo`, etc.)
The `TypeLoader` class is the .NET Core replacement for this feature.

## Why a new api?

ReflectionOnlyLoad on NETFX was not a separately designed feature. It was grafted onto standard Reflection (it is a special assembly context with Invoke paths turned off) and as such
imposes many restrictions:

- Despite not supporting execution, you cannot use ReflectionOnlyLoad to browse assemblies built for a different CPU architecture or bitness than the platform you are running on.

- You are restricted to a single ReflectionOnlyLoad context per process. Thus, every user of ReflectionOnlyLoad affects global state. If the user is a library,
  this is especially impolite.

- You cannot load a different core assembly (`mscorlib` or `System.Private.CoreLib`) than the one that your app is running on. This is a wholly arbitrary restriction
  and exists only because ReflectionOnlyLoad is hacked onto the runtime Reflection system where the core assembly being a one known global was heavily embedded into the implementation.

Because of this, ReflectionOnlyLoad on NETFX is an API oxymoron. Its very architecture undercuts its core premise of treating managed assemblies as
pure data, independent of the execution requirements of the underlying runtime.

## Api Surface Area

```
namespace System.Reflection
{
    public sealed class TypeLoader : IDisposable
    {
        public TypeLoader();
        public TypeLoader(string coreAssemblyName);

        public Assembly LoadFromAssemblyPath(string assemblyPath);
        public Assembly LoadFromByteArray(byte[] assembly);
        public Assembly LoadFromStream(Stream assembly);

        public Assembly LoadFromAssemblyName(AssemblyName assemblyName);
        public Assembly LoadFromAssemblyName(string assemblyName);

        public event Func<TypeLoader, AssemblyName, Assembly> Resolving;

        public string CoreAssemblyName { get; set; }

        public IEnumerable<Assembly> GetAssemblies();

        public void Dispose();
    }
}
```

### Example: Display an assembly's immediate dependencies

```
static void Main(string[] args)
{
    using (TypeLoader tl = new TypeLoader())
    {
        Assembly a = tl.LoadFromAssemblyPath(args[0]);
        Console.WriteLine("Assembly: " + a.GetName());
        Console.WriteLine("Dependencies:");
        foreach (AssemblyName aref in a.GetReferencedAssemblies())
        {
            Console.WriteLine("  " + aref);
        }
    }
}
```

This is a simple but handy command line utility. Note that because of the limited apis it uses, this utility works
fine without the TypeLoader knowing where any of the assembly's dependencies live (even mscorlib!) After all, all of the
data it needs resides physically within the assembly itself.


## TypeLoaders are isolated universes of types.

Each `TypeLoader` represents a closed universe of Type objects loaded for inspection-only purposes.
Each `TypeLoader` defines its own binding rules and is isolated from all other TypeLoaders.

Another way to look at `TypeLoaders` is as a dictionary binding assembly names to loaded Assembly instances.

## TypeLoaders treat assemblies as pure data. You cannot execute them.

TypeLoaders treat assemblies strictly as metadata. There are no restrictions on loading assemblies based
on target platform, CPU architecture or pointer size. There are no restrictions on the assembly designated
as the core assembly ("mscorlib"). 

Also, as long as the metadata is "syntactically correct", the TypeLoader strives to report it "as is" (as long it 
can do so in a way that can be distinguished from valid data) and refrains from judging whether it's "executable." 
This is both for performance reasons (checks cost time) and its intended role as metadata inspection tool.
Examples of things that TypeLoaders let go unchecked include creating generic instances that violate generic 
parameter constraints, and loading type hierachies that would be unloadable in an actual runtime (deriving from sealed classes,
overriding members that don't exist in the ancestor classes, failing to implement all abstract methods, etc.)

You cannot invoke methods, set or get field or property values or instantiate objects using 
the Type objects from a TypeLoader. You can however, use FieldInfo.GetRawConstantValue(),
ParameterInfo.RawDefaultValue and PropertyInfo.GetRawConstantValue(). You can retrieve custom attributes
in CustomAttributeData format but not as instantiated custom attributes. The CustomAttributeExtensions
extension api will not work with these Types nor will the IsDefined() family of api.

## You are in complete charge of the binding policy.

There is no binding policy baked into the TypeLoader. You must either manually load all dependencies
or subscribe to the Resolving event to load dependencies as needed. The TypeLoader strives to avoid 
loading dependencies unless needed. Therefore, it is possible to do useful analysis of an assembly even
in the absence of dependencies. For example, retrieving an assembly's name and the names of its (direct)
dependencies can be done without having any of those dependencies on hand.

To bind assemblies, the TypeLoader raises the Resolving event. You implement the binding algorithm by
subscribing to that event. The event should load the requested assembly and return it. To do this,
it can use LoadFromAssemblyPath() or one of its variants (LoadFromStream(), LoadFromByteArray()).

### Example: Display the type-forwarders in an assembly

```
static void Main(string[] args)
{
    using (TypeLoader tl = new TypeLoader())
    {
        tl.Resolving +=
            delegate (TypeLoader sender, AssemblyName assemblyName)
            {
                const string probeDirectory = "...";
                string simpleName = assemblyName.Name;
                string path = Path.Combine(probeDirectory, simpleName + ".dll");
                if (!File.Exists(path))
                    return null;  // Don't throw an exception if the assembly doesn't exist. Return null.
                return sender.LoadFromAssemblyPath(path);
            };
    
        Assembly a = tl.LoadFromAssemblyPath(args[0]);
        foreach (Type t in a.GetForwardedTypes())
        {
            Console.WriteLine(t.AssemblyQualifiedName);
        }
    }
}
```

This is a task that requires that the TypeLoader locate dependencies. The TypeLoader itself cannot locate
dependencies on its own. You should subscribe to the `Resolving` event as this example does. This sample
implements a very simple-minded but effective binding policy that ignores version, culture and public key
and looks for a file named "*name*.dll" in a single directory.

### Resolving Event Handlers and error indication

To indicate that a requested assembly cannot be found, event handlers should return `null`
rather than throwing an exception. This will commit the failure (the event handler will not get
called again for the same name) and TypeLoader will surface the FileNotFoundException as appropriate.

If the assembly is found but has a problem (such a bad image format or an IO error), event handlers
should throw (or more likely, allow the BadImageException thrown by the Load api to go unhandled.)

TypeLoaders do not catch exceptions thrown out of handlers. They will propagate out to the application
and no binding will occur.

Following these rules will ensure that the "throwOnError: false" version of Assembly.GetType() filter errors as intended.
### More about Resolving event handlers

Once an assembly has been bound, no assembly with the same assembly name identity
can be bound again from a different location unless the MVID's are identical.

Handlers will generally not get called twice for the assembly name, unless two threads race to
resolve the same name. Even then, only one returned Assembly will safely "win the race" and be bound to
the name - the other is silently discarded.

TypeLoaders intentionally perform no "ref-def" matching. This is because the question of whether
a ref name "matches" a def name is a policy which TypeLoaders strive to not to prescribe. It also
the kind of arbitrary restriction that TypeLoaders strives not to have.


## For full functionality, TypeLoaders need to be told what the core assembly is.

The ECMA metadata format uses a special encoding for certain "well known system types" such as
String, Int32 and Object. This encoding does not specify the assembly that the type lives in. The
runtime that loads the assembly is expected to "just know." This "core assembly" is typically named
`mscorlib`, `System.Private.CoreLib` or `netstandard`.

Since TypeLoaders are not bound to any particular runtime implementation, it cannot "just know." You must
tell it the assembly name of the core assembly. You can do this in one of two ways:

1. Pass it as an argument to the `TypeLoader` constructor.

2. Set the CoreAssemblyName property after construction.

The core assembly name must be a parseable assembly name. It does not need to be a full strong name.
All that matters is that your `Resolving` event handler accepts the name and returns an acceptable core assembly.

The designated core assembly does not need to contain the core types physically. It can type-forward them to another
assembly. So the "mscorlib" facade that ships with .Net Core is actually an excellent choice for a core assembly.

You should avoid specifying "System.Runtime" as the core assembly name as this assembly lacks some of the
interop-related pseudo custom attribute types such as `DllImportAttribute`. If you have no interest
in probing for pseudo custom attributes, this assembly may suffice. The custom attribute apis will
skip those attributes in their results.

### Example: Display the full type hierarchy of a selected type:

```
static void Main(string[] args)
{
    const string probeDirectory = "...";
    
    using (TypeLoader tl = new TypeLoader())
    {
        tl.Resolving +=
            delegate (TypeLoader sender, AssemblyName assemblyName)
            {
                string simpleName = assemblyName.Name;
                string path = Path.Combine(probeDirectory, simpleName + ".dll");
                if (!File.Exists(path))
                    return null;  // Don't throw an exception if the assembly doesn't exist. Return null.
                return sender.LoadFromAssemblyPath(path);
            };
    
        Assembly mscorlib = tl.LoadFromAssemblyPath(Path.Combine(probeDirectory, "mscorlib.dll"));
        tl.CoreAssemblyName = mscorlib.FullName;
    
        Assembly a = tl.LoadFromAssemblyPath(args[0]);
        Type t = a.GetType(args[1], throwOnError: true);
        while (t != null)
        {
            Console.WriteLine(t.FullName);
            t = t.BaseType;
        }
    }
}
```

This is a task that requires specifying the core assembly name. That's because a base type can
be a typespec (e.g. `EqualityComparer<string>`) which specifies `string` using the short signature form.
TypeLoaders can only locate the `String` type if you specified a core assembly.

### The core assembly name: setting via the constructor vs. the property.

Setting the core name upfront is the cleanest option. In some cases, though, you may wish to use the TypeLoader to
discover the core assembly identity and set the core assembly name afterward using the property. You can safely use
the following api:

```
TypeLoader.LoadFromStream(), LoadFromAssemblyPath(), LoadFromByteArray()
Assembly.GetName(), Assembly.FullName, Assembly.GetReferencedAssemblies()
Assembly.GetTypes(), Assembly.DefinedTypes, Assembly.GetExportedTypes(), Assembly.GetForwardedTypes()
Assembly.GetType(string, bool, bool)
Type.Name, Type.FullName, Type.AssemblyQualifiedName
```

as none of these will trigger an internal search of the core assembly. These may be useful in identifying the core assembly.

Once the TypeLoader has used the CoreAssemblyName, however, you can no longer change this property. Attempting to set the CoreAssemblyName
after this point will trigger an InvalidOperationException.

If you do not specify a core assembly, or the core assembly cannot be bound or if the core assembly is missing types, this will 
affect the behavior of the TypeLoader as follows:

- Apis that need to parse signatures or typespecs and return the results as Types will throw. For example, 
  MethodBase.ReturnType, MethodBase.GetParameters(), Type.BaseType, Type.GetInterfaces().

- Apis that need to compare types to well known core types will not throw and the comparison will evaluate to "false."
  For example, if you do not specify a core assembly, Type.IsPrimitive will return false for everything,
  even types named "System.Int32". Similarly, Type.GetTypeCode() will return TypeCode.Object for everything.

- If a metadata entity sets flags that surface as a pseudo-custom attribute, and the core assembly does not contain the pseudo-custom attribute
  type, the necessary constructor or any of the parameter types of the constructor, the TypeLoader will not throw. It will omit the pseudo-custom
  attribute from the list of returned attributes.

## TypeLoaders must be explicitly disposed.

Once loaded, the underlying file may be locked for the duration of the TypeLoader's lifetime. The TypeLoader can
also allocate native memory blocks. You can release both by disposing the TypeLoader object. The behavior of any Type,
Assembly or other reflection objects handed out by the TypeLoader is undefined after disposal. Though objects provided
by the TypeLoader strive to throw an ObjectDisposedException, this is not guaranteed. Some apis may return fixed or previously
cached data. Accessing objects *during* a Dispose may result in a unmanaged access violation and failfast.

Individual assemblies cannot be disposed. They can only be disposed collectively by disposing the TypeLoader
that created them.

## TypeLoaders are not AssemblyLoadContexts

The api surface area of `TypeLoader` superfically resembles that of `AssemblyLoadContext`. This is not
surprising as they are both assembly loaders. However, TypeLoaders are not a special kind of AssemblyLoadContext
and should not be confused as such. `AssemblyLoadContext` is still runtime Reflection, albeit extended
to allow loading multiple instances of the assembly. `TypeLoaders` are cross-platform data-based Reflection.

## Equality of Type, Member and Assembly objects

NETFX runtime Reflection never made promises about how Reflection objects other than System.Type were compared
(in practice, caching made equality "work" often enough for apps to rely on them. Unfortunately.)

TypeLoader, on the other, makes explicit promises:


The right way to compare two Reflection objects dispensed by the TypeLoader are:

```
      m1 == m2
      m1.Equals(m2)
```

but not

```
      object.ReferenceEquals(m1, m2) // WRONG
      (object)m1 == (object)m2       // WRONG
```


That the following descriptions are not literal descriptions of how Equals() is implemented. The TypeLoader
reserves the right to implement Equals() as "object.ReferenceEquals()" and intern the associated objects in such
a way that Equals() works "as if" it were comparing those things.

- Each TypeLoader permits only one Assembly instance per assembly identity so equality of assemblies is the same as the
  equality of their assembly identity.

- Modules are compared by comparing their containing assemblies and their row indices in the assembly's manifest file table.

- Defined types are compared by comparing their containing modules and their row indices in the module's TypeDefinition table.

- Constructed types (arrays, byrefs, pointers, generic instances) are compared by comparing all of their component types.

- Generic parameter types are compared by comparing their containing Modules and their row indices in the module's GenericParameter table.

- Constructors, methods, fields, events and properties are compared by comparing their declaring types, their row indices in their respective
  token tables and their ReflectedType property.

- Parameters are compared by comparing their declaring member and their position index.

## Multithreading
   
The TypeLoader and the reflection objects it hands out are all multithread-safe and logically immutable, 
with the following provisos:

- Adding (or removing) handlers to the TypeLoader's events is not thread-safe. These should be done prior
  to any multithreaded access to the TypeLoader.

- No Loads or inspections of reflection objects can be done during or after disposing the owning TypeLoader.

## Supported Target Frameworks

`System.Reflection.TypeLoader` builds in two different flavors: netcore and netstandard.

.NET Core added a number of apis (IsSZArray, IsVariableBoundArray, IsTypeDefinition, IsGenericTypeParameter, IsGenericMethodParameter,
HasSameMetadataDefinitionAs, to name a few.) to the Reflection surface area.

The Reflection objects dispensed by TypeLoaders support all the new apis *provided* that you are using the netcore build of System.Reflection.TypeLoader.dll.

If you are using the netstandard build of System.Reflection.TypeLoader.dll, the NetCore-specific apis are not supported. Attempting to invoke
them will generate a NotImplementedException or NullReferenceException (unfortunately, we can't improve the exceptions thrown because
they are being thrown by code this library doesn't control.) Because of this, it is recommended that apps built against NetCore use the NetCore build of
TypeLoader.dll.



