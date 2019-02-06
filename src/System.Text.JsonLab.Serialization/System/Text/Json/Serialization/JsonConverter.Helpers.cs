// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        private static void GetPreviousState<T>(ref List<T> previous, ref T state, int index) where T:new()
        {
            if (previous == null)
            {
                previous = new List<T>();
            }

            if (index >= previous.Count)
            {
                Debug.Assert(index == previous.Count);
                state = new T();
                previous.Add(state);
            }
            else
            {
                state = previous[index];
            }
        }

        private static void SetPreviousState<T>(ref List<T> previous, in T state, int index)
        {
            if (previous == null)
            {
                previous = new List<T>();
            }

            if (index >= previous.Count)
            {
                Debug.Assert(index == previous.Count);
                previous.Add(state);
            }
            else
            {
                previous[index] = state;
            }
        }
    }
}
