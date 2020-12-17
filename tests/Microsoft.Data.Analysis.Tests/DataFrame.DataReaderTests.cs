using System;
using Xunit;

namespace Microsoft.Data.Analysis.Tests
{
    public class DataFrameDataReaderTests
    {
        [Fact]
        public void TestDataReader()
        {
            //TODO: ArrowString throws. No idea what an ArrowString is.
            //var df = DataFrameTests.MakeDataFrameWithAllColumnTypes(100, true);
            var df = DataFrameTests.MakeDataFrameWithAllMutableColumnTypes(100, true);
            var reader = df.GetDataReader();
            var schema = reader.GetColumnSchema();

            int rowCount = 0;
            int colCount = reader.FieldCount;
            long intVal;
            while (reader.Read())
            {                
                for(int i = 0; i < colCount; i++)
                {
                    if (reader.IsDBNull(i))
                        continue;
                    var type = reader.GetFieldType(i);

                    switch (Type.GetTypeCode(type)) {
                        case TypeCode.Boolean:
                            reader.GetBoolean(i);
                            break;
                        case TypeCode.Int32:
                            intVal = reader.GetInt32(i);
                            Assert.Equal(rowCount, intVal);
                            break;
                        case TypeCode.String:
                            var str = reader.GetString(i);
                            break;
                    }
                }
                rowCount++;
            }

            Assert.Equal(100, rowCount);
        }
    }
}
