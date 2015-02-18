using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.NuGet
{
    public class ResolvePackages : Task
	{
		[Required]
		public string PackagesDirectory { get; set; }

		[Required]
		public string PackagesConfig { get; set; }

		[Output]
		public ITaskItem[] References { get; private set; }

		[Output]
        public ITaskItem[] Dependencies { get; private set; }

		public override bool Execute()
		{
            Log.LogMessage(MessageImportance.Low, "Using PackagesDirectory: {0}", PackagesDirectory);
            string nuGetDirectory = Path.Combine(PackagesDirectory, @"NuGetExe");
            string nuGetPath = Path.Combine(nuGetDirectory, @"nuget.exe");

            PackageManager.InstallNugetExe(nuGetPath);
            if(!PackageManager.InstallPackages(PackagesDirectory, PackagesConfig, nuGetPath, Log)) { return false; }

            List<ITaskItem> references = new List<ITaskItem>();
            List<ITaskItem> dependencies = new List<ITaskItem>();

            var installedPackages = PackageManager.ReadPackagesConfig(PackagesConfig);
            foreach (var package in installedPackages)
            {
                var packageRoot = Path.Combine(PackagesDirectory, package.FullName);
                var architecture = "amd64";
                if (package.FullName.StartsWith("CoreClr")) // this is temporary till we add support for implementation packages
                {
                    var native = Path.Combine(packageRoot, "native\\windows", architecture);
                    foreach (var file in Directory.EnumerateFiles(native)) {
                        dependencies.Add(new TaskItem(Path.Combine(native, file)));
                    }

                    var architectureDependentLibs = Path.Combine(packageRoot, "lib\\aspnetcore50", architecture);
                    foreach (var file in Directory.EnumerateFiles(architectureDependentLibs))
                    {
                        dependencies.Add(new TaskItem(Path.Combine(native, file)));
                    }

                    var otherLibs = Path.Combine(packageRoot, "lib\\aspnetcore50");
                    foreach (var file in Directory.EnumerateFiles(otherLibs))
                    {
                        dependencies.Add(new TaskItem(Path.Combine(native, file)));
                    }
                    continue;
                }

                var referencePath = Path.Combine(packageRoot, "lib\\contract", package.Id + ".dll");
                var dependencyPath = Path.Combine(packageRoot, "lib\\aspnetcore50", package.Id + ".dll");
       
                if (File.Exists(referencePath))
                {
                    references.Add(new TaskItem(referencePath));
                    Log.LogMessage(MessageImportance.Low, "Adding reference: {0}", referencePath);
                }
                else if (File.Exists(dependencyPath))
                {
                    references.Add(new TaskItem(dependencyPath));
                    Log.LogMessage(MessageImportance.Low, "Adding reference: {0}", dependencyPath);
                }
                else
                {
                    Log.LogWarning("Reference not found: {0}", referencePath);
                }

                if (!File.Exists(dependencyPath))
                {
                    Log.LogWarning("Dependency not found: {0}", dependencyPath);
                }
                else
                {
                    dependencies.Add(new TaskItem(dependencyPath));
                    Log.LogMessage(MessageImportance.Low, "Adding dependency: {0}", dependencyPath);
                }
            }

            References = references.ToArray();
            Dependencies = dependencies.ToArray();

            return true;
        }
	}
}
