using System.Collections.Generic;

public class Project
{
	public bool Locked {get; set;}
	public string Version {get; set;}
	public Dictionary<string,Dictionary<string,Package>> Targets {get; set;}
	public Dictionary<string,PackageFile> Libraries {get; set;}
	public Dictionary<string,List<string>> ProjectFileDependencyGroups {get; set;}
}
