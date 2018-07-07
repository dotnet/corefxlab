﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class InvalidPinEventException : GpioException
    {
        public InvalidPinEventException(PinEvent pinEvent)
            : base($"GPIO pin event '{pinEvent}' invalid")
        {
        }
    }
}
