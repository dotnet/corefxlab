using System;
using System.Collections.Generic;
using System.Text;

namespace System.Devices.Gpio.Devices
{
    internal abstract class GpioDeviceInfo
    {
        public static GpioDeviceInfo Create(GpioDeviceKind deviceKind)
        {
            GpioDeviceInfo result;

            switch (deviceKind)
            {
                case GpioDeviceKind.RaspberryPi:
                    result = new RaspberryPiInfo();
                    break;

                default: throw new Exception($"Unsupported GPIO device '{deviceKind}'");
            }

            return result;
        }

        public abstract int PinCount { get; }

        public abstract int ConvertPinNumber(int number, GpioScheme from, GpioScheme to);
    }
}
