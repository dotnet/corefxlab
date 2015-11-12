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
        public static ProjectProperties InitializeProperties(Settings settings, Log log)
        {
            // General Properites
            var properties = new ProjectProperties();
            var currentDirectory = Directory.GetCurrentDirectory();

            var buildDll = settings.Target == "library";

            properties.ProjectDirectory = Path.Combine(currentDirectory);
            properties.PackagesDirectory = Path.Combine(properties.ProjectDirectory, "packages");
            properties.OutputDirectory = Path.Combine(properties.ProjectDirectory, "bin");
            properties.ToolsDirectory = Path.Combine(properties.ProjectDirectory, "tools");
            properties.AssemblyName = Path.GetFileName(properties.ProjectDirectory);
            properties.OutputType = buildDll ? ".dll" : (settings.Runtime.StartsWith("ubuntu") || settings.Runtime.StartsWith("linux")) ? "" : ".exe";
            properties.Target = "DNXCore,Version=v5.0";
            properties.RuntimeIdentifier = settings.Runtime;
            FindCompiler(properties);

            AddToListWithoutDuplicates(properties.Sources, ParseProjectFile(properties, settings.ProjectFile, "Compile"));
            
            foreach (var file in settings.SourceFiles.Where(f => !f.StartsWith(properties.PackagesDirectory) && !f.StartsWith(properties.OutputDirectory) &&
                                                              !f.StartsWith(properties.ToolsDirectory)))
            {
                AddToListWithoutDuplicates(properties.Sources, file);
            }

            if (!string.IsNullOrWhiteSpace(settings.Recurse) && settings.Recurse != "*.cs")
            {
                AddToListWithoutDuplicates(properties.Sources,
                    Directory.GetFiles(properties.ProjectDirectory, settings.Recurse, SearchOption.AllDirectories));
            }

            if (properties.Sources.Count == 1)
            {
                properties.AssemblyName = Path.GetFileNameWithoutExtension(properties.Sources[0]);
            }

            var platformOptionSpecicifcation = GetPlatformOption(settings.Platform);

            // The anycpu32bitpreferred setting is valid only for executable (.EXE) files
            if (!(settings.Platform == "anycpu32bitpreferred" && buildDll))
                properties.CscOptions.Add("/platform:" + settings.Platform);

            // Packages
            properties.Packages.Add(@"""Microsoft.NETCore"": ""5.0.1-beta-23505""");
            properties.Packages.Add(@"""System.Console"": ""4.0.0-beta-23505""");

            properties.Packages.Add(@"""System.Text.Formatting"": ""0.1.0-d103015-1""");
            properties.Packages.Add(@"""System.Slices"": ""0.1.0-d103015-1""");
            properties.Packages.Add(@"""System.Buffers"": ""0.1.0-d103015-1""");

            properties.Packages.Add(@"""Microsoft.NETCore.ConsoleHost"": ""1.0.0-beta-23505""");
            properties.Packages.Add(@"""Microsoft.NETCore.Runtime"": ""1.0.1-beta-23505""");

            // add explicit references
            if (settings.References != null)
            {
                properties.References.AddRange(settings.References);
            }

            // CSC OPTIONS
            properties.CscOptions.Add("/nostdlib");
            properties.CscOptions.Add("/noconfig");
            if (settings.Unsafe)
            {
                properties.CscOptions.Add("/unsafe");
            }

            if (settings.Optimize)
            {
                properties.CscOptions.Add("/optimize");
            }

            if (!string.IsNullOrWhiteSpace(settings.Debug))
            {
                properties.CscOptions.Add("/debug:" + settings.Debug);
            }

            properties.CscOptions.Add("/target:" + settings.Target);

            LogProperties(log, properties, "Initialized Properties Log:", buildDll);

            AddToListWithoutDuplicates(properties.Packages, ParseProjectFile(properties, settings.ProjectFile, "Package"));

            LogProperties(log, properties, "Adjusted Properties Log:", buildDll);

            return properties;
        }

        public static string GetPlatformOption(string optionSpecification)
        {
            if (optionSpecification == "anycpu32bitpreferred" || optionSpecification == "x86")
            {
                return "x86";
            }
            return "x64";
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

        private static void LogProperties(this Log log, ProjectProperties project, string heading, bool buildDll)
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

        private static void Adjust(ProjectProperties properties, string adjustmentFilePath, ICollection<string> list)
        { 
            if (!File.Exists(adjustmentFilePath)) return;
            foreach (var line in File.ReadAllLines(adjustmentFilePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("//")) continue; // commented out line
                var adjustment = line.Substring(1).Trim();
                if (line.StartsWith("-"))
                {
                    list.Remove(Path.Combine(properties.PackagesDirectory, adjustment));
                }
                else
                {
                    list.Add(Path.Combine(properties.PackagesDirectory, adjustment));
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
            string cscEnvPath = Environment.GetEnvironmentVariable("CSCPATH");
            if (cscEnvPath != null)
            {
                string fullCscPath = Path.Combine(cscEnvPath, "csc.exe");
                if (File.Exists(fullCscPath))
                {
                    properties.CscPath = fullCscPath;
                    return;
                }
            }

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