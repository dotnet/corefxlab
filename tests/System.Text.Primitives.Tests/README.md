# Running Performance Tests

These performance tests use the [xunit.performance](https://github.com/Microsoft/xunit-performance) harness. In order to run these tests, perform the following steps.

1. Clone the [corefx](https://github.com/dotnet/corefx) repo to your machine.
2. Run the build script in the main directory of that repo. (At the moment, there is no reliable way to create the proper test environment without using the scripts within the corefx repo).
3. Use the Developer Command Prompt for VS2015 appropriate for your architecture and run `msbuild src\System.Linq\tests\Performance\System.Linq.Performance.Tests.csproj /p:Performance=true /p:OSGroup=Windows_NT /t:RebuildAndTest /p:TargetOS=Windows_NT` from the main directory of your corefx repo. This will set up the correct environment for xunit.performance testing. This step is considered a success only if System.Linq.Performance.Tests actually runs and places test results in testResults.xml. __NOTE:__ _This step fails in the CoreFX master branch as of this readme update. To work around this, switch to a release branch such as origin/release/1.1.0 and try this step again. There is no need to run the main corefx build script again._
4. Build your cloned corefxlab repo. (You can do this concurrently with the previous two steps).
5. Within your cloned corefxlab repo, navigate to `corefxlab/tests/System.Text.Primitives.Tests/bin/Debug/netcoreapp1.0`. Copy the contents of this directory to the cloned corefx repo in `corefx/bin/tests/Windows_NT.AnyCPU.Debug/
System.Linq.Performance.Tests`.
6. From the command line, navigate to that directory. Then run `corerun Microsoft.DotNet.xunit.performance.runner.cli.dll System.Text.Primitives.Tests.dll -trait Benchmark=true -runnerhost CoreRun.exe -runner xunit.console.netcore.exe -runid testResults -verbose`.
__tests run here__
7. The results of the tests will be placed in a file called testResults.xml.

__Final Note:__ _The corefxlab performance tests can fail in this environment if corefxlab\tests\System.Text.Primitives.Tests\project.json corefx\src\System.Linq\tests\Performance\project.json reference different versions of Microsoft.DotNet.xunit.performance. Make sure they reference the same version, e.g., 1.0.0-alpha-build0040._