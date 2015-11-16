// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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

            var specifiedSourceFilenames = Array.FindAll(args,
                element => element.EndsWith(".cs") && !element.StartsWith("/"));
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
                   ValidateAndSetOptionSpecifications(Array.Find(args, element => element.StartsWith("/reference")),
                       Settings.SetReferenceSpecification) &&
                   ValidateAndSetOptionSpecifications(Array.Find(args, element => element.StartsWith("/recurse")),
                       Settings.SetRecurseSpecification) &&
                   ValidateAndSetOptionSpecifications(Array.Find(args, element => element.StartsWith("/runtime")),
                       Settings.SetRuntimeSpecification);
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
                "{0} [/log] [/target:{{exe|library}}] [/recurse:<wildcard>] [/debug:{{full|pdbonly}}] [/optimize] [/unsafe] [/platform:{{anycpu|anycpu32bitpreferred|x86|x64}}] [/reference:<path to dll>] [/runtime:<runtime>] [ProjectFile] [SourceFiles]",
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
                "           /reference  - specifies additional reference dlls to compile against");
            Console.WriteLine(
                "           /runtime    - specifies which runtime to target from the list specified in project.json, default is win7-x64");
            Console.WriteLine(
                "           ProjectFile - specifies which project file to use, default to the one in the current directory, if only one exists");
            Console.WriteLine("           SourceFiles - specifices which source files to compile");
            Console.WriteLine("NOTE #1: uses csc.exe in <project>\\tools subdirectory, or csc.exe on the path.");
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

            if (!GetDependencies(properties, log))
            {
                log.WriteLine("Unable to get dependencies from the project.lock.json file.");
            }

            if (!CscAction.Execute(properties, Log)) return;
            if (Settings.Target != "library")
            {
                ConvertToCoreConsoleAction(properties);
            }
            OutputRuntimeDependenciesAction(properties, log);
            Console.WriteLine("bin\\{0} created", properties.AssemblyNameAndExtension);
        }

        private static void OutputRuntimeDependenciesAction(ProjectProperties properties, Log log)
        {
            foreach (var dependencyFolder in properties.Dependencies)
            {
                Helpers.CopyFile(dependencyFolder, properties.OutputDirectory, log);
            }
        }

        private static void ConvertToCoreConsoleAction(ProjectProperties properties)
        {
            string dllPath;
            if (string.IsNullOrEmpty(properties.OutputType))
            {
                dllPath = properties.OutputAssemblyPath + ".dll";
            }
            else
            {
                dllPath = Path.ChangeExtension(properties.OutputAssemblyPath, "dll");
            }

            if (File.Exists(dllPath))
            {
                File.Delete(dllPath);
            }
            File.Move(properties.OutputAssemblyPath, dllPath);

            var coreConsolePath = properties.Dependencies.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p).Equals("coreconsole", StringComparison.OrdinalIgnoreCase));
            File.Copy(coreConsolePath, properties.OutputAssemblyPath);
        }

        private static bool GetDependencies(ProjectProperties properties, Log log)
        {
            var projectLockFile = Path.Combine(properties.ProjectDirectory, "project.lock.json");

            if (!File.Exists(projectLockFile))
            {
                projectLockFile = Path.Combine(properties.ToolsDirectory, "project.lock.json");
            }

            var jsonString = File.ReadAllText(projectLockFile);
            var docJsonOutput = JsonConvert.DeserializeObject<ProjectLockJson>(jsonString);

            var target = properties.Target;
            if (!string.IsNullOrEmpty(properties.RuntimeIdentifier))
            {
                target += "/" + properties.RuntimeIdentifier;
            }
            Dictionary<string, Target> packages;
            docJsonOutput.Targets.TryGetValue(target, out packages);
            if (packages == null)
            {
                log.Error("Packages for the specified target not found {0}.", properties.Target);
                return false;
            }

            var references = new List<string>();
            var dependencies = new List<string>();

            foreach (var package in packages)
            {
                if (package.Value.Compile != null)
                {
                    var compileKeys = package.Value.Compile.Keys;
                    if (compileKeys.Count != 0)
                    {
                        references.AddRange(
                            compileKeys.Select(
                                key => properties.ProjectDirectory + "/packages/" + package.Key + "/" + key));
                    }
                }

                if (package.Value.Native != null)
                {
                    var nativeKeys = package.Value.Native.Keys;
                    if (nativeKeys.Count != 0)
                    {
                        dependencies.AddRange(
                            nativeKeys.Select(
                                key => properties.ProjectDirectory + "/packages/" + package.Key + "/" + key));
                    }
                }

                if (package.Value.Runtime != null)
                {
                    var runtimeKeys = package.Value.Runtime.Keys;
                    if (runtimeKeys.Count != 0)
                    {
                        dependencies.AddRange(
                            runtimeKeys.Select(
                                key => properties.ProjectDirectory + "/packages/" + package.Key + "/" + key));
                    }
                }
            }

            references.RemoveAll(x => x.EndsWith("_._"));

            foreach (var reference in references)
            {
                properties.References.Add(Path.Combine(properties.PackagesDirectory, reference));
            }
            foreach (var outputAssembly in dependencies)
            {
                properties.Dependencies.Add(outputAssembly);
            }

            return true;
        }
    }
}