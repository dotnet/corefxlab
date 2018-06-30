// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public static class GpioExtensions
    {
        public static void Set(this GpioPin pin)
        {
            pin.Write(GpioPinValue.High);
        }

        public static void Clear(this GpioPin pin)
        {
            pin.Write(GpioPinValue.Low);
        }

        public static void Toogle(this GpioPin pin)
        {
            GpioPinValue value = pin.Read();

            switch (value)
            {
                case GpioPinValue.Low: value = GpioPinValue.High; break;
                case GpioPinValue.High: value = GpioPinValue.Low; break;
            }

            pin.Write(value);
        }
    }
}
