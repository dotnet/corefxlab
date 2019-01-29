// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Linq;

namespace System.Text.Json.Serialization.Policies
{
    internal static class ArrayConverter
    {
        public static IEnumerable CreateFromList(Type elementType, IList sourceList)
        {
            Array array;
            Array probe;

            if (sourceList.Count > 0 && (probe = sourceList[0] as Array) != null)
            {
                array = Array.CreateInstance(probe.GetType(), sourceList.Count);

                int i = 0;
                foreach (Array childArray in sourceList.OfType<Array>())
                {
                    array.SetValue(childArray, i++);
                }
            }
            else
            {
                array = Array.CreateInstance(elementType, sourceList.Count);
                sourceList.CopyTo(array, 0);
            }

            return array;
        }
    }
}
