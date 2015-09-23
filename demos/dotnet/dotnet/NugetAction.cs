// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;

namespace dotnet
{
    internal static class NugetAction
    {
        public static bool GetNugetAndRestore(ProjectProperties properties, Log log)
        {
            var nugetFile = Path.Combine(properties.ToolsDirectory, "nuget.exe");
            var success = true;
            if (!File.Exists(nugetFile))
            {
                success = DownloadNugetAction(properties, nugetFile);
            }

            var projectJsonFile = Path.Combine(properties.ProjectDirectory, "project.json");
            if (!File.Exists(projectJsonFile))
            {
                projectJsonFile = Path.Combine(properties.ToolsDirectory, "project.json");
            }

            if (!success || !RestorePackagesAction(properties, log, nugetFile, projectJsonFile))
            {
                Console.WriteLine("Failed to get nuget or restore packages.");
                return false;
            }
            return true;
        }

        public static bool DownloadNugetAction(ProjectProperties properties, string nugetFile)
        {
            CreateDefaultProjectJson(properties);
            CreateNugetConfig(properties);

            var client = new HttpClient();
            var requestUri = new Uri(@"http://dist.nuget.org/win-x86-commandline/v3.1.0-beta/nuget.exe",
                UriKind.Absolute);

            var sourceStreamTask = client.GetStreamAsync(requestUri);
            try
            {
                sourceStreamTask.Wait();
            }
            catch (AggregateException exception)
            {
                foreach (var ex in exception.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }

            using (var sourceStream = sourceStreamTask.Result)
            using (var destinationStream = new FileStream(nugetFile, FileMode.Create, FileAccess.Write))
            {
                var buffer = new byte[1024];
                while (true)
                {
                    var read = sourceStream.Read(buffer, 0, buffer.Length);
                    if (read < 1) break;
                    destinationStream.Write(buffer, 0, read);
                }
            }

            return true;
        }

        private static void CreateDefaultProjectJson(ProjectProperties properties)
        {
            var fileName = Path.Combine(properties.ToolsDirectory, "project.json");
            var fs = new FileStream(fileName, FileMode.Create);
            using (var file = new StreamWriter(fs, Encoding.UTF8))
            {
                file.WriteLine(@"{");
                file.WriteLine(@"    ""dependencies"": {");

                for (var index = 0; index < properties.Packages.Count; index++)
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

        private static void CreateNugetConfig(ProjectProperties properties)
        {
            var fileName = Path.Combine(properties.ToolsDirectory, "nuget.config");
            var fs = new FileStream(fileName, FileMode.Create);
            using (var file = new StreamWriter(fs, Encoding.UTF8))
            {
                file.WriteLine(@"<?xml version = ""1.0"" encoding=""utf-8""?>");
                file.WriteLine(@"<configuration>");
                file.WriteLine(@"    <packageSources>");

                file.WriteLine(
                    @"        <add key = ""netcore-prototype"" value=""https://www.myget.org/F/netcore-package-prototyping""/>");
                file.WriteLine(
                    @"        <add key = ""nuget.org"" value = ""https://api.nuget.org/v3/index.json"" protocolVersion = ""3""/>");
                file.WriteLine(@"        <add key = ""nuget.org"" value = ""https://www.nuget.org/api/v2/""/>");

                file.WriteLine(@"    </packageSources>");
                file.WriteLine(@"</configuration>");
            }
        }

        public static bool RestorePackagesAction(ProjectProperties properties, Log log, string nugetFile, string jsonFile)
        {
            Console.WriteLine("restoring packages");

            if (!File.Exists(nugetFile))
            {
                Console.WriteLine("Could not find file {0}.", nugetFile);
                return false;
            }

            if (!File.Exists(jsonFile))
            {
                Console.WriteLine("Could not find file {0}.", jsonFile);
                return false;
            }

            var processSettings = new ProcessStartInfo
            {
                FileName = nugetFile,
                Arguments =
                    "restore " + jsonFile + " -PackagesDirectory " + properties.PackagesDirectory + " -ConfigFile " +
                    Path.Combine(properties.ToolsDirectory, "nuget.config"),
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            log.WriteLine("Executing {0}", processSettings.FileName);
            log.WriteLine("Arguments: {0}", processSettings.Arguments);
            log.WriteLine("project.json:\n{0}", File.ReadAllText(jsonFile));

            using (var process = Process.Start(processSettings))
            {
                try
                {
                    if (process != null)
                    {
                        var output = process.StandardOutput.ReadToEnd();
                        var error = process.StandardError.ReadToEnd();
                        log.WriteLine(output);
                        log.Error(error);
                        process.WaitForExit();
                        var exitCode = process.ExitCode;
                        if (exitCode != 0) Console.WriteLine("Process exit code: {0}", exitCode);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            return true;
        }
    }
}