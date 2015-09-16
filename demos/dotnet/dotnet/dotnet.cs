// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;

static class Program
{
    static Log Log = new global::Log();

    // Arguments specifications used for parsing
    static readonly List<String> commandFunctions = new List<String>() { "/new", "/clean", "/edit", "/?", "/help" };
    static readonly List<String> commandSwitches = new List<String>() { "/log", "/optimize", "/unsafe" };
    static readonly List<String> commandSwitchesWithSpecifications = new List<String>() { "/target", "/recurse", "/debug" };
    static readonly List<String> targetSpecifications = new List<String>() { "exe", "library" };
    static readonly List<String> debugSpecifications = new List<String>() { "full", "pdbonly" };

    static void Main(string[] args)
    {
        // Defaults
        if (args.Length == 0)
        {
            args = new string[] { "/target:exe" };
        }

        Log.IsEnabled = Array.Exists(args, element => element == "/log");

        bool buildDll = false;

        // Incorrect arguments passed in or user asked for help
        bool printUsage = false;

        bool compilerFunctionSelected = false;

        // Command arguments parsing
        foreach (var argument in args)
        {
            if (printUsage || compilerFunctionSelected) break;

            var argOptions = argument.Split(':');
            var compilerOption = argOptions[0];

            if (commandFunctions.Contains(compilerOption))
            {
                compilerFunctionSelected = true;
                switch (compilerOption)
                {
                    case "/new":
                        OtherActions.CreateNewProject(Environment.CurrentDirectory);
                        break;

                    case "/edit":
                        var path = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\*\\shell\\Ticino", "Icon", null);
                        Process.Start(path);
                        break;

                    case "/clean":
                        Clean(args, Log);
                        break;

                    case "/?":
                    case "/help":
                    default:
                        printUsage = true;
                        break;
                }
            }
            else if (commandSwitches.Contains(compilerOption))
            {
                // Do nothing, just pass those switches to the CSC Options
            }
            else if (commandSwitchesWithSpecifications.Contains(compilerOption))
            {
                if (argOptions.Length != 2)
                {
                    Console.WriteLine("Please specify the {0} compiler option correctly.", compilerOption);
                    printUsage = true;
                }
                else
                {
                    var commandSpecification = argOptions[1];
                    switch (compilerOption)
                    {
                        case "/target":
                            if (!targetSpecifications.Contains(commandSpecification))
                            {
                                Console.WriteLine("Please specify the {0} compiler option correctly.", compilerOption);
                                printUsage = true;
                            }
                            else
                            {
                                buildDll = commandSpecification == "library";
                            }
                            break;

                        case "/debug":
                            if (!debugSpecifications.Contains(commandSpecification))
                            {
                                Console.WriteLine("Please specify the {0} compiler option correctly.", compilerOption);
                                printUsage = true;
                            }
                            break;

                        case "/recurse":
                            // Do nothing, just pass the switch with the wildcard to find the source files
                            break;

                        default:
                            printUsage = true;
                            break;
                    }
                }
            }
            else
            {
                if (!File.Exists(compilerOption))
                {
                    Console.WriteLine("Could not find the file \"{0}\" in the current directory.", compilerOption);
                    printUsage = true;
                }
            }

        }

        if (printUsage)
        {
            PrintUsage();
        }
        else if (!compilerFunctionSelected)
        {
            Build(args, Log, buildDll);
        }

    }

