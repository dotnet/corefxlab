using System;
using System.Collections.Generic;
using System.Data;

namespace Microsoft.Data.Analysis
{
    partial class DataFrame
    {
        public static DataFrame Load(IDataReader dataReader, int numberOfRowsToRead = -1, bool addIndexColumn = false)
        {
            var schema = SchemaTable.GetSchema(dataReader);

            var columns = new List<DataFrameColumn>(schema.Count);
            for (int i = 0; i < schema.Count; ++i)
            {
                var col = schema[i];
                columns.Add(CreateColumn(col.DataType, col.ColumnName, i));
            }

            DataFrame dataFrame = new DataFrame(columns);

            object[] rowData = new object[schema.Count];

            long rowNumber = 0;
            while (dataReader.Read() && (numberOfRowsToRead == -1 || rowNumber < numberOfRowsToRead))
            {
                dataReader.GetValues(rowData);

                for (int i = 0; i < rowData.Length; i++)
                {
                    if (rowData[i] == DBNull.Value)
                    {
                        rowData[i] = null;
                    }
                }
                dataFrame.Append(rowData, true);
                ++rowNumber;
            }

            if (addIndexColumn)
            {
                var indexColumn = new PrimitiveDataFrameColumn<int>("IndexColumn", columns[0].Length);
                for (int i = 0; i < columns[0].Length; i++)
                {
                    indexColumn[i] = i;
                }
                columns.Insert(0, indexColumn);
            }
            return dataFrame;
        }
    }
}
