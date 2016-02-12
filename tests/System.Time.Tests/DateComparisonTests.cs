// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Time.Tests
{
    public class DateComparisonTests
    {
        [Fact]
        public void CanCompareDates_Static_Before()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 2);

            var c = Date.Compare(d1, d2);

            Assert.Equal(-1, c);
        }

        [Fact]
        public void CanCompareDates_Static_Equal()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var c = Date.Compare(d1, d2);

            Assert.Equal(0, c);
        }

        [Fact]
        public void CanCompareDates_Static_After()
        {
            var d1 = new Date(2000, 1, 2);
            var d2 = new Date(2000, 1, 1);

            var c = Date.Compare(d1, d2);

            Assert.Equal(1, c);
        }

        [Fact]
        public void CanCompareDates_Instance_Before()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 2);

            var c = d1.CompareTo(d2);

            Assert.Equal(-1, c);
        }

        [Fact]
        public void CanCompareDates_Instance_Equal()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var c = d1.CompareTo(d2);

            Assert.Equal(0, c);
        }

        [Fact]
        public void CanCompareDates_Instance_After()
        {
            var d1 = new Date(2000, 1, 2);
            var d2 = new Date(2000, 1, 1);

            var c = d1.CompareTo(d2);

            Assert.Equal(1, c);
        }

        [Fact]
        public void CanCompareDates_Object_Before()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 2);

            var c = d1.CompareTo((object)d2);

            Assert.Equal(-1, c);
        }

        [Fact]
        public void CanCompareDates_Object_Equal()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var c = d1.CompareTo((object)d2);

            Assert.Equal(0, c);
        }

        [Fact]
        public void CanCompareDates_Object_Null()
        {
            var d = new Date();
            var c = d.CompareTo(null);
            Assert.Equal(1, c);
        }

        [Fact]
        public void CannotCompareDates_Object_NonDate()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var c = new Date().CompareTo(0);
            });
        }

        [Fact]
        public void CanEquateDates_Static_Equal()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = Date.Equals(d1, d2);

            Assert.True(b);
        }

        [Fact]
        public void CanEquateDates_Static_NotEqual()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = Date.Equals(d1, d2);

            Assert.False(b);
        }

        [Fact]
        public void CanEquateDates_Instance_Equal()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1.Equals(d2);

            Assert.True(b);
        }

        [Fact]
        public void CanEquateDates_Instance_NotEqual()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = d1.Equals(d2);

            Assert.False(b);
        }

        [Fact]
        public void CanEquateDates_Object_Equal()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1.Equals((object)d2);

            Assert.True(b);
        }

        [Fact]
        public void CanEquateDates_Object_NotEqual()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = d1.Equals((object)d2);

            Assert.False(b);
        }

        [Fact]
        public void CanEquateDates_Object_Null()
        {
            var d = new Date();
            var b = d.Equals(null);
            Assert.False(b);
        }

        [Fact]
        public void CanEquateDates_Object_NonTime()
        {
            var d = new Date();
            var b = d.Equals(0);
            Assert.False(b);
        }


        [Fact]
        public void CanCompareDates_UsingOperator_LT1()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = d1 < d2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_LT2()
        {
            var d1 = new Date(2001, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1 < d2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_LTE1()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = d1 <= d2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_LTE2()
        {
            var d1 = new Date(2001, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1 <= d2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_LTE3()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1 <= d2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_EQ1()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1 == d2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_EQ2()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = d1 == d2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_NE1()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = d1 != d2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_NE2()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1 != d2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_GTE1()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1 >= d2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_GTE2()
        {
            var d1 = new Date(2001, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1 >= d2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_GTE3()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = d1 >= d2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_GT1()
        {
            var d1 = new Date(2001, 1, 1);
            var d2 = new Date(2000, 1, 1);

            var b = d1 > d2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareDates_UsingOperator_GT2()
        {
            var d1 = new Date(2000, 1, 1);
            var d2 = new Date(2001, 1, 1);

            var b = d1 > d2;

            Assert.False(b);
        }

        [Fact]
        public void CanDetermineDateInBetween_Start()
        {
            Date startDate = new Date(2000, 1, 1);
            Date testDate = new Date(2000, 1, 1);
            Date endDate = new Date(2000, 1, 2);

            bool between = testDate.IsBetween(startDate, endDate);
            Assert.True(between);
        }

        [Fact]
        public void CanDetermineDateInBetween_Inside()
        {
            Date startDate = new Date(2000, 1, 1);
            Date testDate = new Date(2000, 1, 2);
            Date endDate = new Date(2000, 1, 3);

            bool between = testDate.IsBetween(startDate, endDate);
            Assert.True(between);
        }

        [Fact]
        public void CanDetermineDateInBetween_Before()
        {
            Date testDate = new Date(2000, 1, 1);
            Date startDate = new Date(2000, 1, 2);
            Date endDate = new Date(2000, 1, 3);

            bool between = testDate.IsBetween(startDate, endDate);
            Assert.False(between);
        }

        [Fact]
        public void CanDetermineDateInBetween_After()
        {
            Date startDate = new Date(2000, 1, 1);
            Date endDate = new Date(2000, 1, 2);
            Date testDate = new Date(2000, 1, 3);

            bool between = testDate.IsBetween(startDate, endDate);
            Assert.False(between);
        }

        [Fact]
        public void CanDetermineDateInBetween_InclusiveAfter()
        {
            Date startDate = new Date(2000, 1, 1);
            Date endDate = new Date(2000, 1, 2);
            Date testDate = new Date(2000, 1, 2);

            bool between = testDate.IsBetween(startDate, endDate);
            Assert.True(between);
        }
        
        [Fact]
        public void CanDetermineDateInBetween_ExclusiveAfter()
        {
            Date startDate = new Date(2000, 1, 1);
            Date endDate = new Date(2000, 1, 2);
            Date testDate = new Date(2000, 1, 2);

            bool between = testDate.IsBetween(startDate, endDate, true);
            Assert.False(between);
        }

        
        
    }
}
