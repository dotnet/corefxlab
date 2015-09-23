// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace dotnet
{
    internal static class ProjectPropertiesHelpers
    {
        public static ProjectProperties InitializeProperties(string[] args, Log log, bool buildDll = false)
        {
            // General Properites
            var properties = new ProjectProperties();
            var currentDirectory = Directory.GetCurrentDirectory();

            properties.ProjectDirectory = Path.Combine(currentDirectory);
            properties.PackagesDirectory = Path.Combine(properties.ProjectDirectory, "packages");
            properties.OutputDirectory = Path.Combine(properties.ProjectDirectory, "bin");
            properties.ToolsDirectory = Path.Combine(properties.ProjectDirectory, "tools");
            properties.AssemblyName = Path.GetFileName(properties.ProjectDirectory);
            properties.OutputType = buildDll ? ".dll" : ".exe";
            FindCompiler(properties);

            var specifiedProjectFilename = Array.Find(args, element => element.EndsWith(".dotnetproj"));
            var specifiedProjectFile = specifiedProjectFilename == null
                ? null
                : Directory.GetFiles(properties.ProjectDirectory, specifiedProjectFilename);
            var projectFiles = Directory.GetFiles(properties.ProjectDirectory, "*.dotnetproj");
            var projectFilename = specifiedProjectFile == null
                ? projectFiles.Length == 1 ? projectFiles[0] : ""
                : specifiedProjectFile.Length == 1 ? specifiedProjectFile[0] : "";

            AddToListWithoutDuplicates(properties.Sources, ParseProjectFile(properties, projectFilename, "Compile"));

            var specifiedSourceFilenames = Array.FindAll(args,
                element => element.EndsWith(".cs") && !element.StartsWith("/"));

            foreach (var sourceFilename in specifiedSourceFilenames)
            {
                var sourceFiles = Directory.GetFiles(properties.ProjectDirectory, sourceFilename);
                foreach (var f in sourceFiles)
                {
                    if (!f.StartsWith(properties.PackagesDirectory) && !f.StartsWith(properties.OutputDirectory) &&
                        !f.StartsWith(properties.ToolsDirectory))
                    {
                        AddToListWithoutDuplicates(properties.Sources, f);
                    }
                }
            }

            var recurseOption = Array.Find(args, element => element.StartsWith("/recurse"));
            var recurseWildcard = "";
            if (recurseOption != null && recurseOption.Split(':').Length == 2)
            {
                recurseWildcard = recurseOption.Split(':')[1];
            }

            if (recurseOption != null && recurseWildcard != "*.cs")
            {
                AddToListWithoutDuplicates(properties.Sources,
                    Directory.GetFiles(properties.ProjectDirectory, recurseWildcard, SearchOption.AllDirectories));
            }

            if (properties.Sources.Count == 1)
            {
                properties.AssemblyName = Path.GetFileNameWithoutExtension(properties.Sources[0]);
            }

            var platformOption = Array.Find(args, element => element.StartsWith("/platform"));

            var platformOptionSpecicifcation = "x64";
            if (platformOption != null && platformOption.Split(':').Length == 2)
            {
                platformOptionSpecicifcation = GetPlatformOption(platformOption);
            }

            if (platformOption != null)
            {
                // The anycpu32bitpreferred setting is valid only for executable (.EXE) files
                if (!(platformOption == "/platform:anycpu32bitpreferred" && buildDll))
                    properties.CscOptions.Add(platformOption);
            }

            // Packages
            properties.Packages.Add(@"""Microsoft.NETCore"": ""5.0.0""");
            properties.Packages.Add(@"""System.Console"": ""4.0.0-beta-23123""");
            //properties.Packages.Add(@"""Microsoft.NETCore.Console"": ""1.0.0-beta-*""");
            properties.Packages.Add(GetConsoleHost(platformOptionSpecicifcation));
            properties.Packages.Add(GetRuntimeCoreClr(platformOptionSpecicifcation));

            // References
            properties.References.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Runtime\4.0.20\ref\dotnet\System.Runtime.dll"));
            properties.References.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Console\4.0.0-beta-23123\ref\dotnet\System.Console.dll"));

            // Runtime Dependencies
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                GetRuntimeCoreClrDependencyNative(platformOptionSpecicifcation, "win7")));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                GetRuntimeCoreClrDependencyLibrary(platformOptionSpecicifcation, "win7")));

            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Runtime\4.0.20\lib\netcore50"));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Console\4.0.0-beta-23123\lib\DNXCore50"));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory, @"System.IO\4.0.10\lib\netcore50"));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Threading\4.0.10\lib\netcore50"));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                @"System.IO.FileSystem.Primitives\4.0.0\lib\dotnet"));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Text.Encoding\4.0.10\lib\netcore50"));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Threading.Tasks\4.0.10\lib\netcore50"));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Text.Encoding.Extensions\4.0.10\lib\netcore50"));
            properties.Dependencies.Add(Path.Combine(properties.PackagesDirectory,
                @"System.Runtime.InteropServices\4.0.20\lib\netcore50"));

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
            AddToListWithoutDuplicates(properties.Packages, ParseProjectFile(properties, projectFilename, "Package"));

            LogProperties(log, properties, "Adjusted Properties Log:", buildDll);

            return properties;
        }

        public static string GetPlatformOption(string option)
        {
            var tempSpecification = option.Split(':')[1];
            if (tempSpecification == "anycpu32bitpreferred" || tempSpecification == "x86")
            {
                return "x86";
            }
            return "x64";
        }

        public static string GetRuntimeCoreClr(string platform)
        {
            // platform - x86, x64, arm
            return "\"Microsoft.NETCore.Runtime.CoreCLR-" + platform.ToLower() + "\": \"1.0.0\"";
        }

        public static string GetConsoleHost(string platform)
        {
            // platform - x86, x64, arm
            return "\"Microsoft.NETCore.ConsoleHost-" + platform.ToLower() + "\": \"1.0.0-beta-23123\"";
        }

        public static string GetConsoleHostNative(string platform, string os)
        {
            // platform - x86, x64, arm
            return "Microsoft.NETCore.ConsoleHost-" + platform.ToLower() + "\\1.0.0-beta-23123\\runtimes\\" +
                   os.ToLower() + "-" + platform.ToLower() + "\\native";
        }

        public static string GetRuntimeCoreClrDependencyNative(string platform, string os)
        {
            // platform - x86, x64, arm
            // os - win7, win8
            return "Microsoft.NETCore.Runtime.CoreCLR-" + platform.ToLower() + "\\1.0.0\\runtimes\\" + os.ToLower() +
                   "-" + platform.ToLower() + "\\native";
        }

        public static string GetRuntimeCoreClrDependencyLibrary(string platform, string os)
        {
            // platform - x86, x64, arm
            // os - win7, win8
            return "Microsoft.NETCore.Runtime.CoreCLR-" + platform.ToLower() + "\\1.0.0\\runtimes\\" + os.ToLower() +
                   "-" + platform.ToLower() + "\\lib\\dotnet";
        }

        private static void AddToListWithoutDuplicates(ICollection<string> list, List<string> files)
        {
            foreach (var file in files.Where(file => !list.Contains(file)))
            {
                list.Add(file);
            }
        }

        private static void AddToListWithoutDuplicates(ICollection<string> list, string file)
        {
            if (!list.Contains(file))
            {
                list.Add(file);
            }
        }

        private static void AddToListWithoutDuplicates(ICollection<string> list, IEnumerable<string> files)
        {
            foreach (var file in files.Where(file => !list.Contains(file)))
            {
                list.Add(file);
            }
        }

        private static void LogProperties(this Log log, ProjectProperties project, string heading, bool buildDll = false)
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

        private static void Adjust(string adjustmentFilePath, ICollection<string> list)
        {
            if (!File.Exists(adjustmentFilePath)) return;
            foreach (var line in File.ReadAllLines(adjustmentFilePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("//")) continue; // commented out line
                var adjustment = line.Substring(1).Trim();
                if (line.StartsWith("-"))
                {
                    list.Remove(adjustment);
                }
                else
                {
                    list.Add(adjustment);
                }
            }
        }

        private static List<string> ParseProjectFile(ProjectProperties properties, string projectFile,
            string elementName)
        {
            var attributes = GetAttributes(elementName);
            if (!File.Exists(projectFile) || attributes == null)
            {
                return new List<string>();
            }

            var attributeValues = new List<string>();
            using (var xmlReader = XmlReader.Create(projectFile))
            {
                xmlReader.MoveToContent();
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == elementName)
                    {
                        attributeValues.AddRange(attributes.Select(attribute => xmlReader.GetAttribute(attribute)));
                    }
                }
            }

            List<string> values;
            switch (elementName)
            {
                case "Compile":
                    values = GetSourcesFromProjectFile(properties, attributeValues);
                    break;
                case "Package":
                    values = GetPackagesFromProjectFile(attributeValues);
                    break;
                default:
                    values = new List<string>();
                    break;
            }
            return values;
        }

        private static List<string> GetAttributes(string elementName)
        {
            var compileAttributes = new List<string> {"Include"};
            var packageAttributes = new List<string> {"Id", "Version"};
            List<string> attributes;
            switch (elementName)
            {
                case "Compile":
                    attributes = compileAttributes;
                    break;
                case "Package":
                    attributes = packageAttributes;
                    break;
                default:
                    attributes = null;
                    break;
            }
            return attributes;
        }

        private static List<string> GetSourcesFromProjectFile(ProjectProperties properties,
            IEnumerable<string> attributeValues)
        {
            var sourceFiles = new List<string>();
            foreach (var val in attributeValues)
            {
                if (val == "*.cs")
                {
                    sourceFiles.AddRange(Directory.GetFiles(properties.ProjectDirectory, "*.cs"));
                }
                else if (val.EndsWith("\\*.cs"))
                {
                    sourceFiles.AddRange(
                        Directory.GetFiles(Path.Combine(properties.ProjectDirectory, val.Replace("\\*.cs", "")), "*.cs"));
                }
                else
                {
                    sourceFiles.Add(Path.Combine(properties.ProjectDirectory, val));
                }
            }
            return sourceFiles;
        }

        private static List<string> GetPackagesFromProjectFile(IReadOnlyList<string> attributeValues)
        {
            var packages = new List<string>();
            for (var i = 0; i < attributeValues.Count; i += 2)
            {
                packages.Add("\"" + attributeValues[i] + "\": \"" + attributeValues[i + 1] + "\"");
            }
            return packages;
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
    }
}