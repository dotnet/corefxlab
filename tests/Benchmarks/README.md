# CoreFxLab Benchmarks

This project contains performance benchmarks.

## Run the Performance Tests

**Pre-requisite:** The dotnet cli is available at the root in the dotnetcli directory. On a clean repo, build.cmd at the root installs the latest available dotnet.exe.

**Pre-requisite:** To use dotnet cli from the root directory remember to set `DOTNET_MULTILEVEL_LOOKUP` environment variable to `0`!

    $env:DOTNET_MULTILEVEL_LOOKUP=0

1. Navigate to the benchmarks directory (corefxlab\tests\Benchmarks\)

2. Run the benchmarks in Release, choose one of the benchmarks when prompted

```log
    ..\..\dotnetcli\dotnet.exe run -c Release
```
   
3. To run specific tests only, pass in the filter to the harness:

```log
   ..\..\dotnetcli\dotnet.exe run -c Release -- --filter namespace*
   ..\..\dotnetcli\dotnet.exe run -c Release -- --filter *typeName*
   ..\..\dotnetcli\dotnet.exe run -c Release -- --filter *.methodName
   ..\..\dotnetcli\dotnet.exe run -c Release -- --filter namespace.typeName.methodName
```

4. To find out more about supported command line arguments run

```log
   ..\..\dotnetcli\dotnet.exe run -c Release -- --help
```
