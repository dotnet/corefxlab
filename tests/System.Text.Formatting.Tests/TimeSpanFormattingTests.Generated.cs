// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// THIS FILE IS AUTOGENERATED

using System.Globalization;
using System.Text;
using System.Buffers;
using System.Buffers.Text;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public partial class SystemTextFormattingTests
    {
        public void CheckTimeSpan(TimeSpan value, string format)
        {
            var parsed = StandardFormat.Parse(format);
            var formatter = new StringFormatter();
            formatter.Append(value, parsed);
            var result = formatter.ToString();
            var clrResult = value.ToString(format, CultureInfo.InvariantCulture);
            Assert.Equal(clrResult, result);
        }
        
        [Fact]
        public void TimeSpanFormatG()
        {
            
            // format G
            CheckTimeSpan(TimeSpan.MinValue, "G");
            CheckTimeSpan(TimeSpan.MaxValue, "G");
            CheckTimeSpan(new TimeSpan(-597541364), "G");
            CheckTimeSpan(new TimeSpan(1034657452), "G");
            CheckTimeSpan(new TimeSpan(-2128880930), "G");
            CheckTimeSpan(new TimeSpan(-1249396686), "G");
            CheckTimeSpan(new TimeSpan(568757918), "G");
            CheckTimeSpan(new TimeSpan(242760937), "G");
            CheckTimeSpan(new TimeSpan(1039627054), "G");
            CheckTimeSpan(new TimeSpan(-539625891), "G");
            CheckTimeSpan(new TimeSpan(-2081988522), "G");
            CheckTimeSpan(new TimeSpan(1439405730), "G");
            CheckTimeSpan(new TimeSpan(-1988877353), "G");
            CheckTimeSpan(new TimeSpan(1649323406), "G");
            CheckTimeSpan(new TimeSpan(1251608416), "G");
            CheckTimeSpan(new TimeSpan(910390888), "G");
            CheckTimeSpan(new TimeSpan(-578131831), "G");
            CheckTimeSpan(new TimeSpan(1091964721), "G");
            CheckTimeSpan(new TimeSpan(-697222132), "G");
            CheckTimeSpan(new TimeSpan(569627714), "G");
        }
        
        [Fact]
        public void TimeSpanFormatc()
        {
            
            // format c
            CheckTimeSpan(TimeSpan.MinValue, "c");
            CheckTimeSpan(TimeSpan.MaxValue, "c");
            CheckTimeSpan(new TimeSpan(-2015815770), "c");
            CheckTimeSpan(new TimeSpan(-1397727426), "c");
            CheckTimeSpan(new TimeSpan(1720336262), "c");
            CheckTimeSpan(new TimeSpan(-1188575091), "c");
            CheckTimeSpan(new TimeSpan(523061914), "c");
            CheckTimeSpan(new TimeSpan(-895692313), "c");
            CheckTimeSpan(new TimeSpan(-349859009), "c");
            CheckTimeSpan(new TimeSpan(2137082378), "c");
            CheckTimeSpan(new TimeSpan(-788622211), "c");
            CheckTimeSpan(new TimeSpan(1800438645), "c");
            CheckTimeSpan(new TimeSpan(-1259968094), "c");
            CheckTimeSpan(new TimeSpan(764542939), "c");
            CheckTimeSpan(new TimeSpan(78630430), "c");
            CheckTimeSpan(new TimeSpan(684818620), "c");
            CheckTimeSpan(new TimeSpan(-1759609814), "c");
            CheckTimeSpan(new TimeSpan(1137120042), "c");
            CheckTimeSpan(new TimeSpan(1175992869), "c");
            CheckTimeSpan(new TimeSpan(-18815684), "c");
        }
        
    }
}
