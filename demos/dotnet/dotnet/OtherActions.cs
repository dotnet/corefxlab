// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;

namespace dotnet
{
    internal static class OtherActions
    {
        internal static void CreateNewProject(string directory)
        {
            var fileName = Path.Combine(directory, "main.cs");
            var fs = new FileStream(fileName, FileMode.Create);
            using (var file = new StreamWriter(fs, Encoding.UTF8))
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

        internal static void Clean(string[] args, Log log)
        {
            var previous = log.IsEnabled;
            log.IsEnabled = false;
            var properties = ProjectPropertiesHelpers.InitializeProperties(args, log);
            log.IsEnabled = previous;
            try
            {
                if (Directory.Exists(properties.ToolsDirectory)) Directory.Delete(properties.ToolsDirectory, true);
                if (Directory.Exists(properties.OutputDirectory)) Directory.Delete(properties.OutputDirectory, true);
                if (Directory.Exists(properties.PackagesDirectory))
                    Directory.Delete(properties.PackagesDirectory, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during cleanup: {0}", e.Message);
            }
        }
    }
}