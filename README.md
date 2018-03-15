# .NET Core Lab 

This repo is for experimentation and exploring new ideas that may or may not make it into the main corefx repo.

### Build & Test Status

|    | x64 Debug | x64 Release |
|:---|----------------:|------------------:|
|**Windows NT**|[![x64-debug](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/windows_nt_debug/badge/icon)](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/windows_nt_debug/lastCompletedBuild/testReport)|[![x64-release](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/windows_nt_release/badge/icon)](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/windows_nt_release/lastCompletedBuild/testReport)
|**Ubuntu 16.04**|[![x64-debug](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/ubuntu16.04_debug/badge/icon)](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/ubuntu16.04_debug/lastCompletedBuild/testReport)|[![x64-release](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/ubuntu16.04_release/badge/icon)](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/ubuntu16.04_release/lastCompletedBuild/testReport)
|**OSX 10.12**|[![x64-debug](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/osx10.12_debug/badge/icon)](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/osx10.12_debug/lastCompletedBuild/testReport)|[![x64-release](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/osx10.12_release/badge/icon)](https://ci.dot.net/job/dotnet_corefxlab/job/master/job/osx10.12_release/lastCompletedBuild/testReport)

## Roadmap

While this repo is meant for experimentation, we still want to focus our efforts
in a particular direction, specifically to work on areas aligned with our
[roadmap](docs/roadmap.md).

The general idea is that we mature our thinking in this repo and either decide
not to pursue it or that we want to productize in which case we'll eventually
migrate the code to the appropriate location, usually the
[dotnet/corefx](https://github.com/dotnet/corefx) repository.

Of course, this doesn't mean we're not willing to explore areas that aren't part
of our roadmap, but we'd prefer if these would start with a document, and not
with code. This allows us to collaborate on how we want to approach specific
holes or issues with our platform without being drowned in large PRs.

## Components

Currently, this repo contains the following experimental components:

* **System.Slices**
These APIs moved to System.Memory (a component in the CoreFx repo) and to System.Buffers.Primitives (a component in this repo)

* **System.Buffers.Primitives**
A set of features for representing and manipulating managed, native buffers. The package complements Span\<T\> and ReadOnlySpan\<T\> primitives of System.Memory package. See more information about the features at [span.md](docs/specs/span.md) and [memory.md](docs/specs/memory.md).

* **System.Text.Formatting**. 
System.Text.Formatting APIs are similar to the existing StringBuilder and TextWriter APIs. 
They are designed to format values into text streams and to build complex strings. 
But these APIs are optimized for creating text for the Web. 
They do formatting with minimum GC heap allocations (1/6 of allocations in some scenarios) and can format directly to UTF8 streams. 
This can result in significant performance wins for software that does a lot of text formatting for the Web, e.g. generating HTML, JSON, XML. 
See more information on this component and code samples at the [Wiki]( https://github.com/dotnet/corefxlab/wiki). 

* **System.Text.Primitives**
The System.Text.Primitives library contains fast, non-allocating integral parsing APIs. They are designed for scenarios in which a byte buffer
and an index are accepted as input and a parsed value is desired as output (e.g. in a web server). These APIs present significant performance gains
over converting the buffer to a string, indexing into the string, and then parsing.

* **System.IO.FileSystem.Watcher.Polling**. 
.NET's FileSystemWatcher has low overhead, but it can miss some changes. This is acceptable in many scenarios, but in some, it might be not. 
This component, PollingWatcher, allows to monitory directory changes by polling, and so will never miss a change. It is optimized to minimize 
allocations, when no changes are detected. In fact, it does not allocate anything on the GC heap when there are no changes detected.

* **System.Time**.
This project augments the date and time APIs in .NET.  It adds two new core types: `Date` and `Time`.
These types will ultimately be submited for inclusion in `System.Runtime`.

* **System.Collections.Generic.MultiValueDictionary**.
The `MultiValueDictionary` is a generic collection that functions similarly to a `Dictionary<TKey, ICollection<TValue>>` with some added validation
and ease of use functions. It can also be compared to a Lookup with the exception that the `MultiValueDictionary` is mutable. It allows custom 
setting of the internal collections so that uniqueness of values can be chosen by specifying either a `HashSet<TValue>` or `List<TValue>`. Some of the
design decisions as well as introductions to usage can be found in the old blog posts introducing it [here](http://blogs.msdn.com/b/dotnet/archive/2014/06/20/would-you-like-a-multidictionary.aspx) and [here](http://blogs.msdn.com/b/dotnet/archive/2014/08/05/multidictionary-becomes-multivaluedictionary.aspx).

* **System.CommandLine**.
The purpose of this library is to make command line tools first class by providing a command line parser. Here are the goals: designed for cross-platform usage, lightweight with minimal configuration, optional but built-in support for help, validation, and response files, support for multiple commands, like version control tools. See the [README.md](src/System.CommandLine/README.md) for more information.

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

Symbols:
```
https://dotnet.myget.org/F/dotnet-corefxlab/symbols/
```

You can add this feed among your NuGet sources and install the packages (keep in mind that packages are pre-release packages).

## License

This project is licensed under the [MIT license](LICENSE).

## .NET Foundation

This project is a part of the [.NET Foundation].

[.NET Foundation]: http://www.dotnetfoundation.org/projects
[.NET Foundation forums]: http://forums.dotnetfoundation.org/

There are many .NET related projects on GitHub.

- [.NET home repo](https://github.com/Microsoft/dotnet) - links to 100s of .NET projects, from Microsoft and the community.
- [ASP.NET Core home](https://github.com/aspnet/home) - the best place to start learning about ASP.NET Core.

## Building and Testing

To build the projects in this repo, you have a few options:

* Download or install a [new version of the .NET CLI](https://github.com/dotnet/cli#installers-and-binaries) for your operating system. Then, simply invoke the tool to build individual projects (dotnet restore and then dotnet build).
* (On Windows) Invoke build.cmd. This will download an acceptable version of the .NET CLI automatically and use it to build the entire repository. NOTE: Don't invoke `scripts/build.ps1` directly. It requires that some environment be set in order for it to work correctly. `build.cmd` does this.
* (On Windows) Open the solution file in Visual Studio 2015. NOTE: This requires unreleased plugins to work at this point in time.
Using VS Code, see https://aka.ms/vscclrdogfood.
* If using Visual Studio, install the following VSIX to have IDE support for C#7.3 features that this project uses. - https://dotnet.myget.org/F/roslyn/vsix/0b5e8ddb-f12d-4131-a71d-77acc26a798f-2.8.0.6270809.vsix

## Measuring Performance

For details, please refer to the [PerfHarness documentation](scripts/PerfHarness/README.md).
