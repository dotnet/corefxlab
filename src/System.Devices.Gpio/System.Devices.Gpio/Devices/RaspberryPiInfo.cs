using System;
using System.Collections.Generic;
using System.Text;

namespace System.Devices.Gpio.Devices
{
    internal class RaspberryPiInfo : GPIODeviceInfo
    {
        public override int PinCount => 40;

        public override int ConvertPinNumber(int number, GPIOScheme from, GPIOScheme to)
        {
            int result = -1;

            switch (from)
            {
                case GPIOScheme.BCM:
                    switch (to)
                    {
                        case GPIOScheme.BCM:
                            result = number;
                            break;

                        case GPIOScheme.Board:
                            //throw new NotImplementedException();
                            break;

                        default: throw new Exception($"Unsupported GPIO scheme '{to}'");
                    }
                    break;

                case GPIOScheme.Board:
                    switch (to)
                    {
                        case GPIOScheme.Board:
                            result = number;
                            break;

                        case GPIOScheme.BCM:
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
