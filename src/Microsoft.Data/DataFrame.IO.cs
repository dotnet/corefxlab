using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.Data
{

    public static class Utilities
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
        /// <param name="dtypes">column types (can be empty)</param>
        /// <param name="nrows">number of rows to read</param>
        /// <param name="guess_rows">number of rows used to guess types</param>
        /// <param name="encoding">text encoding</param>
        /// <param name="index">add one column with the row index</param>
        /// <returns>DataFrame</returns>
        public static DataFrame ReadCsv(string filename,
                                char sep = ',', bool header = true,
                                string[] names = null, Type[] dtypes = null,
                                int nrows = -1, int guess_rows = 10,
                                Encoding encoding = null, bool index = false)
        {
            return ReadStream(() => new StreamReader(filename, encoding ?? Encoding.ASCII),
                              sep: sep, header: header, names: names, dtypes: dtypes, nrows: nrows,
                              guess_rows: guess_rows, index: index);
        }

        /// <summary>
        /// Reads a text file as a DataFrame.
        /// Follows pandas API.
        /// </summary>
        /// <param name="createStream">function which creates a stream</param>
        /// <param name="sep">column separator</param>
        /// <param name="header">has a header or not</param>
        /// <param name="names">column names (can be empty)</param>
        /// <param name="dtypes">column types (can be empty)</param>
        /// <param name="nrows">number of rows to read</param>
        /// <param name="guess_rows">number of rows used to guess types</param>
        /// <param name="index">add one column with the row index</param>
        /// <returns>DataFrame</returns>
        public static DataFrame ReadStream(Func<StreamReader> createStream,
                                char sep = ',', bool header = true,
                                string[] names = null, Type[] dtypes = null,
                                long nrows = -1, int guess_rows = 10, bool index = false)
        {
            var linesForGuessType = new List<string[]>();
            long rowline = 0;
            int numberOfColumns = 0;

            // First pass: schema and number of rows.
            using (var st = createStream())
            {
                string line = st.ReadLine();
                int nbline = 0;
                while (line != null && (nrows == -1 || rowline < nrows))
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
                        if (linesForGuessType.Count < guess_rows)
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

            DataFrame df = new DataFrame();

            // Guesses types and adds columns.
            for (int i = 0; i < numberOfColumns; ++i)
            {
                Type kind = GuessKind(i, linesForGuessType);
                if (kind == typeof(bool))
                {
                    BaseColumn boolColumn = new PrimitiveColumn<bool>(names[i]);
                    df.InsertColumn(i, boolColumn);
                }
                else if (kind == typeof(float))
                {
                    BaseColumn floatColumn = new PrimitiveColumn<float>(names[i]);
                    df.InsertColumn(i, floatColumn);
                }
                else if (kind == typeof(string))
                {
                    BaseColumn stringColumn = new StringColumn(names[i], 0);
                    df.InsertColumn(i, stringColumn);
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
                while (line != null && (nrows == -1 || rowline < nrows))
                {
                    var spl = line.Split(sep);
                    if (header && nbline == 0)
                    {
                        // Skips.
                    }
                    else
                    {
                        AppendRow(df, rowline, spl);
                        ++rowline;
                    }
                    ++nbline;
                    line = st.ReadLine();
                }
            }

            df.SetTableRowCount(rowline);
            if (index)
            {
                PrimitiveColumn<int> indexColumn = new PrimitiveColumn<int>("IndexColumn", df.RowCount);
                for (int i = 0; i < df.RowCount; i++)
                {
                    indexColumn[i] = i;
                }
                df.InsertColumn(0, indexColumn);

            }
            return df;
        }

        private static void AppendRow(DataFrame df, long rowIndex, string[] values)
        {
            for (int i = 0; i < df.ColumnCount; i++)
            {
                BaseColumn column = df.Column(i);
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
