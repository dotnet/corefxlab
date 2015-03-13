using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.NuGet
{
    public class ResolvePackages : Task
	{
		[Required]
		public string PackagesDirectory { get; set; }

		[Required]
		public string PackagesConfig { get; set; }

		public ITaskItem[] Environment { get; private set; }

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

                // add contracts
                bool thereWereNoContracts = true;
                var contractsRoot = Path.Combine(packageRoot, "ref\\any");
				if (Directory.Exists(contractsRoot))
				{
					foreach (var file in Directory.EnumerateFiles(contractsRoot))
					{
                        if (file.EndsWith("_._"))
                        {
                            thereWereNoContracts = false;
                            break;
                        }
                        else
                        {
                            references.Add(new TaskItem(Path.Combine(contractsRoot, file)));
                            thereWereNoContracts = false;
                        }
					}
				}
                
                // add portable assemblies
				var portableRoot = Path.Combine(packageRoot, @"lib\any");
				if (Directory.Exists(portableRoot))
				{
					foreach (var file in Directory.EnumerateFiles(portableRoot))
					{
						dependencies.Add(new TaskItem(Path.Combine(portableRoot, file)));

						if (thereWereNoContracts)
						{
							references.Add(new TaskItem(Path.Combine(portableRoot, file)));
						}
					}
				}

                // add CoreFx specific
                var runtimeRoot = Path.Combine(packageRoot, @"lib\aspnetcore50");
                if (Directory.Exists(runtimeRoot))
                {
                    foreach (var file in Directory.EnumerateFiles(runtimeRoot))
                    {
                        dependencies.Add(new TaskItem(Path.Combine(runtimeRoot, file)));
                    }
                }

                // add environment specific assemblies (these are specified in the csproj file
                foreach (var environment in Environment)
				{
					var environmentDependencies = Path.Combine(packageRoot, environment.ItemSpec);
					if (Directory.Exists(environmentDependencies))
					{
						Log.LogMessage("Environment dependencies included: {0}", environmentDependencies);
						foreach (var file in Directory.EnumerateFiles(environmentDependencies))
						{
							dependencies.Add(new TaskItem(Path.Combine(environmentDependencies, file)));
						}
					}
				}
            }

            References = references.ToArray();
            Dependencies = dependencies.ToArray();

            return true;
        }
	}
}
