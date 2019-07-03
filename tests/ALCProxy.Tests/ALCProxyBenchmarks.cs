using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ALCProxy.Tests
{
    public class ALCProxyBenchmarks
    {
        ALCProxyTests tester;
        public ALCProxyBenchmarks(){
            tester = new ALCProxyTests();
        }

        [Benchmark]
        public void TestBasicContextLoading() => tester.TestBasicContextLoading();



        //public static void Main(string[] args)
        //{
        //    var summary = BenchmarkRunner.Run<ALCProxyBenchmarks>();
        //}

    }
}
