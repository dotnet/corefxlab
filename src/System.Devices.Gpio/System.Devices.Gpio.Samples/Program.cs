using System.Diagnostics;
using System.Threading;

namespace System.Devices.Gpio.Samples
{
    internal unsafe class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //BlinkingLED();
                //ButtonLED();
                //Driver_BlinkingLED();
                Driver_ButtonLED();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Done!");
        }

        private static void BlinkingLED()
        {
            using (var led = GpioPin.Create(GpioDeviceKind.RaspberryPi, GpioScheme.BCM, 26))
            {
                led.Open(GpioPinMode.Output);

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
            using (var button = GpioPin.Create(GpioDeviceKind.RaspberryPi, GpioScheme.BCM, 18))
            using (var led = GpioPin.Create(GpioDeviceKind.RaspberryPi, GpioScheme.BCM, 26))
            {
                button.Open(GpioPinMode.Input);
                led.Open(GpioPinMode.Output);

                var watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    var value = button.Read();
                    led.Write(value);
                }
            }
        }

        private static void Driver_BlinkingLED()
        {
            const int led = 26;

            using (var driver = new RaspberryPiDriver())
            {
                driver.SetPinMode(led, GpioPinMode.Output);

                for (var i = 0; i < 5; ++i)
                {
                    driver.Output(led, GpioPinValue.High);
                    Thread.Sleep(1 * 1000);

                    driver.Output(led, GpioPinValue.Low);
                    Thread.Sleep(1 * 1000);
                }
            }
        }

        private static void Driver_ButtonLED()
        {
            const int button = 18;
            const int led = 26;

            using (var driver = new RaspberryPiDriver())
            {
                driver.SetPinMode(button, GpioPinMode.Input);
                driver.SetPinMode(led, GpioPinMode.Output);

                var watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    var value = driver.Input(button);
                    driver.Output(led, value);
                }
            }
        }
    }
}
