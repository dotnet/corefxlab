// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Microsoft.Data
{
    public partial class DataFrame : IDataView
    {
        internal interface IPrimitiveGetter<T>
        {
            ValueGetter<T> GetValueGetter();
            void GetGetter(ref T value);
        }

        internal static class PrimitiveGetter<T>
        {
            public static IPrimitiveGetter<T> Instance { get; } = PrimitiveGetter.Getter<T>();
        }

        internal static class PrimitiveGetter
        {
            internal static DataFrame _dataFrame;
            internal static long _position;
            internal static int _columnIndex;
            public static IPrimitiveGetter<T> Getter<T>()
            {
                if (typeof(T) == typeof(bool))
                {
                    return (IPrimitiveGetter<T>)new BoolGetter();
                }
                else if (typeof(T) == typeof(byte))
                {
                    return (IPrimitiveGetter<T>)new ByteGetter();
                }
                else if (typeof(T) == typeof(char))
                {
                    return (IPrimitiveGetter<T>)new CharGetter();
                }
                else if (typeof(T) == typeof(decimal))
                {
                    return (IPrimitiveGetter<T>)new DecimalGetter();
                }
                else if (typeof(T) == typeof(double))
                {
                    return (IPrimitiveGetter<T>)new DoubleGetter();
                }
                else if (typeof(T) == typeof(float))
                {
                    return (IPrimitiveGetter<T>)new FloatGetter();
                }
                else if (typeof(T) == typeof(int))
                {
                    return (IPrimitiveGetter<T>)new IntGetter();
                }
                else if (typeof(T) == typeof(long))
                {
                    return (IPrimitiveGetter<T>)new LongGetter();
                }
                else if (typeof(T) == typeof(sbyte))
                {
                    return (IPrimitiveGetter<T>)new SByteGetter();
                }
                else if (typeof(T) == typeof(short))
                {
                    return (IPrimitiveGetter<T>)new ShortGetter();
                }
                else if (typeof(T) == typeof(uint))
                {
                    return (IPrimitiveGetter<T>)new UIntGetter();
                }
                else if (typeof(T) == typeof(ulong))
                {
                    return (IPrimitiveGetter<T>)new ULongGetter();
                }
                else if (typeof(T) == typeof(ushort))
                {
                    return (IPrimitiveGetter<T>)new UShortGetter();
                }
                throw new NotSupportedException();
            }
        }

        internal class DecimalGetter : IPrimitiveGetter<decimal>
        {
            public void GetGetter(ref decimal value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<decimal>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<decimal> GetValueGetter() => GetGetter;
        }

        internal class BoolGetter : IPrimitiveGetter<bool>
        {
            public void GetGetter(ref bool value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<bool>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<bool> GetValueGetter() => GetGetter;
        }

        internal class UShortGetter : IPrimitiveGetter<ushort>
        {
            public void GetGetter(ref ushort value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<ushort>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<ushort> GetValueGetter() => GetGetter;
        }

        internal class ULongGetter : IPrimitiveGetter<ulong>
        {
            public void GetGetter(ref ulong value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<ulong>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<ulong> GetValueGetter() => GetGetter;
        }

        internal class IntGetter : IPrimitiveGetter<int>
        {
            public void GetGetter(ref int value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<int>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<int> GetValueGetter() => GetGetter;
        }

        internal class UIntGetter : IPrimitiveGetter<uint>
        {
            public void GetGetter(ref uint value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<uint>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<uint> GetValueGetter() => GetGetter;
        }

        internal class ShortGetter : IPrimitiveGetter<short>
        {
            public void GetGetter(ref short value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<short>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<short> GetValueGetter() => GetGetter;
        }

        internal class SByteGetter : IPrimitiveGetter<sbyte>
        {
            public void GetGetter(ref sbyte value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<sbyte>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<sbyte> GetValueGetter() => GetGetter;
        }

        internal class LongGetter : IPrimitiveGetter<long>
        {
            public void GetGetter(ref long value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<long>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<long> GetValueGetter() => GetGetter;
        }

        internal class FloatGetter : IPrimitiveGetter<float>
        {
            public void GetGetter(ref float value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<float>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<float> GetValueGetter() => GetGetter;
        }

        internal class DoubleGetter : IPrimitiveGetter<double>
        {
            public void GetGetter(ref double value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<double>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<double> GetValueGetter() => GetGetter;
        }


        internal class ByteGetter : IPrimitiveGetter<byte>
        {
            public void GetGetter(ref byte value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<byte>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<byte> GetValueGetter() => GetGetter;
        }

        internal class CharGetter : IPrimitiveGetter<char>
        {
            public void GetGetter(ref char value)
            {
                value = (PrimitiveGetter._dataFrame.Column(PrimitiveGetter._columnIndex) as PrimitiveColumn<char>)[PrimitiveGetter._position] ?? default;
            }

            public ValueGetter<char> GetValueGetter() => GetGetter;
        }

        public class RowCursor : DataViewRowCursor
        {
            private long _position = -1;
            private static long _batch = -1;
            private long _rowBatch = ++_batch;
            Func<int, bool> _isColumnActive;
            private DataFrame _dataFrame;

            public RowCursor(DataFrame dataFrame, long startRowIndex, Func<int, bool> isColumnActive)
            {
                _dataFrame = dataFrame;
                _isColumnActive = isColumnActive;
            }

            public override long Position => _position;

            public override long Batch => _rowBatch;

            public override DataViewSchema Schema => _dataFrame.Schema;

            public override ValueGetter<TValue> GetGetter<TValue>(DataViewSchema.Column column)
            {
                if (!IsColumnActive(column))
                    throw new ArgumentOutOfRangeException(nameof(column));
                void valueGetterImplementation(ref TValue value)
                {
                    if (value is ReadOnlyMemory<char> chars)
                    {
                        // String is represented as a ReadOnlyMemory<char> in ML.NET
                        value = (TValue)(object)_dataFrame.Column(column.Index).ToString().AsMemory();
                    }
                    else
                    {
                        value = (TValue)_dataFrame.Column(column.Index)[_position];
                    }
                }
                ValueGetter<TValue> ret;
                try
                {
                    PrimitiveGetter._dataFrame = _dataFrame;
                    PrimitiveGetter._position = _position;
                    PrimitiveGetter._columnIndex = column.Index;
                    ret = PrimitiveGetter<TValue>.Instance.GetValueGetter();
                }
                catch (NotSupportedException)
                {
                    // Not a primitive column
                    ret = valueGetterImplementation;
                }
                return ret;
            }

            public override ValueGetter<DataViewRowId> GetIdGetter()
            {
                // Not totally sure about this. Is that what this function is supposed to do?
                void IdGetterImplementation(ref DataViewRowId id)
                {
                    id = new DataViewRowId((ulong)_position, 0);
                }
                return IdGetterImplementation;
            }

            public override bool IsColumnActive(DataViewSchema.Column column)
            {
                return _isColumnActive(column.Index);
            }

            public override bool MoveNext()
            {
                _position++;
                return _position < _dataFrame.RowCount;
            }
        }
        public bool CanShuffle => true;

        private DataViewSchema _schema;
        public DataViewSchema Schema
        {
            get
            {
                if (_schema != null && _schema.Count == ColumnCount)
                {
                    return _schema;
                }
                var schemaBuilder = new DataViewSchema.Builder();
                for (int i = 0; i < ColumnCount; i++)
                {
                    BaseColumn baseColumn = Column(i);
                    switch (baseColumn.DataType)
                    {
                        case Type boolType when boolType == typeof(bool):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.BooleanDataViewType.Instance);
                            break;
                        case Type byteType when byteType == typeof(byte):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.Byte);
                            break;
                        case Type doubleType when doubleType == typeof(double):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.Double);
                            break;
                        case Type floatType when floatType == typeof(float):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.Single);
                            break;
                        case Type intType when intType == typeof(int):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.Int32);
                            break;
                        case Type longType when longType == typeof(long):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.Int64);
                            break;
                        case Type sbyteType when sbyteType == typeof(sbyte):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.SByte);
                            break;
                        case Type shortType when shortType == typeof(short):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.Int16);
                            break;
                        case Type uintType when uintType == typeof(uint):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.UInt32);
                            break;
                        case Type ulongType when ulongType == typeof(ulong):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.UInt64);
                            break;
                        case Type ushortType when ushortType == typeof(ushort):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.NumberDataViewType.UInt16);
                            break;
                        case Type stringType when stringType == typeof(string):
                            schemaBuilder.AddColumn(baseColumn.Name, ML.Data.TextDataViewType.Instance);
                            break;
                        case Type charType when charType == typeof(char):
                        case Type decimalType when decimalType == typeof(decimal):
                        default:
                            throw new NotSupportedException();
                    }
                }
                _schema = schemaBuilder.ToSchema();
                return _schema;
            }
            set
            {
                _schema = value;
            }
        }

        public long? GetRowCount() => RowCount;

        public DataViewRowCursor GetRowCursor(IEnumerable<DataViewSchema.Column> columnsNeeded, Random rand = null)
        {
            HashSet<int> neededColumns = new HashSet<int>();
            foreach (var col in columnsNeeded)
            {
                neededColumns.Add(col.Index);
            }
            bool needCol(int colIndex) => neededColumns.Contains(colIndex);
            return new RowCursor(this, 0, needCol);
        }

        public DataViewRowCursor[] GetRowCursorSet(IEnumerable<DataViewSchema.Column> columnsNeeded, int n, Random rand = null)
        {
            // TODO: Simple change to support parallel cursors
            return new DataViewRowCursor[] { GetRowCursor(columnsNeeded, rand) };
        }
    }
}
