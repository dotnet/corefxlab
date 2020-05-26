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

* **System.Buffers.Primitives**
A set of features for representing and manipulating managed, native buffers. The package complements Span\<T\> and ReadOnlySpan\<T\> primitives of System.Memory package. See more information about the features at [span.md](docs/specs/span.md) and [memory.md](docs/specs/memory.md).

* **System.IO.FileSystem.Watcher.Polling**. 
.NET's FileSystemWatcher has low overhead, but it can miss some changes. This is acceptable in many scenarios, but in some, it might be not. 
This component, PollingWatcher, allows to monitory directory changes by polling, and so will never miss a change. It is optimized to minimize 
allocations, when no changes are detected. In fact, it does not allocate anything on the GC heap when there are no changes detected.

* **System.Text.Formatting**. 
System.Text.Formatting APIs are similar to the existing StringBuilder and TextWriter APIs. 
They are designed to format values into text streams and to build complex strings. 
But these APIs are optimized for creating text for the Web. 
They do formatting with minimum GC heap allocations (1/6 of allocations in some scenarios) and can format directly to UTF8 streams. 
This can result in significant performance wins for software that does a lot of text formatting for the Web, e.g. generating HTML, JSON, XML. 
See more information on this component and code samples at the [Wiki](https://github.com/dotnet/corefxlab/wiki). 

* **System.Text.Primitives**
The System.Text.Primitives library contains fast, non-allocating integral parsing APIs. They are designed for scenarios in which a byte buffer
and an index are accepted as input and a parsed value is desired as output (e.g. in a web server). These APIs present significant performance gains
over converting the buffer to a string, indexing into the string, and then parsing.

* **System.Time**.
This project augments the date and time APIs in .NET.  It adds two new core types: `Date` and `Time`.
These types will ultimately be submited for inclusion in `System.Runtime`.

* **System.Collections.Generic.MultiValueDictionary**.
The `MultiValueDictionary` is a generic collection that functions similarly to a `Dictionary<TKey, ICollection<TValue>>` with some added validation
and ease of use functions. It can also be compared to a Lookup with the exception that the `MultiValueDictionary` is mutable. It allows custom 
setting of the internal collections so that uniqueness of values can be chosen by specifying either a `HashSet<TValue>` or `List<TValue>`. Some of the
design decisions as well as introductions to usage can be found in the old blog posts introducing it [here](http://blogs.msdn.com/b/dotnet/archive/2014/06/20/would-you-like-a-multidictionary.aspx) and [here](http://blogs.msdn.com/b/dotnet/archive/2014/08/05/multidictionary-becomes-multivaluedictionary.aspx).

More libraries are coming soon. Stay tuned!

[blog post]: http://blogs.msdn.com/b/dotnet/archive/2014/11/12/net-core-is-open-source.aspx

## Archived Projects

The following projects were moved to the archived_projects directory since they do not have any stewards and are no longer under active development.
We will no longer publish new packages for these to MyGet and possibly remove them all together in the future.

* **System.CommandLine**.
The purpose of this library is to make command line tools first class by providing a command line parser. Here are the goals: designed for cross-platform usage, lightweight with minimal configuration, optional but built-in support for help, validation, and response files, support for multiple commands, like version control tools. See the [README.md](archived_projects/src/System.CommandLine/README.md) for more information.

* **System.Devices.Gpio**.
This experimental package for accessing GPIO pins on the Raspberry Pi 3 (Broadcom BCM2837), ODROID-XU4, and BeagleBone Black (AM3358/9) has been moved to the [dotnet/iot repo](https://github.com/dotnet/iot/).  The original prototype from corefxlab has been archived and will eventually be removed. 

* **System.Drawing.Graphics**.
A prototype of .NET Framework's System.Drawing.Graphics on [LibGD](https://en.wikipedia.org/wiki/GD_Graphics_Library) (instead of using [GDIPlus](https://en.wikipedia.org/wiki/Graphics_Device_Interface)).  Some background information can be found [here](https://blogs.msdn.microsoft.com/dotnet/2017/01/19/net-core-image-processing/).  See the [README.txt](archived_projects/src/System.Drawing.Graphics/README.txt) for more information on building the archived project.

## Related Projects

For an overview of all the .NET related projects, have a look at the
[.NET home repository](https://github.com/Microsoft/dotnet).

## How to Use
You can get the .NET Core Lab packages from the **dotnet-experimental** feed: 

```
https://dev.azure.com/dnceng/public/_packaging?_a=feed&feed=dotnet-experimental

or

https://dotnetfeed.blob.core.windows.net/dotnet-experimental/index.json
```

Symbols: 
```
https://dev.azure.com/dnceng/public/_packaging?_a=feed&feed=dotnet-experimental-symbols
```

You can add this feed among your NuGet sources and install the packages (keep in mind that packages are pre-release packages).

To produce a stable build for a project, set the following property in the .csproj. After the build artifacts have been produced, consider incrementing the minor version number in the csproj to preserve artifact version ordering.
```
<DotNetFinalVersionKind>release</DotNetFinalVersionKind>
```
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

To build the projects in this repo, here is what you need to do:

1. The easiest way to build the repo is to invoke `build.cmd` (on Windows) or `build.sh` (on Linux) via the command line after you clone it. When you run `build.cmd` or `build.sh`, the following happens:
   - The NuGet packages for the `corefxlab.sln` solution are restored
   - The `corefxlab.sln` solution (which contains all the active source and test projects) is built
   - For all the supported command line options, run `build -h` at the root. 
2. After you have have run `build` at least once, you can open the `corefxlab.sln` solution file in [Visual Studio 2017](https://www.visualstudio.com/downloads/) (Community, Professional, or Enterprise), on Windows. Make sure you have the .NET Core workload installed (see [runtime windows build instructions](https://github.com/dotnet/runtime/blob/master/docs/workflow/requirements/windows-requirements.md) for more details). Also, make sure to add the `dotnetcli` folder path to your system path environment variable. If you are using VS Code, see https://aka.ms/vscclrdogfood.
   - If you cannot change the system path, then download or install the [new version of the .NET CLI](https://github.com/dotnet/cli#installers-and-binaries) for your operating system at the default global location `C:\Program Files\dotnet`, which is referenced by VS.
3. The unit tests for each project can be run on the command line or in Visual Studio. 
   - In Visual Studio, once corefxlab.sln has been built, open the `Test Explorer` window to locate and run the unit tests.
   - On the command line, running `dotnet test` on the root discovers and run all the unit tests in `corefxlab.sln`. To test individual projects, first navigate to the test project with the unit tests you want to run. For ex: `cd \path\to\corefxlab\tests\Microsoft.Data.Analysis.Tests`. `dotnet test` will discover and run all the unit tests in `Microsoft.Data.Analysis.Tests.csproj`. For more command line options such as filtering tests, `dotnet test -h` lists the supported options.

## Troubleshooting

### It was not possible to find any compatible framework version

There are two main reasons for receiving this error:

 1. You don't have the latest version. Run `build` to install the latest versions.
 2. The wrong `dotnet.exe` is being located. 
    - From the command line, ensure that you are running `dotnet.exe` from the  `dotnetcli` directory (run `dotnet --info`).
    - Alternatively, you can add `[RepoPath]\corefxlab\dotnetcli` to you system path, "ahead" of `C:\Program Files\dotnet`.
    - For building and running tests within VS, you'll need to use this latter option.

## Measuring Performance

All the performance tests live in the `tests\Benchmarks` directory. To learn how run them please go to the corresponding [README](tests/Benchmarks/README.md). For details on BenchmarkDotNet, please refer to [its GitHub page](https://github.com/dotnet/BenchmarkDotNet).
