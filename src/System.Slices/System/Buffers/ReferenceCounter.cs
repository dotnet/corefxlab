// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Runtime
{
    /// <summary>
    /// Make sure the struct is not copied, i.e. pass it only by reference
    /// </summary>
    /// <remarks>
    /// The  counter is not completly race-free. Reading GetGlobalCount and AddReference/Release are subject to a race.
    /// </remarks>
    public struct ReferenceCounter
    {
        // thread local counts that can be updated very efficiently
        [ThreadStatic]
        static ObjectTable t_threadLocalCounts;

        // all thread local counts; these are tallied up when global count is comupted
        static ObjectTable[] s_allTables = new ObjectTable[Environment.ProcessorCount];
        static int s_allTablesCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddReference(object obj)
        {
            var localCounts = t_threadLocalCounts;
            if (localCounts == null)
            {
                localCounts = AddThreadLocalTable();
            }
            localCounts.Increment(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(object obj)
        {
            var localCounts = t_threadLocalCounts;
            localCounts.Decrement(obj);
        }

        // TODO: can we detect if the object was only refcounted on one thread and its the current thread? If yes, we don't need to synchronize?
        public uint GetGlobalCount(object obj)
        {
            var allTables = s_allTables;
            lock (allTables)
            {
                uint globalCount = 0;
                for (int index = 0; index < s_allTablesCount; index++)
                {
                    globalCount += allTables[index].GetCount(obj);
                }
                return globalCount;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetThreadLocalCount(object obj)
        {
            var localCounts = t_threadLocalCounts;
            if (localCounts == null) return 0;
            return localCounts.GetCount(obj);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ObjectTable AddThreadLocalTable()
        {
            Debug.Assert(t_threadLocalCounts == null);
            var localCounts = new ObjectTable();
            t_threadLocalCounts = localCounts;
            lock (s_allTables)
            {
                    var allTables = s_allTables;
                    if (s_allTablesCount >= allTables.Length)
                    {
                        var newAllTables = new ObjectTable[allTables.Length << 1];
                        allTables.CopyTo(newAllTables, 0);
                        s_allTables = newAllTables;
                        allTables = newAllTables;
                    }
                    allTables[s_allTablesCount++] = localCounts;
            }
            return localCounts;
        }
    }

    // This datastructure holds a collection of object to reference count mappings
    sealed class ObjectTable
    {
        // if you change this constant, update ResizingObjectTableWorks test.
        const int DefaultTableCapacity = 16;

        ObjectReferences[] _items;
        int _count;

        struct ObjectReferences
        {
            public object Obj;
            public uint ReferenceCount;

            public override string ToString()
            {
                return ReferenceCount.ToString();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Increment(object obj)
        {
            if (_items == null)
            {
                _items = new ObjectReferences[DefaultTableCapacity];
            }
            var index = FindExistingOrNewIndex(obj);
            _items[index].ReferenceCount++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Decrement(object obj)
        {
            var index = FindExistingIndex(obj);
            if(index == -1) {
                ThrowCountNotPositive();
            }
            _items[index].ReferenceCount--;
            if (_items[index].ReferenceCount == 0) _count--;
        }

        private void ThrowCountNotPositive()
        {
            throw new InvalidOperationException("the object's count is not greater than zero");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindExistingOrNewIndex(object obj)
        {
            if(_count == 0)
            {
                _items[0].Obj = obj;
                _count = 1;
                return 0;
            }

            int freeIndex = -1;
            int foundObjectCount = 0;
            int index;
            for (index = 0; index < _items.Length; index++)
            {
                // if found the object, just return the index 
                if (ReferenceEquals(_items[index].Obj, obj))
                {
                    return index;
                }

                // the slot is free, remember it; we want to use the first free slot we found, if the object is not found later 
                if (_items[index].ReferenceCount == 0)
                {
                    if (freeIndex == -1) freeIndex = index;
                }
                // count number of objects found but not matching; if it's equal to number of objects in the array, we can exit the loop
                else
                {
                    foundObjectCount++;
                    if (foundObjectCount == _count) {
                        index++;
                        break;
                    }
                }
            }

            if (freeIndex != -1)
            {
                _items[freeIndex].Obj = obj;
                _count++;
                return freeIndex;
            }

            // resize
            if (index == _items.Length)
            {
                var larger = new ObjectReferences[_items.Length << 1];
                _items.CopyTo(larger, 0);
                _items = larger;
            }

            _items[index].Obj = obj;
            _count++;
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindExistingIndex(object obj)
        {
            for (int index = 0; index < _items.Length; index++)
            {
                if (ReferenceEquals(_items[index].Obj, obj))
                {
                    return index;
                }
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint GetCount(object obj)
        {
            var index = FindExistingIndex(obj);
            if (index == -1) return 0;
            return _items[index].ReferenceCount;
        }
    }
}