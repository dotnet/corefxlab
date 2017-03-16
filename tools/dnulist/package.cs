using System.Collections.Generic;

public class Package
{
	public string Name {get; set;}
	public Dictionary<string,string> Dependencies {get; set;}
	public Dictionary<string,object> Compile {get; set;}
	public Dictionary<string,object> Runtime {get; set;}
}
