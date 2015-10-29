// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace dotnet
{
    internal static class CscAction
    {
        public static bool Execute(ProjectProperties properties, Log log)
        {
            Console.WriteLine("compiling");
            var processSettings = new ProcessStartInfo
            {
                FileName = properties.CscPath,
                Arguments = properties.FormatCscArguments()
            };

            log.WriteLine("Executing {0}", processSettings.FileName);
            log.WriteLine("Csc Arguments: {0}", processSettings.Arguments);

            processSettings.CreateNoWindow = true;
            processSettings.RedirectStandardOutput = true;
            processSettings.UseShellExecute = false;

            Process cscProcess;
            try
            {
                cscProcess = Process.Start(processSettings);
            }
            catch (Win32Exception)
            {
                Console.WriteLine("ERROR: csc.exe needs to be on the path.");
                return false;
            }

            if (cscProcess == null) return false;
            var output = cscProcess.StandardOutput.ReadToEnd();
            log.WriteLine(output);

            cscProcess.WaitForExit();

            return !output.Contains("error CS");
        }

        private static string FormatReferenceOption(this ProjectProperties project)
        {
            var builder = new StringBuilder();
            builder.Append(" /r:");
            var first = true;
            foreach (var reference in project.References)
            {
                if (!first)
                {
                    builder.Append(',');
                }
                else
                {
                    first = false;
                }
                builder.Append(reference);
            }
            builder.Append(" ");
            return builder.ToString();
        }

        private static string FormatSourcesOption(this ProjectProperties project)
        {
            var builder = new StringBuilder();
            foreach (var source in project.Sources)
            {
                builder.Append(" ");
                builder.Append(source);
            }
            return builder.ToString();
        }

        private static string FormatCscOptions(this ProjectProperties project)
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

        private static string FormatCscArguments(this ProjectProperties project)
        {
            return project.FormatCscOptions() + project.FormatReferenceOption() + project.FormatSourcesOption();
        }
    }
}