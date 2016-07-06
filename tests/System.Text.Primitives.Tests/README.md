# Running Performance Tests

These performance tests use the [xunit.performance](https://github.com/Microsoft/xunit-performance) harness. In order to run these tests, perform the following steps.

1. Clone the [corefx](https://github.com/dotnet/corefx) repo to your machine.
2. Run the build script in the main directory of that repo. (At the moment, there is no reliable way to create the proper test environment without using the scripts within the corefx repo).
3. Once you have done this, return to your cloned corefxlab repo and build it. (You can do this concurrently with the previous step).
4. Within your cloned repo, navigate to `corefxlab/tests/System.Text.Primitives.Tests/bin/Debug/netcoreapp1.0`. Copy the contents of this directory to the cloned corefx repo in `corefx/bin/tests/Windows_NT.AnyCPU.Debug/System.Collections.NonGeneric.Tests/netcoreapp1.0`.
5. From the command line, navigate to that directory. Then run `xunit.performance.run.exe System.Text.Primitives.Tests.dll -trait Benchmark=true -runnerhost CoreRun.exe -runner xunit.console.netcore.exe -runid testResults -verbose`
6. The results of the tests will be placed in a file called testResults.xml.