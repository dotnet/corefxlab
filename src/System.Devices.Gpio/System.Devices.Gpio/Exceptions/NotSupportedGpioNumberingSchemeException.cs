// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class NotSupportedGpioNumberingSchemeException : GpioException
    {
        public NotSupportedGpioNumberingSchemeException(GpioNumberingScheme numbering)
            : base($"Unsupported GPIO pin numbering scheme {numbering}")
        {
        }
    }
}
