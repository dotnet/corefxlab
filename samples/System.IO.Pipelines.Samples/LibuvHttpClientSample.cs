using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class LibuvHttpClientSample
    {
        public static async Task Run()
        {
            var client = new HttpClient(new LibuvHttpClientHandler());

            while (true)
            {
                var response = await client.GetAsync("http://localhost:5000");

                Console.WriteLine(response);

                Console.WriteLine(await response.Content.ReadAsStringAsync());

                await Task.Delay(1000);
            }
        }
    }
}
