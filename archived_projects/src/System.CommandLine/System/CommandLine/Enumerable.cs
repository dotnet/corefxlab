// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.CommandLine
{
    // Low level replacement of LINQ Enumerable class. In particular, this implementation
    // doesn't use generic virtual methods.
    // System.CommandLine needs to be usable for small .NET Native apps that don't carry the
    // overhead of expensive runtime features such as the generic virtual methods.
    internal static class Enumerable
    {
        public static IEnumerable<T> Empty<T>()
        {
            return Linq.Enumerable.Empty<T>();
        }

        public static IEnumerable<U> Select<T, U>(this IEnumerable<T> values, Func<T, U> func)
        {
            Debug.Assert(values != null);

            foreach (T value in values)
            {
                yield return func(value);
            }
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> values, Func<T, bool> func)
        {
            Debug.Assert(values != null);

            foreach (T value in values)
            {
                if (func(value))
                    yield return value;
            }
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return Linq.Enumerable.Concat(first, second);
        }

        public static bool All<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return Linq.Enumerable.All(source, predicate);
        }

        public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return Linq.Enumerable.Any(source, predicate);
        }

        public static bool Any<T>(this IEnumerable<T> source)
        {
            return Linq.Enumerable.Any(source);
        }

        public static T[] ToArray<T>(this IEnumerable<T> source)
        {
            return Linq.Enumerable.ToArray(source);
        }

        public static T Last<T>(this IEnumerable<T> source)
        {
            return Linq.Enumerable.Last(source);
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> source)
        {
            return Linq.Enumerable.FirstOrDefault(source);
        }

        public static T First<T>(this IEnumerable<T> source)
        {
            return Linq.Enumerable.First(source);
        }

        public static int Max(this IEnumerable<int> source)
        {
            return Linq.Enumerable.Max(source);
        }
    }
}
