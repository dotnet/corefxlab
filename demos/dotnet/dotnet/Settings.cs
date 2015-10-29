using System;
using System.Collections.Generic;
using System.IO;

namespace dotnet
{
    internal class Settings
    {
        public Settings()
        {
            Target = "exe";
            Platform = "anycpu";
            Log = false;
            Unsafe = false;
            Optimize = false;
            SourceFiles = new List<string>();
        }

        private static readonly List<string> CommandFunctions = new List<string>
        {
            "/new",
            "/clean"
        };

        private static readonly List<string> HelpCommands = new List<string>
        {
            "/?",
            "/help"
        };

        private static readonly List<string> CommandSwitches = new List<string>
        {
            "/log",
            "/optimize",
            "/unsafe"
        };

        private static readonly List<string> CommandSwitchesWithSpecifications = new List<string>
        {
            "/target",
            "/recurse",
            "/debug",
            "/platform",
            "/reference"
        };

        private static readonly List<string> TargetSpecifications = new List<string>
        {
            "exe",
            "library"
        };

        private static readonly List<string> DebugSpecifications = new List<string>
        {
            "full",
            "pdbonly"
        };

        private static readonly List<string> PlatformSpecifications = new List<string>
        {
            "anycpu",
            "anycpu32bitpreferred",
            "x64",
            "x86"
        };

        public string Target;
        public string Platform;
        public string Recurse;
        public string Debug;

        public bool Log;
        public bool Unsafe;
        public bool Optimize;

        public string ProjectFile;

        public List<string> SourceFiles;
        public List<string> References;

        public bool SetTargetSpecification(string specification)
        {
            if (!TargetSpecifications.Contains(specification)) return false;
            Target = specification;
            return true;
        }

        public bool SetPlatformSpecification(string specification)
        {
            if (!PlatformSpecifications.Contains(specification)) return false;
            Platform = specification;
            return true;
        }

        public bool SetReferenceSpecification(string specification)
        {
            References = new List<string>(specification.Split(','));
            return true;
        }

        public bool SetDebugSpecification(string specification)
        {
            if (!DebugSpecifications.Contains(specification)) return false;
            Debug = specification;
            return true;
        }

        public bool SetRecurseSpecification(string specification)
        {
            Recurse = specification;
            return true;
        }

        public bool NeedHelp(string[] args)
        {
            return Array.Exists(args, element => HelpCommands.Contains(element));
        }

        public bool IsValid(string[] args)
        {
            foreach (var argument in args)
            {
                var argOptions = argument.Split(':');
                var compilerOption = argument.StartsWith("/") ? argOptions[0] : argument;
                if (!CommandFunctions.Contains(compilerOption) && !CommandSwitches.Contains(compilerOption) &&
                    !CommandSwitchesWithSpecifications.Contains(compilerOption) && !FilesExist(compilerOption))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool FilesExist(string file)
        {
            try
            {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), file);

                if (!File.Exists(file) && files.Length == 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
