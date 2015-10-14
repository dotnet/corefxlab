using System.Collections.Generic;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Json.Tests
{
    public class JsonReaderTests
    {
        [Fact]
        public void ReadJsonUtf8()
        {
            Console.WriteLine(Json);
            var str = new Utf8String(Json);
            var jsonReader = new JsonReader(str);
            var result = Read(jsonReader);
            Assert.Equal(ExpectedAge, result.Age);
            Assert.Equal(ExpectedBalance, result.Balance);
            Assert.Equal(_expectedFullName, new Utf8String(result.FirstName + " " + result.LastName));
            var phoneList = new List<Utf8String> {result.Phone1, result.Phone2};
            Assert.Equal(ExpectedPhone, phoneList.ToArray());
            Assert.Equal(_expectedAddress, new Utf8String(result.Street + ", " + result.City + ", " + result.Zip));
            Assert.Equal(ExpectedId1, result.Id1);
            Assert.Equal(ExpectedId2, result.Id2);
            Assert.Equal(ExpectedId3, result.Id3);
        }

        private const string Json =
            "{\"age\":30,\"balance\":-1000,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1 Microsoft Way\",\"city\":\"Redmond\",\"zip\":98052}\"IDs\" \n : [ \t 0425 \n , -70 \n, 9223372036854775807 ] \t }";

        private const uint ExpectedAge = 30;
        private const int ExpectedBalance = -1000;
        private const uint ExpectedId1 = 425;
        private const int ExpectedId2 = -70;
        private const long ExpectedId3 = 9223372036854775807;
        private readonly Utf8String _expectedFullName = new Utf8String("John Smith");

        private static readonly Utf8String[] ExpectedPhone =
        {
            new Utf8String("425-000-1212"),
            new Utf8String("425-000-1213")
        };

        private readonly Utf8String _expectedAddress = new Utf8String("1 Microsoft Way, Redmond, 98052");

        private static ReadObject Read(JsonReader json)
        {
            var output = new ReadObject();

            json.ReadObjectStart();
            json.ReadProperty();
            output.Age = json.ReadPropertyValueAsUInt();
            json.ReadProperty();
            output.Balance = json.ReadPropertyValueAsInt();
            json.ReadProperty();
            output.FirstName = json.ReadPropertyValueAsString();
            json.ReadProperty();
            output.LastName = json.ReadPropertyValueAsString();
            json.ReadMember();
            json.ReadArrayStart();
            output.Phone1 = json.ReadMemberValueAsString();
            output.Phone2 = json.ReadMemberValueAsString();
            json.ReadArrayEnd();
            json.ReadMember();
            json.ReadObjectStart();
            json.ReadProperty();
            output.Street = json.ReadPropertyValueAsString();
            json.ReadProperty();
            output.City = json.ReadPropertyValueAsString();
            json.ReadProperty();
            output.Zip = json.ReadPropertyValueAsUInt();
            json.ReadObjectEnd();
            json.ReadArrayStart();
            output.Id1 = json.ReadMemberValueAsUInt();
            output.Id2 = json.ReadMemberValueAsInt();
            output.Id3 = json.ReadPropertyValueAsLongInt();
            json.ReadArrayEnd();
            json.ReadObjectEnd();
            return output;
        }

        public class ReadObject
        {
            public uint Age { get; set; }
            public int Balance { get; set; }
            public Utf8String FirstName { get; set; }
            public Utf8String LastName { get; set; }
            public Utf8String Phone1 { get; set; }
            public Utf8String Phone2 { get; set; }
            public Utf8String Street { get; set; }
            public Utf8String City { get; set; }
            public uint Zip { get; set; }
            public uint Id1 { get; set; }
            public int Id2 { get; set; }
            public long Id3 { get; set; }
        }
    }
}
