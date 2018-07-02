// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public static class Extensions
    {
        public static void Set(this Pin pin)
        {
            pin.Write(PinValue.High);
        }

        public static void Clear(this Pin pin)
        {
            pin.Write(PinValue.Low);
        }

        public static void Toogle(this Pin pin)
        {
            PinValue value = pin.Read();

            switch (value)
            {
                case PinValue.Low: value = PinValue.High; break;
                case PinValue.High: value = PinValue.Low; break;
            }

            pin.Write(value);
        }
    }
}
