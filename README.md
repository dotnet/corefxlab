# .NET Core Lab

This repo is for experimentation and exploring new ideas that may or may not make it into the main corefx repo.

Currently, this repo contains the following experimental components:

* **System.Text.Formatting**. 
System.Text.Formatting APIs are similar to the existing StringBuilder and TextWriter APIs. 
They are designed to format values into text streams and to build complex strings. 
But these APIs are optimized for creating text for the Web. 
They do formatting with minimum GC heap allocations (1/6 of allocations in some scenarios) and can format directly to UTF8 streams. 
This can result in significant performance wins for software that does a lot of text formatting for the Web, e.g. generating HTML, JSON, XML. 
See more information on this component and code samples at the [Wiki]( https://github.com/dotnet/corefxlab/wiki). 

* **System.IO.FileSystem.Watcher.Polling**. 
.NET's FileSystemWatcher has low overhead, but it can miss some changes. This is acceptable in many scenarios, but in some, it might be not. 
This component, PollingWatcher, allows to monitory directory changes by polling, and so will never miss a change. It is optimized to minimize 
allocations, when no changes are detected. In fact, it does not allocate anything on the GC heap when there are no changes detected. 

* **System.Threading.Tasks.Channels**.
The System.Threading.Tasks.Channels library provides a set of synchronization data structures for passing data between producers and consumers. 
Whereas the existing System.Threading.Tasks.Dataflow library is focused on pipelining and connecting together dataflow "blocks" which encapsulate 
both storage and processing, System.Threading.Tasks.Channels is focused purely on the storage aspect, with data structures used to provide the 
hand-offs between participants explicitly coded to use the storage. The library is designed to be used with async/await in C#.  See the
[README.md](https://github.com/dotnet/corefxlab/blob/master/src/System.Threading.Tasks.Channels/README.md) for more information.

* **System.Time**.
This project augments the date and time APIs in .NET.  It adds two new core types: `Date` and `TimeOfDay`.
It also provides extension methods to enhance the functionality of the existing `DateTime`, `DateTimeOffset` and `TimeZoneInfo` types.

* **System.Collections.Generic.MultiValueDictionary**.
The MultiValueDictionary is a generic collection that functions similarly to a Dictionary<TKey, ICollection<TValue>> with some added validation
and ease of use functions. It can also be compared to a Lookup with the exception that the MultiValueDictionary is mutable. It allows custom 
setting of the internal collections so that uniqueness of values can be chosen by specifying either a HashSet<TValue> or List<TValue>. Some of the
design decisions as well as introductions to usage can be found in the old blog posts introducing it [here](http://blogs.msdn.com/b/dotnet/archive/2014/06/20/would-you-like-a-multidictionary.aspx) and [here](http://blogs.msdn.com/b/dotnet/archive/2014/08/05/multidictionary-becomes-multivaluedictionary.aspx).

* **System.CommandLine**.
The purpose of this library is to make command line tools first class by providing a command line parser. Here are the goals: designed for cross-platform usage, lightweight with minimal configuration, optional but built-in support for help, validation, and response files, support for multiple commands, like version control tools. See the [README.md](https://github.com/dotnet/corefxlab/blob/master/src/System.CommandLine/README.md) for more information.

More libraries are coming soon. Stay tuned!

[blog post]: http://blogs.msdn.com/b/dotnet/archive/2014/11/12/net-core-is-open-source.aspx

## Related Projects

For an overview of all the .NET related projects, have a look at the
[.NET home repository](https://github.com/Microsoft/dotnet).

## How to Use
You can get the .NET Core Lab packages from **dotnet-corefxlab** MyGet feed: 

```
https://dotnet.myget.org/F/dotnet-corefxlab/

or

https://dotnet.myget.org/F/dotnet-corefxlab/api/v3/index.json (preview support)
```

You can add this feed among your NuGet sources and install the packages (keep in mind that packages are pre-release packages).

## License

This project is licensed under the [MIT license](LICENSE).

## .NET Foundation

This project is a part of the [.NET Foundation].

[.NET Foundation]: http://www.dotnetfoundation.org/projects
[.NET Foundation forums]: http://forums.dotnetfoundation.org/

## Building and Testing

To build the projects in this repo, you have a few options:

* Download or install a new version of the .NET CLI from here for your operating system. Then, simply invoke the tool to build individual projects (dotnet restore and then dotnet build).
* (On Windows) Invoke build.cmd. This will download an acceptable version of the .NET CLI automatically and use it to build the entire repository. NOTE: Don't invoke `scripts/build.ps1` directly. It requires that some environment be set in order for it to work correctly. `build.cmd` does this.
* (On Windows) Open the solution file in Visual Studio 2015. NOTE: This requires unreleased plugins to work at this point in time.
Using VS Code, see https://aka.ms/vscclrdogfood.
