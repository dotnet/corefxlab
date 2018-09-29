// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal static class JsonThrowHelper
    {
        public static void ThrowArgumentException(string message)
        {
            throw GetArgumentException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetArgumentException(string message)
        {
            return new ArgumentException(message);
        }

        public static void ThrowArgumentExceptionInvalidUtf8String()
        {
            throw GetArgumentExceptionInvalidUtf8String();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentException GetArgumentExceptionInvalidUtf8String()
        {
            return new ArgumentException("Invalid or incomplete UTF-8 string");
        }

        public static void ThrowFormatException()
        {
            throw GetFormatException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static FormatException GetFormatException()
        {
            return new FormatException();
        }

        public static void ThrowOutOfMemoryException()
        {
            throw GetOutOfMemoryException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static OutOfMemoryException GetOutOfMemoryException()
        {
            return new OutOfMemoryException();
        }

        public static void ThrowNotImplementedException()
        {
            throw GetNotImplementedException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static NotImplementedException GetNotImplementedException()
        {
            return new NotImplementedException("Reading JSON containing comments is not yet supported.");
        }

        public static void ThrowJsonReaderException(ref Utf8JsonReader json, ExceptionResource resource = ExceptionResource.Default, byte nextByte = default, ReadOnlySpan<byte> bytes = default)
        {
            GetJsonReaderException(ref json);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetJsonReaderException(ref Utf8JsonReader json)
        {
            var jsonInstrumented = new Instrumented.Utf8JsonReader(json._buffer, json._isFinalBlock,
                json._isRetry ? json.State : default)
            {
                MaxDepth = json.MaxDepth
            };
            while (jsonInstrumented.Read()) ;
            Debug.Assert(false, "We should never reach this point since we should have thrown JsonReaderException already.");
        }

        public static void ThrowJsonReaderException(ref Instrumented.Utf8JsonReader json, ExceptionResource resource = ExceptionResource.Default, byte nextByte = default, ReadOnlySpan<byte> bytes = default)
        {
            GetJsonReaderException(ref json, resource, nextByte, bytes);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetJsonReaderException(ref Instrumented.Utf8JsonReader json, ExceptionResource resource, byte nextByte, ReadOnlySpan<byte> bytes)
        {
            throw new JsonReaderException(GetResourceString(ref json, resource, (char)nextByte, Encoding.UTF8.GetString(bytes.ToArray())), json._lineNumber, json._position);
        }

        public static void ThrowInvalidCastException()
        {
            throw GetInvalidCastException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidCastException GetInvalidCastException()
        {
            return new InvalidCastException();
        }

        public static void ThrowKeyNotFoundException()
        {
            throw GetKeyNotFoundException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static KeyNotFoundException GetKeyNotFoundException()
        {
            return new KeyNotFoundException();
        }

        public static void ThrowInvalidOperationException(string message)
        {
            throw GetInvalidOperationException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidOperationException GetInvalidOperationException(string message)
        {
            return new InvalidOperationException(message);
        }

        public static void ThrowInvalidOperationException()
        {
            throw GetInvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static InvalidOperationException GetInvalidOperationException()
        {
            return new InvalidOperationException();
        }

        public static void ThrowIndexOutOfRangeException()
        {
            throw GetIndexOutOfRangeException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static IndexOutOfRangeException GetIndexOutOfRangeException()
        {
            return new IndexOutOfRangeException();
        }

        // This function will convert an ExceptionResource enum value to the resource string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ref Instrumented.Utf8JsonReader json, ExceptionResource resource, char character, string characters)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionResource), resource),
                "The enum value is not defined, please check the ExceptionResource Enum.");

            string formatString = ExceptionStrings.ResourceManager.GetString(resource.ToString());
            string message = formatString;
            switch (resource)
            {
                case ExceptionResource.ArrayDepthTooLarge:
                    message = string.Format(formatString, json.Depth, json.MaxDepth);
                    break;
                case ExceptionResource.ArrayEndWithinObject:
                    if (json.Depth <= 0)
                    {
                        formatString = ExceptionStrings.ResourceManager.GetString(ExceptionResource.DepthMustBePositive.ToString());
                        message = string.Format(formatString, json.Depth);
                    }
                    else
                    {
                        message = ExceptionStrings.ResourceManager.GetString(ExceptionResource.ArrayEndWithinObject.ToString());
                    }
                    break;
                case ExceptionResource.EndOfStringNotFound:
                    break;
                case ExceptionResource.ExpectedDigitNotFound:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.ExpectedDigitNotFoundEndOfData:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.ExpectedEndAfterSingleJson:
                    message = string.Format(formatString, json._buffer[json.CurrentIndex]);
                    break;
                case ExceptionResource.ExpectedEndOfDigitNotFound:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.ExpectedNextDigitComponentNotFound:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.ExpectedNextDigitEValueNotFound:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.ExpectedSeparaterAfterPropertyNameNotFound:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.ExpectedStartOfPropertyNotFound:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.ExpectedStartOfPropertyOrValueNotFound:
                    break;
                case ExceptionResource.ExpectedStartOfValueNotFound:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.ExpectedValueAfterPropertyNameNotFound:
                    break;
                case ExceptionResource.FoundInvalidCharacter:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.InvalidEndOfJson:
                    message = string.Format(formatString, json.TokenType);
                    break;
                case ExceptionResource.ObjectDepthTooLarge:
                    message = string.Format(formatString, json.Depth, json.MaxDepth);
                    break;
                case ExceptionResource.ObjectEndWithinArray:
                    if (json.Depth <= 0)
                    {
                        formatString = ExceptionStrings.ResourceManager.GetString(ExceptionResource.DepthMustBePositive.ToString());
                        message = string.Format(formatString, json.Depth);
                    }
                    else
                    {
                        message = ExceptionStrings.ResourceManager.GetString(ExceptionResource.ObjectEndWithinArray.ToString());
                    }
                    break;
                case ExceptionResource.Default:
                    break;
                case ExceptionResource.ExpectedFalse:
                    message = string.Format(formatString, characters);
                    break;
                case ExceptionResource.ExpectedNull:
                    message = string.Format(formatString, characters);
                    break;
                case ExceptionResource.ExpectedTrue:
                    message = string.Format(formatString, characters);
                    break;
            }

            return message;
        }

        internal enum ExceptionResource
        {
            ArrayDepthTooLarge,
            ArrayEndWithinObject,
            Default,
            DepthMustBePositive,
            EndOfStringNotFound,
            ExpectedDigitNotFound,
            ExpectedDigitNotFoundEndOfData,
            ExpectedEndAfterSingleJson,
            ExpectedEndOfDigitNotFound,
            ExpectedFalse,
            ExpectedNextDigitComponentNotFound,
            ExpectedNextDigitEValueNotFound,
            ExpectedNull,
            ExpectedSeparaterAfterPropertyNameNotFound,
            ExpectedStartOfPropertyNotFound,
            ExpectedStartOfPropertyOrValueNotFound,
            ExpectedStartOfValueNotFound,
            ExpectedTrue,
            ExpectedValueAfterPropertyNameNotFound,
            FoundInvalidCharacter,
            InvalidEndOfJson,
            ObjectDepthTooLarge,
            ObjectEndWithinArray,
        }
    }
}
