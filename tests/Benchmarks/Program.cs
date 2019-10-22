// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Running;

namespace Benchmarks
{
    public class Program
    {
        /// <summary>
        /// execute dotnet run -c Release and choose the benchmarks you want to run
        /// or dotnet run -- --help to learn more about available command line arguments
        /// </summary>
        public static void Main(string[] args)
            => BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args);
    }
}