    static void PrintUsage()
    {
        string appName = Environment.GetCommandLineArgs()[0];
        Console.WriteLine("{0} /? or /help        - help", appName);
        Console.WriteLine("{0} /new               - creates template sources for a new console app", appName);
        Console.WriteLine("{0} /clean             - deletes tools, packages, and bin project subdirectories", appName);
        Console.WriteLine("{0} /edit              - starts code editor", appName);
        Console.WriteLine("{0} [/log] [/target:{{exe|library}}] [/recurse:<wildcard>] [/debug:{{full|pdbonly}}] [/optimize] [/unsafe] [ProjectFile]", appName);
        Console.WriteLine("           /log        - logs diagnostics info");
        Console.WriteLine("           /target     - compiles the sources in the current directory into an exe (default) or dll");
        Console.WriteLine("           /recurse    - compiles the sources in the current directory and subdirectories specified by the wildcard");
        Console.WriteLine("           /debug      - generates debugging information");
        Console.WriteLine("           /optimize   - enables optimizations performed by the compiler");
        Console.WriteLine("           /unsafe     - allows compilation of code that uses the unsafe keyword");
        Console.WriteLine("           ProjectFile - specifies which project file to use, default to the one in the current directory, if only one exists");
        // TODO: Can the source files be specified explicitly?
        //Console.WriteLine("           SourceFiles - specifices which source files to compile");
        Console.WriteLine("NOTE #1: uses csc.exe in <project>\\tools subdirectory, or csc.exe on the path.");
        Console.WriteLine("NOTE #2: packages.txt, dependencies.txt, references.txt, cscoptions.txt can be used to override details.");
    }

    static void Clean(string[] args, Log log)
    {
        var previous = log.IsEnabled; //TODO: this 
        log.IsEnabled = false;
        var properties = ProjectPropertiesHelpers.InitializeProperties(args, log);
        log.IsEnabled = previous;
        if (Directory.Exists(properties.ToolsDirectory)) Directory.Delete(properties.ToolsDirectory, true);
        if (Directory.Exists(properties.OutputDirectory)) Directory.Delete(properties.OutputDirectory, true);
        if (Directory.Exists(properties.PackagesDirectory)) Directory.Delete(properties.PackagesDirectory, true);
    }

    static void Build(string[] args, Log log, bool buildDll = false)
    {
        var properties = ProjectPropertiesHelpers.InitializeProperties(args, log, buildDll);

        if (properties.Sources.Count == 0)
        {
            Console.WriteLine("no sources found");
            return;
        }

        if (!Directory.Exists(properties.OutputDirectory))
        {
            Directory.CreateDirectory(properties.OutputDirectory);
        }

        if (!Directory.Exists(properties.ToolsDirectory))
        {
            Directory.CreateDirectory(properties.ToolsDirectory);
        }

        if (!Directory.Exists(properties.PackagesDirectory))
        {
            Directory.CreateDirectory(properties.PackagesDirectory);
        }

        NugetAction.DownloadNugetAction(properties);
        NugetAction.RestorePackagesAction(properties, Log);

        if (CscAction.Execute(properties, Log))
        {
            if (!buildDll)
            {
                ConvertToCoreConsoleAction(properties);
            }
            OutputRuntimeDependenciesAction(properties);
            Console.WriteLine("bin\\{0} created", properties.AssemblyNameAndExtension);
        }
    }

    static void OutputRuntimeDependenciesAction(ProjectProperties properties)
    {
        foreach (var dependencyFolder in properties.Dependencies)
        {
            Helpers.CopyAllFiles(dependencyFolder, properties.OutputDirectory);
        }
    }

    static void ConvertToCoreConsoleAction(ProjectProperties properties)
    {
        var dllPath = Path.ChangeExtension(properties.OutputAssemblyPath, "dll");
        if (File.Exists(dllPath))
        {
            File.Delete(dllPath);
        }
        File.Move(properties.OutputAssemblyPath, dllPath);
        File.Copy(Path.Combine(properties.PackagesDirectory, @"Microsoft.NETCore.ConsoleHost-x86\1.0.0-beta-23123\runtimes\win7-x86\native\CoreConsole.exe"), properties.OutputAssemblyPath);
    }
}

