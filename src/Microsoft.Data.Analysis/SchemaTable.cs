using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace Microsoft.Data.Analysis
{
    static class SchemaTable
    {
        internal static ReadOnlyCollection<DbColumn> GetSchema(IDataReader dataReader)
        {
            if (dataReader is DbDataReader ddr && ddr.CanGetColumnSchema())
            {
                return ddr.GetColumnSchema();
            }
            // this code path is needed to support running on versions prior to net5
            // in net5 (maybe earlier) the above CanGetColumnSchema() will always be
            // true, and implements this same logic internally.
            var schemaTable = dataReader.GetSchemaTable();
            var schema = new List<DbColumn>(schemaTable.Rows.Count);

            var columns = schemaTable.Columns;
            foreach (DataRow row in schemaTable.Rows)
            {
                schema.Add(new DataRowDbColumn(row, columns));
            }
            return new ReadOnlyCollection<DbColumn>(schema);
        }

        // this implementation is copied from System.Data.Common v5.0.0
        private sealed class DataRowDbColumn : DbColumn
        {
            private readonly DataColumnCollection _schemaColumns;
            private readonly DataRow _schemaRow;

            public DataRowDbColumn(DataRow readerSchemaRow, DataColumnCollection readerSchemaColumns)
            {
                _schemaRow = readerSchemaRow;
                _schemaColumns = readerSchemaColumns;

                base.ColumnName = GetDbColumnValue<string>(SchemaTableColumn.ColumnName);
                base.AllowDBNull = GetDbColumnValue<bool?>(SchemaTableColumn.AllowDBNull);
                base.DataType = GetDbColumnValue<Type>(SchemaTableColumn.DataType);
            }

            T GetDbColumnValue<T>(string columnName)
            {
                if (_schemaColumns.Contains(columnName))
                {
                    object obj = _schemaRow[columnName];
                    if (obj is T)
                    {
                        return (T)obj;
                    }
                }
                return default(T);
            }
        }
    }
}
