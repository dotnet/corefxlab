using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    throw new FormatException(string.Format(Strings.LessColumnsThatExpected, nbline + 1)); // Will this really work?

                var val = line[col];
                try
                {
                    bool.Parse(val);
                    res = DetermineType(nbline == 0, typeof(bool), res);
                    ++nbline;
                    continue;
                }
                catch (Exception)
                {
                    if (string.IsNullOrEmpty(val))
                    {
                        res = DetermineType(nbline == 0, typeof(bool), res);
                        continue;
                    }
                }
                try
                {
                    float.Parse(val);
                    res = DetermineType(nbline == 0, typeof(float), res);
                    ++nbline;
                    continue;
                }
                catch (Exception)
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
        /// Reads a text file as a IDataView.
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

        private delegate StreamReader FunctionCreateStreamReader();

        /// <summary>
        /// Reads a text file as a IDataView.
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
        private static DataFrame ReadStream(FunctionCreateStreamReader createStream,
                                char sep = ',', bool header = true,
                                string[] names = null, Type[] dtypes = null,
                                long nrows = -1, int guess_rows = 10, bool index = false)
        {
            var lines = new List<string[]>();
            long rowline = 0;

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
                        if (lines.Count < guess_rows)
                            lines.Add(spl);
                    }
                    ++nbline;
                    line = st.ReadLine();
                }
            }

            if (lines.Count == 0)
                throw new FormatException(Strings.EmptyFile);
            int numCol = lines.Select(c => c.Length).Max();
            var df = new DataFrame();

            // Guesses types and adds columns.
            for (int i = 0; i < numCol; ++i)
            {
                Type kind = GuessKind(i, lines);
                switch (kind)
                {
                    case Type boolType when boolType == typeof(bool):
                        BaseColumn boolColumn = new PrimitiveColumn<bool>(names[i]);
                        df.InsertColumn(i, boolColumn);
                        break;
                    case Type byteType when byteType == typeof(byte):
                        BaseColumn byteColumn = new PrimitiveColumn<byte>(names[i]);
                        df.InsertColumn(i, byteColumn);
                        break;
                    case Type charType when charType == typeof(char):
                        BaseColumn charColumn = new PrimitiveColumn<char>(names[i]);
                        df.InsertColumn(i, charColumn);
                        break;
                    case Type decimalType when decimalType == typeof(decimal):
                        BaseColumn decimalColumn = new PrimitiveColumn<decimal>(names[i]);
                        df.InsertColumn(i, decimalColumn);
                        break;
                    case Type doubleType when doubleType == typeof(double):
                        BaseColumn doubleColumn = new PrimitiveColumn<double>(names[i]);
                        df.InsertColumn(i, doubleColumn);
                        break;
                    case Type floatType when floatType == typeof(float):
                        BaseColumn floatColumn = new PrimitiveColumn<float>(names[i]);
                        df.InsertColumn(i, floatColumn);
                        break;
                    case Type intType when intType == typeof(int):
                        BaseColumn intColumn = new PrimitiveColumn<int>(names[i]);
                        df.InsertColumn(i, intColumn);
                        break;
                    case Type longType when longType == typeof(long):
                        BaseColumn longColumn = new PrimitiveColumn<long>(names[i]);
                        df.InsertColumn(i, longColumn);
                        break;
                    case Type sbyteType when sbyteType == typeof(sbyte):
                        BaseColumn sbyteColumn = new PrimitiveColumn<sbyte>(names[i]);
                        df.InsertColumn(i, sbyteColumn);
                        break;
                    case Type shortType when shortType == typeof(short):
                        BaseColumn shortColumn = new PrimitiveColumn<short>(names[i]);
                        df.InsertColumn(i, shortColumn);
                        break;
                    case Type uintType when uintType == typeof(uint):
                        BaseColumn uintColumn = new PrimitiveColumn<uint>(names[i]);
                        df.InsertColumn(i, uintColumn);
                        break;
                    case Type ulongType when ulongType == typeof(ulong):
                        BaseColumn ulongColumn = new PrimitiveColumn<ulong>(names[i]);
                        df.InsertColumn(i, ulongColumn);
                        break;
                    case Type ushortType when ushortType == typeof(ushort):
                        BaseColumn ushortColumn = new PrimitiveColumn<ushort>(names[i]);
                        df.InsertColumn(i, ushortColumn);
                        break;
                    case Type stringType when stringType == typeof(string):
                        BaseColumn stringColumn = new StringColumn(names[i], 0);
                        df.InsertColumn(i, stringColumn);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            // Fills values.
            using (var st = createStream())
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
                        df.AppendRow(rowline, spl);
                        ++rowline;
                    }
                    ++nbline;
                    line = st.ReadLine();
                }
            }

            if (index)
            {
                // TODO: Bug in InsertColumn. Can only insert at the end. Should support inserting in the middle
                PrimitiveColumn<int> indexColumn = new PrimitiveColumn<int>("IndexColumn", df.RowCount);
                for (int i = 0; i < df.RowCount; i++)
                {
                    indexColumn[i] = i;
                }
                df.InsertColumn(df.ColumnCount, indexColumn);

            }

            return df;
        }

        private void AppendRow(long index, string[] values) => _table.AppendRow(index, values);
    }
}
