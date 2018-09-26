// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace System.Text.Http.Parser
{
    internal static class StatusCodes
    {
        public const int Status100Continue = 100;
        public const int Status412PreconditionFailed = 412;
        public const int Status413RequestEntityTooLarge = 413;
        public const int Status413PayloadTooLarge = 413;
        public const int Status414RequestUriTooLong = 414;
        public const int Status414UriTooLong = 414;
        public const int Status415UnsupportedMediaType = 415;
        public const int Status416RequestedRangeNotSatisfiable = 416;
        public const int Status416RangeNotSatisfiable = 416;
        public const int Status417ExpectationFailed = 417;
        public const int Status418ImATeapot = 418;
        public const int Status419AuthenticationTimeout = 419;
        public const int Status421MisdirectedRequest = 421;
        public const int Status422UnprocessableEntity = 422;
        public const int Status423Locked = 423;
        public const int Status424FailedDependency = 424;
        public const int Status426UpgradeRequired = 426;
        public const int Status428PreconditionRequired = 428;
        public const int Status429TooManyRequests = 429;
        public const int Status431RequestHeaderFieldsTooLarge = 431;
        public const int Status451UnavailableForLegalReasons = 451;
        public const int Status500InternalServerError = 500;
        public const int Status501NotImplemented = 501;
        public const int Status502BadGateway = 502;
        public const int Status503ServiceUnavailable = 503;
        public const int Status504GatewayTimeout = 504;
        public const int Status505HttpVersionNotsupported = 505;
        public const int Status506VariantAlsoNegotiates = 506;
        public const int Status507InsufficientStorage = 507;
        public const int Status508LoopDetected = 508;
        public const int Status411LengthRequired = 411;
        public const int Status510NotExtended = 510;
        public const int Status410Gone = 410;
        public const int Status408RequestTimeout = 408;
        public const int Status101SwitchingProtocols = 101;
        public const int Status102Processing = 102;
        public const int Status200OK = 200;
        public const int Status201Created = 201;
        public const int Status202Accepted = 202;
        public const int Status203NonAuthoritative = 203;
        public const int Status204NoContent = 204;
        public const int Status205ResetContent = 205;
        public const int Status206PartialContent = 206;
        public const int Status207MultiStatus = 207;
        public const int Status208AlreadyReported = 208;
        public const int Status226IMUsed = 226;
        public const int Status300MultipleChoices = 300;
        public const int Status301MovedPermanently = 301;
        public const int Status302Found = 302;
        public const int Status303SeeOther = 303;
        public const int Status304NotModified = 304;
        public const int Status305UseProxy = 305;
        public const int Status306SwitchProxy = 306;
        public const int Status307TemporaryRedirect = 307;
        public const int Status308PermanentRedirect = 308;
        public const int Status400BadRequest = 400;
        public const int Status401Unauthorized = 401;
        public const int Status402PaymentRequired = 402;
        public const int Status403Forbidden = 403;
        public const int Status404NotFound = 404;
        public const int Status405MethodNotAllowed = 405;
        public const int Status406NotAcceptable = 406;
        public const int Status407ProxyAuthenticationRequired = 407;
        public const int Status409Conflict = 409;
        public const int Status511NetworkAuthenticationRequired = 511;
    }

    public sealed class BadHttpRequestException : IOException
    {
        private BadHttpRequestException(string message, int statusCode)
            : this(message, statusCode, null)
        { }

        private BadHttpRequestException(string message, int statusCode, Http.Method? requiredMethod)
            : base(message)
        {
            StatusCode = statusCode;

            if (requiredMethod.HasValue)
            {
                // TODO: Removed. Verify that it's ok.
                //AllowedHeader = HttpUtilities.MethodToString(requiredMethod.Value);
            }
        }

        internal int StatusCode { get; }

        // TODO: Removed. Verify that it's ok.
        //internal StringValues AllowedHeader { get; }

        internal static BadHttpRequestException GetException(RequestRejectionReason reason)
        {
            BadHttpRequestException ex;
            switch (reason)
            {
                case RequestRejectionReason.InvalidRequestHeadersNoCRLF:
                    ex = new BadHttpRequestException("Invalid request headers: missing final CRLF in header fields.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.InvalidRequestLine:
                    ex = new BadHttpRequestException("Invalid request line.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.MalformedRequestInvalidHeaders:
                    ex = new BadHttpRequestException("Malformed request: invalid headers.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.MultipleContentLengths:
                    ex = new BadHttpRequestException("Multiple Content-Length headers.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.UnexpectedEndOfRequestContent:
                    ex = new BadHttpRequestException("Unexpected end of request content.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.BadChunkSuffix:
                    ex = new BadHttpRequestException("Bad chunk suffix.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.BadChunkSizeData:
                    ex = new BadHttpRequestException("Bad chunk size data.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.ChunkedRequestIncomplete:
                    ex = new BadHttpRequestException("Chunked request incomplete.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.InvalidCharactersInHeaderName:
                    ex = new BadHttpRequestException("Invalid characters in header name.", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.RequestLineTooLong:
                    ex = new BadHttpRequestException("Request line too long.", StatusCodes.Status414UriTooLong);
                    break;
                case RequestRejectionReason.HeadersExceedMaxTotalSize:
                    ex = new BadHttpRequestException("Request headers too long.", StatusCodes.Status431RequestHeaderFieldsTooLarge);
                    break;
                case RequestRejectionReason.TooManyHeaders:
                    ex = new BadHttpRequestException("Request contains too many headers.", StatusCodes.Status431RequestHeaderFieldsTooLarge);
                    break;
                case RequestRejectionReason.RequestTimeout:
                    ex = new BadHttpRequestException("Request timed out.", StatusCodes.Status408RequestTimeout);
                    break;
                case RequestRejectionReason.OptionsMethodRequired:
                    ex = new BadHttpRequestException("Method not allowed.", StatusCodes.Status405MethodNotAllowed, Http.Method.Options);
                    break;
                case RequestRejectionReason.ConnectMethodRequired:
                    ex = new BadHttpRequestException("Method not allowed.", StatusCodes.Status405MethodNotAllowed, Http.Method.Connect);
                    break;
                default:
                    ex = new BadHttpRequestException("Bad request.", StatusCodes.Status400BadRequest);
                    break;
            }
            return ex;
        }

        internal static BadHttpRequestException GetException(RequestRejectionReason reason, string detail)
        {
            BadHttpRequestException ex;
            switch (reason)
            {
                case RequestRejectionReason.InvalidRequestLine:
                    ex = new BadHttpRequestException($"Invalid request line: '{detail}'", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.InvalidRequestTarget:
                    ex = new BadHttpRequestException($"Invalid request target: '{detail}'", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.InvalidRequestHeader:
                    ex = new BadHttpRequestException($"Invalid request header: '{detail}'", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.InvalidContentLength:
                    ex = new BadHttpRequestException($"Invalid content length: {detail}", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.UnrecognizedHTTPVersion:
                    ex = new BadHttpRequestException($"Unrecognized HTTP version: '{detail}'", StatusCodes.Status505HttpVersionNotsupported);
                    break;
                case RequestRejectionReason.FinalTransferCodingNotChunked:
                    ex = new BadHttpRequestException($"Final transfer coding is not \"chunked\": \"{detail}\"", StatusCodes.Status400BadRequest);
                    break;
                case RequestRejectionReason.LengthRequired:
                    ex = new BadHttpRequestException($"{detail} request contains no Content-Length or Transfer-Encoding header", StatusCodes.Status411LengthRequired);
                    break;
                case RequestRejectionReason.LengthRequiredHttp10:
                    ex = new BadHttpRequestException($"{detail} request contains no Content-Length header", StatusCodes.Status400BadRequest);
                    break;
                default:
                    ex = new BadHttpRequestException("Bad request.", StatusCodes.Status400BadRequest);
                    break;
            }
            return ex;
        }
    }
}
