// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace dotnet
{
    internal static class Program
    {
        private static readonly Log Log = new Log();
        private static readonly Settings Settings = new Settings();

        private static void Main(string[] args)
        {
            if (Array.Exists(args, element => element == "/new"))
            {
                OtherActions.CreateNewProject();
                return;
            }
            if (Array.Exists(args, element => element == "/clean"))
            {
                OtherActions.Clean(Settings, Log);
                return;
            }

            if (!ParseArguments(args))
            {
                PrintUsage();
            }
            else
            {
                Build(Settings, Log);
            }
        }

        private static bool ParseArguments(string[] args)
        {
            if (Settings.NeedHelp(args) || !Settings.IsValid(args))
            {
                return false;
            }

            Settings.Log = Log.IsEnabled = Array.Exists(args, element => element == "/log");
            Settings.Optimize = Array.Exists(args, element => element == "/optimize");
            Settings.Unsafe = Array.Exists(args, element => element == "/unsafe");

            var currentDirectory = Directory.GetCurrentDirectory();

            var specifiedProjectFilename = Array.Find(args, element => element.EndsWith(".dotnetproj"));
            var specifiedProjectFile = specifiedProjectFilename == null
                ? null
                : Directory.GetFiles(currentDirectory, specifiedProjectFilename);
            var projectFiles = Directory.GetFiles(currentDirectory, "*.dotnetproj");
            Settings.ProjectFile = specifiedProjectFile == null
                ? projectFiles.Length == 1 ? projectFiles[0] : ""
                : specifiedProjectFile.Length == 1 ? specifiedProjectFile[0] : "";

            var specifiedSourceFilenames = Array.FindAll(args, element => element.EndsWith(".cs") && !element.StartsWith("/"));
            foreach (var sourceFilename in specifiedSourceFilenames)
            {
                Settings.SourceFiles.AddRange(Directory.GetFiles(currentDirectory, sourceFilename));
            }

            return ValidateAndSetOptionSpecifications(Array.Find(args, element => element.StartsWith("/target")),
                Settings.SetTargetSpecification) &&
                   ValidateAndSetOptionSpecifications(Array.Find(args, element => element.StartsWith("/platform")),
                       Settings.SetPlatformSpecification) &&
                   ValidateAndSetOptionSpecifications(Array.Find(args, element => element.StartsWith("/debug")),
                       Settings.SetDebugSpecification) &&
                   ValidateAndSetOptionSpecifications(Array.Find(args, element => element.StartsWith("/recurse")),
                       Settings.SetRecurseSpecification);
        }

        private static bool ValidateAndSetOptionSpecifications(string option, Func<string, bool> setFunction)
        {
            if (option == null) return true;
            if (option.Split(':').Length == 2)
            {
                var optionSpecification = option.Split(':')[1];
                if (!setFunction(optionSpecification))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private static void PrintUsage()
        {
            const string appName = "dotnet.exe";
            Console.WriteLine("{0} /? or /help        - help", appName);
            Console.WriteLine("{0} /new               - creates template sources for a new console app", appName);
            Console.WriteLine("{0} /clean             - deletes tools, packages, and bin project subdirectories",
                appName);
            Console.WriteLine(
                "{0} [/log] [/target:{{exe|library}}] [/recurse:<wildcard>] [/debug:{{full|pdbonly}}] [/optimize] [/unsafe] [/platform:{{anycpu|anycpu32bitpreferred|x86|x64}}] [ProjectFile] [SourceFiles]",
                appName);
            Console.WriteLine("           /log        - logs diagnostics info");
            Console.WriteLine(
                "           /target     - compiles the sources in the current directory into an exe (default) or dll");
            Console.WriteLine(
                "           /recurse    - compiles the sources in the current directory and subdirectories specified by the wildcard");
            Console.WriteLine("           /debug      - generates debugging information");
            Console.WriteLine("           /optimize   - enables optimizations performed by the compiler");
            Console.WriteLine("           /unsafe     - allows compilation of code that uses the unsafe keyword");
            Console.WriteLine(
                "           /platform   - specifies which platform this code can run on, default is anycpu");
            Console.WriteLine(
                "           ProjectFile - specifies which project file to use, default to the one in the current directory, if only one exists");
            Console.WriteLine("           SourceFiles - specifices which source files to compile");
            Console.WriteLine("NOTE #1: uses csc.exe in <project>\\tools subdirectory, or csc.exe on the path.");
            Console.WriteLine("NOTE #2: dependencies.txt, references.txt can be used to override details.");
        }

        private static void Build(Settings settings, Log log)
        {
            var properties = ProjectPropertiesHelpers.InitializeProperties(settings, log);

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

            if (!NugetAction.GetNugetAndRestore(properties, log))
            {
                return;
            }

            if (!CscAction.Execute(properties, Log)) return;
            if (Settings.Target != "library")
            {
                ConvertToCoreConsoleAction(properties);
            }
            OutputRuntimeDependenciesAction(properties);
            Console.WriteLine("bin\\{0} created", properties.AssemblyNameAndExtension);
        }

        private static void OutputRuntimeDependenciesAction(ProjectProperties properties)
        {
            foreach (var dependencyFolder in properties.Dependencies)
            {
                Helpers.CopyAllFiles(dependencyFolder, properties.OutputDirectory);
            }
        }

        private static void ConvertToCoreConsoleAction(ProjectProperties properties)
        {
            var dllPath = Path.ChangeExtension(properties.OutputAssemblyPath, "dll");
            if (File.Exists(dllPath))
            {
                File.Delete(dllPath);
            }
            File.Move(properties.OutputAssemblyPath, dllPath);

            var coreConsolePath =
                ProjectPropertiesHelpers.GetConsoleHostNative(ProjectPropertiesHelpers.GetPlatformOption(Settings.Platform), "win7") +
                "\\CoreConsole.exe";
            File.Copy(Path.Combine(properties.PackagesDirectory, coreConsolePath), properties.OutputAssemblyPath);
        }
    }
}