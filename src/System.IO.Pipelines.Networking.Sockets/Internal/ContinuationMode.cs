// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines.Networking.Sockets.Internal
{
    /// <summary>
    /// Used by Signal to control how callbacks are invoked
    /// </summary>
    internal enum ContinuationMode
    {
        Synchronous,
        ThreadPool,
        // TODO: sync-context? but if so: whose? the .Current at creation? at SetResult?
    }
}
