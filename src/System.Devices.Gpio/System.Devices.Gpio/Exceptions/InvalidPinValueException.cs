// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class InvalidPinValueException : GpioException
    {
        public InvalidPinValueException(PinValue value)
            : base($"Invalid GPIO pin value '{value}'")
        {
        }

        public InvalidPinValueException(string value)
            : base($"Invalid GPIO pin value '{value}'")
        {
        }
    }
}
