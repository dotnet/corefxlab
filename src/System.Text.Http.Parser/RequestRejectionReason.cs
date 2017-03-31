// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Http.Parser
{
    internal enum RequestRejectionReason
    {
        UnrecognizedHTTPVersion,
        InvalidRequestLine,
        InvalidRequestHeader,
        InvalidRequestHeadersNoCRLF,
        MalformedRequestInvalidHeaders,
        InvalidContentLength,
        MultipleContentLengths,
        UnexpectedEndOfRequestContent,
        BadChunkSuffix,
        BadChunkSizeData,
        ChunkedRequestIncomplete,
        InvalidRequestTarget,
        InvalidCharactersInHeaderName,
        RequestLineTooLong,
        HeadersExceedMaxTotalSize,
        TooManyHeaders,
        RequestTimeout,
        FinalTransferCodingNotChunked,
        LengthRequired,
        LengthRequiredHttp10,
        OptionsMethodRequired,
        ConnectMethodRequired,
    }
}
