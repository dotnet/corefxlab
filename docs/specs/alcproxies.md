# Proxy Creation for AssemblyLoadContext to Replace AppDomain
## Problem
`AssemblyLoadContext`s currently work as an `Assembly` resolution context to work as a semi-replacement for `AppDomain`. They allow multiple versions of an `Assembly` to coexist in the same process. However, the implementation of `AssemblyLoadContext` creates only a `Type` boundary between an Application and context, compared to the `Object` boundary that `AppDomain` created. This can create complications for developers intending on using `AssemblyLoadContext` in similar ways to `AppDomain`.
In the past, a common use of AppDomain was to create objects in another `AppDomain` and use them as if they were a type in the original domain. 

## Comparison

### AppDomain
For AppDomains, developers could load objects and execute methods like so (Shortened version from [here](https://docs.microsoft.com/dotnet/api/system.appdomain?view=netframework-4.8 )):


```C#
public class SampleType : MarshalByRefObject
{
    // Call this method via a proxy.
    public void SomeMethod()
    {
        Console.WriteLine("Hello!");
    }
}
```

...and then within the main application...


```C#
...

AppDomainSetup ads = new AppDomainSetup();
ads.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

...

AppDomain ad2 = AppDomain.CreateDomain("AD #2", null, ads);

SampleType mbrt = 
            (SampleType) ad2.CreateInstanceAndUnwrap(
                exeAssembly, 
                typeof(SampleType).FullName
            );
			
mbrt.SomeMethod();

...
```

This allows for programs and classes to be loaded with an extra boundary of isolation, to help prevent things like unstable code bringing down a process. This was removed due to the support for the implementation (`TransparentProxy`) being fairly expensive.

### AssemblyLoadContext
Currently, there is no easy way to load an object from an ALC and call methods normally on it. However, you can use reflection like so:
```C#
AssemblyLoadContext alc = new AssemblyLoadContext(assemblyPath, IsCollectible: true); //Create the ALC
Assembly aa = alc.LoadFromAssemblyPath(assemblyPath); //Load the assembly into the ALC
Type classType = aa.GetType("AssemblyName.DesiredType");

// For instantiating below, you can also use Activator.GetInstance(classType) if you know there is a default constructor that takes no arguments
object obj = (classType.GetConstructor(Type.EmptyTypes).Invoke(new object[] { }));

obj.GetType().GetMethod("SomeMethod").Invoke(obj, null);
```
The problem here is that due to Type Isolation of ALCs, you cannot directly cast the instantiated object to its original type or an inherited type, as the CLR believes them to be different types from what's loaded from the ALC. 

There are two ways to use the instance from the ALC:
* Use reflection to invoke calls of the object's type, gotten directly using object.GetType()
* Use a shared assembly between the user and target assembly that holds a class/interface that the type you want inherits from, and using said shared type instead of the type you want directly (See [this example](https://github.com/dotnet/samples/tree/master/core/extensions/AppWithPlugin ) to see it working).
* Use `DispatchProxy` to create a proxy within the user ALC, pointing to an object in the target ALC. This also requires a shared assembly.

Anything requiring a shared assembly is inadequate for what we're trying to do, as one of the main benefits of AppDomains having totally separate types was with version compatibility. In .NET Framework, different versions of assemblies could be loaded into separate AppDomains, and communication could occur between objects as if they were one shared type. For example, if a user has a program that uses dependency A, and wants to load a plugin that has classes that inherit from older versions of dependency A, AppDomains would still be able to send and receive information between the plugin and the main program. If we enforce shared assemblies with this API, we lose the ability to allow for things like this.

## Proposed Solution

### Goals
This proposal is for an API that can effectively recreate the object creation capabilities in AppDomain. The goals of this design are:
* To allow for people are able to create isolated objects without the direct use of reflection. 
* To preserve AppDomains ease of use when creating isolated objects.
* To continue support for ALC unloads by destroying instantiated object when their ALC is unloaded, as well as other ALC features.
* To allow for plugin compatibility to work even with minor versioning changes being made to the application.
* Attempt to design around allowing inter-process communication between the user's process and the process where the object instance will be made.

One thing that's especially interesting is use of this design to not only allow for communication between ALCs, but expanding the design to work for inter-process communication.

### Design
```C#
interface ISampleInterface {
	public void SampleMethod();
	...
}
class SampleType : SampleInterface {
	...
	public void SampleMethod(){
		...
	}
	...
}

...


public static void Main(){
	AssemblyLoadContext alc = new AssemblyLoadContext(assemblyPath, isCollectible: true);
	ProxyBuilder p = new ProxyBuilder(typeof(SampleInterface), alc, assemblyPath);
	SampleInterface testObject = (SampleInterface)p.CreateObject("SampleType");
	testObject.SampleMethod();
	...

}
```
The API surface allows for developers to create the ProxyBuilder object, and then create an instance of the proxy type, which works exactly like a normal instance of the target type, with the limitation that the target type must inherit from an interface that can be used to build the proxy object. This does limit the use of the object slightly, as use of the target object is limited to methods that exist in the interface.

---

### General Design

![](../img/ALCProxyDesign.svg)

The ProxyBuilder API will generate five separate objects:
* `ProxyBuilder`: Responsible for creating `ProxyObject`, `ClientObject`, and `DispatchBuilder` and returns the instance of the desired proxy to the user for use.
* `DispatchBuilder`: Constructs `ServerObject` in the target ALC.
* `ProxyObject`: The object that the user has access to, it allows for method calls to run on the `TargetObject` in the target ALC. When a method is called, `ProxyObject` pipes its command to the `ServerObject`, which will eventually from the user context to the new proxy now in the target ALC using binary serialization.
* `ClientObject`: Sends messages to the `ServerObject` in the target ALC requesting `TargetObject`'s method to be called.
* `ServerObject`: Invokes `TargetObject`'s methods, and pipes the output back to `ClientObject` and the user ALC.

When a method is called, `ProxyObject` pipes its command to the `ClientObject`, which will serialize or encode the method information and any parameters passed to the `ProxyObject`, and send them to `ServerObject` through some sort of request. `ServerObject` then deserializes the data, and uses reflection to invoke the method in `TargetObject`. Once the method runs and `ServerObject` gets the returned object, it sends it back to `ClientObject`, which sends it back to `ProxyObject` to be "returned" to the user.

A `DispatchProxy` can be used to create a "fake" object of a given type, which acts as if it was the instance we wanted to use. Having `ProxyObject` inherit from `DispatchProxy` allows us to easily add more extra actions when we call the object, such as deserialization of objects across the type barrier (See the "Type-Isolation Barrier" section below).

This design is dependent on the ability for the `ProxyBuilder` to create dynamic assemblies and types within a different ALC/Process. Currently we may be able to do so by loading the `DispatchBuilder` type into the ALC directly, and then creating the proxy object from there, while also using a similar builder to create the user-side proxy that make gRPC calls. We can also wait (See [#38426](https://github.com/dotnet/corefx/issues/38426 )), but this option will not be available until said issue can be fixed, or a comparable fix is made.

### Variant for ALC Revisions with `DispatchProxy`

![](../img/ALCProxyAlternate.svg)

One thing that's nice about `DispatchProxy` is that it allows for us to load a target Type directly from the target ALC to the user ALC, allowing us to remove the `ServerObject`, and instead just call the object's methods directly. This only works for in-process communications, though, and by removing `ServerObject`, we may lose the ability to allow for consistent unloadability (due to how the `TargetObject` may return objects), so this approach needs to be investigated further.


### The Type-Isolation barrier, and Client/Server send options
One of the problems with getting proxies like this working is getting past the type-isolation barrier from ALCs. For example, if you want to use type `Foo`, loading `Foo` directly vs using an ALC generates what the runtime says are completely different types, even if the implementation is the same. 

![](../img/ALCContextError.PNG)

This happens for any non-simple types that attempt to cross the barrier, which is why API needs to use reflection to get the type of the ProxyObject instead of being able to grab the equivalent type in the user context.

The type-isolation barrier becomes a larger issue for parameters and return types in particular, as when we pass a `Foo` type into the proxy, we need to transform it into the `ALC.Foo` type during the transition. There are a few ways we could do this:
* Build proxies for each non-simple type that can communicate across the ALC boundaries
* Make everything a shared type (This is not recommended)
* Encode any passed objects and decode them on the other side, creating an effective "copy" of the type in the form `ALC.Foo`

The first two have their own issues, as we would either need to figure out how to build those proxies on the fly from the IL-Generated `ProxyObject`/`ServerObject`, or lose out on the benefit of versioning compatibility. The third option is what `ServerObject` and `ClientObject` intend on setting a framework for; allowing for a (possibly interchangeable) connection framework to be used to send messages to and from the ALCs (or processes).

There are a few ways we could implement the client/server encoding and messaging. The few that I've been looking at are:
* Add a second `DispatchProxy` that can call across the barrier and keep our abstraction layer.
* Using `BinaryFormatter` to serialize data across the boundary
* Similar serialization with `DataContractSerializer`
* Using gRPC to send objects across

#### Second `DispatchProxy`
Making the `ClientObject` a `DispatchProxy` object would work for making calls over ALCs, though not for out-of-process calls. It's not the perfect solution, as it would require two `Reflection.Invoke` calls (one from client to server, and one from server to target), but it would allow us to enforce our isolation policy from the custom `ServerObject` being able to mess with our return calls to make sure we were enforcing our constraints (see "ALC Unloading" below).

#### Binary Serialization
Using `BinaryFormatter`, types and their information can be serialized and sent across the ALC barrier, and deserialized using a class for decoding, which can then call the specific `TargetObject` methods.

While Binary Serialization in concept is a good idea, there are too many issues that make this an ineffective solution. `BinaryFormatter` is a depricated API, many types aren't serializable, limiting the use of types we could proxy. 

#### XML/JSON Serialization using `DataContractSerializer`
Instead of moving a specific type through a `BinaryFormatter`, using `DataContractSerializer` would allow us to move any public or private pieces of an object, and then recreate it on the other side.

Benefits of `DataContractSerializer`:
* It may be easier compared to `BinaryFormatter` to cast this effectively across the type-isolation barrier.
* Adding attributes is similar to `BinaryFormatter`, so any type should be able to make it across the barrier.

Concerns with `DataContractSerializer`:
* Many similar concerns to `BinaryFormatter`, such as potential security issues and the manual use of [DataContractAttribute] and [DataMemberAttribute] needing to be used. There may also be limits to what types can be serialized using `DataContractSerializer`.

#### gRPC
Using the [gRPC](https://grpc.io/ ) framework, we'll have a much easier time simulating inter-process communication, as it's a major part of the framework.

Benefits of gRPC:
* Easy inter-process communication.
* Simple easy to implement framework, with [some support for .NET](https://github.com/grpc/grpc-dotnet ).

Concerns with gRPC:
* Encoding objects in a way that we can manually decode and pass them into the `TargetObject` needs to be investigated further, to make sure it works.
* Limited to using http/2 for transport, which doesn't work for in-process communication (at least for current .NET implementations)


The current plan is to build with either a second `DispatchProxy` for in-proc ALCs and gRPC for cross-proc connections. This is since serialization has a lot of potential issues that may affect the API later down the road. gRPC will probably work fine for inter-process communication, but for in-proc, we need to find an alternative to communicate between ALCs.

### Extensibility
One nice thing about the design is its ability to have its components "slotted" in and out. The main part of the project that has multiple options/paths that could be taken are all in the implementation of `ServerObject` and `ClientObject`. As long as the client can receive an instruction with info from the `ProxyObject`, and the server can invoke the `TargetObject`, the middle communication between the two can be implemented in multiple ways. If implemented right, this allows for the project to be open for anyone to add their own implementation of the communication between ALCs to suit their needs, possibly using some of the alternative ideas listed in this document.

### Notes on handling specifics

#### Versioning
There are a few guidelines we're depending on to make sure versioning works for our interfaces:
* The name of the interface needs to remain the same.
* Method signatures that have already been created can't be edited or deleted, though new ones can be added.

There may be cases where the main program attempts to make a method call to an older plugin, where the method doesn't exist. When the API makes a call to `TargetObject` through `ServerObject`, it will be using `Reflection.Invoke`, meaning if there ever is a case where we don't have the requested method we're calling, the invoke should throw a `TargetException`, which we could *potentially* do something with, but probably not.

#### ALC Unloading
During the `AssemblyLoadContext.Unloading` event, the `ClientObject` can remove any reference it has to the `ServerObject`, allowing for the server to unload with the ALC. Since there are no other connections into the ALC other then the connection between client and server, the target ALC should be able to unload everything in itself correctly. Afterwards, the `ProxyObject` could remove its reference to the `ClientObject`, opening it up to be collected by the GC. 

There's no need for the server to dereference the target object, since they are both within the target ALC, and can be removed during the unload process.

There are a few potential places where this isolation isn't perfect, which is discussed in the next point.

#### Call-by-value vs more proxy creation
Let's say you want to call method `M()` on your `TargetObject`, which returns an object of type `A`, a non-primitive. When it returns the object from the target ALC (where `A` was created) back to the user ALC (where we made the original call for `M()`), we've created an extra reference between the ALCs that we need to deal with. There are a few ways we could try and fix this issue:
* Don't support non-primitive objects being passed to and from the ALCs
This isn't recommended since it limits use of the target ALC by quite a lot, but for a quick and dirty version, it could work.
* For each non-primitive object, generate another proxy that passes back to the user ALC instead of the reference to the normal object. When we unload, we can then remove this proxy as well, keeping the isolation barrier intact.
This is very similar to how AppDomains handle MarshallByRef objects, and it keeps the functionality the same, but also adds a lot more overhead from creating the proxies during runtime for every non-primitive object.
* Keep everything call-by-value, by making sure any `ServerObject` returns to the user ALC is the metadata of any object, that gets reconstructed when returning to `ClientObject`
This option is nice since it works for both inter-process and in-process communication. However, having every object only able to return by value means that we'll be limited to making calls changing objects to a single ALC, and the multiple copies of an object may increase space and cause performance issues.

For my planned implementation, I'll be trying to implement call-by-value, since it keeps the design consistent for all types (primitive and non-primitive) and for in-proc vs cross-proc. However, due to the design for extensibility, it's possible that anyone who wants to change the API can implement any of these options in their own way, by changing the interactions of `ClientObject` and `ServerObject`. If someone wants to limit interactions between plugins and the main context to primitives only, they're welcome to do so with this design.

#### Generic Types
For generics, the API should probably be able to allow users to pass in a list of types "in order" that are being used for the particular instance of an object they are creating. 

`DispatchProxy` should be OK to use generic types, but it may be an issue funneling these types through whatever server structure is created. 

For constraints on certain generics, the compiler should pick up any issues with constraints from the given interface, but may not from constraints for the TargetObject, and will throw an error.

#### Performance
No matter the performance of this API, by design it won't have the broader performance issues that AppDomains had in .Net Framework, since they work very similarly to normal classes. However, there may be some overhead and performance issues on any objects being contacted by the proxies, which should be investigated if they come up.

Some hard points of performance that probably will need to be looked at:
* Client/server calls on in-process ALCs
* Multiple uses of `Reflection.Invoke` possibly being called

#### Special Language Features
Language features such as `in`, `ref`, and `out` should be investigated further, but we probably don't want them since the design is based around keeping everything call-by-value.

Other language features should also be investigated for the future.

## Open Questions
* The current prototype for proof-of-concept has the `ServerObject` using `Reflection.Invoke()` to call the TargetObject methods, is there a more performant solution to running the methods of TargetObject?
* How should the API deal with generic types when creating our different proxy classes?
* Is there anything special that we need to do to deal with other language features, such as lambdas?
