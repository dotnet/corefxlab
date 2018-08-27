// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.JsonLab
{
    internal ref struct CustomDb
    {
        public Span<byte> Span;
        public int Index;
        private byte[] _rentedBuffer;
        ArrayPool<byte> _pool;
        
        public CustomDb(ArrayPool<byte> pool, int initialSize)
        {
            _pool = pool;
            _rentedBuffer = _pool.Rent(initialSize);
            Span = _rentedBuffer;
            Index = 0;
        }

        public void Dispose()
        {
            if (_pool != null)
                _pool.Return(_rentedBuffer);
            Span = Span<byte>.Empty;
            Index = 0;
        }

        public void Resize()
        {
            Span = Span.Slice(0, Index);
        }

        public Span<byte> Slice(int startIndex) => Span.Slice(startIndex);

        public Span<byte> Slice(int startIndex, int length) => Span.Slice(startIndex, length);

        public int Length => Span.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(JsonValueType jsonType, int startLocation, int LengthOrNumberOfRows = DbRow.UnknownSize)
        {
            Debug.Assert(jsonType >= JsonValueType.Object && jsonType <= JsonValueType.Unknown);
            Debug.Assert(startLocation >= 0);
            Debug.Assert(LengthOrNumberOfRows >= DbRow.UnknownSize);

            if (Index >= Span.Length - DbRow.Size)
                Enlarge();
            var dbRow = new DbRow(jsonType, startLocation, LengthOrNumberOfRows);
            MemoryMarshal.Write(Span.Slice(Index), ref dbRow);
            Index += DbRow.Size;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Enlarge()
        {
            int size = Span.Length * 2;
            byte[] newArray = _pool.Rent(size);
            Span<byte> newDbSpace = newArray;
            Span.Slice(0, Index).CopyTo(newDbSpace);
            Span = newDbSpace;
            _pool.Return(_rentedBuffer);
            _rentedBuffer = newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetLength(int index, int length)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            Debug.Assert(length >= 0);
            MemoryMarshal.Write(Span.Slice(index + 4), ref length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //TODO: Resolve little endian/big endian issue
        public void SetNumberOfRows(int index, int numberOfRows)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            Debug.Assert(numberOfRows >= 1 && numberOfRows <= 0x0FFFFFFF);
            byte previous = (byte)(Span[index + DbRow.Size - 1] >> 4);
            int value = (previous << 28) | numberOfRows;
            MemoryMarshal.Write(Span.Slice(index + DbRow.Size - 4), ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        //TODO: Resolve little endian/big endian issue
        public void SetHasChildren(int index)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            index += DbRow.Size - 1;
            byte previous = Span[index];
            Span[index] = (byte)(1 << 7 | previous);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int FindIndexOfFirstUnsetSizeOrLength(JsonValueType lookupType)
        {
            Debug.Assert(lookupType == JsonValueType.Object || lookupType == JsonValueType.Array);
            return BackwardPass(lookupType);
        }

        private int ForwardPass(JsonValueType lookupType)
        {
            for (int i = 0; i < Span.Length; i += DbRow.Size)
            {
                DbRow row = MemoryMarshal.Read<DbRow>(Span.Slice(i));
                if (row.SizeOrLength == DbRow.UnknownSize && row.JsonType == lookupType)
                {
                    return i;
                }
                if (!row.IsSimpleValue)
                {
                    i += row.NumberOfRows * DbRow.Size;
                }
            }
            // We should never reach here.
            Debug.Assert(false);
            return -1;
        }

        private int BackwardPass(JsonValueType lookupType)
        {
            // TODO: Investigate performance impact of adding "skip" logic similar to ForwardPass
            for (int i = Index - DbRow.Size; i >= DbRow.Size; i -= DbRow.Size)
            {
                DbRow row = MemoryMarshal.Read<DbRow>(Span.Slice(i));
                if (row.SizeOrLength == DbRow.UnknownSize && row.JsonType == lookupType)
                {
                    return i;
                }
            }
            // We should never reach here.
            Debug.Assert(false);
            return -1;
        }

        public DbRow Get(int index = 0)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            return index == 0
            ? MemoryMarshal.Read<DbRow>(Span)
            : MemoryMarshal.Read<DbRow>(Span.Slice(index));
        }

        public int GetLocation(int index = 0)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            return index == 0
            ? MemoryMarshal.Read<int>(Span)
            : MemoryMarshal.Read<int>(Span.Slice(index));
        }

        public int GetSizeOrLength(int index)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            return MemoryMarshal.Read<int>(Span.Slice(index + 4));
        }

        public JsonValueType GetJsonType(int index = 0)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            int union = MemoryMarshal.Read<int>(Span.Slice(index + 8));
            return (JsonValueType)((union & 0x70000000) >> 28);
        }

        public bool GetHasChildren(int index = 0)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            int union = MemoryMarshal.Read<int>(Span.Slice(index + 8));
            return union < 0;
        }

        public int GetNumberOfRows(int index = 0)
        {
            Debug.Assert(index >= 0 && index <= Span.Length - DbRow.Size);
            int union = MemoryMarshal.Read<int>(Span.Slice(index + 8));
            return union & 0x0FFFFFFF;
        }

        public string PrintDatabase()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nameof(DbRow.Location) + "\t" + nameof(DbRow.SizeOrLength) + "\t" + nameof(DbRow.JsonType) + "\t" + nameof(DbRow.HasChildren) + "\t" + nameof(DbRow.NumberOfRows) + "\r\n");
            for (int i = 0; i < Span.Length; i += DbRow.Size)
            {
                DbRow record = MemoryMarshal.Read<DbRow>(Span.Slice(i));
                sb.Append(record.Location + "\t" + record.SizeOrLength + "\t" + record.JsonType + "\t" + record.HasChildren + "\t" + record.NumberOfRows + "\r\n");
            }
            return sb.ToString();
        }
    }
}
