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

static class Program
{
    static bool IsLoggingEnabled;

    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            args = new string[] { "/build" };
        }

        switch (args[0])
        {
            case "/new":
                OtherActions.CreateNewProject(Environment.CurrentDirectory);
                break;

            case "/edit":
                var path = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\*\\shell\\Ticino", "Icon", null);
                Process.Start(path);
                break;

            case "/build":
            case "/log":
                Build(args);
                break;

            case "/help":
            case "?":
            default:
                PrintUsage();
                break;
        }
    }

    private static void Build(string[] args)
    {
        var properties = InitializeProperties(args);

        if (args.Length > 0 && args[0] == "/log")
        {
            IsLoggingEnabled = true;
        }

        if (IsLoggingEnabled) Console.WriteLine("Initialized Properties:");
        properties.Log(Console.Out);

        Adjust(Path.Combine(properties.ProjectDirectory, "dependencies.txt"), properties.Dependencies);
        Adjust(Path.Combine(properties.ProjectDirectory, "references.txt"), properties.References);
        Adjust(Path.Combine(properties.ProjectDirectory, "cscoptions.txt"), properties.CscOptions);
        Adjust(Path.Combine(properties.ProjectDirectory, "packages.txt"), properties.Packages);

        if (IsLoggingEnabled) Console.WriteLine("Adjusted Properties:");
        properties.Log(Console.Out);

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

        DownloadNugetAction(properties);

        RestorePackagesAction(properties);

        if (CscAction.Execute(properties, IsLoggingEnabled))
        {
            ConvertToCoreConsoleAction(properties);
            OutputRuntimeDependenciesAction(properties);
        }

        Console.WriteLine("bin\\{0}.exe created", properties.AssemblyName);
    }

    private static void PrintUsage()
    {
        string appName = Environment.GetCommandLineArgs()[0];
        Console.WriteLine("{0}.exe [/log] - compiles sources in current direcotry. optionally logs diagnostics info.", appName);
        Console.WriteLine("{0}.exe /new   - creates template sources for a new console app", appName);
        Console.WriteLine("{0}.exe /edit  - starts code editor", appName);
        Console.WriteLine("{0}.exe /?     - help", appName);
    }

    static void Log(this ProjectProperties project, TextWriter log)
    {
        if (!IsLoggingEnabled) return;
        log.WriteLine("ProjectDirectory     {0}", project.ProjectDirectory);
        log.WriteLine("PackagesDirectory    {0}", project.PackagesDirectory);
        log.WriteLine("OutputDirectory      {0}", project.OutputDirectory);
        log.WriteLine("ToolsDirectory       {0}", project.ToolsDirectory);
        log.WriteLine("ExecutableFilename   {0}", project.AssemblyName);
        log.WriteLine("csc.exe Path         {0}", project.CscPath);
        log.WriteLine("output path          {0}", project.OutputAssemblyPath);
        LogList(project.Sources, "SOURCES", log);
        LogList(project.Packages, "PACKAGES", log);
        LogList(project.References, "REFERENCES", log);
        LogList(project.Dependencies, "DEPENDENCIES", log);
        LogList(project.CscOptions, "CSCOPTIONS", log);
        log.WriteLine();
    }

    static void LogList(List<string> list, string listName, TextWriter log)
    {
        log.WriteLine("{0}:", listName);
        foreach (var str in list)
        {
            log.Write('\t');
            log.WriteLine(str);
        }
    }

    private static void Adjust(string adjustmentFilePath, List<string> list)
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

    private static ProjectProperties InitializeProperties(string[] args)
    {

        // General Properites
        ProjectProperties properties = new ProjectProperties();

        properties.ProjectDirectory = Path.Combine(Environment.CurrentDirectory);
        properties.PackagesDirectory = Path.Combine(properties.ProjectDirectory, "packages");
        properties.OutputDirectory = Path.Combine(properties.ProjectDirectory, "bin");
        properties.ToolsDirectory = Path.Combine(properties.ProjectDirectory, "tools");
        properties.AssemblyName = Path.GetFileName(properties.ProjectDirectory);
        FindCompiler(properties);

        // Sources
        properties.Sources.AddRange(Directory.GetFiles(properties.ProjectDirectory, "*.cs"));
        if (properties.Sources.Count == 1)
        {
            properties.AssemblyName = Path.GetFileNameWithoutExtension(properties.Sources[0]);
        }

        // Packages
        properties.Packages.Add(@"""Microsoft.NETCore.Console"": ""1.0.0-beta-*""");
        properties.Packages.Add(@"""Microsoft.NETCore.ConsoleHost-x86"": ""1.0.0-beta-23123""");

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

        return properties;
    }

    private static void FindCompiler(ProjectProperties properties)
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

    static void OutputRuntimeDependenciesAction(ProjectProperties properties)
    {
        foreach (var dependencyFolder in properties.Dependencies) { 
            Helpers.CopyAllFiles(dependencyFolder, properties.OutputDirectory);
        }
    }

    static void ConvertToCoreConsoleAction(ProjectProperties properties)
    {
        var dllPath = Path.ChangeExtension(properties.OutputAssemblyPath, "dll");
        if(File.Exists(dllPath))
        {
            File.Delete(dllPath);
        }
        File.Move(properties.OutputAssemblyPath, dllPath);
        File.Copy(Path.Combine(properties.PackagesDirectory, @"Microsoft.NETCore.ConsoleHost-x86\1.0.0-beta-23123\runtimes\win7-x86\native\CoreConsole.exe"), properties.OutputAssemblyPath);
    }

    static void RestorePackagesAction(ProjectProperties properties)
    {
        Console.WriteLine("restoring packages");

        var projectFile = Path.Combine(properties.ProjectDirectory, "project.json");
        if (!File.Exists(projectFile))
        {
            projectFile = Path.Combine(properties.ToolsDirectory, "project.json");
        }
        var processSettings = new ProcessStartInfo();
        processSettings.FileName = Path.Combine(properties.ToolsDirectory, "nuget.exe");
        processSettings.Arguments = "restore " + projectFile + " -PackagesDirectory " + properties.PackagesDirectory;
        processSettings.CreateNoWindow = true;
        processSettings.UseShellExecute = false;
        var process = Process.Start(processSettings);
        process.WaitForExit();
    }

    static void DownloadNugetAction(ProjectProperties properties)
    {
        CreateDefaultProjectJson(properties);

        string destination = Path.Combine(properties.ToolsDirectory, "nuget.exe");
        if (File.Exists(destination))
        {
            return;
        }

        var client = new HttpClient();
        using (var sourceStreamTask = client.GetStreamAsync(@"http://dist.nuget.org/win-x86-commandline/v3.1.0-beta/nuget.exe"))
        {
            sourceStreamTask.Wait();
            using(var sourceStream = sourceStreamTask.Result)
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

static class CscAction
{
    public static bool Execute(ProjectProperties properties, bool log)
    {
        Console.WriteLine("compiling");
        var processSettings = new ProcessStartInfo();
        processSettings.FileName = properties.CscPath;
        processSettings.Arguments = properties.FormatCscArguments();

        if (log)
        {
            Console.WriteLine("Executing {0}", processSettings.FileName);
            Console.WriteLine("Csc Arguments: {0}", processSettings.Arguments);
        }

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

        cscProcess.WaitForExit();
        var output = cscProcess.StandardOutput.ReadToEnd();
        Console.WriteLine(output);

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
        foreach (var source in Directory.EnumerateFiles(Path.Combine(project.ProjectDirectory), "*.cs"))
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

static class OtherActions
{
    internal static void CreateNewProject(string directory)
    {
        using (var file = new StreamWriter(Path.Combine(properties.ProjectDirectory, "main.cs"), false))
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

    internal static void CreateNugetConfig(ProjectProperties properties)
    {
        using (var file = new StreamWriter(Path.Combine(properties.ToolsDirectory, "nuget.config"), false))
        {
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

