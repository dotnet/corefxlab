using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace System.Reflection.Metadata.Cil.Tests
{
    public class CilTests
    {
        private static string GetFileNameFromResourceName(string s)
        {
            // A.B.C.D.filename.extension
            string[] parts = s.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                return null;
            }

            // filename.extension
            return parts[parts.Length - 2] + "." + parts[parts.Length - 1];
        }

        [Fact]
        public void TestMethod1()
        {
            var watch = new Stopwatch();

            var path = Path.Combine(AppContext.BaseDirectory, "Demo1.exe");
            if (!File.Exists(path))
            {                
                var thisAssembly = typeof(CilTests).GetTypeInfo().Assembly;
                var resourceName =
                    thisAssembly.GetManifestResourceNames().First(r => r.EndsWith("Demo1.exe"));

                var fileName = GetFileNameFromResourceName(resourceName);

                using (var fileStream = File.Create(Path.Combine(AppContext.BaseDirectory, fileName)))
                {
                    using (var resource = thisAssembly.GetManifestResourceStream(resourceName))
                    {
                        resource.CopyTo(fileStream);
                    }
                }
            }

            string outputDirectory = AppContext.BaseDirectory;
            if (!File.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var assembly = CilAssembly.Create(path);
            watch.Start();
            using (var file = new StreamWriter(new FileStream($"{outputDirectory}/foo.il", FileMode.Create)))
            {
                assembly.WriteTo(file);
                watch.Stop();
                file.WriteLine("//Time elapsed: " + watch.Elapsed);
                assembly.Dispose();
            }
        }
    }
}
