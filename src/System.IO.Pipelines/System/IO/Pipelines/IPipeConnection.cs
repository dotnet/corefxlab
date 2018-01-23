// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    /// <summary>
    /// Defines a class that provides a connection from which data can be read from and written to.
    /// </summary>
    public interface IPipeConnection : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="PipeReader"/> half of the duplex connection.
        /// </summary>
        PipeReader Input { get; }

        /// <summary>
        /// Gets the <see cref="PipeWriter"/> half of the duplex connection.
        /// </summary>
        PipeWriter Output { get; }
    }
}
