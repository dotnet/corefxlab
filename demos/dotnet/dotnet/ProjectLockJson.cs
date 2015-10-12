// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace dotnet
{
    internal class ProjectLockJson
    {
        public Dictionary<string, Dictionary<string, Target>> Targets;
    }

    internal class Target
    {
        public string Type;
        public Dictionary<string, string> Dependencies;
        public Dictionary<string, CompileRef> Compile;
        public Dictionary<string, Runtime> Runtime;
        public Dictionary<string, Native> Native;
    }

    internal struct CompileRef
    {
    }

    internal struct Runtime
    {
    }

    internal struct Native
    {
    }
}
