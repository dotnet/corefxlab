// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;

struct Properties
{
    public string ProjectDirectory;
    public string PackagesDirectory;
    public string OutputDirectory;
    public string ToolsDirectory;
    public string ExecutableFilename;
    public List<string> Sources;
    public List<string> References;
    public List<string> Dependencies;
    public List<string> CscOptions;
    public List<string> Packages;
    public bool VerboseLogging;

    public string ExecutablePath
    {
        get { return Path.Combine(OutputDirectory, ExecutableFilename); }
    }

    string FormatReferenceOption()
    {
        var builder = new StringBuilder();
        builder.Append(" /r:");
        bool first = true;
        foreach (var reference in References)
        {
            if (!first) { builder.Append(','); }
            else { first = false; }
            builder.Append(Path.Combine(PackagesDirectory, reference));
        }
        builder.Append(" ");
        return builder.ToString();
    }

    string FormatSourcesOption()
    {
        var builder = new StringBuilder();
        foreach (var source in Directory.EnumerateFiles(Path.Combine(ProjectDirectory), "*.cs"))
        {
            builder.Append(" ");
            builder.Append(source);
        }
        return builder.ToString();
    }

    string FormatCscOptions()
    {
        var builder = new StringBuilder();
        foreach (var option in CscOptions)
        {
            builder.Append(" ");
            builder.Append(option);
        }
        builder.Append(" ");
        builder.Append("/out:");
        builder.Append(ExecutablePath);
        builder.Append(" ");
        return builder.ToString();
    }

    public string FormatCscArguments()
    {
        return FormatCscOptions() + FormatReferenceOption() + FormatSourcesOption();
    }

    internal void Log(TextWriter log)
    {
        if (!VerboseLogging) return;
        log.WriteLine("ProjectDirectory     {0}", ProjectDirectory);
        log.WriteLine("PackagesDirectory    {0}", PackagesDirectory);
        log.WriteLine("OutputDirectory      {0}", OutputDirectory);
        log.WriteLine("ToolsDirectory       {0}", ToolsDirectory);
        log.WriteLine("ExecutableFilename   {0}", ExecutableFilename);
        LogList(Sources, "SOURCES", log);
        LogList(Packages, "PACKAGES", log);
        LogList(References, "REFERENCES", log);
        LogList(Dependencies, "DEPENDENCIES", log);
        LogList(CscOptions, "CSCOPTIONS", log);
        log.WriteLine();
    }

    static void LogList(List<string> list, string listName, TextWriter log)
    {
        log.WriteLine("{0}:", listName);
        foreach(var str in list)
        {
            log.Write('\t');
            log.WriteLine(str);
        }
    }
}

static class Program
{
    static void Main(string[] args)
    {
        var properties = InitializeProperties(args);

        if (args.Length > 0 && args[0] == "/new")
        {
            CreateNewProject(properties);
            return;
        }

        if(properties.VerboseLogging) Console.WriteLine("Initialized Properties:");
        properties.Log(Console.Out);

        Adjust(Path.Combine(properties.ProjectDirectory, "dependencies.txt"), properties.Dependencies);
        Adjust(Path.Combine(properties.ProjectDirectory, "references.txt"), properties.References);
        Adjust(Path.Combine(properties.ProjectDirectory, "cscoptions.txt"), properties.CscOptions);
        Adjust(Path.Combine(properties.ProjectDirectory, "packages.txt"), properties.Packages);

        if (properties.VerboseLogging) Console.WriteLine("Adjusted Properties:");
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

        if (CscAction(properties))
        {
            ConvertToCoreConsoleAction(properties);
            OutputRuntimeDependenciesAction(properties);
        }

        Console.WriteLine("bin\\{0} created", properties.ExecutableFilename);
    }

    private static void CreateNewProject(Properties properties)
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

