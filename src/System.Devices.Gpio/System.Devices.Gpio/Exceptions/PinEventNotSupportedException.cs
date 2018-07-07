// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class PinEventNotSupportedException : GpioException
    {
        public PinEventNotSupportedException(PinEvent pinEvent)
            : base($"GPIO pin event '{pinEvent}' not supported")
        {
        }

        public PinEventNotSupportedException(string pinEvent)
            : base($"GPIO pin event '{pinEvent}' not supported")
        {
        }
    }
}
