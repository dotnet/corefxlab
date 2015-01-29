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
            dependencies.Add(new TaskItem(@"NotYetPackages\CoreCLR\coreclr.dll"));
            dependencies.Add(new TaskItem(@"NotYetPackages\CoreCLR\mscorlib.dll"));

            var installedPackages = PackageManager.ReadPackagesConfig(PackagesConfig);
            foreach (var package in installedPackages)
            {
                var packageRoot = Path.Combine(PackagesDirectory, package.FullName);
                var referencePath = Path.Combine(packageRoot, "lib\\contract", package.Id + ".dll");
                var dependencyPath = Path.Combine(packageRoot, "lib\\aspnetcore50", package.Id + ".dll");
       
                if (!File.Exists(referencePath))
                {
                    Log.LogWarning("Reference not found: {0}", referencePath);
                }
                else
                {
                    references.Add(new TaskItem(referencePath));
                    Log.LogMessage(MessageImportance.Low, "Adding reference: {0}", referencePath);
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
