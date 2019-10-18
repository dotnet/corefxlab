// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.Data
{

    public partial class DataFrame
    {
        private static Type GuessKind(int col, List<string[]> read)
        {
            Type res = typeof(string);
            int nbline = 0;
            foreach (var line in read)
            {
                if (col >= line.Length)
                    throw new FormatException(string.Format(Strings.LessColumnsThatExpected, nbline + 1));

                string val = line[col];
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

        private static Type DetermineType(bool first, Type suggested, Type previous)
        {
            if (first)
                return suggested;
            else
                return MaxKind(suggested, previous);
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

        /// <summary>
        /// Reads a text file as a DataFrame.
        /// Follows pandas API.
        /// </summary>
        /// <param name="filename">filename</param>
        /// <param name="separator">column separator</param>
        /// <param name="header">has a header or not</param>
        /// <param name="columnNames">column names (can be empty)</param>
        /// <param name="dataTypes">column types (can be empty)</param>
        /// <param name="numRows">number of rows to read</param>
        /// <param name="guessRows">number of rows used to guess types</param>
        /// <param name="addIndexColumn">add one column with the row index</param>
        /// <returns>DataFrame</returns>
        public static DataFrame ReadCsv(string filename,
                                char separator = ',', bool header = true,
                                string[] columnNames = null, Type[] dataTypes = null,
                                int numRows = -1, int guessRows = 10,
                                bool addIndexColumn = false)
        {
            return ReadStream(() => new StreamReader(filename),
                              separator: separator, header: header, columnNames: columnNames, dataTypes: dataTypes, numberOfRowsToRead: numRows,
                              guessRows: guessRows, addIndexColumn: addIndexColumn);
        }

        /// <summary>
        /// Reads a text file as a DataFrame.
        /// Follows pandas API.
        /// </summary>
        /// <param name="createStream">function which creates a stream</param>
        /// <param name="separator">column separator</param>
        /// <param name="header">has a header or not</param>
        /// <param name="columnNames">column names (can be empty)</param>
        /// <param name="dataTypes">column types (can be empty)</param>
        /// <param name="numberOfRowsToRead">number of rows to read not including the header(if present)</param>
        /// <param name="guessRows">number of rows used to guess types</param>
        /// <param name="addIndexColumn">add one column with the row index</param>
        /// <returns>DataFrame</returns>
        public static DataFrame ReadStream(Func<StreamReader> createStream,
                                char separator = ',', bool header = true,
                                string[] columnNames = null, Type[] dataTypes = null,
                                long numberOfRowsToRead = -1, int guessRows = 10, bool addIndexColumn = false)
        {
            var linesForGuessType = new List<string[]>();
            long rowline = 0;
            int numberOfColumns = 0;

            if (header == true && numberOfRowsToRead != -1)
                numberOfRowsToRead++;

            // First pass: schema and number of rows.
            using (var st = createStream())
            {
                string line = st.ReadLine();
                while (line != null)
                {
                    if ((numberOfRowsToRead == -1) || rowline < numberOfRowsToRead)
                    {
                        if (linesForGuessType.Count < guessRows)
                        {
                            var spl = line.Split(separator);
                            if (header && rowline == 0)
                            {
                                if (columnNames == null)
                                    columnNames = spl;
                            }
                            else
                            {
                                linesForGuessType.Add(spl);
                                numberOfColumns = Math.Max(numberOfColumns, spl.Length);
                            }
                        }
                    }
                    ++rowline;
                    if (rowline == numberOfRowsToRead)
                        break;
                    line = st.ReadLine();
                }
            }

            if (linesForGuessType.Count == 0)
                throw new FormatException(Strings.EmptyFile);

            List<DataFrameColumn> columns = new List<DataFrameColumn>(numberOfColumns);

            // Guesses types and adds columns.
            for (int i = 0; i < numberOfColumns; ++i)
            {
                Type kind = GuessKind(i, linesForGuessType);
                if (kind == typeof(bool))
                {
                    DataFrameColumn boolColumn = new PrimitiveDataFrameColumn<bool>(columnNames == null ? "Column" + i.ToString() : columnNames[i], header == true ? rowline - 1 : rowline);
                    columns.Add(boolColumn);
                }
                else if (kind == typeof(float))
                {
                    DataFrameColumn floatColumn = new PrimitiveDataFrameColumn<float>(columnNames == null ? "Column" + i.ToString() : columnNames[i], header == true ? rowline - 1 : rowline);
                    columns.Add(floatColumn);
                }
                else if (kind == typeof(string))
                {
                    DataFrameColumn stringColumn = new StringDataFrameColumn(columnNames == null ? "Column" + i.ToString() : columnNames[i], header == true ? rowline - 1 : rowline);
                    columns.Add(stringColumn);
                }
                else
                    throw new NotSupportedException(nameof(kind));
            }

            // Fills values.
            using (StreamReader st = createStream())
            {
                string line = st.ReadLine();
                rowline = 0;
                while (line != null && (numberOfRowsToRead == -1 || rowline < numberOfRowsToRead))
                {
                    var spl = line.Split(separator);
                    if (header && rowline == 0)
                    {
                        // Skips.
                    }
                    else
                    {
                        AppendRow(columns, header == true ? rowline - 1 : rowline, spl);
                    }
                    ++rowline;
                    line = st.ReadLine();
                }
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
            return new DataFrame(columns);
        }

        private static void AppendRow(List<DataFrameColumn> columns, long rowIndex, string[] values)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                DataFrameColumn column = columns[i];
                string val = values[i];
                Type dType = column.DataType;
                if (dType == typeof(bool))
                {
                    bool boolParse = bool.TryParse(val, out bool boolResult);
                    if (boolParse)
                    {
                        column[rowIndex] = boolResult;
                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(val))
                        {
                            column[rowIndex] = null;
                            continue;
                        }
                        throw new ArgumentException(Strings.MismatchedValueType + typeof(bool), nameof(val));
                    }
                }
                else if (dType == typeof(float))
                {
                    bool floatParse = float.TryParse(val, out float floatResult);
                    if (floatParse)
                    {
                        column[rowIndex] = floatResult;
                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(val))
                        {
                            column[rowIndex] = null;
                            continue;
                        }
                        throw new ArgumentException(Strings.MismatchedValueType + typeof(float), nameof(val));
                    }
                }
                else if (dType == typeof(string))
                {
                    column[rowIndex] = values[i];
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
