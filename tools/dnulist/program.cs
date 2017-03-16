using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class Program
{
    private static void Main (string[] args)
    {
        // project.lock.json reader
        // See PrintHelp for arg info
        
        const string Lockfile = "project.lock.json";
        Project project;
        
        Console.WriteLine("project.json.lock file reader");
        
        using (var reader = new StreamReader(Lockfile))
        {
            project = JsonConvert.DeserializeObject<Project>(reader.ReadToEnd());
        }
        
        if (args == null || args.Length == 0)
        {
            PrintTargets(project);
            Console.WriteLine();
            Console.WriteLine("Use --help argument to print help.");
        }
        else
        {
            var commands = GetArguments(args);
            const string Target = "--target";
            const string Copy = "--copy";
            const string Verbose = "--verbose";
            const string Help = "--help";
            
            if (commands == null)
            {
                Console.WriteLine("Bad argument!");
                Console.WriteLine();
                PrintHelp();
            }
            else if (commands.ContainsKey("--targets"))
            {
                PrintTargets(project);
            }
            else if (commands.ContainsKey(Target) && commands.ContainsKey(Copy) && !string.IsNullOrEmpty(commands[Copy]))
            {
                bool verbose = commands.ContainsKey(Verbose);
                CopyTarget(project, commands[Target], commands[Copy], verbose);
            }
            else if (commands.ContainsKey(Target))
            {
                bool verbose = commands.ContainsKey(Verbose);
                PrintTarget(project,commands[Target], verbose);
            }
            else if (commands.ContainsKey(Help))
            {
                Console.WriteLine();
                PrintHelp();
            }
        }
    }
    
    private static void PrintTargets(Project project)
    {
        Console.WriteLine ("This project has {0} targets.", project.Targets.Count);
        Console.WriteLine();
        
        foreach (var t in project.Targets.Keys)
        {
            Console.WriteLine(t);
        }
    }
    
    private static void PrintTarget(Project project, string target, bool verbose = false)
    {
        Dictionary<string,Package> packages;
        var targetFound = project.Targets.TryGetValue(target, out packages);
        
        if (targetFound && packages.Count > 0)
        {
            Console.WriteLine("Package dependencies for target: {0}",target);
            
            foreach (var package in packages.Keys)
            {
                Console.WriteLine(package);
                if (verbose)
                {
                    var p = packages[package];
               
                    if (p.Compile != null)
                    {
                        foreach (var key in p.Compile.Keys)
                        {
                            Console.WriteLine("**Compile: {0}", key);
                        }
                    }
                    
                    if (p.Runtime != null)
                    {
                        foreach (var key in p.Runtime.Keys)
                        {
                            Console.WriteLine("**Runtime: {0}", key);
                        }
                    }
                    
                    if (p.Dependencies != null)
                    {
                        foreach (var key in p.Dependencies.Keys)
                        {
                            Console.WriteLine("**Dependencies: {0}", key);
                        }
                    }
                }
            }
        }
        else if (targetFound)
        {
            Console.WriteLine("No packages found for {0}.",target);
        }
        else
        {
            Console.WriteLine("{0} is not a target for this project.", target);
        }
    }    
    
    private static void CopyTarget(Project project, string target, string path, bool verbose = false)
    {
        const string packageRoot = "/Users/richlander/.dnx/packages";
        string compilePath = Path.Combine(path, "compile");
        string runtimePath = Path.Combine(path,"runtime");
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(compilePath);
            Directory.CreateDirectory(runtimePath);
        }
        else if (!Directory.Exists(compilePath))
        {
            Directory.CreateDirectory(compilePath);
        }
        else if (!Directory.Exists(runtimePath))
        {
            Directory.CreateDirectory(runtimePath);
        }
        
        Dictionary<string,Package> packages;
        var targetFound = project.Targets.TryGetValue(target, out packages);
            
        if (targetFound && packages.Count > 0)
        {
            foreach (var package in packages.Keys)
            {   
                Console.WriteLine("Copying package: {0}", package);
                
                var p = packages[package];         
                if (p.Compile != null)
                {
                    foreach (var lib in p.Compile.Keys)
                    {
                        var source = Path.Combine(packageRoot,package,lib);
                        var destination = Path.Combine(compilePath,Path.GetFileName(lib));;
                        File.Copy(source,destination, true);
                        
                        if (verbose)
                        {
                            Console.WriteLine("**Compile: {0}", lib);
                        }
                    }
                }
                
                if (p.Runtime != null)
                {
                    foreach (var lib in p.Runtime.Keys)
                    {
                        var source = Path.Combine(packageRoot,package,lib);
                        var destination = Path.Combine(runtimePath,Path.GetFileName(lib));;
                        File.Copy(source,destination,true);
                        
                        if (verbose)
                        {
                            Console.WriteLine("**Runtime: {0}", lib);
                        }
                    }
                }
            }
        }
        else if (targetFound)
        {
            Console.WriteLine("No packages found for {0}.",target);
        }
        else
        {
            Console.WriteLine("{0} is not a target for this project.", target);
        }
    }
    
    private static void PrintHelp()
    {
        Console.WriteLine("Primary arguments:");
        Console.WriteLine("--target - prints targets");
        Console.WriteLine("--target [target] - selects targets to print (the default) or otherwise act upon");
        Console.WriteLine("--help - prints help");
        Console.WriteLine();
        Console.WriteLine("Additional arguments:");
        Console.WriteLine("--copy [path to copy to] - copies reference and runtime assemblies for a target");
        Console.WriteLine("--verbose - print maximum amount of info");
    }
    
    private static Dictionary<string,string> GetArguments(string[] args)
    {
        const string ArgSyntax = "--";
        var commands = new Dictionary<string,string>();
        int count = 0;

        while (count < args.Length)
        {   
            if (count +1 == args.Length && args[count].StartsWith(ArgSyntax))
            {
                commands.Add(args[count],string.Empty);
            }
            else if (count +1 == args.Length && !args[count].StartsWith(ArgSyntax))
            {
                // bad argument
                return null;
            }
            else if (args[count].StartsWith(ArgSyntax) && !args[count+1].StartsWith(ArgSyntax))
            {
                commands.Add(args[count],args[count+1]);
                count++;
            }
            else if (args[count].StartsWith(ArgSyntax))
            {
                commands.Add(args[count],string.Empty);
            }
            count++;
        }  
        return commands;
    }
}
