using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Buffered;
using System.Text;
using System.Text.Utf8;
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
                                    Accept-Language: en-US,en;q=0.8,pt-BR;q=0.6,pt;q=0.4
";

        private ByteSpan _headers;
        private HttpHeaders _httpHeaders;

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

            _httpHeaders = new HttpHeaders(_headers);
        }

        [TestMethod]
        public void It_counts_the_number_of_headers_correctly()
        {            
            _httpHeaders.Count.Should().Be(8);
        }

        [TestMethod]
        public void It_can_get_the_value_of_a_particular_header()
        {
            _httpHeaders["Host"].ToString().Should().Be(" localhost:8080");
        }

        [TestMethod]
        public void Returns_empty_string_when_header_is_not_present()
        {
            ((IEnumerable)_httpHeaders["Content-Length"]).Should().BeEmpty();
        }
    }
}
