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
    public class TimeOfDaySerializationTests
    {
        private readonly ITestOutputHelper _output;

        public TimeOfDaySerializationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CanSerializeTimeOfDayWithDataContractSerializer()
        {
            var obj = new TestObject { TestTime = new TimeOfDay(13, 24, 56, 789) };
            var serializer = new DataContractSerializer(typeof(TestObject));

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, obj);
            }

            var xml = sb.ToString();
            _output.WriteLine(xml);

            Assert.Contains("<TestTime>13:24:56.789</TestTime>", xml);
        }

        [Fact]
        public void CanDeserializeTimeOfDayWithDataContractSerializer()
        {
            const string xml = "<TestObject xmlns=\"http://schemas.datacontract.org/2004/07/System.Time.Tests\"><TestTime>13:24:56.789</TestTime></TestObject>";
            var serializer = new DataContractSerializer(typeof (TestObject));

            TestObject obj;
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                obj = (TestObject)serializer.ReadObject(reader);
            }

            Assert.NotNull(obj);
            Assert.Equal(new TimeOfDay(13, 24, 56, 789), obj.TestTime);
        }

        [Fact]
        public void CanSerializeTimeOfDayWithXmlSerializer()
        {
            var obj = new TestObject { TestTime = new TimeOfDay(13, 24, 56, 789) };
            var serializer = new XmlSerializer(typeof(TestObject));

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, obj);
            }

            var xml = sb.ToString();
            _output.WriteLine(xml);

            Assert.Contains("<TestTime>13:24:56.789</TestTime>", xml);
        }

        [Fact]
        public void CanDeserializeTimeOfDayWithDataXmlSerializer()
        {
            const string xml = "<TestObject><TestTime>13:24:56.789</TestTime></TestObject>";
            var serializer = new XmlSerializer(typeof(TestObject));

            TestObject obj;
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                obj = (TestObject)serializer.Deserialize(reader);
            }

            Assert.NotNull(obj);
            Assert.Equal(new TimeOfDay(13, 24, 56, 789), obj.TestTime);
        }

        /*      TODO: Can't find XsdDataContractExporter in netcore.  Is there another way to validate the schema type?
        [Fact]
        public void CanExportTimeOfDaySchema()
        {
            var exporter = new XsdDataContractExporter();
            exporter.Export(typeof(TimeOfDay));
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

            Assert.Contains("<xsd:element name=\"time\" nillable=\"true\" type=\"xsd:time\" />", schemas);
        }
        */

        [DataContract(Name = "TestObject")]
        public class TestObject
        {
            [DataMember]
            public TimeOfDay TestTime { get; set; }
        }
    }
}