static class ProjectPropertiesHelpers
{
    public static ProjectProperties InitializeProperties(string[] args, Log log, bool buildDll = false)
    {
        // General Properites
        ProjectProperties properties = new ProjectProperties();

        properties.ProjectDirectory = Path.Combine(Environment.CurrentDirectory);
        properties.PackagesDirectory = Path.Combine(properties.ProjectDirectory, "packages");
        properties.OutputDirectory = Path.Combine(properties.ProjectDirectory, "bin");
        properties.ToolsDirectory = Path.Combine(properties.ProjectDirectory, "tools");
        properties.AssemblyName = Path.GetFileName(properties.ProjectDirectory);
        properties.OutputType = buildDll ? ".dll" : ".exe";
        FindCompiler(properties);

        var specifiedProjectFilename = Array.Find(args, element => element.EndsWith(".dotnetproj"));

        if (specifiedProjectFilename != null)
        {
            var specifiedProjectFile = Directory.GetFiles(properties.ProjectDirectory, specifiedProjectFilename);
            if (specifiedProjectFile.Length == 1)
            {
                properties.Sources.AddRange(ParseProjectFile(properties, specifiedProjectFile[0]));
            }
        }
        else
        {
            var projectFiles = Directory.GetFiles(properties.ProjectDirectory, "*.dotnetproj");
            if (projectFiles.Length == 1)
            {
                properties.Sources.AddRange(ParseProjectFile(properties, projectFiles[0]));
            }
        }

        var recurseOption = Array.Find(args, element => element.StartsWith("/recurse"));
        var recurseWildcard = "";
        if (recurseOption != null && recurseOption.Split(':').Length == 2)
        {
            recurseWildcard = recurseOption.Split(':')[1];
        }

        var sourceFiles = Directory.GetFiles(properties.ProjectDirectory, "*.cs", recurseWildcard == "*.cs" ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        foreach (var f in sourceFiles)
        {
            if (!f.StartsWith(properties.PackagesDirectory) && !f.StartsWith(properties.OutputDirectory) && !f.StartsWith(properties.ToolsDirectory))
            {
                properties.Sources.Add(f);
            }
        }

        if (recurseOption != null && recurseWildcard != "*.cs")
        {
            var subdirectoryPath = Path.Combine(properties.ProjectDirectory, recurseWildcard); // TODO: AllDirectories OR TopDirectoryOnly? Right now TopDirectoryOnly
            properties.Sources.AddRange(Directory.GetFiles(subdirectoryPath, "*.cs"));
        }

        if (properties.Sources.Count == 1)
        {
            properties.AssemblyName = Path.GetFileNameWithoutExtension(properties.Sources[0]);
        }

        // Packages
        properties.Packages.Add(@"""Microsoft.NETCore"": ""5.0.0""");
        properties.Packages.Add(@"""System.Console"": ""4.0.0-beta-23123""");
        //properties.Packages.Add(@"""Microsoft.NETCore.Console"": ""1.0.0-beta-*""");
        properties.Packages.Add(@"""Microsoft.NETCore.ConsoleHost-x86"": ""1.0.0-beta-23123""");
        properties.Packages.Add(@"""Microsoft.NETCore.Runtime.CoreCLR-x86"": ""1.0.0""");

        // References
        properties.References.Add(Path.Combine(properties.PackagesDirectory, @"System.Runtime\4.0.20\ref\dotnet\System.Runtime.dll"));
        properties.References.Add(Path.Combine(properties.PackagesDirectory, @"System.Console\4.0.0-beta-23123\ref\dotnet\System.Console.dll"));

        // Runtime Dependencies
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"Microsoft.NETCore.Runtime.CoreCLR-x86\1.0.0\runtimes\win7-x86\native"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"Microsoft.NETCore.Runtime.CoreCLR-x86\1.0.0\runtimes\win7-x86\lib\dotnet"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.Runtime\4.0.20\lib\netcore50"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.Console\4.0.0-beta-23123\lib\DNXCore50"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.IO\4.0.10\lib\netcore50"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.Threading\4.0.10\lib\netcore50"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.IO.FileSystem.Primitives\4.0.0\lib\dotnet"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.Text.Encoding\4.0.10\lib\netcore50"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.Threading.Tasks\4.0.10\lib\netcore50"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.Text.Encoding.Extensions\4.0.10\lib\netcore50"));
        properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.Runtime.InteropServices\4.0.20\lib\netcore50"));

