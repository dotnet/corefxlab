using System;
using System.Collections.Generic;
using System.Text;

namespace System.Devices.Gpio.Devices
{
    internal class RaspberryPiInfo : GpioDeviceInfo
    {
        public override int PinCount => 40;

        public override int ConvertPinNumber(int number, GpioScheme from, GpioScheme to)
        {
            int result = -1;

            switch (from)
            {
                case GpioScheme.BCM:
                    switch (to)
                    {
                        case GpioScheme.BCM:
                            result = number;
                            break;

                        case GpioScheme.Board:
                            //throw new NotImplementedException();
                            break;

                        default: throw new Exception($"Unsupported GPIO scheme '{to}'");
                    }
                    break;

                case GpioScheme.Board:
                    switch (to)
                    {
                        case GpioScheme.Board:
                            result = number;
                            break;

                        case GpioScheme.BCM:
                            //throw new NotImplementedException();
                            break;

                        default: throw new Exception($"Unsupported GPIO scheme '{to}'");
                    }
                    break;

                default: throw new Exception($"Unsupported GPIO Pin scheme '{from}'");
            }

            return result;
        }
    }
}
