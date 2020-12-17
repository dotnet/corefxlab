using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Microsoft.Data.Analysis
{
    partial class DataFrame
    {
        /// <summary>
        /// Gets a <see cref="DbDataReader"/> that reads the contents of the DataFrame.
        /// </summary>
        /// <returns>A <see cref="DbDataReader"/>.</returns>
        public DataReader GetDataReader()
        {
            return new DataFrameDataReader(this);
        }

        // Why this interstitial type, and not just make DataFrameDataReader public?
        // Because this makes it very clear what the new public surface area is,
        // and feels cleaner than a bunch of <inheritdocs/>.

        /// <summary>
        /// A DbDataReader that accesses the contents of a DataFrame. 
        /// </summary>
        public abstract class DataReader : DbDataReader, IDbColumnSchemaGenerator
        {
            /// <summary>
            /// Gets the current 0-based row number.
            /// </summary>
            public abstract int Row { get; }

            /// <summary>
            /// Gets the column schema.
            /// </summary>
            public abstract ReadOnlyCollection<DbColumn> GetColumnSchema();
        }

        class DataFrameDataReader : DataReader
        {
            DataFrame dataFrame;

            // indicates the current row.
            // -1 at start, -2 at end.
            int row;

            class DataFrameDbColumn : DbColumn
            {
                public DataFrameDbColumn(DataFrameColumn col, int ordinal)
                {
                    
                    this.ColumnOrdinal = ordinal;
                    this.ColumnName = col.Name;
                    this.DataType = col.DataType;
                    this.DataTypeName = this.DataType.Name;
                    this.AllowDBNull = col.NullCount == 0;
                    this.IsReadOnly = true;
                }
            }

            public DataFrameDataReader(DataFrame dataFrame)
            {
                this.dataFrame = dataFrame;
                row = -1;
            }

            public override object this[int ordinal] => GetValue(ordinal);

            public override object this[string name] => GetValue(GetOrdinal(name));

            public override int Depth => 0;

            public override int FieldCount => dataFrame.Columns.Count;

            public override bool HasRows => dataFrame.Rows.Count > 0;

            public override bool IsClosed => dataFrame != null;

            public override int RecordsAffected => 0;

            public override bool GetBoolean(int ordinal)
            {
                return GetPrimitiveValue<bool>(ordinal);
            }

            public override byte GetByte(int ordinal)
            {
                return GetPrimitiveValue<byte>(ordinal);
            }

            public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
            {
                throw new NotSupportedException();
            }

            public override char GetChar(int ordinal)
            {
                return GetPrimitiveValue<char>(ordinal);
            }

            public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
            {
                // TODO: should this be implemented for string columns?
                throw new NotSupportedException();
            }

            public override string GetDataTypeName(int ordinal)
            {
                return GetFieldType(ordinal).Name;
            }

            public override DateTime GetDateTime(int ordinal)
            {
                // TODO: does DataFrame support dates? I don't see any date column.
                throw new NotSupportedException();
            }

            public override decimal GetDecimal(int ordinal)
            {
                return GetPrimitiveValue<decimal>(ordinal);
            }

            public override double GetDouble(int ordinal)
            {
                return GetPrimitiveValue<double>(ordinal);
            }

            public override IEnumerator GetEnumerator()
            {
                return new DbEnumerator(this);
            }

            public override Type GetFieldType(int ordinal)
            {
                return dataFrame.Columns[ordinal].DataType;
            }

            public override float GetFloat(int ordinal)
            {
                return GetPrimitiveValue<float>(ordinal);
            }

            public override Guid GetGuid(int ordinal)
            {
                throw new NotSupportedException();
            }

            public override short GetInt16(int ordinal)
            {
                return GetPrimitiveValue<short>(ordinal);
            }

            public override int GetInt32(int ordinal)
            {
                return GetPrimitiveValue<int>(ordinal);
            }

            public override long GetInt64(int ordinal)
            {
                return GetPrimitiveValue<long>(ordinal);
            }

            public override string GetName(int ordinal)
            {
                return dataFrame.Columns[ordinal].Name;
            }

            public override int GetOrdinal(string name)
            {
                for (int i = 0; i < dataFrame.Columns.Count; i++)
                {
                    if (dataFrame.Columns[i].Name == name)
                        return i;
                }
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            public override string GetString(int ordinal)
            {
                return (string)GetValue(ordinal);
            }

            public override object GetValue(int ordinal)
            {
                var type = this.GetFieldType(ordinal);

                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        return this.GetBoolean(ordinal);
                    case TypeCode.Char:
                        return this.GetChar(ordinal);
                    case TypeCode.Byte:
                        return this.GetByte(ordinal);
                    case TypeCode.Int16:
                        return this.GetInt16(ordinal);
                    case TypeCode.Int32:
                        return this.GetInt32(ordinal);
                    case TypeCode.Int64:
                        return this.GetInt64(ordinal);
                    case TypeCode.Single:
                        return this.GetFloat(ordinal);
                    case TypeCode.Double:
                        return this.GetDouble(ordinal);
                    case TypeCode.Decimal:
                        return this.GetDecimal(ordinal);
                    case TypeCode.String:
                        return this.GetString(ordinal);
                    case TypeCode.DateTime:
                    default:
                        throw new NotSupportedException();
                }
            }

            public override int GetValues(object[] values)
            {
                var c = Math.Min(values.Length, dataFrame.Columns.Count);
                for (int i = 0; i < c; i++)
                {
                    values[i] = GetValue(i);
                }
                return c;
            }

            // accessor to get primitive values without boxing
            T GetPrimitiveValue<T>(int ordinal) where T : unmanaged
            {
                return ((PrimitiveDataFrameColumn<T>)dataFrame.Columns[ordinal])[row].Value;
            }

            public override bool IsDBNull(int ordinal)
            {
                return false;
            }

            public override bool NextResult()
            {
                return false;
            }

            public override int Row
            {
                get
                {
                    if (row < 0)
                        throw new InvalidOperationException();
                    return row;
                }
            }

            public override bool Read()
            {
                if (row == -2)
                    return false;

                row++;

                if (row < dataFrame.Rows.Count)
                {
                    return true;
                }
                row = -2;
                return false;
            }


            public override ReadOnlyCollection<DbColumn> GetColumnSchema()
            {
                // don't bother caching. This should only be accessed once.
                return
                    new ReadOnlyCollection<DbColumn>(
                        dataFrame
                        .Columns
                        .Select(
                            (col, idx) => (DbColumn)new DataFrameDbColumn(col, idx)
                        ).ToList()
                    );
            }

            public override void Close()
            {
                this.dataFrame = null;
            }

            public override DataTable GetSchemaTable()
            {
                return SchemaTable.GetSchemaTable(GetColumnSchema());
            }
        }
    }
}
