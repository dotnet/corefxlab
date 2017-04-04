// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net.Http;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public abstract class HttpClientSampleBase<THandler> : ISample where THandler : HttpMessageHandler, new()
    {
        public async Task Run()
        {
            var client = new HttpClient(new THandler());

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
