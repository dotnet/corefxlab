@echo off
setlocal

packages\NuGet.exe pack System.Buffers.nuspec
packages\NuGet.exe pack System.CommandLine.nuspec
packages\NuGet.exe pack System.Slices.nuspec
packages\NuGet.exe pack System.IO.FileSystem.Watcher.Polling.nuspec
packages\NuGet.exe pack System.Text.Utf8.nuspec
packages\NuGet.exe pack System.Text.Formatting.nuspec
packages\NuGet.exe pack System.Text.Json.nuspec
packages\NuGet.exe pack System.Collections.Generic.MultiValueDictionary.nuspec

packages\nuget push System.Buffers.*.nupkg %1% -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget push System.CommandLine.*.nupkg %1% -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget push System.Slices.*.nupkg %1% -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget push System.FileSystem.Watcher.Polling.*.nupkg %1% -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget push System.Text.Json.*.nupkg %1% -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget push System.Text.Utf8.*.nupkg %1% -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget push System.Text.Formatting.*.nupkg %1% -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget push System.Collections.Generic.MultiValueDictionary.*.nupkg %1% -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
