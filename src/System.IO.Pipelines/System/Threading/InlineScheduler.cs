// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.IO.Pipelines
{
    public class InlineScheduler : IScheduler
    {
        public static readonly InlineScheduler Default = new InlineScheduler();

        public void Schedule(Action<object> action, object state)
        {
            action(state);
        }
    }
}
