// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    interface IDebugEnumerable<T>
    {
        IEnumerator<T> GetEnumerator();
    }

    internal sealed class DebugEnumeratorDebugView<T>
    {
        public DebugEnumeratorDebugView(IDebugEnumerable<T> enumerable)
        {
            var list = new List<T>();
            foreach (T item in enumerable)
            {
                list.Add(item);
            }
            Items = list.ToArray();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items { get; }
    }
}
