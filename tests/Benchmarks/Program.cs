// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Http.Parser.Benchmarks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    public class Program
    {
        /// <summary>
        /// execute dotnet run -c Release and choose the benchmarks you want to run
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
            => BenchmarkSwitcher
                .FromAssemblyAndTypes(typeof(Program).Assembly, GetGenericBenchmarks())         
                .Run(args/*, CreateClrVsCoreConfig() uncomment it to run Clr vs .NET Core comparison*/);

        // BenchmarkDotNet can run generic benchmarks, if it knows what generic type arguments to use
        private static Type[] GetGenericBenchmarks()
        {
            var types = new List<Type>
            {
                typeof(HttpParser<Request>),
                typeof(HttpParser<RequestStruct>)
            };

            foreach (Type type in JsonBenchmarks.SerializerBenchmarks.GetTypes())
            {
                types.Add(type);
            }
            return types.ToArray();
        }

        private static IConfig CreateClrVsCoreConfig()
            => DefaultConfig.Instance
                .With(Job.ShortRun.With(Runtime.Clr).AsBaseline().WithId("CLR"))
                .With(Job.ShortRun.With(Runtime.Core).WithId("Core"));
    }
}
