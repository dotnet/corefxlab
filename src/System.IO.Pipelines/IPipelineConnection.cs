using System;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Defines a class that provides a duplex channel from which data can be read from and written to.
    /// </summary>
    public interface IPipelineConnection : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IPipelineReader"/> half of the duplex channel.
        /// </summary>
        IPipelineReader Input { get; }

        /// <summary>
        /// Gets the <see cref="IPipelineWriter"/> half of the duplex channel.
        /// </summary>
        IPipelineWriter Output { get; }
    }
}
