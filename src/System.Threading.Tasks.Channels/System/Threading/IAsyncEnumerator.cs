// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks.Channels
{
    /// <summary>Represents an enumerator accessed asynchronously.</summary>
    /// <typeparam name="T">Specifies the type of the data enumerated.</typeparam>
    public interface IAsyncEnumerator<out T>
    {
        /// <summary>Asynchronously move the enumerator to the next element.</summary>
        /// <returns>
        /// A task that returns true if the enumerator was successfully advanced to the next item, 
        /// or false if no more data was available in the collection.
        /// </returns>
        Task<bool> MoveNextAsync();

        /// <summary>Gets the current element being enumerated.</summary>
        T Current { get; }
    }
}
