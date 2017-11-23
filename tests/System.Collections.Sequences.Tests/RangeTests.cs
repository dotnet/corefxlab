
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Collections.Sequences.Tests
{
    /// <summary>
    /// Tests for the Range<T?> structure.
    /// </summary>
    public class RangeNullableTests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(false, new Range<Int32?>(null, null).IsEmpty());
            Assert.Equal(false, new Range<Int32?>(null, 1).IsEmpty());
            Assert.Equal(false, new Range<Int32?>(1, null).IsEmpty());
            Assert.Equal(true, new Range<Int32?>(1, 1).IsEmpty());
        }

        [Fact]
        public void IsNormalized()
        {
            Assert.Equal(true, new Range<Int32?>(null, null).IsNormalized());
            Assert.Equal(true, new Range<Int32?>(null, 1).IsNormalized());
            Assert.Equal(true, new Range<Int32?>(1, null).IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<Int32?>(null, null).Normalize();
            Assert.Equal(null, range.From);
            Assert.Equal(null, range.To);

            range = new Range<Int32?>(null, 1).Normalize();
            Assert.Equal(null, range.From);
            Assert.Equal(1, range.To);

            range = new Range<Int32?>(1, null).Normalize();
            Assert.Equal(1, range.From);
            Assert.Equal(null, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<Int32?>(null, null);
            Assert.Equal(false, range.Contains(null));
            Assert.Equal(true, range.Contains(1));

            range = new Range<Int32?>(null, 1);
            Assert.Equal(false, range.Contains(null));
            Assert.Equal(true, range.Contains(0));
            Assert.Equal(false, range.Contains(1));

            range = new Range<Int32?>(1, null);
            Assert.Equal(false, range.Contains(null));
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(true, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<Int32?>(null, null);
            var range2 = new Range<Int32?>(null, null);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));

            range1 = new Range<Int32?>(null, 0);
            range2 = new Range<Int32?>(null, null);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));

            range1 = new Range<Int32?>(0, null);
            range2 = new Range<Int32?>(null, null);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));

            range1 = new Range<Int32?>(null, 0);
            range2 = new Range<Int32?>(0, null);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Int32?>(null, 1);
            range2 = new Range<Int32?>(2, null);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Int32?>(null, 2);
            range2 = new Range<Int32?>(1, null);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<Int32?>(null, null);
            var range2 = new Range<Int32?>(null, null);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal(null, intersection1.From);
            Assert.Equal(null, intersection1.To);

            range1 = new Range<Int32?>(null, 0);
            range2 = new Range<Int32?>(null, null);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal(null, intersection1.From);
            Assert.Equal(0, intersection1.To);

            range1 = new Range<Int32?>(0, null);
            range2 = new Range<Int32?>(null, null);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal(0, intersection1.From);
            Assert.Equal(null, intersection1.To);

            range1 = new Range<Int32?>(null, 0);
            range2 = new Range<Int32?>(0, null);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Int32?>(null, 1);
            range2 = new Range<Int32?>(2, null);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Int32?>(null, 2);
            range2 = new Range<Int32?>(1, null);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal(1, intersection1.From);
            Assert.Equal(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<SByte> structure.
    /// </summary>
    public class RangeSByteTests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<SByte>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<SByte>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<SByte>(0, 1);
            Assert.Equal<SByte>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<SByte>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<SByte>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<SByte>(0, 1).Normalize();
            Assert.Equal<SByte>(0, range.From);
            Assert.Equal<SByte>(1, range.To);

            range = new Range<SByte>(1, 0).Normalize();
            Assert.Equal<SByte>(0, range.From);
            Assert.Equal<SByte>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<SByte>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<SByte>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<SByte>(0, 0);
            var range2 = new Range<SByte>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<SByte>(0, 1);
            range2 = new Range<SByte>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<SByte>(0, 1);
            range2 = new Range<SByte>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<SByte>(0, 3);
            range2 = new Range<SByte>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<SByte>(0, 0);
            var range2 = new Range<SByte>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<SByte>(0, 1);
            range2 = new Range<SByte>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<SByte>(0, 1);
            range2 = new Range<SByte>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<SByte>(0, intersection1.From);
            Assert.Equal<SByte>(1, intersection1.To);
            
            range1 = new Range<SByte>(0, 3);
            range2 = new Range<SByte>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<SByte>(1, intersection1.From);
            Assert.Equal<SByte>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<Int16> structure.
    /// </summary>
    public class RangeInt16Tests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<Int16>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<Int16>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<Int16>(0, 1);
            Assert.Equal<Int16>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<Int16>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<Int16>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<Int16>(0, 1).Normalize();
            Assert.Equal<Int16>(0, range.From);
            Assert.Equal<Int16>(1, range.To);

            range = new Range<Int16>(1, 0).Normalize();
            Assert.Equal<Int16>(0, range.From);
            Assert.Equal<Int16>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<Int16>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<Int16>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<Int16>(0, 0);
            var range2 = new Range<Int16>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Int16>(0, 1);
            range2 = new Range<Int16>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<Int16>(0, 1);
            range2 = new Range<Int16>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<Int16>(0, 3);
            range2 = new Range<Int16>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<Int16>(0, 0);
            var range2 = new Range<Int16>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Int16>(0, 1);
            range2 = new Range<Int16>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<Int16>(0, 1);
            range2 = new Range<Int16>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Int16>(0, intersection1.From);
            Assert.Equal<Int16>(1, intersection1.To);
            
            range1 = new Range<Int16>(0, 3);
            range2 = new Range<Int16>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Int16>(1, intersection1.From);
            Assert.Equal<Int16>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<Int32> structure.
    /// </summary>
    public class RangeInt32Tests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<Int32>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<Int32>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<Int32>(0, 1);
            Assert.Equal<Int32>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<Int32>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<Int32>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<Int32>(0, 1).Normalize();
            Assert.Equal<Int32>(0, range.From);
            Assert.Equal<Int32>(1, range.To);

            range = new Range<Int32>(1, 0).Normalize();
            Assert.Equal<Int32>(0, range.From);
            Assert.Equal<Int32>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<Int32>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<Int32>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<Int32>(0, 0);
            var range2 = new Range<Int32>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Int32>(0, 1);
            range2 = new Range<Int32>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<Int32>(0, 1);
            range2 = new Range<Int32>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<Int32>(0, 3);
            range2 = new Range<Int32>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<Int32>(0, 0);
            var range2 = new Range<Int32>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Int32>(0, 1);
            range2 = new Range<Int32>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<Int32>(0, 1);
            range2 = new Range<Int32>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Int32>(0, intersection1.From);
            Assert.Equal<Int32>(1, intersection1.To);
            
            range1 = new Range<Int32>(0, 3);
            range2 = new Range<Int32>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Int32>(1, intersection1.From);
            Assert.Equal<Int32>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<Int64> structure.
    /// </summary>
    public class RangeInt64Tests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<Int64>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<Int64>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<Int64>(0, 1);
            Assert.Equal<Int64>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<Int64>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<Int64>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<Int64>(0, 1).Normalize();
            Assert.Equal<Int64>(0, range.From);
            Assert.Equal<Int64>(1, range.To);

            range = new Range<Int64>(1, 0).Normalize();
            Assert.Equal<Int64>(0, range.From);
            Assert.Equal<Int64>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<Int64>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<Int64>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<Int64>(0, 0);
            var range2 = new Range<Int64>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Int64>(0, 1);
            range2 = new Range<Int64>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<Int64>(0, 1);
            range2 = new Range<Int64>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<Int64>(0, 3);
            range2 = new Range<Int64>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<Int64>(0, 0);
            var range2 = new Range<Int64>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Int64>(0, 1);
            range2 = new Range<Int64>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<Int64>(0, 1);
            range2 = new Range<Int64>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Int64>(0, intersection1.From);
            Assert.Equal<Int64>(1, intersection1.To);
            
            range1 = new Range<Int64>(0, 3);
            range2 = new Range<Int64>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Int64>(1, intersection1.From);
            Assert.Equal<Int64>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<Byte> structure.
    /// </summary>
    public class RangeByteTests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<Byte>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<Byte>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<Byte>(0, 1);
            Assert.Equal<Byte>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<Byte>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<Byte>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<Byte>(0, 1).Normalize();
            Assert.Equal<Byte>(0, range.From);
            Assert.Equal<Byte>(1, range.To);

            range = new Range<Byte>(1, 0).Normalize();
            Assert.Equal<Byte>(0, range.From);
            Assert.Equal<Byte>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<Byte>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<Byte>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<Byte>(0, 0);
            var range2 = new Range<Byte>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Byte>(0, 1);
            range2 = new Range<Byte>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<Byte>(0, 1);
            range2 = new Range<Byte>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<Byte>(0, 3);
            range2 = new Range<Byte>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<Byte>(0, 0);
            var range2 = new Range<Byte>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Byte>(0, 1);
            range2 = new Range<Byte>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<Byte>(0, 1);
            range2 = new Range<Byte>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Byte>(0, intersection1.From);
            Assert.Equal<Byte>(1, intersection1.To);
            
            range1 = new Range<Byte>(0, 3);
            range2 = new Range<Byte>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Byte>(1, intersection1.From);
            Assert.Equal<Byte>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<UInt16> structure.
    /// </summary>
    public class RangeUInt16Tests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<UInt16>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<UInt16>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<UInt16>(0, 1);
            Assert.Equal<UInt16>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<UInt16>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<UInt16>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<UInt16>(0, 1).Normalize();
            Assert.Equal<UInt16>(0, range.From);
            Assert.Equal<UInt16>(1, range.To);

            range = new Range<UInt16>(1, 0).Normalize();
            Assert.Equal<UInt16>(0, range.From);
            Assert.Equal<UInt16>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<UInt16>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<UInt16>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<UInt16>(0, 0);
            var range2 = new Range<UInt16>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<UInt16>(0, 1);
            range2 = new Range<UInt16>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<UInt16>(0, 1);
            range2 = new Range<UInt16>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<UInt16>(0, 3);
            range2 = new Range<UInt16>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<UInt16>(0, 0);
            var range2 = new Range<UInt16>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<UInt16>(0, 1);
            range2 = new Range<UInt16>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<UInt16>(0, 1);
            range2 = new Range<UInt16>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<UInt16>(0, intersection1.From);
            Assert.Equal<UInt16>(1, intersection1.To);
            
            range1 = new Range<UInt16>(0, 3);
            range2 = new Range<UInt16>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<UInt16>(1, intersection1.From);
            Assert.Equal<UInt16>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<UInt32> structure.
    /// </summary>
    public class RangeUInt32Tests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<UInt32>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<UInt32>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<UInt32>(0, 1);
            Assert.Equal<UInt32>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<UInt32>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<UInt32>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<UInt32>(0, 1).Normalize();
            Assert.Equal<UInt32>(0, range.From);
            Assert.Equal<UInt32>(1, range.To);

            range = new Range<UInt32>(1, 0).Normalize();
            Assert.Equal<UInt32>(0, range.From);
            Assert.Equal<UInt32>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<UInt32>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<UInt32>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<UInt32>(0, 0);
            var range2 = new Range<UInt32>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<UInt32>(0, 1);
            range2 = new Range<UInt32>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<UInt32>(0, 1);
            range2 = new Range<UInt32>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<UInt32>(0, 3);
            range2 = new Range<UInt32>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<UInt32>(0, 0);
            var range2 = new Range<UInt32>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<UInt32>(0, 1);
            range2 = new Range<UInt32>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<UInt32>(0, 1);
            range2 = new Range<UInt32>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<UInt32>(0, intersection1.From);
            Assert.Equal<UInt32>(1, intersection1.To);
            
            range1 = new Range<UInt32>(0, 3);
            range2 = new Range<UInt32>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<UInt32>(1, intersection1.From);
            Assert.Equal<UInt32>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<UInt64> structure.
    /// </summary>
    public class RangeUInt64Tests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<UInt64>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<UInt64>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<UInt64>(0, 1);
            Assert.Equal<UInt64>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<UInt64>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<UInt64>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<UInt64>(0, 1).Normalize();
            Assert.Equal<UInt64>(0, range.From);
            Assert.Equal<UInt64>(1, range.To);

            range = new Range<UInt64>(1, 0).Normalize();
            Assert.Equal<UInt64>(0, range.From);
            Assert.Equal<UInt64>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<UInt64>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<UInt64>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<UInt64>(0, 0);
            var range2 = new Range<UInt64>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<UInt64>(0, 1);
            range2 = new Range<UInt64>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<UInt64>(0, 1);
            range2 = new Range<UInt64>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<UInt64>(0, 3);
            range2 = new Range<UInt64>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<UInt64>(0, 0);
            var range2 = new Range<UInt64>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<UInt64>(0, 1);
            range2 = new Range<UInt64>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<UInt64>(0, 1);
            range2 = new Range<UInt64>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<UInt64>(0, intersection1.From);
            Assert.Equal<UInt64>(1, intersection1.To);
            
            range1 = new Range<UInt64>(0, 3);
            range2 = new Range<UInt64>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<UInt64>(1, intersection1.From);
            Assert.Equal<UInt64>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<Single> structure.
    /// </summary>
    public class RangeSingleTests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<Single>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<Single>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<Single>(0, 1);
            Assert.Equal<Single>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<Single>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<Single>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<Single>(0, 1).Normalize();
            Assert.Equal<Single>(0, range.From);
            Assert.Equal<Single>(1, range.To);

            range = new Range<Single>(1, 0).Normalize();
            Assert.Equal<Single>(0, range.From);
            Assert.Equal<Single>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<Single>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<Single>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<Single>(0, 0);
            var range2 = new Range<Single>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Single>(0, 1);
            range2 = new Range<Single>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<Single>(0, 1);
            range2 = new Range<Single>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<Single>(0, 3);
            range2 = new Range<Single>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<Single>(0, 0);
            var range2 = new Range<Single>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Single>(0, 1);
            range2 = new Range<Single>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<Single>(0, 1);
            range2 = new Range<Single>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Single>(0, intersection1.From);
            Assert.Equal<Single>(1, intersection1.To);
            
            range1 = new Range<Single>(0, 3);
            range2 = new Range<Single>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Single>(1, intersection1.From);
            Assert.Equal<Single>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<Double> structure.
    /// </summary>
    public class RangeDoubleTests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<Double>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<Double>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<Double>(0, 1);
            Assert.Equal<Double>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<Double>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<Double>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<Double>(0, 1).Normalize();
            Assert.Equal<Double>(0, range.From);
            Assert.Equal<Double>(1, range.To);

            range = new Range<Double>(1, 0).Normalize();
            Assert.Equal<Double>(0, range.From);
            Assert.Equal<Double>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<Double>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<Double>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<Double>(0, 0);
            var range2 = new Range<Double>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Double>(0, 1);
            range2 = new Range<Double>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<Double>(0, 1);
            range2 = new Range<Double>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<Double>(0, 3);
            range2 = new Range<Double>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<Double>(0, 0);
            var range2 = new Range<Double>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Double>(0, 1);
            range2 = new Range<Double>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<Double>(0, 1);
            range2 = new Range<Double>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Double>(0, intersection1.From);
            Assert.Equal<Double>(1, intersection1.To);
            
            range1 = new Range<Double>(0, 3);
            range2 = new Range<Double>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Double>(1, intersection1.From);
            Assert.Equal<Double>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<Decimal> structure.
    /// </summary>
    public class RangeDecimalTests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<Decimal>(0, 0).IsEmpty());
            Assert.Equal(false, new Range<Decimal>(0, 1).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<Decimal>(0, 1);
            Assert.Equal<Decimal>(1, range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<Decimal>(0, 1);
            Assert.Equal(true, range.IsNormalized());

            range = new Range<Decimal>(1, 0);
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<Decimal>(0, 1).Normalize();
            Assert.Equal<Decimal>(0, range.From);
            Assert.Equal<Decimal>(1, range.To);

            range = new Range<Decimal>(1, 0).Normalize();
            Assert.Equal<Decimal>(0, range.From);
            Assert.Equal<Decimal>(1, range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<Decimal>(1, 1);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(false, range.Contains(1));
            Assert.Equal(false, range.Contains(2));

            range = new Range<Decimal>(1, 2);
            Assert.Equal(false, range.Contains(0));
            Assert.Equal(true, range.Contains(1));
            Assert.Equal(false, range.Contains(2));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<Decimal>(0, 0);
            var range2 = new Range<Decimal>(0, 1);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<Decimal>(0, 1);
            range2 = new Range<Decimal>(1, 2);
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<Decimal>(0, 1);
            range2 = new Range<Decimal>(0, 1);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<Decimal>(0, 3);
            range2 = new Range<Decimal>(1, 2);
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<Decimal>(0, 0);
            var range2 = new Range<Decimal>(0, 1);
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<Decimal>(0, 1);
            range2 = new Range<Decimal>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<Decimal>(0, 1);
            range2 = new Range<Decimal>(0, 1);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Decimal>(0, intersection1.From);
            Assert.Equal<Decimal>(1, intersection1.To);
            
            range1 = new Range<Decimal>(0, 3);
            range2 = new Range<Decimal>(1, 2);
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<Decimal>(1, intersection1.From);
            Assert.Equal<Decimal>(2, intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<DateTime> structure.
    /// </summary>
    public class RangeDateTimeTests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/01/0001 00:00:00")).IsEmpty());
            Assert.Equal(false, new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00")).IsEmpty());
        }
        
        [Fact]
        public void Length()
        {
            var range = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            Assert.Equal<TimeSpan>(TimeSpan.Parse("1.00:00:00"), range.Length());
        }

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            Assert.Equal(true, range.IsNormalized());

            range = new Range<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), DateTime.Parse("01/01/0001 00:00:00"));
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00")).Normalize();
            Assert.Equal<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), range.From);
            Assert.Equal<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), range.To);

            range = new Range<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), DateTime.Parse("01/01/0001 00:00:00")).Normalize();
            Assert.Equal<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), range.From);
            Assert.Equal<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            Assert.Equal(false, range.Contains(DateTime.Parse("01/01/0001 00:00:00")));
            Assert.Equal(false, range.Contains(DateTime.Parse("01/02/0001 00:00:00")));
            Assert.Equal(false, range.Contains(DateTime.Parse("01/03/0001 00:00:00")));

            range = new Range<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), DateTime.Parse("01/03/0001 00:00:00"));
            Assert.Equal(false, range.Contains(DateTime.Parse("01/01/0001 00:00:00")));
            Assert.Equal(true, range.Contains(DateTime.Parse("01/02/0001 00:00:00")));
            Assert.Equal(false, range.Contains(DateTime.Parse("01/03/0001 00:00:00")));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/01/0001 00:00:00"));
            var range2 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            range2 = new Range<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), DateTime.Parse("01/03/0001 00:00:00"));
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            range2 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/04/0001 00:00:00"));
            range2 = new Range<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), DateTime.Parse("01/03/0001 00:00:00"));
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/01/0001 00:00:00"));
            var range2 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            range2 = new Range<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), DateTime.Parse("01/03/0001 00:00:00"));
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            range2 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/02/0001 00:00:00"));
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), intersection1.From);
            Assert.Equal<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), intersection1.To);
            
            range1 = new Range<DateTime>(DateTime.Parse("01/01/0001 00:00:00"), DateTime.Parse("01/04/0001 00:00:00"));
            range2 = new Range<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), DateTime.Parse("01/03/0001 00:00:00"));
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<DateTime>(DateTime.Parse("01/02/0001 00:00:00"), intersection1.From);
            Assert.Equal<DateTime>(DateTime.Parse("01/03/0001 00:00:00"), intersection1.To);
        }
    }

    /// <summary>
    /// Tests for the Range<String> structure.
    /// </summary>
    public class RangeStringTests
    {
        [Fact]
        public void IsEmpty()
        {
            Assert.Equal(true, new Range<String>("aa", "aa").IsEmpty());
            Assert.Equal(false, new Range<String>("aa", "ab").IsEmpty());
        }
        

        [Fact]
        public void IsNormalized()
        {
            var range = new Range<String>("aa", "ab");
            Assert.Equal(true, range.IsNormalized());

            range = new Range<String>("ab", "aa");
            Assert.Equal(false, range.IsNormalized());
        }

        [Fact]
        public void Normalize()
        {
            var range = new Range<String>("aa", "ab").Normalize();
            Assert.Equal<String>("aa", range.From);
            Assert.Equal<String>("ab", range.To);

            range = new Range<String>("ab", "aa").Normalize();
            Assert.Equal<String>("aa", range.From);
            Assert.Equal<String>("ab", range.To);
        }

        [Fact]
        public void Contains()
        {
            var range = new Range<String>("ab", "ab");
            Assert.Equal(false, range.Contains("aa"));
            Assert.Equal(false, range.Contains("ab"));
            Assert.Equal(false, range.Contains("ac"));

            range = new Range<String>("ab", "ac");
            Assert.Equal(false, range.Contains("aa"));
            Assert.Equal(true, range.Contains("ab"));
            Assert.Equal(false, range.Contains("ac"));
        }

        [Fact]
        public void Intersects()
        {
            var range1 = new Range<String>("aa", "aa");
            var range2 = new Range<String>("aa", "ab");
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));

            range1 = new Range<String>("aa", "ab");
            range2 = new Range<String>("ab", "ac");
            Assert.Equal(false, range1.Intersects(range2));
            Assert.Equal(false, range2.Intersects(range1));
            
            range1 = new Range<String>("aa", "ab");
            range2 = new Range<String>("aa", "ab");
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
            
            range1 = new Range<String>("aa", "ad");
            range2 = new Range<String>("ab", "ac");
            Assert.Equal(true, range1.Intersects(range2));
            Assert.Equal(true, range2.Intersects(range1));
        }

        [Fact]
        public void Intersect()
        {
            var range1 = new Range<String>("aa", "aa");
            var range2 = new Range<String>("aa", "ab");
            var intersection1 = range1.Intersect(range2);
            var intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());

            range1 = new Range<String>("aa", "ab");
            range2 = new Range<String>("ab", "ac");
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(true, intersection1.IsEmpty());
            Assert.Equal(true, intersection2.IsEmpty());
            
            range1 = new Range<String>("aa", "ab");
            range2 = new Range<String>("aa", "ab");
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<String>("aa", intersection1.From);
            Assert.Equal<String>("ab", intersection1.To);
            
            range1 = new Range<String>("aa", "ad");
            range2 = new Range<String>("ab", "ac");
            intersection1 = range1.Intersect(range2);
            intersection2 = range2.Intersect(range1);
            Assert.Equal(intersection1.From, intersection2.From);
            Assert.Equal(intersection1.To, intersection2.To);
            Assert.Equal<String>("ab", intersection1.From);
            Assert.Equal<String>("ac", intersection1.To);
        }
    }
}
