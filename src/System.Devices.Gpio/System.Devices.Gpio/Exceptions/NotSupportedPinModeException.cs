// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class NotSupportedPinModeException : GpioException
    {
        public NotSupportedPinModeException(PinMode mode)
            : base($"Not supported GPIO pin mode '{mode}'")
        {
        }

        public NotSupportedPinModeException(string mode)
            : base($"Not supported GPIO pin mode '{mode}'")
        {
        }

        public NotSupportedPinModeException(uint mode)
            : base($"Not supported GPIO pin mode '{mode}'")
        {
        }
    }
}
