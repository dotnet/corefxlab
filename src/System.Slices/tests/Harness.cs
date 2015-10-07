// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

class Program
{
    public static void Main()
    {
        Console.WriteLine("TESTING...");
        Console.WriteLine("==========");
        var sw = Stopwatch.StartNew();
        var t = new Tester();
        if (t.RunTests(new Tests())) {
            Console.WriteLine("Success! ({0})", sw.Elapsed);
        }
        else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} Failures ({1})", t.Failures, sw.Elapsed);
            Console.ResetColor();
        }
    }
}

class Tester
{
    int m_failures;

    public int Failures
    {
        get { return m_failures; }
    }

    public bool Success
    {
        get { return (m_failures == 0); }
    }

    public void Assert(bool condition, string msg = "")
    {
        if (!condition) {
            m_failures++;
            Console.WriteLine("    # assertion failed [{0}]", msg);
        }
    }

    public void AssertEqual<T>(T x, T y)
        where T : IEquatable<T>
    {
        Assert(x.Equals(y), String.Format("{0} != {1}", x, y));
    }

    public bool RunTests(object tests)
    {
        // Run all methods that match the pattern 'bool Test*(Tester t)'
        var sw = new Stopwatch();
        var testMethods =
            tests.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (var testMethod in testMethods) {
            if (testMethod.Name.StartsWith("Test") &&
                testMethod.ReturnType == typeof(bool) &&
                testMethod.GetParameters().Length == 1 &&
                testMethod.GetParameters()[0].ParameterType == typeof(Tester)) {

                Console.WriteLine("Run {0}...", testMethod.Name);

                JitWarmUp(testMethod, tests);
                CleanUpMemory();

                sw.Start();
                int failures = m_failures;
                bool success = (bool)testMethod.Invoke(tests, new object[] { this });
                sw.Stop();
                if (success && m_failures == failures) {
                    Console.WriteLine("    - success ({0})", sw.Elapsed);
                }
                else {
                    m_failures++;
                    Console.WriteLine("    - failure ({0})", sw.Elapsed);
                }
                sw.Reset();
            }
        }
        return Success;
    }

    public void CleanUpMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    void JitWarmUp(MethodInfo testMethod, object tests)
    {
        SilentExecution(() => testMethod.Invoke(tests, new object[] { this }));
    }

    void SilentExecution(Action command)
    {
        var output = Console.Out;
        try {
            Console.SetOut(TextWriter.Null);
            command.Invoke();
        }
        finally {
            Console.SetOut(output);
        }
    }
}
