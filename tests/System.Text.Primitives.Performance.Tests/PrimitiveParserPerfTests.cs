using System.Runtime.CompilerServices;

namespace System.Text.Primitives.Tests
{
    public partial class PrimitiveParserPerfTests
    {
        private const int LoadIterations = 30000;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoNotIgnore(uint value, int consumed)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DoNotIgnore(ulong value, int consumed)
        {
        }

        private static void PrintTestName(string testString, [CallerMemberName] string testName = "")
        {
            if (testString != null)
            {
                Console.WriteLine("{0} called with test string \"{1}\".", testName, testString);
            }
            else
            {
                Console.WriteLine("{0} called with no test string.", testName);
            }
        }
    }
}
