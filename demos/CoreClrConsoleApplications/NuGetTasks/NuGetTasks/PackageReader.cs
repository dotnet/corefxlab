using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Microsoft.NuGet
{
    class PackageManager
    {
        public static List<Package> ReadPackagesConfig(string packagesConfigPath)
        {
            var packages = new List<Package>();

            using (Stream stream = File.OpenRead(packagesConfigPath))
            using (XmlReader reader = XmlReader.Create(stream))
            {
                while (reader.Read())
                {
                    if (reader.Name == "package")
                    {
                        var id = reader.GetAttribute("id");
                        var version = reader.GetAttribute("version");
                        packages.Add(new Package() { Id = id, Version = version });
                    }
                }
            }

            return packages;
        }

        public static void InstallNugetExe(string nugetExePath)
        {
            if (!File.Exists(nugetExePath))
            {
                var directory = Path.GetDirectoryName(nugetExePath);
                Directory.CreateDirectory(directory);
                var client = new System.Net.WebClient();
                client.DownloadFile("http://nuget.org/nuget.exe", nugetExePath);
            }
        }

        public static bool InstallPackages(string packagesDirectory, string packagesConfigPath, string nugetExePath, TaskLoggingHelper log)
        {
            var nugetArguments = @"install -o " + packagesDirectory + " -Prerelease -NonInteractive " + packagesConfigPath;
            log.LogMessage("Installing: " + nugetExePath + " " + nugetArguments);
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.FileName = nugetExePath;
            psi.Arguments = nugetArguments;
            psi.CreateNoWindow = true;
            var process = Process.Start(psi);
            if (!process.WaitForExit(20000))
            {
                log.LogError("Packages installation timed out.");
                return false;
            }
            return true;
        }

        public static bool InstallPackage(string packagesDirectory, Package package, string nugetExePath, TaskLoggingHelper log)
        {
            var nugetArguments = @"install " + package.Id + " -Version " + package.Version + " -o " + packagesDirectory + " -Prerelease -NonInteractive";
            log.LogMessage("Installing: " + nugetExePath + " " + nugetArguments);
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.FileName = nugetExePath;
            psi.Arguments = nugetArguments;
            psi.CreateNoWindow = true;
            var process = Process.Start(psi);
            if (!process.WaitForExit(5000))
            {
                log.LogError("Packages installation timed out.");
                return false;
            }
            return true;
        }
    }

    struct Package
    {
        public string Id;
        public string Version;

        public string FullName
        {
            get { return Id + "." + Version; }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
