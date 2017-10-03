// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Validators;
using BenchmarkDotNet.Toolchains.CsProj;

namespace System.IO.Pipelines.Performance.Tests
{
    public class CoreConfig : ManualConfig
    {
        public CoreConfig()
        {
            Add(JitOptimizationsValidator.FailOnError);
            Add(StatisticColumn.OperationsPerSecond);
            Add(MemoryDiagnoser.Default);

            Add(Job.Default
                .With(BenchmarkDotNet.Environments.Runtime.Core)
                .With(CsProjCoreToolchain.NetCoreApp20)
                .WithRemoveOutliers(false)
                .With(new GcMode() { Server = true })
                .With(RunStrategy.Throughput)
                .WithLaunchCount(3)
                .WithWarmupCount(5)
                .WithTargetCount(10));
        }
    }
}
