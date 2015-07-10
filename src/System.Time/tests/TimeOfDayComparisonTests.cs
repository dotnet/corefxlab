// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace System.Time.Tests
{
    public class TimeOfDayComparisonTests
    {
        [Fact]
        public void CanCompareTimes_Static_Before()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var c = TimeOfDay.Compare(t1, t2);

            Assert.Equal(-1, c);
        }

        [Fact]
        public void CanCompareTimes_Static_Equal()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var c = TimeOfDay.Compare(t1, t2);

            Assert.Equal(0, c);
        }

        [Fact]
        public void CanCompareTimes_Static_After()
        {
            var t1 = new TimeOfDay(0, 1);
            var t2 = new TimeOfDay(0, 0);

            var c = TimeOfDay.Compare(t1, t2);

            Assert.Equal(1, c);
        }

        [Fact]
        public void CanCompareTimes_Instance_Before()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var c = t1.CompareTo(t2);

            Assert.Equal(-1, c);
        }

        [Fact]
        public void CanCompareTimes_Instance_Equal()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var c = t1.CompareTo(t2);

            Assert.Equal(0, c);
        }

        [Fact]
        public void CanCompareTimes_Instance_After()
        {
            var t1 = new TimeOfDay(0, 1);
            var t2 = new TimeOfDay(0, 0);

            var c = t1.CompareTo(t2);

            Assert.Equal(1, c);
        }

        [Fact]
        public void CanCompareTimes_Object_Before()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var c = t1.CompareTo((object)t2);

            Assert.Equal(-1, c);
        }

        [Fact]
        public void CanCompareTimes_Object_Equal()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var c = t1.CompareTo((object)t2);

            Assert.Equal(0, c);
        }

        [Fact]
        public void CanCompareTimes_Object_Null()
        {
            var t = new TimeOfDay();
            var c = t.CompareTo(null);
            Assert.Equal(1, c);
        }

        [Fact]
        public void CannotCompareTimes_Object_NonTime()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var c = new TimeOfDay().CompareTo(0);
            });
        }

        [Fact]
        public void CanEquateTimes_Static_Equal()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var b = TimeOfDay.Equals(t1, t2);

            Assert.True(b);
        }

        [Fact]
        public void CanEquateTimes_Static_NotEqual()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = TimeOfDay.Equals(t1, t2);

            Assert.False(b);
        }

        [Fact]
        public void CanEquateTimes_Instance_Equal()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var b = t1.Equals(t2);

            Assert.True(b);
        }

        [Fact]
        public void CanEquateTimes_Instance_NotEqual()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = t1.Equals(t2);

            Assert.False(b);
        }

        [Fact]
        public void CanEquateTimes_Object_Equal()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var b = t1.Equals((object)t2);

            Assert.True(b);
        }

        [Fact]
        public void CanEquateTimes_Object_NotEqual()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = t1.Equals((object)t2);

            Assert.False(b);
        }

        [Fact]
        public void CanEquateTimes_Object_Null()
        {
            var t = new TimeOfDay();
            var b = t.Equals(null);
            Assert.False(b);
        }

        [Fact]
        public void CanEquateTimes_Object_NonTime()
        {
            var t = new TimeOfDay();
            var b = t.Equals(0);
            Assert.False(b);
        }
        
        [Fact]
        public void CanCompareTimes_UsingOperator_LT1()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = t1 < t2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_LT2()
        {
            var t1 = new TimeOfDay(0, 1);
            var t2 = new TimeOfDay(0, 0);

            var b = t1 < t2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_LTE1()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = t1 <= t2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_LTE2()
        {
            var t1 = new TimeOfDay(0, 1);
            var t2 = new TimeOfDay(0, 0);

            var b = t1 <= t2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_LTE3()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var b = t1 <= t2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_EQ1()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var b = t1 == t2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_EQ2()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = t1 == t2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_NE1()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = t1 != t2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_NE2()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var b = t1 != t2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_GTE1()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 0);

            var b = t1 >= t2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_GTE2()
        {
            var t1 = new TimeOfDay(0, 1);
            var t2 = new TimeOfDay(0, 0);

            var b = t1 >= t2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_GTE3()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = t1 >= t2;

            Assert.False(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_GT1()
        {
            var t1 = new TimeOfDay(0, 1);
            var t2 = new TimeOfDay(0, 0);

            var b = t1 > t2;

            Assert.True(b);
        }

        [Fact]
        public void CanCompareTimes_UsingOperator_GT2()
        {
            var t1 = new TimeOfDay(0, 0);
            var t2 = new TimeOfDay(0, 1);

            var b = t1 > t2;

            Assert.False(b);
        }


        [Fact]
        public void CanDetermineTimeInBetween_NormalInside()
        {
            TimeOfDay startTime = new TimeOfDay(10, 0);
            TimeOfDay testTime = new TimeOfDay(10, 0);
            TimeOfDay endTime = new TimeOfDay(12, 0);

            bool between = testTime.IsBetween(startTime, endTime);
            Assert.True(between);
        }

        [Fact]
        public void CanDetermineTimeInBetween_NormalBefore()
        {
            TimeOfDay testTime = new TimeOfDay(9, 0);
            TimeOfDay startTime = new TimeOfDay(10, 0);
            TimeOfDay endTime = new TimeOfDay(12, 0);

            bool between = testTime.IsBetween(startTime, endTime);
            Assert.False(between);
        }

        [Fact]
        public void CanDetermineTimeInBetween_NormalAfter()
        {
            TimeOfDay startTime = new TimeOfDay(10, 0);
            TimeOfDay endTime = new TimeOfDay(12, 0);
            TimeOfDay testTime = new TimeOfDay(12, 0);

            bool between = testTime.IsBetween(startTime, endTime);
            Assert.False(between);
        }

        [Fact]
        public void CanDetermineTimeInBetween_OverMidnightInside()
        {
            TimeOfDay startTime = new TimeOfDay(23, 0);
            TimeOfDay testTime = new TimeOfDay(23, 0);
            TimeOfDay endTime = new TimeOfDay(1, 0);

            bool between = testTime.IsBetween(startTime, endTime);
            Assert.True(between);
        }

        [Fact]
        public void CanDetermineTimeInBetween_OverMidnightBefore()
        {
            TimeOfDay testTime = new TimeOfDay(22, 0);
            TimeOfDay startTime = new TimeOfDay(23, 0);
            TimeOfDay endTime = new TimeOfDay(1, 0);

            bool between = testTime.IsBetween(startTime, endTime);
            Assert.False(between);
        }

        [Fact]
        public void CanDetermineTimeInBetween_OverMidnightAfter()
        {
            TimeOfDay startTime = new TimeOfDay(23, 0);
            TimeOfDay endTime = new TimeOfDay(1, 0);
            TimeOfDay testTime = new TimeOfDay(1, 0);

            bool between = testTime.IsBetween(startTime, endTime);
            Assert.False(between);
        }
    }
}
