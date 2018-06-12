using System;
using System.Diagnostics;
using System.Threading;

namespace System.Devices.Gpio.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //BlinkingLED();
                ButtonLED();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Done!");
        }

        private static void BlinkingLED()
        {
            using (var led = GPIOPin.Create(GPIODeviceKind.RaspberryPi, GPIOScheme.BCM, 26))
            {
                led.Open(GPIOPinMode.Output);

                for (var i = 0; i < 5; ++i)
                {
                    led.Write(true);
                    Thread.Sleep(1 * 1000);

                    led.Write(false);
                    Thread.Sleep(1 * 1000);
                }
            }
        }

        private static void ButtonLED()
        {
            using (var button = GPIOPin.Create(GPIODeviceKind.RaspberryPi, GPIOScheme.BCM, 18))
            using (var led = GPIOPin.Create(GPIODeviceKind.RaspberryPi, GPIOScheme.BCM, 26))
            {
                button.Open(GPIOPinMode.Input);
                led.Open(GPIOPinMode.Output);

                var watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    var value = button.Read();
                    led.Write(value);
                }
            }
        }
    }
}
