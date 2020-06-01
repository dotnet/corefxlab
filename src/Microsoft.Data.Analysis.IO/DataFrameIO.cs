// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Microsoft.Data.Analysis.IO
{
    public static class DataFrameIO
    {
        private static string GetColumnName(string[] columnNames, int columnIndex)
        {
            return columnNames == null ? "Column" + columnIndex.ToString() : columnNames[columnIndex];
        }

        private static DataFrameColumn CreateColumn(Type kind, string[] columnNames, int columnIndex)
        {
            DataFrameColumn ret;
            if (kind == typeof(bool))
            {
                ret = new BooleanDataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(int))
            {
                ret = new Int32DataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(float))
            {
                ret = new SingleDataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(string))
            {
                ret = new StringDataFrameColumn(GetColumnName(columnNames, columnIndex), 0);
            }
            else if (kind == typeof(long))
            {
                ret = new Int64DataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(decimal))
            {
                ret = new DecimalDataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(byte))
            {
                ret = new ByteDataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(char))
            {
                ret = new CharDataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(double))
            {
                ret = new DoubleDataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(sbyte))
            {
                ret = new SByteDataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(short))
            {
                ret = new Int16DataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(uint))
            {
                ret = new UInt32DataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(ulong))
            {
                ret = new UInt64DataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else if (kind == typeof(ushort))
            {
                ret = new UInt16DataFrameColumn(GetColumnName(columnNames, columnIndex));
            }
            else
            {
                throw new NotSupportedException(nameof(kind));
            }
            return ret;
        }

        private static Type MaxKind(Type a, Type b)
        {
            if (a == typeof(string) || b == typeof(string))
                return typeof(string);
            if (a == typeof(float) || b == typeof(float))
                return typeof(float);
            if (a == typeof(bool) || b == typeof(bool))
                return typeof(bool);
            return typeof(string);
        }

        private static Type DetermineType(bool first, Type suggested, Type previous)
        {
            return first ? suggested : MaxKind(suggested, previous);
        }

        private static Type GuessKind(int col, List<string[]> read)
        {
            Type res = typeof(string);
            int nbline = 0;
            foreach (var line in read)
            {
                if (col >= line.Length)
                    throw new FormatException(string.Format(Strings.LessColumnsThatExpected, nbline + 1));

                string val = line[col];

                if (string.Equals(val, "null", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                bool boolParse = bool.TryParse(val, out bool boolResult);
                if (boolParse)
                {
                    res = DetermineType(nbline == 0, typeof(bool), res);
                    ++nbline;
                    continue;
                }
                else
                {
                    if (string.IsNullOrEmpty(val))
                    {
                        res = DetermineType(nbline == 0, typeof(bool), res);
                        continue;
                    }
                }
                bool floatParse = float.TryParse(val, out float floatResult);
                if (floatParse)
                {
                    res = DetermineType(nbline == 0, typeof(float), res);
                    ++nbline;
                    continue;
                }
                else
                {
                    if (string.IsNullOrEmpty(val))
                    {
                        res = DetermineType(nbline == 0, typeof(float), res);
                        continue;
                    }
                }
                res = DetermineType(nbline == 0, typeof(string), res);
                ++nbline;
            }
            return res;
        }

        public static DataFrame LoadCsv(Func<Stream> csvStream,
                                Action<TextFieldParser> SetTextFieldParserProperties,
                                char separator = ',', bool header = true,
                                string[] columnNames = null, Type[] dataTypes = null,
                                long numberOfRowsToRead = -1, int guessRows = 10, bool addIndexColumn = false,
                                Encoding encoding = null)
        {
            Stream stream = csvStream();
            if (!stream.CanSeek)
            {
                throw new ArgumentException(Strings.NonSeekableStream, nameof(stream));
            }

            if (dataTypes == null && guessRows <= 0)
            {
                throw new ArgumentException(string.Format(Strings.ExpectedEitherGuessRowsOrDataTypes, nameof(guessRows), nameof(dataTypes)));
            }

            var linesForGuessType = new List<string[]>();
            long rowline = 0;
            int numberOfColumns = dataTypes?.Length ?? 0;

            if (header == true && numberOfRowsToRead != -1)
            {
                numberOfRowsToRead++;
            }

            List<DataFrameColumn> columns;
            // First pass: schema and number of rows.
            using (TextFieldParser parser = new TextFieldParser(stream, defaultEncoding: encoding ?? Encoding.UTF8))
            {
                parser.SetDelimiters(separator.ToString());
                SetTextFieldParserProperties(parser);
                while (!parser.EndOfData)
                {
                    if ((numberOfRowsToRead == -1) || rowline < numberOfRowsToRead)
                    {
                        if (linesForGuessType.Count < guessRows || (header && rowline == 0))
                        {
                            var spl = parser.ReadFields();
                            if (header && rowline == 0)
                            {
                                if (columnNames == null)
                                {
                                    columnNames = spl;
                                }
                            }
                            else
                            {
                                linesForGuessType.Add(spl);
                                numberOfColumns = Math.Max(numberOfColumns, spl.Length);
                            }
                        }
                    }
                    ++rowline;
                    if (rowline == guessRows || guessRows == 0)
                    {
                        break;
                    }
                }

                if (rowline == 0)
                {
                    throw new FormatException(Strings.EmptyFile);
                }

                columns = new List<DataFrameColumn>(numberOfColumns);
                // Guesses types or looks up dataTypes and adds columns.
                for (int i = 0; i < numberOfColumns; ++i)
                {
                    Type kind = dataTypes == null ? GuessKind(i, linesForGuessType) : dataTypes[i];
                    columns.Add(CreateColumn(kind, columnNames, i));
                }
            }

            using (TextFieldParser parser = new TextFieldParser(csvStream(), defaultEncoding: encoding ?? Encoding.UTF8))
            {
                parser.SetDelimiters(separator.ToString());
                SetTextFieldParserProperties(parser);
                DataFrame ret = new DataFrame(columns);
                // Fills values.
                rowline = 0;
                while (!parser.EndOfData && (numberOfRowsToRead == -1 || rowline < numberOfRowsToRead))
                {
                    var spl = parser.ReadFields();
                    if (header && rowline == 0)
                    {
                        // Skips.
                    }
                    else
                    {
                        ret.Append(spl, inPlace: true);
                    }
                    ++rowline;
                }

                if (addIndexColumn)
                {
                    PrimitiveDataFrameColumn<int> indexColumn = new PrimitiveDataFrameColumn<int>("IndexColumn", columns[0].Length);
                    for (int i = 0; i < columns[0].Length; i++)
                    {
                        indexColumn[i] = i;
                    }
                    columns.Insert(0, indexColumn);
                }
                return ret;
            }
        }
    }
}
