// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace dotnet
{
    internal static class Program
    {
        private static readonly Log Log = new Log();

        // Arguments specifications used for parsing
        private static readonly List<string> CommandFunctions = new List<string> {"/new", "/clean", "/?", "/help"};
        private static readonly List<string> CommandSwitches = new List<string> {"/log", "/optimize", "/unsafe"};

        private static readonly List<string> CommandSwitchesWithSpecifications = new List<string>
        {
            "/target",
            "/recurse",
            "/debug",
            "/platform"
        };

        private static readonly List<string> TargetSpecifications = new List<string> {"exe", "library"};
        private static readonly List<string> DebugSpecifications = new List<string> {"full", "pdbonly"};

        private static readonly List<string> PlatformSpecifications = new List<string>
        {
            "anycpu",
            "anycpu32bitpreferred",
            "x64",
            "x86"
        };

        private static void Main(string[] args)
        {
            ParseArguments(args);
        }

        private static void ParseArguments(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            // Defaults
            if (args.Length == 0)
            {
                args = new[] {"/target:exe"};
            }

            Log.IsEnabled = Array.Exists(args, element => element == "/log");

            var buildDll = false;

            // Incorrect arguments passed in or user asked for help
            var printUsage = false;

            var compilerFunctionSelected = false;

            // Command arguments parsing
            foreach (var argument in args)
            {
                if (printUsage || compilerFunctionSelected) break;

                var argOptions = argument.Split(':');
                var compilerOption = argument.StartsWith("/") ? argOptions[0] : argument;

                if (CommandFunctions.Contains(compilerOption))
                {
                    compilerFunctionSelected = true;
                    switch (compilerOption)
                    {
                        case "/new":
                            OtherActions.CreateNewProject(currentDirectory);
                            break;

                        case "/clean":
                            OtherActions.Clean(args, Log);
                            break;

                        case "/?":
                        case "/help":
                        default:
                            printUsage = true;
                            break;
                    }
                }
                else if (CommandSwitches.Contains(compilerOption))
                {
                    // Do nothing, just pass those switches to the CSC Options
                }
                else if (CommandSwitchesWithSpecifications.Contains(compilerOption))
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
                                if (!TargetSpecifications.Contains(commandSpecification))
                                {
                                    Console.WriteLine("Please specify the {0} compiler option correctly.",
                                        compilerOption);
                                    printUsage = true;
                                }
                                else
                                {
                                    buildDll = commandSpecification == "library";
                                }
                                break;

                            case "/debug":
                                if (!DebugSpecifications.Contains(commandSpecification))
                                {
                                    Console.WriteLine("Please specify the {0} compiler option correctly.",
                                        compilerOption);
                                    printUsage = true;
                                }
                                break;

                            case "/platform":
                                if (!PlatformSpecifications.Contains(commandSpecification))
                                {
                                    Console.WriteLine("Please specify the {0} compiler option correctly.",
                                        compilerOption);
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
                    try
                    {
                        var files = Directory.GetFiles(currentDirectory, compilerOption);

                        if (!File.Exists(compilerOption) && files.Length == 0)
                        {
                            Console.WriteLine("Could not find the file \"{0}\" in the current directory.",
                                compilerOption);
                            printUsage = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception when trying to parse arguments: {0}", e.Message);
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

        private static void Build(string[] args, Log log, bool buildDll = false)
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

            if (!NugetAction.DownloadNugetAction(properties) || !NugetAction.RestorePackagesAction(properties, Log))
            {
                Console.WriteLine("Failed to get nuget or restore packages.");
                return;
            }

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

            var defaultOption = "/platform:anycpu";
            var platformOption = properties.CscOptions.Find(x => x.StartsWith("/platform"));
            var coreConsolePath =
                ProjectPropertiesHelpers.GetConsoleHostNative(
                    ProjectPropertiesHelpers.GetPlatformOption(platformOption ?? defaultOption), "win7") +
                "\\CoreConsole.exe";
            File.Copy(Path.Combine(properties.PackagesDirectory, coreConsolePath), properties.OutputAssemblyPath);
        }
    }
}