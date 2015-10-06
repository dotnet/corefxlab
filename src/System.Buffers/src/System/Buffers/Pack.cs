// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System.Threading;

namespace System.Buffers
{
    public static class Pack
    {
        public static Two<T> Create<T>(T first, T second)
        {
            return new Two<T>(first, second);
        }
        public static Three<T> Create<T>(T first, T second, T third)
        {
            return new Three<T>(first, second, third);
        }
        public static Four<T> Create<T>(T first, T second, T third, T fourth)
        {
            return new Four<T>(first, second, third, fourth);
        }

        public struct Two<T>
        {
            T First;
            T Second;

            public Two(T first, T second)
            {
                First = first;
                Second = second;
            }

            public T this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0: return First;
                        case 1: return Second;
                        default: throw new IndexOutOfRangeException();
                    }
                }

                set
                {
                    switch (index)
                    {
                        case 0: First = value; break;
                        case 1: Second = value; break;
                        default: throw new IndexOutOfRangeException();
                    }
                }
            }
        }

        public struct Three<T>
        {
            T First;
            T Second;
            T Third;

            public Three(T first, T second, T third)
            {
                First = first;
                Second = second;
                Third = third;
            }

            public T this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0: return First;
                        case 1: return Second;
                        case 2: return Third;
                        default: throw new IndexOutOfRangeException();
                    }
                }

                set
                {
                    switch (index)
                    {
                        case 0: First = value; break;
                        case 1: Second = value; break;
                        case 2: Third = value; break;
                        default: throw new IndexOutOfRangeException();
                    }
                }
            }
        }

        public struct Four<T>
        {
            T First;
            T Second;
            T Third;
            T Fourth;

            public Four(T first, T second, T third, T fourth)
            {
                First = first;
                Second = second;
                Third = third;
                Fourth = fourth;
            }

            public T this[int index]
            {
                get
                {
                    switch (index)
                    {
                        case 0: return First;
                        case 1: return Second;
                        case 2: return Third;
                        case 3: return Fourth;
                        default: throw new IndexOutOfRangeException();
                    }
                }

                set
                {
                    switch (index)
                    {
                        case 0: First = value; break;
                        case 1: Second = value; break;
                        case 2: Third = value; break;
                        case 3: Fourth = value; break;
                        default: throw new IndexOutOfRangeException();
                    }
                }
            }
        }
    }
}