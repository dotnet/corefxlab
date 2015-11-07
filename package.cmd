@echo off
setlocal

packages\NuGet.exe pack .\nuget\System.Buffers.nuspec -OutputDirectory .\nuget
packages\NuGet.exe pack .\nuget\System.CommandLine.nuspec -OutputDirectory .\nuget
packages\NuGet.exe pack .\nuget\System.Slices.nuspec -OutputDirectory .\nuget
packages\NuGet.exe pack .\nuget\System.IO.FileSystem.Watcher.Polling.nuspec -OutputDirectory .\nuget
packages\NuGet.exe pack .\nuget\System.Text.Utf8.nuspec -OutputDirectory .\nuget
packages\NuGet.exe pack .\nuget\System.Text.Formatting.nuspec -OutputDirectory .\nuget
packages\NuGet.exe pack .\nuget\System.Text.Json.nuspec -OutputDirectory .\nuget
packages\NuGet.exe pack .\nuget\System.Collections.Generic.MultiValueDictionary.nuspec -OutputDirectory .\nuget
packages\NuGet.exe pack .\nuget\System.Net.Libuv.nuspec -OutputDirectory .\nuget

packages\nuget.exe push .\nuget\System.Buffers.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget.exe push .\nuget\System.CommandLine.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget.exe push .\nuget\System.Slices.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget.exe push .\nuget\System.FileSystem.Watcher.Polling.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget.exe push .\nuget\System.Text.Json.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget.exe push .\nuget\System.Text.Utf8.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget.exe push .\nuget\System.Text.Formatting.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget.exe push .\nuget\System.Collections.Generic.MultiValueDictionary.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
packages\nuget.exe push .\nuget\System.Net.Libuv.0.1.0-d103015-1.nupkg %1 -Source https://www.myget.org/F/dotnet-corefxlab/api/v2/package
