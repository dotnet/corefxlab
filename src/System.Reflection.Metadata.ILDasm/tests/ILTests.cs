using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using ILDasmLibrary;

namespace ILDasmLibraryTest
{
    [TestClass]
    public class ILTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                string path = "Assemblies/Demo1.exe";
                if (!File.Exists(path))
                {
                    Assert.Fail("File not found");
                    return;
                }
                var assembly = ILAssembly.Create(path);
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
