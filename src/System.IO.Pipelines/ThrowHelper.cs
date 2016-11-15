using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
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

        public static void ThrowArgumentOutOfRangeException_BufferRequestTooLarge(int maxSize)
        {
            throw GetArgumentOutOfRangeException_BufferRequestTooLarge(maxSize);
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
        public static NotSupportedException GetNotSupportedException()
        {
            return new NotSupportedException();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentNullException GetArgumentNullException(ExceptionArgument argument)
        {
            return new ArgumentNullException(GetArgumentName(argument));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ArgumentOutOfRangeException GetArgumentOutOfRangeException_BufferRequestTooLarge(int maxSize)
        {
            return new ArgumentOutOfRangeException(GetArgumentName(ExceptionArgument.size),
                $"Cannot allocate more than {maxSize} bytes in a single buffer");
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

            // Should be look up with enviorment resources
            string resourceString = null;
            switch (argument)
            {
                case ExceptionResource.AlreadyProducing:
                    resourceString = "Already producing.";
                    break;
                case ExceptionResource.NotProducingNoAlloc:
                    resourceString = "No ongoing producing operation. Make sure Alloc() was called.";
                    break;
                case ExceptionResource.NotProducingToComplete:
                    resourceString = "No ongoing producing operation to complete.";
                    break;
                case ExceptionResource.AlreadyConsuming:
                    resourceString = "Already consuming.";
                    break;
                case ExceptionResource.NotConsumingToComplete:
                    resourceString = "No ongoing consuming operation to complete.";
                    break;
                case ExceptionResource.NoConcurrentReads:
                    resourceString = "Concurrent reads are not supported.";
                    break;
                case ExceptionResource.GetResultNotCompleted:
                    resourceString = "can't GetResult unless completed";
                    break;

            }
            return resourceString ?? $"Error ResourceKey not defined {argument}.";
        }
    }

    internal enum ExceptionArgument
    {
        minimumSize,
        bytesWritten,
        destination,
        offset,
        length,
        data,
        size
    }

    internal enum ExceptionResource
    {
        AlreadyProducing,
        NotProducingNoAlloc,
        NotProducingToComplete,
        AlreadyConsuming,
        NotConsumingToComplete,
        NoConcurrentReads,
        GetResultNotCompleted
    }
}