    private static void Adjust(string adjustmentFilePath, List<string> list)
    {
        if (File.Exists(adjustmentFilePath))
        {
            foreach (var line in File.ReadAllLines(adjustmentFilePath))
            {
                if (String.IsNullOrWhiteSpace(line)) continue;
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

    private static Properties InitializeProperties(string[] args)
    {

        // General Properites
        Properties properties = new Properties();
        properties.ProjectDirectory = Path.Combine(Environment.CurrentDirectory);
        properties.PackagesDirectory = Path.Combine(properties.ProjectDirectory, "packages");
        properties.OutputDirectory = Path.Combine(properties.ProjectDirectory, "bin");
        properties.ToolsDirectory = Path.Combine(properties.ProjectDirectory, "tools");
        properties.ExecutableFilename = Path.GetFileName(properties.ProjectDirectory) + ".exe";

        if (args.Length > 0 && args[0] == "/log")
        {
            properties.VerboseLogging = true;
        }

        // Sources
        properties.Sources = new List<string>(Directory.GetFiles(properties.ProjectDirectory, "*.cs"));
        if (properties.Sources.Count == 1)
        {
            properties.ExecutableFilename = Path.GetFileNameWithoutExtension(properties.Sources[0]) + ".exe";
        }

        // Packages
        properties.Packages = new List<string>();
        properties.Packages.Add(@"""Microsoft.NETCore.Console"": ""1.0.0-beta-*""");
        properties.Packages.Add(@"""Microsoft.NETCore.ConsoleHost-x86"": ""1.0.0-beta-23123""");

        // References
        properties.References = new List<string>();
        properties.References.Add(Path.Combine(properties.PackagesDirectory, @"System.Runtime\4.0.20\ref\dotnet\System.Runtime.dll"));
        properties.References.Add(Path.Combine(properties.PackagesDirectory, @"System.Console\4.0.0-beta-23123\ref\dotnet\System.Console.dll"));

        // Runtime Dependencies
        properties.Dependencies = new List<string>();
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
        properties.CscOptions = new List<string>();
        properties.CscOptions.Add("/nostdlib");
        properties.CscOptions.Add("/noconfig");

        return properties;
    }

    static void CopyAllFiles(string sourceFolder, string destinationFolder)
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

    static bool CscAction(Properties properties)
    {
        Console.WriteLine("compiling");
        var processSettings = new ProcessStartInfo();
        processSettings.FileName = "csc.exe";
        processSettings.Arguments = properties.FormatCscArguments();

        if(properties.VerboseLogging)
        {
            Console.WriteLine("Csc Arguments: {0}", processSettings.Arguments);
        }

        processSettings.CreateNoWindow = true;
        processSettings.RedirectStandardOutput = true;
        processSettings.UseShellExecute = false;
        var cscProcess = Process.Start(processSettings);
        cscProcess.WaitForExit();
        var output = cscProcess.StandardOutput.ReadToEnd();
        Console.WriteLine(output);

        if (output.Contains("error CS")) return false;
        return true;
    }

    static void OutputRuntimeDependenciesAction(Properties properties)
    {
        foreach (var dependencyFolder in properties.Dependencies) { 
            CopyAllFiles(dependencyFolder, properties.OutputDirectory);
        }
    }

    static void ConvertToCoreConsoleAction(Properties properties)
    {
        var dllPath = Path.ChangeExtension(properties.ExecutablePath, "dll");
        if(File.Exists(dllPath))
        {
            File.Delete(dllPath);
        }
        File.Move(properties.ExecutablePath, dllPath);
        File.Copy(Path.Combine(properties.PackagesDirectory, @"Microsoft.NETCore.ConsoleHost-x86\1.0.0-beta-23123\runtimes\win7-x86\native\CoreConsole.exe"), properties.ExecutablePath);
    }

    static void RestorePackagesAction(Properties properties)
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

    static void DownloadNugetAction(Properties properties)
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

        //webClient.DownloadFile(@"http://dist.nuget.org/win-x86-commandline/v3.1.0-beta/nuget.exe", destination);
    }

    static void CreateDefaultProjectJson(Properties properties)
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

