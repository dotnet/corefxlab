// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class InvalidGpioPinValueException : GpioException
    {
        public InvalidGpioPinValueException(GpioPinValue value)
            : base($"Invalid GPIO pin value '{value}'")
        {
        }

        public InvalidGpioPinValueException(string value)
            : base($"Invalid GPIO pin value '{value}'")
        {
        }
    }
}
