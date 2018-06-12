using System;
using System.Collections.Generic;
using System.Text;

namespace System.Devices.Gpio.Devices
{
    internal abstract class GPIODeviceInfo
    {
        public static GPIODeviceInfo Create(GPIODeviceKind deviceKind)
        {
            GPIODeviceInfo result;

            switch (deviceKind)
            {
                case GPIODeviceKind.RaspberryPi:
                    result = new RaspberryPiInfo();
                    break;

                default: throw new Exception($"Unsupported GPIO device '{deviceKind}'");
            }

            return result;
        }

        public abstract int PinCount { get; }

        public abstract int ConvertPinNumber(int number, GPIOScheme from, GPIOScheme to);
    }
}
