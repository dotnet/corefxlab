// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    internal class ThrowHelper
    {
        public static void ThrowArgumentOutOfRangeException(ExceptionArgument argument)
        {
            throw GetArgumentOutOfRangeException(argument);
        }

        public static void ThrowInvalidOperationException(ExceptionResource resource)
        {
            throw GetInvalidOperationException(resource);
        }

        public static void ThrowArgumentNullException(ExceptionArgument argument)
        {
            throw GetArgumentNullException(argument);
        }

        public static void ThrowNotSupportedException()
        {
            throw GetNotSupportedException();
        }
        
        public static void ThrowCursorOutOfBoundsException()
        {
            throw GetCursorOutOfBoundsException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(argument));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static InvalidOperationException GetInvalidOperationException(ExceptionResource resource)
        {
            return new InvalidOperationException(GetResourceString(resource));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static NotSupportedException GetNotSupportedException()
        {
            return new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentNullException GetArgumentNullException(ExceptionArgument argument)
        {
            return new ArgumentNullException(GetArgumentName(argument));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception GetCursorOutOfBoundsException()
        {
            return new InvalidOperationException("Cursor is out of bounds");
        }

        private static string GetArgumentName(ExceptionArgument argument)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionArgument), argument),
                "The enum value is not defined, please check the ExceptionArgument Enum.");

            return argument.ToString();
        }

        private static string GetResourceString(ExceptionResource argument)
        {
            Debug.Assert(Enum.IsDefined(typeof(ExceptionResource), argument),
                "The enum value is not defined, please check the ExceptionResource Enum.");

            // Should be look up with environment resources
            string resourceString = null;
            switch (argument)
            {
                case ExceptionResource.UnexpectedSegmentType:
                    resourceString = "Unexpected segment type";
                    break;
                case ExceptionResource.EndCursorNotReached:
                    resourceString = "Segment chain ended without reaching end cursor location";
                    break;
            }

            resourceString = resourceString ?? $"Error ResourceKey not defined {argument}.";

            return resourceString;
        }
    }

    internal enum ExceptionArgument
    {
        destination,
        offset,
        length,
        data,
        size
    }

    internal enum ExceptionResource
    {
        UnexpectedSegmentType,
        EndCursorNotReached
    }
}
