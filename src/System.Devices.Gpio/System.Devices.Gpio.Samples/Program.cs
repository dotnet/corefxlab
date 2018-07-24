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
                    case 0:
                        Unix_BlinkingLED();
                        break;
                    case 1:
                        Unix_ButtonLED();
                        break;

                    case 2:
                        RaspberryPi_BlinkingLED();
                        break;
                    case 3:
                        RaspberryPi_ButtonLED();
                        break;

                    case 4:
                        UnixDriver_BlinkingLED();
                        break;
                    case 5:
                        UnixDriver_ButtonLED();
                        break;

                    case 6:
                        RaspberryPiDriver_BlinkingLED();
                        break;
                    case 7:
                        RaspberryPiDriver_ButtonLED();
                        break;

                    case 8:
                        RaspberryPiDriver_ButtonPullDown();
                        break;

                    case 9:
                        UnixDriver_DetectButton();
                        break;
                    case 10:
                        UnixDriver_DetectButtonLED();
                        break;

                    case 11:
                        RaspberryPiDriver_DetectButton();
                        break;
                    case 12:
                        RaspberryPiDriver_DetectButtonLED();
                        break;

                    case 13:
                        UnixDriver_ButtonWait();
                        break;
                    case 14:
                        RaspberryPiDriver_ButtonWait();
                        break;

                    case 15:
                        RaspberryPi_ButtonPullDown();
                        break;

                    case 16:
                        Unix_DetectButton();
                        break;
                    case 17:
                        Unix_DetectButtonLED();
                        break;

                    case 18:
                        RaspberryPi_DetectButton();
                        break;
                    case 19:
                        RaspberryPi_DetectButtonLED();
                        break;

                    case 20:
                        Unix_ButtonWait();
                        break;
                    case 21:
                        RaspberryPi_ButtonWait();
                        break;

                    case 22:
                        Unix_LCD();
                        break;
                    case 23:
                        RaspberryPi_LCD();
                        break;

                    case 24:
                        Spi_Roundtrip();
                        break;

                    case 25:
                        Unix_Spi_Bme280();
                        break;
                    case 26:
                        RaspberryPi_Spi_Bme280();
                        break;

                    case 27:
                        Unix_Spi_Bme280_LCD();
                        break;
                    case 28:
                        RaspberryPi_Spi_Bme280_LCD();
                        break;

                    default:
                        ShowUsage();
                        break;
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
            Console.WriteLine();
            Console.WriteLine($"        9 -> {nameof(UnixDriver_DetectButton)}");
            Console.WriteLine($"       10 -> {nameof(UnixDriver_DetectButtonLED)}");
            Console.WriteLine();
            Console.WriteLine($"       11 -> {nameof(RaspberryPiDriver_DetectButton)}");
            Console.WriteLine($"       12 -> {nameof(RaspberryPiDriver_DetectButtonLED)}");
            Console.WriteLine();
            Console.WriteLine($"       13 -> {nameof(UnixDriver_ButtonWait)}");
            Console.WriteLine($"       14 -> {nameof(RaspberryPiDriver_ButtonWait)}");
            Console.WriteLine();
            Console.WriteLine($"       15 -> {nameof(RaspberryPi_ButtonPullDown)}");
            Console.WriteLine();
            Console.WriteLine($"       16 -> {nameof(Unix_DetectButton)}");
            Console.WriteLine($"       17 -> {nameof(Unix_DetectButtonLED)}");
            Console.WriteLine();
            Console.WriteLine($"       18 -> {nameof(RaspberryPi_DetectButton)}");
            Console.WriteLine($"       19 -> {nameof(RaspberryPi_DetectButtonLED)}");
            Console.WriteLine();
            Console.WriteLine($"       20 -> {nameof(Unix_ButtonWait)}");
            Console.WriteLine($"       21 -> {nameof(RaspberryPi_ButtonWait)}");
            Console.WriteLine();
            Console.WriteLine($"       22 -> {nameof(Unix_LCD)}");
            Console.WriteLine($"       23 -> {nameof(RaspberryPi_LCD)}");
            Console.WriteLine();
            Console.WriteLine($"       24 -> {nameof(Spi_Roundtrip)}");
            Console.WriteLine();
            Console.WriteLine($"       25 -> {nameof(Unix_Spi_Bme280)}");
            Console.WriteLine($"       26 -> {nameof(RaspberryPi_Spi_Bme280)}");
            Console.WriteLine();
            Console.WriteLine($"       27 -> {nameof(Unix_Spi_Bme280_LCD)}");
            Console.WriteLine($"       28 -> {nameof(RaspberryPi_Spi_Bme280_LCD)}");
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
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin led = controller.OpenPin(26, PinMode.Output);

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
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin button = controller.OpenPin(18, PinMode.Input);
                Pin led = controller.OpenPin(26, PinMode.Output);

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
                driver.OpenPin(led);
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
                driver.OpenPin(button);
                driver.SetPinMode(button, PinMode.Input);

                driver.OpenPin(led);
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
                driver.OpenPin(button);
                driver.SetPinMode(button, PinMode.InputPullDown);

                driver.OpenPin(led);
                driver.SetPinMode(led, PinMode.Output);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    PinValue value = driver.Input(button);
                    driver.Output(led, value);
                }
            }
        }

        private static void RaspberryPi_ButtonPullDown()
        {
            Console.WriteLine(nameof(RaspberryPi_ButtonPullDown));
            ButtonPullDown(new RaspberryPiDriver());
        }

        private static void ButtonPullDown(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin button = controller.OpenPin(18, PinMode.InputPullDown);
                Pin led = controller.OpenPin(26, PinMode.Output);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    PinValue value = button.Read();
                    led.Write(value);
                }
            }
        }

        private static void UnixDriver_DetectButton()
        {
            Console.WriteLine(nameof(UnixDriver_DetectButton));
            Driver_DetectButton(new UnixDriver(RaspberryPiPinCount));
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
                PinMode buttonMode = PinMode.Input;

                if (driver.IsPinModeSupported(PinMode.InputPullDown))
                {
                    buttonMode = PinMode.InputPullDown;
                }

                driver.OpenPin(button);
                driver.SetPinMode(button, buttonMode);

                driver.SetDebounce(button, TimeSpan.FromMilliseconds(100));
                driver.SetPinEventsToDetect(button, PinEvent.SyncBoth);
                driver.ValueChanged += OnPinValueChanged1;
                driver.SetEnableRaisingPinEvents(button, true);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(1 * 100);

                    if (s_buttonPressed)
                    {
                        Console.WriteLine($"Button press!");
                    }
                }
            }
        }

        private static void Unix_DetectButton()
        {
            Console.WriteLine(nameof(Unix_DetectButton));
            DetectButton(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_DetectButton()
        {
            Console.WriteLine(nameof(RaspberryPi_DetectButton));
            DetectButton(new RaspberryPiDriver());
        }

        private static void DetectButton(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin button = controller.OpenPin(18, PinMode.Input);

                if (button.IsModeSupported(PinMode.InputPullDown))
                {
                    button.Mode = PinMode.InputPullDown;
                }

                button.DebounceTimeout = TimeSpan.FromMilliseconds(100);
                button.NotifyEvents = PinEvent.SyncBoth;
                button.ValueChanged += OnPinValueChanged1;
                button.EnableRaisingEvents = true;

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(1 * 100);

                    if (s_buttonPressed)
                    {
                        Console.WriteLine($"Button press!");
                    }
                }
            }
        }

        private static bool s_buttonPressed = false;

        private static void OnPinValueChanged1(object sender, PinValueChangedEventArgs e)
        {
            if (s_buttonPressed)
            {
                Console.WriteLine($"Button up!");
            }
            else
            {
                Console.WriteLine($"Button down!");
            }

            s_buttonPressed = !s_buttonPressed;
        }

        private static void UnixDriver_DetectButtonLED()
        {
            Console.WriteLine(nameof(UnixDriver_DetectButtonLED));
            Driver_DetectButtonLED(new UnixDriver(RaspberryPiPinCount));
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
                PinMode buttonMode = PinMode.Input;

                if (driver.IsPinModeSupported(PinMode.InputPullDown))
                {
                    buttonMode = PinMode.InputPullDown;
                }

                driver.OpenPin(button);
                driver.SetPinMode(button, buttonMode);

                driver.OpenPin(led);
                driver.SetPinMode(led, PinMode.Output);

                driver.SetDebounce(button, TimeSpan.FromSeconds(1));
                driver.SetPinEventsToDetect(button, PinEvent.SyncFallingEdge);
                driver.ValueChanged += OnPinValueChanged2;
                driver.SetEnableRaisingPinEvents(button, true);

                PinEvent events = driver.GetPinEventsToDetect(button);
                Console.WriteLine($"Events to detect: {events}");

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(1 * 1000);
                }
            }
        }

        private static void Unix_DetectButtonLED()
        {
            Console.WriteLine(nameof(Unix_DetectButtonLED));
            DetectButtonLED(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_DetectButtonLED()
        {
            Console.WriteLine(nameof(RaspberryPi_DetectButtonLED));
            DetectButtonLED(new RaspberryPiDriver());
        }

        private static void DetectButtonLED(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin button = controller.OpenPin(18, PinMode.Input);

                if (button.IsModeSupported(PinMode.InputPullDown))
                {
                    button.Mode = PinMode.InputPullDown;
                }

                Pin led = controller.OpenPin(26, PinMode.Output);

                button.DebounceTimeout = TimeSpan.FromSeconds(1);
                button.NotifyEvents = PinEvent.SyncFallingEdge;
                button.ValueChanged += OnPinValueChanged2;
                button.EnableRaisingEvents = true;

                PinEvent events = button.NotifyEvents;
                Console.WriteLine($"Events to detect: {events}");

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(1 * 1000);
                }
            }
        }

        private static PinValue s_currentLedValue = PinValue.Low;

        private static void OnPinValueChanged2(object sender, PinValueChangedEventArgs e)
        {
            const int ledPinNumber = 26;

            s_currentLedValue = s_currentLedValue == PinValue.High ? PinValue.Low : PinValue.High;
            Console.WriteLine($"Button pressed! LED value {s_currentLedValue}");

            if (sender is GpioDriver)
            {
                GpioDriver driver = sender as GpioDriver;
                driver.Output(ledPinNumber, s_currentLedValue);
            }
            else if (sender is Pin)
            {
                Pin button = sender as Pin;
                GpioController controller = button.Controller;
                Pin led = controller[ledPinNumber];
                led.Write(s_currentLedValue);
            }
            else
            {
                throw new ArgumentException(nameof(sender));
            }
        }

        private static void UnixDriver_ButtonWait()
        {
            Console.WriteLine(nameof(UnixDriver_ButtonWait));
            Driver_ButtonWait(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPiDriver_ButtonWait()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_ButtonWait));
            Driver_ButtonWait(new RaspberryPiDriver());
        }

        private static void Driver_ButtonWait(GpioDriver driver)
        {
            const int button = 18;

            using (driver)
            {
                PinMode buttonMode = PinMode.Input;

                if (driver.IsPinModeSupported(PinMode.InputPullDown))
                {
                    buttonMode = PinMode.InputPullDown;
                }

                driver.OpenPin(button);
                driver.SetPinMode(button, buttonMode);

                driver.SetDebounce(button, TimeSpan.FromSeconds(1));
                driver.SetPinEventsToDetect(button, PinEvent.SyncRisingEdge);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    bool eventDetected = driver.WaitForPinEvent(button, TimeSpan.FromSeconds(1));

                    if (eventDetected)
                    {
                        Console.WriteLine("Event detected!");
                    }
                    else
                    {
                        Console.WriteLine("Timeout!");
                    }
                }
            }
        }

        private static void Unix_ButtonWait()
        {
            Console.WriteLine(nameof(Unix_ButtonWait));
            ButtonWait(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_ButtonWait()
        {
            Console.WriteLine(nameof(RaspberryPi_ButtonWait));
            ButtonWait(new RaspberryPiDriver());
        }

        private static void ButtonWait(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin button = controller.OpenPin(18, PinMode.Input);

                if (button.IsModeSupported(PinMode.InputPullDown))
                {
                    button.Mode = PinMode.InputPullDown;
                }

                button.DebounceTimeout = TimeSpan.FromSeconds(1);
                button.NotifyEvents = PinEvent.SyncRisingEdge;

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    bool eventDetected = button.WaitForEvent(TimeSpan.FromSeconds(1));

                    if (eventDetected)
                    {
                        Console.WriteLine("Event detected!");
                    }
                    else
                    {
                        Console.WriteLine("Timeout!");
                    }
                }
            }
        }

        private static void Unix_LCD()
        {
            Console.WriteLine(nameof(Unix_LCD));
            LCD(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_LCD()
        {
            Console.WriteLine(nameof(RaspberryPi_LCD));
            LCD(new RaspberryPiDriver());
        }

        private static void LCD(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin rs = controller.OpenPin(0);
                Pin enable = controller.OpenPin(5);
                Pin[] dbs = controller.OpenPins(6, 16, 20, 21);

                var lcd = new LiquidCrystal(rs, enable, dbs);
                lcd.Begin(16, 2);
                lcd.Print("hello, world!");

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    lcd.SetCursor(0, 1);
                    lcd.Print($"{watch.Elapsed.TotalSeconds:0.00} seconds");
                }
            }
        }

        private static void Spi_Roundtrip()
        {
            // For this sample connect SPI0 MOSI with SPI0 MISO.
            var settings = new SpiConnectionSettings(0, 0);
            using (var device = new UnixSpiDevice(settings))
            {
                var writeBuffer = new byte[]
                {
                    0xA, 0xB, 0xC, 0xD, 0xE, 0xF
                };

                var readBuffer = new byte[writeBuffer.Length];

                device.TransferFullDuplex(writeBuffer, readBuffer);

                Console.WriteLine("Sent data:");

                foreach (byte b in writeBuffer)
                {
                    Console.Write("{0:X2} ", b);
                }

                Console.WriteLine();
                Console.WriteLine("Received data:");

                foreach (byte b in readBuffer)
                {
                    Console.Write("{0:X2} ", b);
                }

                Console.WriteLine();
            }
        }

        private static void Unix_Spi_Bme280()
        {
            Console.WriteLine(nameof(Unix_Spi_Bme280));
            Spi_Bme280(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_Spi_Bme280()
        {
            Console.WriteLine(nameof(RaspberryPi_Spi_Bme280));
            Spi_Bme280(new RaspberryPiDriver());
        }

        private static void Spi_Bme280(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin csPin = controller.OpenPin(8);

                var settings = new SpiConnectionSettings(0, 0);
                using (var bme280 = new Bme280(csPin, settings))
                {
                    bool ok = bme280.Begin();

                    if (ok)
                    {
                        Console.WriteLine($"Pressure (hPa/mb)\tHumdity (%)\tTemp (C)\tTemp (F)");
                        Console.WriteLine();

                        for (var i = 0; i < 5; ++i)
                        {
                            bme280.ReadSensor();

                            Console.WriteLine($"{bme280.PressureInHectopascals:0.00} hPa\t\t{bme280.Humidity:0.00} %\t\t{bme280.TemperatureInCelsius:0.00} C\t\t{bme280.TemperatureInFahrenheit:0.00} F");
                            Thread.Sleep(1 * 1000);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error initializing sensor");
                    }
                }
            }
        }

        private static void Unix_Spi_Bme280_LCD()
        {
            Console.WriteLine(nameof(Unix_Spi_Bme280_LCD));
            Spi_Bme280_LCD(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_Spi_Bme280_LCD()
        {
            Console.WriteLine(nameof(RaspberryPi_Spi_Bme280_LCD));
            Spi_Bme280_LCD(new RaspberryPiDriver());
        }

        private static void Spi_Bme280_LCD(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.BCM))
            {
                Pin rs = controller.OpenPin(0);
                Pin enable = controller.OpenPin(5);
                Pin[] dbs = controller.OpenPins(6, 16, 20, 21);

                var lcd = new LiquidCrystal(rs, enable, dbs);
                lcd.Begin(16, 2);

                Pin csPin = controller.OpenPin(8);

                var settings = new SpiConnectionSettings(0, 0);
                using (var bme280 = new Bme280(csPin, settings))
                {
                    bool ok = bme280.Begin();

                    if (ok)
                    {
                        Console.WriteLine($"Pressure (hPa/mb)\tHumdity (%)\tTemp (C)\tTemp (F)");
                        Console.WriteLine();

                        for (var i = 0; i < 3; ++i)
                        {
                            bme280.ReadSensor();

                            Console.WriteLine($"{bme280.PressureInHectopascals:0.00} hPa\t\t{bme280.Humidity:0.00} %\t\t{bme280.TemperatureInCelsius:0.00} C\t\t{bme280.TemperatureInFahrenheit:0.00} F");

                            ShowInfo(lcd, "Pressure", $"{bme280.PressureInHectopascals:0.00} hPa/mb");
                            ShowInfo(lcd, "Humdity", $"{bme280.Humidity:0.00} %");
                            ShowInfo(lcd, "Temperature", $"{bme280.TemperatureInCelsius:0.00} C, {bme280.TemperatureInFahrenheit:0.00} F");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error initializing sensor");
                    }
                }
            }
        }

        private static void ShowInfo(LiquidCrystal lcd, string label, string value)
        {
            lcd.Clear();
            lcd.SetCursor(0, 0);
            lcd.Print(label);

            lcd.SetCursor(0, 1);
            lcd.Print(value);

            Thread.Sleep(3 * 1000);
        }
    }
}
