// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
                .FromAssembly(typeof(Program).Assembly)
                .Run(args/*, CreateClrVsCoreConfig() uncomment it to run Clr vs .NET Core comparison*/);

        private static IConfig CreateClrVsCoreConfig()
            => DefaultConfig.Instance
                .With(Job.ShortRun.With(Runtime.Clr).AsBaseline().WithId("CLR"))
                .With(Job.ShortRun.With(Runtime.Core).WithId("Core"));
    }
}