        // CSC OPTIONS
        properties.CscOptions.Add("/nostdlib");
        properties.CscOptions.Add("/noconfig");
        if (Array.Exists(args, element => element == "/unsafe")) 
        {
            properties.CscOptions.Add("/unsafe");
        }

        if (Array.Exists(args, element => element == "/optimize"))
        {
            properties.CscOptions.Add("/optimize");
        }

        var debugOption = Array.Find(args, element => element.StartsWith("/debug"));

        if (debugOption != null)
        {
           properties.CscOptions.Add(debugOption);
        }

        LogProperties(log, properties, "Initialized Properties Log:", buildDll);

        Adjust(Path.Combine(properties.ProjectDirectory, "dependencies.txt"), properties.Dependencies);
        Adjust(Path.Combine(properties.ProjectDirectory, "references.txt"), properties.References);
        Adjust(Path.Combine(properties.ProjectDirectory, "cscoptions.txt"), properties.CscOptions);
        Adjust(Path.Combine(properties.ProjectDirectory, "packages.txt"), properties.Packages);

        LogProperties(log, properties, "Adjusted Properties Log:", buildDll);

        return properties;
    }

    static void LogProperties(this Log log, ProjectProperties project, string heading, bool buildDll = false)
    {
        if (!log.IsEnabled) return;

        log.WriteLine(heading);
        log.WriteLine("ProjectDirectory     {0}", project.ProjectDirectory);
        log.WriteLine("PackagesDirectory    {0}", project.PackagesDirectory);
        log.WriteLine("OutputDirectory      {0}", project.OutputDirectory);
        log.WriteLine("ToolsDirectory       {0}", project.ToolsDirectory);
        log.WriteLine(buildDll ? "LibraryFilename      {0}" : "ExecutableFilename   {0}", project.AssemblyName);
        log.WriteLine("csc.exe Path         {0}", project.CscPath);
        log.WriteLine("output path          {0}", project.OutputAssemblyPath);
        log.WriteList(project.Sources, "SOURCES");
        log.WriteList(project.Packages, "PACKAGES");
        log.WriteList(project.References, "REFERENCES");
        log.WriteList(project.Dependencies, "DEPENDENCIES");
        log.WriteList(project.CscOptions, "CSCOPTIONS");
        log.WriteLine("-------------------------------------------------");
    }

    static void Adjust(string adjustmentFilePath, List<string> list)
    {
        if (File.Exists(adjustmentFilePath))
        {
            foreach (var line in File.ReadAllLines(adjustmentFilePath))
            {
                if (String.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("//")) continue; // commented out line
                var adjustment = line.Substring(1).Trim();
                if (line.StartsWith(" - "))
                {
                    list.Remove(adjustment);
                }
                else
                {
                    list.Add(adjustment);
                }
            }
        }
    }

    static List<string> ParseProjectFile(ProjectProperties properties, string projectFile)
    {
        var sourceFiles = new List<string>();
        using (XmlReader xmlReader = XmlReader.Create(projectFile))
        {
            xmlReader.MoveToContent();
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "Compile")
                {
                    var sourceFile = xmlReader.GetAttribute("Include");
                    if (sourceFile == "*.cs")
                    {
                        sourceFiles.AddRange(Directory.GetFiles(properties.ProjectDirectory, "*.cs"));
                    }
                    else if(sourceFile.EndsWith("\\*.cs"))
                    {
                        sourceFiles.AddRange(Directory.GetFiles(Path.Combine(properties.ProjectDirectory, sourceFile.Replace("\\*.cs", "")), "*.cs"));
                    }
                    else
                    {
                        sourceFiles.Add(Path.Combine(properties.ProjectDirectory, sourceFile));
                    }
                }
            }
        }
        return sourceFiles;
    }

    static void FindCompiler(ProjectProperties properties)
    {
        properties.CscPath = Path.Combine(properties.ToolsDirectory, "csc.exe");
        if (File.Exists(properties.CscPath))
        {
            return;
        }

        properties.CscPath = @"D:\git\roslyn\Binaries\Debug\core-clr\csc.exe";
        if (!File.Exists(properties.CscPath))
        {
            properties.CscPath = "csc.exe";
        }
    }
}

