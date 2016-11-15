// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Defines a class that provides a connection from which data can be read from and written to.
    /// </summary>
    public interface IPipelineConnection : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IPipelineReader"/> half of the duplex connection.
        /// </summary>
        IPipelineReader Input { get; }

        /// <summary>
        /// Gets the <see cref="IPipelineWriter"/> half of the duplex connection.
        /// </summary>
        IPipelineWriter Output { get; }
    }
}
