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
        /// <param name="sep">column separator</param>
        /// <param name="header">has a header or not</param>
        /// <param name="names">column names (can be empty)</param>
        /// <param name="dTypes">column types (can be empty)</param>
        /// <param name="nRows">number of rows to read</param>
        /// <param name="guessRows">number of rows used to guess types</param>
        /// <param name="index">add one column with the row index</param>
        /// <returns>DataFrame</returns>
        public static DataFrame ReadCsv(string filename,
                                char sep = ',', bool header = true,
                                string[] names = null, Type[] dTypes = null,
                                int nRows = -1, int guessRows = 10,
                                bool index = false)
        {
            return ReadStream(() => new StreamReader(filename),
                              sep: sep, header: header, names: names, dTypes: dTypes, nRows: nRows,
                              guessRows: guessRows, index: index);
        }

        /// <summary>
        /// Reads a text file as a DataFrame.
        /// Follows pandas API.
        /// </summary>
        /// <param name="createStream">function which creates a stream</param>
        /// <param name="sep">column separator</param>
        /// <param name="header">has a header or not</param>
        /// <param name="names">column names (can be empty)</param>
        /// <param name="dTypes">column types (can be empty)</param>
        /// <param name="nRows">number of rows to read</param>
        /// <param name="guessRows">number of rows used to guess types</param>
        /// <param name="index">add one column with the row index</param>
        /// <returns>DataFrame</returns>
        public static DataFrame ReadStream(Func<StreamReader> createStream,
                                char sep = ',', bool header = true,
                                string[] names = null, Type[] dTypes = null,
                                long nRows = -1, int guessRows = 10, bool index = false)
        {
            var linesForGuessType = new List<string[]>();
            long rowline = 0;
            int numberOfColumns = 0;

            // First pass: schema and number of rows.
            using (var st = createStream())
            {
                string line = st.ReadLine();
                int nbline = 0;
                while (line != null && (nRows == -1 || rowline < nRows))
                {
                    var spl = line.Split(sep);
                    if (header && nbline == 0)
                    {
                        if (names == null)
                            names = spl;
                    }
                    else
                    {
                        ++rowline;
                        if (linesForGuessType.Count < guessRows)
                        {
                            linesForGuessType.Add(spl);
                            numberOfColumns = Math.Max(numberOfColumns, spl.Length);
                        }
                    }
                    ++nbline;
                    line = st.ReadLine();
                }
            }

            if (linesForGuessType.Count == 0)
                throw new FormatException(Strings.EmptyFile);

            List<BaseColumn> columns = new List<BaseColumn>(numberOfColumns);

            // Guesses types and adds columns.
            for (int i = 0; i < numberOfColumns; ++i)
            {
                Type kind = GuessKind(i, linesForGuessType);
                if (kind == typeof(bool))
                {
                    BaseColumn boolColumn = new PrimitiveColumn<bool>(names[i]);
                    columns.Add(boolColumn);
                }
                else if (kind == typeof(float))
                {
                    BaseColumn floatColumn = new PrimitiveColumn<float>(names[i]);
                    columns.Add(floatColumn);
                }
                else if (kind == typeof(string))
                {
                    BaseColumn stringColumn = new StringColumn(names[i], 0);
                    columns.Add(stringColumn);
                }
                else
                    throw new NotSupportedException(nameof(kind));
            }

            // Fills values.
            using (StreamReader st = createStream())
            {
                string line = st.ReadLine();
                long nbline = 0;
                rowline = 0;
                while (line != null && (nRows == -1 || rowline < nRows))
                {
                    var spl = line.Split(sep);
                    if (header && nbline == 0)
                    {
                        // Skips.
                    }
                    else
                    {
                        AppendRow(columns, rowline, spl);
                        ++rowline;
                    }
                    ++nbline;
                    line = st.ReadLine();
                }
            }

            if (index)
            {
                PrimitiveColumn<int> indexColumn = new PrimitiveColumn<int>("IndexColumn", columns[0].Length);
                for (int i = 0; i < columns[0].Length; i++)
                {
                    indexColumn[i] = i;
                }
                columns.Insert(0, indexColumn);
            }
            return new DataFrame(columns);
        }

        private static void AppendRow(List<BaseColumn> columns, long rowIndex, string[] values)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                BaseColumn column = columns[i];
                column.Resize(rowIndex + 1);
                string val = values[i];
                bool boolParse = bool.TryParse(val, out bool boolResult);
                if (boolParse)
                {
                    column[rowIndex] = boolResult;
                    continue;
                }
                bool floatParse = float.TryParse(val, out float floatResult);
                if (floatParse)
                {
                    column[rowIndex] = floatResult;
                    continue;
                }
                column[rowIndex] = values[i];
            }
        }
    }
}