static class CscAction
{
    public static bool Execute(ProjectProperties properties, Log log)
    {
        Console.WriteLine("compiling");
        var processSettings = new ProcessStartInfo();
        processSettings.FileName = properties.CscPath;
        processSettings.Arguments = properties.FormatCscArguments();

        log.WriteLine("Executing {0}", processSettings.FileName);
        log.WriteLine("Csc Arguments: {0}", processSettings.Arguments);

        processSettings.CreateNoWindow = true;
        processSettings.RedirectStandardOutput = true;
        processSettings.UseShellExecute = false;

        Process cscProcess = null;
        try
        {
            cscProcess = Process.Start(processSettings);
        }
        catch (Win32Exception)
        {
            Console.WriteLine("ERROR: csc.exe needs to be on the path.");
            return false;
        }

        var output = cscProcess.StandardOutput.ReadToEnd();
        log.WriteLine(output);

        cscProcess.WaitForExit();

        if (output.Contains("error CS")) return false;
        return true;
    }

    static string FormatReferenceOption(this ProjectProperties project)
    {
        var builder = new StringBuilder();
        builder.Append(" /r:");
        bool first = true;
        foreach (var reference in project.References)
        {
            if (!first) { builder.Append(','); }
            else { first = false; }
            builder.Append(Path.Combine(project.PackagesDirectory, reference));
        }
        builder.Append(" ");
        return builder.ToString();
    }

    static string FormatSourcesOption(this ProjectProperties project)
    {
        var builder = new StringBuilder();
        foreach (var source in project.Sources)
        {
            builder.Append(" ");
            builder.Append(source);
        }
        return builder.ToString();
    }

    static string FormatCscOptions(this ProjectProperties project)
    {
        var builder = new StringBuilder();
        foreach (var option in project.CscOptions)
        {
            builder.Append(" ");
            builder.Append(option);
        }

        if (string.Compare(project.OutputType, ".dll") == 0)
        {
            builder.Append(" ");
            builder.Append("/target:library");
        }

        builder.Append(" ");
        builder.Append("/out:");
        builder.Append(project.OutputAssemblyPath);
        builder.Append(" ");
        return builder.ToString();
    }

    static string FormatCscArguments(this ProjectProperties project)
    {
        return project.FormatCscOptions() + project.FormatReferenceOption() + project.FormatSourcesOption();
    }
}

static class NugetAction
{
    static void CreateNugetConfig(ProjectProperties properties)
    {
        using (var file = new StreamWriter(Path.Combine(properties.ToolsDirectory, "nuget.config"), false))
        {
            file.WriteLine(@"<?xml version = ""1.0"" encoding=""utf-8""?>");
            file.WriteLine(@"<configuration>");
            file.WriteLine(@"    <packageSources>");

            file.WriteLine(@"        <add key = ""netcore-prototype"" value=""https://www.myget.org/F/netcore-package-prototyping""/>");
            file.WriteLine(@"        <add key = ""nuget.org"" value = ""https://api.nuget.org/v3/index.json"" protocolVersion = ""3""/>");
            file.WriteLine(@"        <add key = ""nuget.org"" value = ""https://www.nuget.org/api/v2/""/>");

            file.WriteLine(@"    </packageSources>");
            file.WriteLine(@"</configuration>");
        }
    }

