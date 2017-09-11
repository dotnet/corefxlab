// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Threading
{
    public interface IScheduler
    {
        void Schedule(Action<object> action, object state);
    }
}
