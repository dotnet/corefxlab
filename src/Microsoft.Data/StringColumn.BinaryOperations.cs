// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.Data
{
    public partial class StringDataFrameColumn : DataFrameColumn
    {
        public override DataFrameColumn Add(DataFrameColumn column, bool inPlace = false)
        {
            if (Length != column.Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            StringDataFrameColumn ret = inPlace ? this : Clone();
            for (long i = 0; i < Length; i++)
            {
                ret[i] += column[i].ToString();
            }
            return ret;
        }

        public override DataFrameColumn Add<T>(T value, bool inPlace = false)
        {
            StringDataFrameColumn ret = inPlace ? this : Clone();
            string valString = value.ToString();
            for (int i = 0; i < ret._stringBuffers.Count; i++)
            {
                IList<string> buffer = ret._stringBuffers[i];
                int bufferLen = buffer.Count;
                for (int j = 0; j < bufferLen; j++)
                {
                    buffer[j] += valString;
                }
            }
            return ret;
        }

        public override PrimitiveDataFrameColumn<bool> ElementwiseEquals(DataFrameColumn column)
        {
            if (Length != column.Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            PrimitiveDataFrameColumn<bool> ret = new PrimitiveDataFrameColumn<bool>(Name, Length);
            for (long i = 0; i < Length; i++)
            {
                ret[i] = (string)this[i] == column[i]?.ToString();
            }
            return ret;
        }

        public override PrimitiveDataFrameColumn<bool> ElementwiseEquals<T>(T value)
        {
            PrimitiveDataFrameColumn<bool> ret = new PrimitiveDataFrameColumn<bool>(Name, Length);
            string valString = value.ToString();
            for (long i = 0; i < Length; i++)
            {
                ret[i] = (string)this[i] == valString;
            }
            return ret;
        }

        public override PrimitiveDataFrameColumn<bool> ElementwiseNotEquals(DataFrameColumn column)
        {
            if (Length != column.Length)
            {
                throw new ArgumentException(Strings.MismatchedColumnLengths, nameof(column));
            }
            PrimitiveDataFrameColumn<bool> ret = new PrimitiveDataFrameColumn<bool>(Name, Length);
            for (long i = 0; i < Length; i++)
            {
                ret[i] = (string)this[i] != column[i].ToString();
            }
            return ret;
        }

        public override PrimitiveDataFrameColumn<bool> ElementwiseNotEquals<T>(T value)
        {
            PrimitiveDataFrameColumn<bool> ret = new PrimitiveDataFrameColumn<bool>(Name, Length);
            string valString = value.ToString();
            for (long i = 0; i < Length; i++)
            {
                ret[i] = (string)this[i] != valString;
            }
            return ret;
        }
    }
}
