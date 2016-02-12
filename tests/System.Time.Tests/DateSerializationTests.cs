// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace System.Time.Tests
{
    public class DateSerializationTests
    {
        private readonly ITestOutputHelper _output;

        public DateSerializationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanSerializeDateWithDataContractSerializer()
        {
            var obj = new TestObject { TestDate = new Date(2015, 12, 31) };
            var serializer = new DataContractSerializer(typeof(TestObject));

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, obj);
            }

            var xml = sb.ToString();
            _output.WriteLine(xml);

            Assert.Contains("<TestDate>2015-12-31</TestDate>", xml);
        }

        [Fact]
        public void CanDeserializeDateWithDataContractSerializer()
        {
            const string xml = "<TestObject xmlns=\"http://schemas.datacontract.org/2004/07/System.Time.Tests\"><TestDate>2015-12-31</TestDate></TestObject>";
            var serializer = new DataContractSerializer(typeof(TestObject));

            TestObject obj;
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                obj = (TestObject)serializer.ReadObject(reader);
            }

            Assert.NotNull(obj);
            Assert.Equal(new Date(2015, 12, 31), obj.TestDate);
        }

        [Fact]
        public void CanSerializeDateWithXmlSerializer()
        {
            var obj = new TestObject { TestDate = new Date(2015, 12, 31) };
            var serializer = new XmlSerializer(typeof(TestObject));

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, obj);
            }

            var xml = sb.ToString();
            _output.WriteLine(xml);

            Assert.Contains("<TestDate>2015-12-31</TestDate>", xml);
        }

        [Fact]
        public void CanDeserializeDateWithDataXmlSerializer()
        {
            const string xml = "<TestObject><TestDate>2015-12-31</TestDate></TestObject>";
            var serializer = new XmlSerializer(typeof(TestObject));

            TestObject obj;
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                obj = (TestObject)serializer.Deserialize(reader);
            }

            Assert.NotNull(obj);
            Assert.Equal(new Date(2015, 12, 31), obj.TestDate);
        }

        /*      TODO: Can't find XsdDataContractExporter in netcore.  Is there another way to validate the schema type?
        [Fact]
        public void CanExportDateSchema()
        {
            var exporter = new XsdDataContractExporter();
            exporter.Export(typeof(Date));
            var schemaSet = exporter.Schemas;

            var serializer = new XmlSerializer(typeof(XmlSchema));
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                foreach (var schema in schemaSet.Schemas())
                {
                    serializer.Serialize(writer, schema);
                }
            }

            var schemas = sb.ToString();
            _output.WriteLine(schemas);

            Assert.Contains("<xsd:element name=\"date\" nillable=\"true\" type=\"xsd:date\" />", schemas);
        }
        */

        [DataContract(Name = "TestObject")]
        public class TestObject
        {
            [DataMember]
            public Date TestDate { get; set; }
        }
    }
}
