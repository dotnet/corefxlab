// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace dotnet
{
    public class ProjectLockJson
    {
        public Dictionary<string, Dictionary<string, Target>> Targets { get; set; }
    }

    public class Target
    {
        public string Type { get; set; }
        public Dictionary<string, string> Dependencies { get; set; }
        public Dictionary<string, CompileRef> Compile { get; set; }
        public Dictionary<string, Runtime> Runtime { get; set; }
        public Dictionary<string, Native> Native { get; set; }
    }

    public struct CompileRef
    {
    }

    public struct Runtime
    {
    }

    public struct Native
    {
    }
}
