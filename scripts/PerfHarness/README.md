# CoreFxLab Perf Test Harness

This project contains a simple harness to run performance tests and measure the
results.

## Run the Performance Tests
**Pre-requisite:** The dotnet cli is available at the root in the dotnetcli directory. On a clean repo, build.cmd at the root installs the latest available dotnet.exe.

1. Navigate to the PerfHarness directory (corefxlab\scripts\PerfHarness\)

2. Restore this project
   
   (`..\..\dotnetcli\dotnet.exe restore`)

3a. Run the harness--make sure to use the release configuration
   
   (`..\..\dotnetcli\dotnet.exe run -c Release`)
   
3b. To run specific tests only, pass in the type names to the harness (this run types found in any of the assemblies):
   
   (`..\..\dotnetcli\dotnet.exe run -c Release -- --perf:typenames name1 [name2] [...]`)
   
3c. To run specific tests found in a specific assembly only, pass in the assembly name and type names to the harness:
   
   (`..\..\dotnetcli\dotnet.exe run -c Release -- --assembly <name> --perf:typenames name1 [name2] [...]`)
   
   Example: `..\..\dotnetcli\dotnet.exe run -c Release -- --assembly System.Text.Primitives.Tests --perf:typenames System.Text.Primitives.Tests.EncodingPerfComparisonTests`

## Add a New Performance Test

When adding a new xunit-performance compatible test project, simply make the
following changes to enable it to run using this harness:

* Add a project reference in this project's `csproj` file referencing the new
  test project.

* Add the new element to the array returned by `GetTestAssemblies()` inside
  `PerfHarness.cs`. The element should be the name of the perf test project
  assembly, excluding the file extension.

## Xunit-Performance-Api

Here are the list of command line options available when running performance tests (starting from build [1.0.0-alpha-build0048](https://dotnet.myget.org/feed/dotnet-core/package/nuget/xunit.performance.api) onwards).

Option | Description
--- | ---
--perf:outputdir | Specifies the output directory name.
--perf:runid | User defined id given to the performance harness.
--perf:typenames | The (optional) type names of the test classes to run.
--help | Display this help screen.
--version | Display version information.
