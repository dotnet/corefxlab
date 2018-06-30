// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class NotSupportedGpioPinModeException : GpioException
    {
        public NotSupportedGpioPinModeException(GpioPinMode mode)
            : base($"Not supported GPIO pin mode '{mode}'")
        {
        }

        public NotSupportedGpioPinModeException(string mode)
            : base($"Not supported GPIO pin mode '{mode}'")
        {
        }

        public NotSupportedGpioPinModeException(uint mode)
            : base($"Not supported GPIO pin mode '{mode}'")
        {
        }
    }
}
