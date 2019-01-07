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

        public static void ThrowArgumentException(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            GetArgumentException(propertyName, value);
        }
        public static void ThrowArgumentException(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value)
        {
            GetArgumentException(propertyName, value);
        }
        public static void ThrowArgumentException(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
        {
            GetArgumentException(propertyName, value);
        }
        public static void ThrowArgumentException(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
        {
            GetArgumentException(propertyName, value);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetArgumentException(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
            if (propertyName.Length > JsonConstants.MaxTokenSize)
            {
                ThrowArgumentException("propertyName too large");
            }
            else
            {
                Debug.Assert(value.Length > JsonConstants.MaxTokenSize);
                ThrowArgumentException("value too large");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetArgumentException(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value)
        {
            if (propertyName.Length > JsonConstants.MaxTokenSize)
            {
                ThrowArgumentException("propertyName too large");
            }
            else
            {
                Debug.Assert(value.Length > JsonConstants.MaxCharacterTokenSize);
                ThrowArgumentException("value too large");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetArgumentException(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
        {
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize)
            {
                ThrowArgumentException("propertyName too large");
            }
            else
            {
                Debug.Assert(value.Length > JsonConstants.MaxTokenSize);
                ThrowArgumentException("value too large");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetArgumentException(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
        {
            if (propertyName.Length > JsonConstants.MaxCharacterTokenSize)
            {
                ThrowArgumentException("propertyName too large");
            }
            else
            {
                Debug.Assert(value.Length > JsonConstants.MaxCharacterTokenSize);
                ThrowArgumentException("value too large");
            }
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

        public static void ThrowFormatException(byte token)
        {
            throw GetFormatException(token);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static FormatException GetFormatException(byte token)
        {
            return new FormatException(token.ToString());
        }

        public static void ThrowJsonWriterOrArgumentException(ReadOnlySpan<byte> propertyName, int indent)
        {
            GetJsonWriterOrArgumentException(propertyName, indent);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetJsonWriterOrArgumentException(ReadOnlySpan<byte> propertyName, int indent)
        {
            if ((indent & JsonConstants.RemoveFlagsBitMask) >= JsonConstants.MaxWriterDepth)
            {
                ThrowJsonWriterException("Depth too large.");
            }
            else
            {
                Debug.Assert(propertyName.Length > JsonConstants.MaxCharacterTokenSize);
                ThrowArgumentException("Argument too large.");
            }
        }

        public static void ThrowJsonWriterOrArgumentException(ReadOnlySpan<char> propertyName, int indent)
        {
            GetJsonWriterOrArgumentException(propertyName, indent);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetJsonWriterOrArgumentException(ReadOnlySpan<char> propertyName, int indent)
        {
            if ((indent & JsonConstants.RemoveFlagsBitMask) >= JsonConstants.MaxWriterDepth)
            {
                ThrowJsonWriterException("Depth too large.");
            }
            else
            {
                Debug.Assert(propertyName.Length > JsonConstants.MaxCharacterTokenSize);
                ThrowArgumentException("Argument too large.");
            }
        }

        public static void ThrowJsonWriterException(string message)
        {
            throw GetJsonWriterException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static JsonWriterException GetJsonWriterException(string message)
        {
            return new JsonWriterException(message);
        }

        public static void ThrowJsonWriterException(byte token)
        {
            throw GetJsonWriterException(token);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static JsonWriterException GetJsonWriterException(byte token)
        {
            return new JsonWriterException(token.ToString());
        }

        public static void ThrowJsonWriterException(byte token, JsonTokenType tokenType)
        {
            throw GetJsonWriterException(token, tokenType);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static JsonWriterException GetJsonWriterException(byte token, JsonTokenType tokenType)
        {
            // TODO: Fix exception message
            return new JsonWriterException(token.ToString());
        }

        public static void ThrowJsonWriterException(JsonTokenType tokenType)
        {
            throw GetJsonWriterException(tokenType);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static JsonWriterException GetJsonWriterException(JsonTokenType tokenType)
        {
            // TODO: Fix exception message
            return new JsonWriterException("");
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

        public static void ThrowJsonReaderException(ref Utf8Json.Reader json, ExceptionResource resource = ExceptionResource.Default, byte nextByte = default, ReadOnlySpan<byte> bytes = default)
        {
            GetJsonReaderException(ref json, resource, nextByte, bytes);
        }

        public static void ThrowJsonReaderException(ref JsonUtf8Reader json, ExceptionResource resource = ExceptionResource.Default, byte nextByte = default, ReadOnlySpan<byte> bytes = default)
        {
            GetJsonReaderException(ref json, resource, nextByte, bytes);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetJsonReaderException(ref JsonUtf8Reader json, ExceptionResource resource, byte nextByte, ReadOnlySpan<byte> bytes)
        {
            string message = GetResourceString(ref json, resource, (char)nextByte, Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Length));
            message += $" LineNumber: {json._lineNumber} | BytePosition: {json._position}.";
            throw new JsonReaderException(message, json._lineNumber, json._position);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void GetJsonReaderException(ref Utf8Json.Reader json, ExceptionResource resource, byte nextByte, ReadOnlySpan<byte> bytes)
        {
            string message = GetResourceString(ref json, resource, (char)nextByte, Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Length));
            throw new JsonReaderException(message, json._utf8Json._state._lineNumber, json._utf8Json._state._position);
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
        private static string GetResourceString(ref JsonUtf8Reader json, ExceptionResource resource, char character, string characters)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionResource), resource),
                "The enum value is not defined, please check the ExceptionResource Enum.");

            string formatString = ExceptionStrings.ResourceManager.GetString(resource.ToString());
            string message = formatString;
            switch (resource)
            {
                case ExceptionResource.ArrayDepthTooLarge:
                    message = string.Format(formatString, json.CurrentDepth, json.MaxDepth);
                    break;
                case ExceptionResource.ArrayEndWithinObject:
                    if (json.CurrentDepth <= 0)
                    {
                        formatString = ExceptionStrings.ResourceManager.GetString(ExceptionResource.DepthMustBePositive.ToString());
                        message = string.Format(formatString, json.CurrentDepth);
                    }
                    else
                    {
                        message = string.Format(formatString);
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
                    message = string.Format(formatString, character);
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
                    message = string.Format(formatString, json.CurrentDepth, json.MaxDepth);
                    break;
                case ExceptionResource.ObjectEndWithinArray:
                    if (json.CurrentDepth <= 0)
                    {
                        formatString = ExceptionStrings.ResourceManager.GetString(ExceptionResource.DepthMustBePositive.ToString());
                        message = string.Format(formatString, json.CurrentDepth);
                    }
                    else
                    {
                        message = string.Format(formatString);
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
                // This case is covered between ArrayEndWithinObject and ObjectEndWithinArray
                /*case ExceptionResource.DepthMustBePositive:
                    break;*/
                case ExceptionResource.InvalidCharacterWithinString:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.EndOfCommentNotFound:
                    break;
            }

            return message;
        }

        // This function will convert an ExceptionResource enum value to the resource string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ref Utf8Json.Reader json, ExceptionResource resource, char character, string characters)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionResource), resource),
                "The enum value is not defined, please check the ExceptionResource Enum.");

            string formatString = ExceptionStrings.ResourceManager.GetString(resource.ToString());
            string message = formatString;
            switch (resource)
            {
                case ExceptionResource.ArrayDepthTooLarge:
                    message = string.Format(formatString, json.CurrentDepth, json._utf8Json.MaxDepth);
                    break;
                case ExceptionResource.ArrayEndWithinObject:
                    if (json.CurrentDepth <= 0)
                    {
                        formatString = ExceptionStrings.ResourceManager.GetString(ExceptionResource.DepthMustBePositive.ToString());
                        message = string.Format(formatString, json.CurrentDepth);
                    }
                    else
                    {
                        message = string.Format(formatString);
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
                    message = string.Format(formatString, character);
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
                    message = string.Format(formatString, json.CurrentDepth, json._utf8Json.MaxDepth);
                    break;
                case ExceptionResource.ObjectEndWithinArray:
                    if (json.CurrentDepth <= 0)
                    {
                        formatString = ExceptionStrings.ResourceManager.GetString(ExceptionResource.DepthMustBePositive.ToString());
                        message = string.Format(formatString, json.CurrentDepth);
                    }
                    else
                    {
                        message = string.Format(formatString);
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
                // This case is covered between ArrayEndWithinObject and ObjectEndWithinArray
                /*case ExceptionResource.DepthMustBePositive:
                    break;*/
                case ExceptionResource.InvalidCharacterWithinString:
                    message = string.Format(formatString, character);
                    break;
                case ExceptionResource.EndOfCommentNotFound:
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
            EndOfCommentNotFound,
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
            InvalidCharacterWithinString,
            InvalidEndOfJson,
            ObjectDepthTooLarge,
            ObjectEndWithinArray,
        }
    }
}
