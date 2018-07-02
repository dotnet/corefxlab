// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Threading;

namespace System.Devices.Gpio.Samples
{
    internal class Program
    {
        private const int RaspberryPiPinCount = 54;

        private static void Main(string[] args)
        {
            try
            {
                int option = -1;

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

                    case 8: RaspberryPiDriver_ButtonPullDown(); break;
                    case 9: RaspberryPiDriver_DetectButton(); break;
                    case 10: RaspberryPiDriver_DetectButtonLED(); break;

                    default: ShowUsage(); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Done!");
        }

        private static void ShowUsage()
        {
            string assemblyName = Reflection.Assembly.GetEntryAssembly().GetName().Name;

            Console.WriteLine($"Usage: {assemblyName} <arg>");
            Console.WriteLine("       where <arg> can be any of the following options:");
            Console.WriteLine();
            Console.WriteLine($"        0 -> {nameof(Unix_BlinkingLED)}");
            Console.WriteLine($"        1 -> {nameof(Unix_ButtonLED)}");
            Console.WriteLine();
            Console.WriteLine($"        2 -> {nameof(RaspberryPi_BlinkingLED)}");
            Console.WriteLine($"        3 -> {nameof(RaspberryPi_ButtonLED)}");
            Console.WriteLine();
            Console.WriteLine($"        4 -> {nameof(UnixDriver_BlinkingLED)}");
            Console.WriteLine($"        5 -> {nameof(UnixDriver_ButtonLED)}");
            Console.WriteLine();
            Console.WriteLine($"        6 -> {nameof(RaspberryPiDriver_BlinkingLED)}");
            Console.WriteLine($"        7 -> {nameof(RaspberryPiDriver_ButtonLED)}");
            Console.WriteLine();
            Console.WriteLine($"        8 -> {nameof(RaspberryPiDriver_ButtonPullDown)}");
            Console.WriteLine($"        9 -> {nameof(RaspberryPiDriver_DetectButton)}");
            Console.WriteLine($"       10 -> {nameof(RaspberryPiDriver_DetectButtonLED)}");
            Console.WriteLine();
        }

        private static void Unix_BlinkingLED()
        {
            Console.WriteLine(nameof(Unix_BlinkingLED));
            BlinkingLED(new UnixDriver(RaspberryPiPinCount));
        }

        private static void Unix_ButtonLED()
        {
            Console.WriteLine(nameof(Unix_ButtonLED));
            ButtonLED(new UnixDriver(RaspberryPiPinCount));
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
                var led = new Pin(driver, PinNumberingScheme.BCM, 26, PinMode.Output);

                for (var i = 0; i < 5; ++i)
                {
                    led.Write(PinValue.High);
                    Thread.Sleep(1 * 1000);

                    led.Write(PinValue.Low);
                    Thread.Sleep(1 * 1000);
                }
            }
        }

        private static void ButtonLED(GpioDriver driver)
        {
            using (driver)
            {
                var button = new Pin(driver, PinNumberingScheme.BCM, 18, PinMode.Input);
                var led = new Pin(driver, PinNumberingScheme.BCM, 26, PinMode.Output);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    PinValue value = button.Read();
                    led.Write(value);
                }
            }
        }

        private static void UnixDriver_BlinkingLED()
        {
            Console.WriteLine(nameof(UnixDriver_BlinkingLED));
            Driver_BlinkingLED(new UnixDriver(RaspberryPiPinCount));
        }

        private static void UnixDriver_ButtonLED()
        {
            Console.WriteLine(nameof(UnixDriver_ButtonLED));
            Driver_ButtonLED(new UnixDriver(RaspberryPiPinCount));
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
                driver.SetPinMode(led, PinMode.Output);

                for (var i = 0; i < 5; ++i)
                {
                    driver.Output(led, PinValue.High);
                    Thread.Sleep(1 * 1000);

                    driver.Output(led, PinValue.Low);
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
                driver.SetPinMode(button, PinMode.Input);
                driver.SetPinMode(led, PinMode.Output);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    PinValue value = driver.Input(button);
                    driver.Output(led, value);
                }
            }
        }

        private static void RaspberryPiDriver_ButtonPullDown()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_ButtonPullDown));
            Driver_ButtonPullDown(new RaspberryPiDriver());
        }

        private static void Driver_ButtonPullDown(GpioDriver driver)
        {
            const int button = 18;
            const int led = 26;

            using (driver)
            {
                driver.SetPinMode(button, PinMode.InputPullDown);
                driver.SetPinMode(led, PinMode.Output);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    PinValue value = driver.Input(button);
                    driver.Output(led, value);
                }
            }
        }

        private static void RaspberryPiDriver_DetectButton()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_DetectButton));
            Driver_DetectButton(new RaspberryPiDriver());
        }

        private static void Driver_DetectButton(GpioDriver driver)
        {
            const int button = 18;

            using (driver)
            {
                driver.SetPinMode(button, PinMode.InputPullDown);

                driver.SetEventDetection(button, EventKind.SyncRisingEdge, true);
                driver.SetEventDetection(button, EventKind.SyncFallingEdge, true);
                driver.SetEventDetection(button, EventKind.AsyncRisingEdge, false);
                driver.SetEventDetection(button, EventKind.AsyncFallingEdge, false);
                driver.SetEventDetection(button, EventKind.High, false);
                driver.SetEventDetection(button, EventKind.Low, false);

                Stopwatch watch = Stopwatch.StartNew();
                bool buttonPressed = false;

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(1 * 100);
                    bool eventDetected = driver.EventWasDetected(button);

                    if (eventDetected)
                    {
                        if (buttonPressed)
                        {
                            Console.WriteLine($"Button up!");
                        }
                        else
                        {
                            Console.WriteLine($"Button down!");
                        }

                        buttonPressed = !buttonPressed;
                    }
                    else if (buttonPressed)
                    {
                        Console.WriteLine($"Button press!");
                    }
                }
            }
        }

        private static void RaspberryPiDriver_DetectButtonLED()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_DetectButtonLED));
            Driver_DetectButtonLED(new RaspberryPiDriver());
        }

        private static void Driver_DetectButtonLED(GpioDriver driver)
        {
            const int button = 18;
            const int led = 26;

            using (driver)
            {
                driver.SetPinMode(button, PinMode.InputPullDown);
                driver.SetPinMode(led, PinMode.Output);

                driver.SetEventDetection(button, EventKind.SyncRisingEdge, true);
                driver.SetEventDetection(button, EventKind.SyncFallingEdge, false);
                driver.SetEventDetection(button, EventKind.AsyncRisingEdge, false);
                driver.SetEventDetection(button, EventKind.AsyncFallingEdge, false);
                driver.SetEventDetection(button, EventKind.High, false);
                driver.SetEventDetection(button, EventKind.Low, false);

                bool eventDetectionEnabled = driver.GetEventDetection(button, EventKind.SyncRisingEdge);
                Console.WriteLine($"Is event detection enabled? {eventDetectionEnabled}");

                Stopwatch watch = Stopwatch.StartNew();
                PinValue currentLedValue = PinValue.Low;

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(1 * 1000);
                    bool buttonPressed = driver.EventWasDetected(button);

                    if (buttonPressed)
                    {
                        currentLedValue = currentLedValue == PinValue.High ? PinValue.Low : PinValue.High;
                        Console.WriteLine($"Button pressed! LED value {currentLedValue}");

                        driver.Output(led, currentLedValue);
                    }
                }
            }
        }
    }
}
