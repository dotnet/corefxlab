using System;
using System.Net.Http.Buffered;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LowAllocationWebServer.Tests
{
    [TestClass]
    public class GivenAByteArrayWithHttpHeaders
    {
        private string _headersString = @"Host: localhost:8080
                                    Connection: keep-alive
                                    Cache-Control: max-age=0
                                    Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
                                    Upgrade-Insecure-Requests: 1
                                    User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.71 Safari/537.36
                                    Accept-Encoding: gzip, deflate, sdch
                                    Accept-Language: en-US,en;q=0.8,pt-BR;q=0.6,pt;q=0.4";

        private ByteSpan _headers;

        [TestInitialize]
        public void Setup()
        {                        
            var bytes = new UTF8Encoding().GetBytes(_headersString);            

            unsafe
            {
                fixed (byte* buffer = bytes)
                {
                    _headers = new ByteSpan(buffer, bytes.Length);
                }
            }
        }

        [TestMethod]
        public void It_counts_the_number_of_headers_correctly()
        {
            var httpHeaders = new HttpHeaders(_headers);

            httpHeaders.Count.Should().Be(8);
        }
    }
}
