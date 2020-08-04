// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Web;
using System.Web.Hosting;
using Xunit;

namespace System.Web.Compatibility.Tests
{
    public class SystemWebCompatTests
    {
        [Fact]
        public void HttpContextCurrentDoesNotThrow()
        {
            Assert.Null(HttpContext.Current);
        }

        /*
        [Fact]
        public void HttpContextItemsDoesNotThrow()
        {
            var context = new HttpContext(new HttpRequest(string.Empty, string.Empty, string.Empty), new HttpResponse());

            Assert.Null(context.Items);
        }
        */

        [Fact]
        public void HostingEnvironmentApplicationPathGetDoesNotThrow()
        {
            Assert.Null(HostingEnvironment.ApplicationPath);
        }

        [Fact]
        public void HostingEnvironmentIsHostedDoesNotThrow()
        {
            Assert.False(HostingEnvironment.IsHosted);
        }

        [Fact]
        public void HostingEnvironmentSiteNameDoesNotThrow()
        {
            Assert.Null(HostingEnvironment.SiteName);
        }

        [Fact]
        public void HostingEnvironmentMapPathDoesNotThrow()
        {
            Assert.Null(HostingEnvironment.MapPath(null));
        }

        [Fact]
        public void HttpUtilityStillWorks()
        {
            Type httpUtilityType = Type.GetType("System.Web.HttpUtility, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

            Assert.NotNull(httpUtilityType);
            Assert.Equal(httpUtilityType, typeof(HttpUtility));

            Assert.Equal("hello+world", HttpUtility.UrlEncode("hello world"));
        }

    }
}
