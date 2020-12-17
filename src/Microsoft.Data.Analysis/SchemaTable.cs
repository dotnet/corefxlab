using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace Microsoft.Data.Analysis
{
    // supports DataFrame.GetDataReader implementation
    // this is needed specifically to support SqlBulkCopy
    static class SchemaTable
    {
        static DataColumn Add(DataTable table, string name, Type type)
        {
            var col = new DataColumn(name, type);
            table.Columns.Add(col);
            return col;
        }

        // Adapts a new-style IDbColumnSchemaGenerator into the old-style DataTable schema.
        internal static DataTable GetSchemaTable(ReadOnlyCollection<DbColumn> schema)
        {
            object dbNull = DBNull.Value;
            var s = typeof(string);
            var i = typeof(int);
            var h = typeof(short);
            var b = typeof(bool);

            var table = new DataTable("SchemaTable");
            var nameCol = Add(table, SchemaTableColumn.ColumnName, s);
            var ordinalCol = Add(table, SchemaTableColumn.ColumnOrdinal, i);
            var sizeCol = Add(table, SchemaTableColumn.ColumnSize, i);
            var precCol = Add(table, SchemaTableColumn.NumericPrecision, h);
            var scaleCol = Add(table, SchemaTableColumn.NumericScale, h);
            var typeCol = Add(table, SchemaTableColumn.DataType, typeof(Type));
            var allowNullCol = Add(table, SchemaTableColumn.AllowDBNull, b);

            var baseNameCol = Add(table, SchemaTableColumn.BaseColumnName, s);
            var baseSchemaCol = Add(table, SchemaTableColumn.BaseSchemaName, s);
            var baseTableCol = Add(table, SchemaTableColumn.BaseTableName, s);

            var isAliasedCol = Add(table, SchemaTableColumn.IsAliased, b);
            var isExpressionCol = Add(table, SchemaTableColumn.IsExpression, b);
            var isKeyCol = Add(table, SchemaTableColumn.IsKey, b);
            var isLongCol = Add(table, SchemaTableColumn.IsLong, b);
            var isUniqueCol = Add(table, SchemaTableColumn.IsUnique, b);

            var providerTypeCol = Add(table, SchemaTableColumn.ProviderType, i);
            var nvProviderTypeCol = Add(table, SchemaTableColumn.NonVersionedProviderType, i);

            foreach (var col in schema)
            {
                var row = table.NewRow();
                row[nameCol] = col.ColumnName ?? dbNull;
                row[ordinalCol] = col.ColumnOrdinal ?? dbNull;
                row[sizeCol] = col.ColumnSize ?? dbNull;
                row[precCol] = col.NumericPrecision ?? dbNull;
                row[scaleCol] = col.NumericScale ?? dbNull;

                row[typeCol] = col.DataType ?? dbNull;
                row[allowNullCol] = col.AllowDBNull ?? dbNull;
                row[baseNameCol] = col.BaseColumnName ?? dbNull;
                row[baseSchemaCol] = col.BaseSchemaName ?? dbNull;
                row[baseTableCol] = col.BaseTableName ?? dbNull;

                row[isAliasedCol] = col.IsAliased ?? dbNull;
                row[isExpressionCol] = col.IsExpression ?? dbNull;

                row[isKeyCol] = col.IsKey ?? dbNull;
                row[isLongCol] = col.IsLong ?? dbNull;
                row[isUniqueCol] = col.IsUnique ?? dbNull;

                var code = (int)Type.GetTypeCode(col.DataType);
                row[providerTypeCol] = code;
                row[nvProviderTypeCol] = code;

                table.Rows.Add(row);
            }
            return table;
        }
    }
}