    public static void RestorePackagesAction(ProjectProperties properties, Log log)
    {
        Console.WriteLine("restoring packages");

        var projectFile = Path.Combine(properties.ProjectDirectory, "project.json");
        if (!File.Exists(projectFile))
        {
            projectFile = Path.Combine(properties.ToolsDirectory, "project.json");
        }

        var processSettings = new ProcessStartInfo();
        processSettings.FileName = Path.Combine(properties.ToolsDirectory, "nuget.exe");
        processSettings.Arguments = "restore " + projectFile + " -PackagesDirectory " + properties.PackagesDirectory + " -ConfigFile " + Path.Combine(properties.ToolsDirectory, "nuget.config");
        processSettings.CreateNoWindow = true;
        processSettings.UseShellExecute = false;
        processSettings.RedirectStandardOutput = true;
        processSettings.RedirectStandardError = true;

        log.WriteLine("Executing {0}", processSettings.FileName);
        log.WriteLine("Arguments: {0}", processSettings.Arguments);
        log.WriteLine("project.json:\n{0}", File.ReadAllText(projectFile));

        var process = Process.Start(processSettings);
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        log.WriteLine(output);
        log.Error(error);        
        process.WaitForExit();
    }

    public static void DownloadNugetAction(ProjectProperties properties)
    {
        CreateDefaultProjectJson(properties);
        CreateNugetConfig(properties);

        string destination = Path.Combine(properties.ToolsDirectory, "nuget.exe");
        if (File.Exists(destination))
        {
            return;
        }

        var client = new HttpClient();
        using (var sourceStreamTask = client.GetStreamAsync(@"http://dist.nuget.org/win-x86-commandline/v3.1.0-beta/nuget.exe"))
        {
            sourceStreamTask.Wait();
            using (var sourceStream = sourceStreamTask.Result)
            using (var destinationStream = new FileStream(destination, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    var read = sourceStream.Read(buffer, 0, buffer.Length);
                    if (read < 1) break;
                    destinationStream.Write(buffer, 0, read);
                }
            }
        }
    }

    static void CreateDefaultProjectJson(ProjectProperties properties)
    {
        using (var file = new StreamWriter(Path.Combine(properties.ToolsDirectory, "project.json"), false))
        {
            file.WriteLine(@"{");
            file.WriteLine(@"    ""dependencies"": {");

            for (int index = 0; index < properties.Packages.Count; index++)
            {
                var package = properties.Packages[index];
                file.Write(@"        ");
                file.Write(package);
                if (index < properties.Packages.Count - 1)
                {
                    file.WriteLine(",");
                }
                else
                {
                    file.WriteLine();
                }
            }
            file.WriteLine(@"    },");
            file.WriteLine(@"    ""frameworks"": {");
            file.WriteLine(@"        ""dnxcore50"": { }");
            file.WriteLine(@"    }");
            file.WriteLine(@"}");

            //"runtimes": {
            //"win7-x86": { },
            //"win7-x64": { }
            //},
        }
    }
}

static class OtherActions
{
    internal static void CreateNewProject(string directory)
    {
        using (var file = new StreamWriter(Path.Combine(directory, "main.cs"), false))
        {
            file.WriteLine(@"using System;");
            file.WriteLine();
            file.WriteLine(@"static class Program {");

            file.WriteLine(@"    static void Main() {");

            file.WriteLine(@"        Console.WriteLine(""hello world"");");
            file.WriteLine(@"    }");
            file.WriteLine(@"}");
        }
    }
}

static class Helpers
{
    internal static void CopyAllFiles(string sourceFolder, string destinationFolder)
    {
        foreach (var sourceFilePath in Directory.EnumerateFiles(sourceFolder))
        {
            var destinationFilePath = Path.Combine(destinationFolder, Path.GetFileName(sourceFilePath));
            if (File.Exists(destinationFilePath))
            {
                File.Delete(destinationFilePath);
            }
            File.Copy(sourceFilePath, destinationFilePath);
        }
    }
}


