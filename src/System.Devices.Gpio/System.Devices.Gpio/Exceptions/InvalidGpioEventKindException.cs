﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class InvalidGpioEventKindException : GpioException
    {
        public InvalidGpioEventKindException(GpioEventKind kind)
            : base($"Invalid GPIO event kind '{kind}'")
        {

        }
    }
}
