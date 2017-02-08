// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.IO.Pipelines.Samples.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace System.IO.Pipelines.Samples
{
    public class AspNetHttpServerSample
    {
        private static readonly UTF8Encoding _utf8Encoding = new UTF8Encoding(false);
        private static readonly byte[] _helloWorldPayload = Encoding.UTF8.GetBytes("Hello, World!");

        public static void Run()
        {
            using (var httpServer = new HttpServer())
            {
                var host = new WebHostBuilder()
                                    .UseUrls("http://*:5000")
                                    .UseServer(httpServer)
                                    // .UseKestrel()
                                    .Configure(app =>
                                    {
                                        app.Run(context =>
                                        {
                                            context.Response.StatusCode = 200;
                                            context.Response.ContentType = "text/plain";
                                            // HACK: Setting the Content-Length header manually avoids the cost of serializing the int to a string.
                                            //       This is instead of: httpContext.Response.ContentLength = _helloWorldPayload.Length;
                                            context.Response.Headers["Content-Length"] = "13";
                                            return context.Response.Body.WriteAsync(_helloWorldPayload, 0, _helloWorldPayload.Length);
                                        });
                                    })
                                    .Build();
                host.Run();
            }
        }
    }
}