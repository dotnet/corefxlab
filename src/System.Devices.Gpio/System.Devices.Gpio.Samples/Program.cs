using System.Diagnostics;
using System.Threading;

namespace System.Devices.Gpio.Samples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var option = -1;

                if (args.Length > 0)
                {
                    option = Convert.ToInt32(args[0]);
                }

                switch (option)
                {
                    case 0: Unix_BlinkingLED(); break;
                    case 1: Unix_ButtonLED(); break;

                    case 2: RaspberryPi_BlinkingLED(); break;
                    case 3: RaspberryPi_ButtonLED(); break;

                    case 4: UnixDriver_BlinkingLED(); break;
                    case 5: UnixDriver_ButtonLED(); break;

                    case 6: RaspberryPiDriver_BlinkingLED(); break;
                    case 7: RaspberryPiDriver_ButtonLED(); break;

                    default: RaspberryPiDriver_ButtonLED(); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Done!");
        }

        private static void Unix_BlinkingLED()
        {
            Console.WriteLine(nameof(Unix_BlinkingLED));
            BlinkingLED(new UnixDriver());
        }

        private static void Unix_ButtonLED()
        {
            Console.WriteLine(nameof(Unix_ButtonLED));
            ButtonLED(new UnixDriver());
        }

        private static void RaspberryPi_BlinkingLED()
        {
            Console.WriteLine(nameof(RaspberryPi_BlinkingLED));
            BlinkingLED(new RaspberryPiDriver());
        }

        private static void RaspberryPi_ButtonLED()
        {
            Console.WriteLine(nameof(RaspberryPi_ButtonLED));
            ButtonLED(new RaspberryPiDriver());
        }

        private static void BlinkingLED(GpioDriver driver)
        {
            using (driver)
            {
                var led = new GpioPin(driver, GpioScheme.BCM, 26, GpioPinMode.Output);

                for (var i = 0; i < 5; ++i)
                {
                    led.Write(GpioPinValue.High);
                    Thread.Sleep(1 * 1000);

                    led.Write(GpioPinValue.Low);
                    Thread.Sleep(1 * 1000);
                }
            }
        }

        private static void ButtonLED(GpioDriver driver)
        {
            using (driver)
            {
                var button = new GpioPin(driver, GpioScheme.BCM, 18, GpioPinMode.Input);
                var led = new GpioPin(driver, GpioScheme.BCM, 26, GpioPinMode.Output);

                var watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    var value = button.Read();
                    led.Write(value);
                }
            }
        }

        private static void UnixDriver_BlinkingLED()
        {
            Console.WriteLine(nameof(UnixDriver_BlinkingLED));
            Driver_BlinkingLED(new UnixDriver());
        }

        private static void UnixDriver_ButtonLED()
        {
            Console.WriteLine(nameof(UnixDriver_ButtonLED));
            Driver_ButtonLED(new UnixDriver());
        }

        private static void RaspberryPiDriver_BlinkingLED()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_BlinkingLED));
            Driver_BlinkingLED(new RaspberryPiDriver());
        }

        private static void RaspberryPiDriver_ButtonLED()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_ButtonLED));
            Driver_ButtonLED(new RaspberryPiDriver());
        }

        private static void Driver_BlinkingLED(GpioDriver driver)
        {
            const int led = 26;

            using (driver)
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

        private static void Driver_ButtonLED(GpioDriver driver)
        {
            const int button = 18;
            const int led = 26;

            using (driver)
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
