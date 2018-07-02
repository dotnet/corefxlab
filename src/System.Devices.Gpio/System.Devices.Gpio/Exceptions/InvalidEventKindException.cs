// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class InvalidEventKindException : GpioException
    {
        public InvalidEventKindException(EventKind kind)
            : base($"Invalid GPIO event kind '{kind}'")
        {

        }
    }
}
