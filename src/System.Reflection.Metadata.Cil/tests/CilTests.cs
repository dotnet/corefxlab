using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace System.Reflection.Metadata.Cil.Tests
{
    [TestClass]
    public class CilTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                string path = "Assemblies/mscorlib.dll";
                if (!File.Exists(path))
                {
                    Assert.Fail("File not found");
                    return;
                }
                var assembly = CilAssembly.Create(path);
                watch.Start();
                using (StreamWriter file = new StreamWriter("../../Output/foo.il"))
                {
                    assembly.WriteTo(file);
                    watch.Stop();
                    file.WriteLine("//Time elapsed: " + watch.Elapsed);
                    assembly.Dispose();
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
                return;
            }
            Assert.IsTrue(true);
        }
    }
}
