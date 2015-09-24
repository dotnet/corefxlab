dotnet.dotnetproj (along with the explicit list in references.txt and dependencies.txt) contains all the required packages to self-host dotnet.exe using NuGet as of now.

The dotnet-core\project.json file is used by dnu to restore the packages so that dotnet can be compiled into a coreclr app using msbuild and the dotnet-core\dotnet-core.csproj file.

The dotnet.csproj file is the default one (for now) which is used for development in Visual Studio targeting .NET Framework.