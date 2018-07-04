// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class NotSupportedEventKindException : GpioException
    {
        public NotSupportedEventKindException(EventKind kind)
            : base($"Not supported GPIO event kind '{kind}'")
        {
        }

        public NotSupportedEventKindException(string kind)
            : base($"Not supported GPIO event kind '{kind}'")
        {
        }
    }
}
