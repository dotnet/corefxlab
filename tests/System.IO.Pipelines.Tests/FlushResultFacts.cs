using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class FlushResultFacts
    {
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        [Theory]
        public void FlushResultCanBeConstructed(bool cancelled, bool completed)
        {
            var result = new FlushResult(cancelled, completed);

            Assert.Equal(cancelled, result.IsCancelled);
            Assert.Equal(completed, result.IsCompleted);
        }
    }
}
