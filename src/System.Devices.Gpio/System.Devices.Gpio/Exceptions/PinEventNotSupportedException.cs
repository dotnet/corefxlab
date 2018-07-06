// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class NotSupportedPinEventException : GpioException
    {
        public NotSupportedPinEventException(PinEvent pinEvent)
            : base($"Not supported GPIO pin event '{pinEvent}'")
        {
        }

        public NotSupportedPinEventException(string pinEvent)
            : base($"Not supported GPIO pin event '{pinEvent}'")
        {
        }
    }
}
