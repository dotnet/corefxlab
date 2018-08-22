// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Reflection.TypeLoading;
using System.Collections.Generic;

namespace System.Reflection
{
    public sealed partial class TypeLoader : IDisposable
    {
        private readonly struct LoadedAssemblyEntry
        {
            public LoadedAssemblyEntry(RoAssembly assembly, Guid mvid)
            {
                Debug.Assert(assembly != null);

                Mvid = mvid;
                Assembly = assembly;
            }

            public Guid Mvid { get; }
            public RoAssembly Assembly { get; }
        }
    }
}
