# CoreFxLab Perf Test Harness

This project contains a simple harness to run performance tests and measure the
results.

## Run the Performance Tests

1. Make sure all of the tests have been restored
   (`dotnet restore <path-to-tests-dir>`)

2. Restore this project
   (`dotnet restore`)

3. Run the harness--make sure to use the release configuration
   (`dotnet run -c Release`)

## Add a New Performance Test

When adding a new xunit-performance compatible test project, simply make the
following changes to enable it to run using this harness:

* Add a project dependency in this project's `project.json` referencing the new
  test project

* Add the new project to the array returned by `EnumeratePerfTests()` inside
  `PerfHarness.cs`
