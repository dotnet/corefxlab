// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Devices.I2c;
using System.Devices.Spi;
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
                        Unix_BlinkingLed();
                        break;
                    case 1:
                        Unix_ButtonLed();
                        break;

                    case 2:
                        RaspberryPi_BlinkingLed();
                        break;
                    case 3:
                        RaspberryPi_ButtonLed();
                        break;

                    case 4:
                        UnixDriver_BlinkingLed();
                        break;
                    case 5:
                        UnixDriver_ButtonLed();
                        break;

                    case 6:
                        RaspberryPiDriver_BlinkingLed();
                        break;
                    case 7:
                        RaspberryPiDriver_ButtonLed();
                        break;

                    case 8:
                        RaspberryPiDriver_ButtonPullDown();
                        break;

                    case 9:
                        UnixDriver_DetectButton();
                        break;
                    case 10:
                        UnixDriver_DetectButtonLed();
                        break;

                    case 11:
                        RaspberryPiDriver_DetectButton();
                        break;
                    case 12:
                        RaspberryPiDriver_DetectButtonLed();
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
                        Unix_DetectButtonLed();
                        break;

                    case 18:
                        RaspberryPi_DetectButton();
                        break;
                    case 19:
                        RaspberryPi_DetectButtonLed();
                        break;

                    case 20:
                        Unix_ButtonWait();
                        break;
                    case 21:
                        RaspberryPi_ButtonWait();
                        break;

                    case 22:
                        Unix_Lcd();
                        break;
                    case 23:
                        RaspberryPi_Lcd();
                        break;

                    case 24:
                        Spi_Roundtrip();
                        break;

                    case 25:
                        Unix_Spi_Pressure();
                        break;
                    case 26:
                        RaspberryPi_Spi_Pressure();
                        break;

                    case 27:
                        Unix_Spi_Pressure_Lcd();
                        break;
                    case 28:
                        RaspberryPi_Spi_Pressure_Lcd();
                        break;

                    case 29:
                        I2c_Pressure();
                        break;

                    case 30:
                        Unix_I2c_Pressure_Lcd();
                        break;
                    case 31:
                        RaspberryPi_I2c_Pressure_Lcd();
                        break;

                    case 32:
                        I2c_Color();
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
            Console.WriteLine($"        0 -> {nameof(Unix_BlinkingLed)}");
            Console.WriteLine($"        1 -> {nameof(Unix_ButtonLed)}");
            Console.WriteLine();
            Console.WriteLine($"        2 -> {nameof(RaspberryPi_BlinkingLed)}");
            Console.WriteLine($"        3 -> {nameof(RaspberryPi_ButtonLed)}");
            Console.WriteLine();
            Console.WriteLine($"        4 -> {nameof(UnixDriver_BlinkingLed)}");
            Console.WriteLine($"        5 -> {nameof(UnixDriver_ButtonLed)}");
            Console.WriteLine();
            Console.WriteLine($"        6 -> {nameof(RaspberryPiDriver_BlinkingLed)}");
            Console.WriteLine($"        7 -> {nameof(RaspberryPiDriver_ButtonLed)}");
            Console.WriteLine();
            Console.WriteLine($"        8 -> {nameof(RaspberryPiDriver_ButtonPullDown)}");
            Console.WriteLine();
            Console.WriteLine($"        9 -> {nameof(UnixDriver_DetectButton)}");
            Console.WriteLine($"       10 -> {nameof(UnixDriver_DetectButtonLed)}");
            Console.WriteLine();
            Console.WriteLine($"       11 -> {nameof(RaspberryPiDriver_DetectButton)}");
            Console.WriteLine($"       12 -> {nameof(RaspberryPiDriver_DetectButtonLed)}");
            Console.WriteLine();
            Console.WriteLine($"       13 -> {nameof(UnixDriver_ButtonWait)}");
            Console.WriteLine($"       14 -> {nameof(RaspberryPiDriver_ButtonWait)}");
            Console.WriteLine();
            Console.WriteLine($"       15 -> {nameof(RaspberryPi_ButtonPullDown)}");
            Console.WriteLine();
            Console.WriteLine($"       16 -> {nameof(Unix_DetectButton)}");
            Console.WriteLine($"       17 -> {nameof(Unix_DetectButtonLed)}");
            Console.WriteLine();
            Console.WriteLine($"       18 -> {nameof(RaspberryPi_DetectButton)}");
            Console.WriteLine($"       19 -> {nameof(RaspberryPi_DetectButtonLed)}");
            Console.WriteLine();
            Console.WriteLine($"       20 -> {nameof(Unix_ButtonWait)}");
            Console.WriteLine($"       21 -> {nameof(RaspberryPi_ButtonWait)}");
            Console.WriteLine();
            Console.WriteLine($"       22 -> {nameof(Unix_Lcd)}");
            Console.WriteLine($"       23 -> {nameof(RaspberryPi_Lcd)}");
            Console.WriteLine();
            Console.WriteLine($"       24 -> {nameof(Spi_Roundtrip)}");
            Console.WriteLine();
            Console.WriteLine($"       25 -> {nameof(Unix_Spi_Pressure)}");
            Console.WriteLine($"       26 -> {nameof(RaspberryPi_Spi_Pressure)}");
            Console.WriteLine();
            Console.WriteLine($"       27 -> {nameof(Unix_Spi_Pressure_Lcd)}");
            Console.WriteLine($"       28 -> {nameof(RaspberryPi_Spi_Pressure_Lcd)}");
            Console.WriteLine();
            Console.WriteLine($"       29 -> {nameof(I2c_Pressure)}");
            Console.WriteLine();
            Console.WriteLine($"       30 -> {nameof(Unix_I2c_Pressure_Lcd)}");
            Console.WriteLine($"       31 -> {nameof(RaspberryPi_I2c_Pressure_Lcd)}");
            Console.WriteLine();
            Console.WriteLine($"       32 -> {nameof(I2c_Color)}");
            Console.WriteLine();
        }

        private static void Unix_BlinkingLed()
        {
            Console.WriteLine(nameof(Unix_BlinkingLed));
            BlinkingLed(new UnixDriver(RaspberryPiPinCount));
        }

        private static void Unix_ButtonLed()
        {
            Console.WriteLine(nameof(Unix_ButtonLed));
            ButtonLed(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_BlinkingLed()
        {
            Console.WriteLine(nameof(RaspberryPi_BlinkingLed));
            BlinkingLed(new RaspberryPiDriver());
        }

        private static void RaspberryPi_ButtonLed()
        {
            Console.WriteLine(nameof(RaspberryPi_ButtonLed));
            ButtonLed(new RaspberryPiDriver());
        }

        private static void BlinkingLed(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
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

        private static void ButtonLed(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
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

        private static void UnixDriver_BlinkingLed()
        {
            Console.WriteLine(nameof(UnixDriver_BlinkingLed));
            Driver_BlinkingLed(new UnixDriver(RaspberryPiPinCount));
        }

        private static void UnixDriver_ButtonLed()
        {
            Console.WriteLine(nameof(UnixDriver_ButtonLed));
            Driver_ButtonLed(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPiDriver_BlinkingLed()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_BlinkingLed));
            Driver_BlinkingLed(new RaspberryPiDriver());
        }

        private static void RaspberryPiDriver_ButtonLed()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_ButtonLed));
            Driver_ButtonLed(new RaspberryPiDriver());
        }

        private static void Driver_BlinkingLed(GpioDriver driver)
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

        private static void Driver_ButtonLed(GpioDriver driver)
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
            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
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
            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
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

        private static void UnixDriver_DetectButtonLed()
        {
            Console.WriteLine(nameof(UnixDriver_DetectButtonLed));
            Driver_DetectButtonLed(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPiDriver_DetectButtonLed()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_DetectButtonLed));
            Driver_DetectButtonLed(new RaspberryPiDriver());
        }

        private static void Driver_DetectButtonLed(GpioDriver driver)
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

        private static void Unix_DetectButtonLed()
        {
            Console.WriteLine(nameof(Unix_DetectButtonLed));
            DetectButtonLed(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_DetectButtonLed()
        {
            Console.WriteLine(nameof(RaspberryPi_DetectButtonLed));
            DetectButtonLed(new RaspberryPiDriver());
        }

        private static void DetectButtonLed(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
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
            Console.WriteLine($"Button pressed! Led value {s_currentLedValue}");

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
            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
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

        private static void Unix_Lcd()
        {
            Console.WriteLine(nameof(Unix_Lcd));
            Lcd(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_Lcd()
        {
            Console.WriteLine(nameof(RaspberryPi_Lcd));
            Lcd(new RaspberryPiDriver());
        }

        private static void Lcd(GpioDriver driver)
        {
            const int registerSelectPinNumber = 0;
            const int enablePinNumber = 5;
            int[] dataPinNumbers = { 6, 16, 20, 21 };

            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
            {
                Pin registerSelectPin = controller.OpenPin(registerSelectPinNumber);
                Pin enablePin = controller.OpenPin(enablePinNumber);
                Pin[] dataPins = controller.OpenPins(dataPinNumbers);

                var lcd = new LcdController(registerSelectPin, enablePin, dataPins);
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

        private static void Unix_Spi_Pressure()
        {
            Console.WriteLine(nameof(Unix_Spi_Pressure));
            Spi_Bme280(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_Spi_Pressure()
        {
            Console.WriteLine(nameof(RaspberryPi_Spi_Pressure));
            Spi_Bme280(new RaspberryPiDriver());
        }

        private static void Spi_Bme280(GpioDriver driver)
        {
            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
            {
                Pin csPin = controller.OpenPin(8);

                var settings = new SpiConnectionSettings(0, 0);
                var sensor = new PressureTemperatureHumiditySensor(csPin, settings);
                Bme280(sensor);
            }
        }

        private static void I2c_Pressure()
        {
            Console.WriteLine(nameof(I2c_Pressure));
            var settings = new I2cConnectionSettings(1);
            var sensor = new PressureTemperatureHumiditySensor(settings);
            Bme280(sensor);
        }

        private static void Bme280(PressureTemperatureHumiditySensor sensor)
        {
            using (sensor)
            {
                bool ok = sensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing sensor");
                    return;
                }

                Console.WriteLine($"Pressure (hPa/mb)\tHumdity (%)\tTemp (C)\tTemp (F)");
                Console.WriteLine();

                for (var i = 0; i < 5; ++i)
                {
                    sensor.ReadSensor();

                    Console.WriteLine($"{sensor.PressureInHectopascals:0.00} hPa\t\t{sensor.Humidity:0.00} %\t\t{sensor.TemperatureInCelsius:0.00} C\t\t{sensor.TemperatureInFahrenheit:0.00} F");
                    Thread.Sleep(1 * 1000);
                }
            }
        }

        private static void Unix_Spi_Pressure_Lcd()
        {
            Console.WriteLine(nameof(Unix_Spi_Pressure_Lcd));
            Spi_Pressure_Lcd(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_Spi_Pressure_Lcd()
        {
            Console.WriteLine(nameof(RaspberryPi_Spi_Pressure_Lcd));
            Spi_Pressure_Lcd(new RaspberryPiDriver());
        }

        private static void Unix_I2c_Pressure_Lcd()
        {
            Console.WriteLine(nameof(Unix_I2c_Pressure_Lcd));
            I2c_Pressure_Lcd(new UnixDriver(RaspberryPiPinCount));
        }

        private static void RaspberryPi_I2c_Pressure_Lcd()
        {
            Console.WriteLine(nameof(RaspberryPi_I2c_Pressure_Lcd));
            I2c_Pressure_Lcd(new RaspberryPiDriver());
        }

        private static void Spi_Pressure_Lcd(GpioDriver driver)
        {
            const int registerSelectPinNumber = 0;
            const int enablePinNumber = 5;
            const int chipSelectLinePinNumber = 8;
            int[] dataPinNumbers = { 6, 16, 20, 21 };

            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
            {
                Pin registerSelectPin = controller.OpenPin(registerSelectPinNumber);
                Pin enablePin = controller.OpenPin(enablePinNumber);
                Pin[] dataPins = controller.OpenPins(dataPinNumbers);

                var lcd = new LcdController(registerSelectPin, enablePin, dataPins);
                lcd.Begin(16, 2);

                Pin chipSelectLinePin = controller.OpenPin(chipSelectLinePinNumber);

                var settings = new SpiConnectionSettings(0, 0);
                var sensor = new PressureTemperatureHumiditySensor(chipSelectLinePin, settings);
                Pressure_Lcd(lcd, sensor);
            }
        }

        private static void I2c_Pressure_Lcd(GpioDriver driver)
        {
            const int registerSelectPinNumber = 0;
            const int enablePinNumber = 5;
            int[] dataPinNumbers = { 6, 16, 20, 21 };

            using (var controller = new GpioController(driver, PinNumberingScheme.Bcm))
            {
                Pin registerSelectPin = controller.OpenPin(registerSelectPinNumber);
                Pin enablePin = controller.OpenPin(enablePinNumber);
                Pin[] dataPins = controller.OpenPins(dataPinNumbers);

                var lcd = new LcdController(registerSelectPin, enablePin, dataPins);
                lcd.Begin(16, 2);

                var settings = new I2cConnectionSettings(1);
                var sensor = new PressureTemperatureHumiditySensor(settings);
                Pressure_Lcd(lcd, sensor);
            }
        }

        private static void Pressure_Lcd(LcdController lcd, PressureTemperatureHumiditySensor sensor)
        {
            using (sensor)
            {
                bool ok = sensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing sensor");
                    return;
                }

                Console.WriteLine($"Pressure (hPa/mb)\tHumdity (%)\tTemp (C)\tTemp (F)");
                Console.WriteLine();

                for (var i = 0; i < 3; ++i)
                {
                    sensor.ReadSensor();

                    Console.WriteLine($"{sensor.PressureInHectopascals:0.00} hPa\t\t{sensor.Humidity:0.00} %\t\t{sensor.TemperatureInCelsius:0.00} C\t\t{sensor.TemperatureInFahrenheit:0.00} F");

                    ShowInfo(lcd, "Pressure", $"{sensor.PressureInHectopascals:0.00} hPa/mb");
                    ShowInfo(lcd, "Humdity", $"{sensor.Humidity:0.00} %");
                    ShowInfo(lcd, "Temperature", $"{sensor.TemperatureInCelsius:0.00} C, {sensor.TemperatureInFahrenheit:0.00} F");
                }
            }
        }

        private static void ShowInfo(LcdController lcd, string label, string value)
        {
            lcd.Clear();
            lcd.SetCursor(0, 0);
            lcd.Print(label);

            lcd.SetCursor(0, 1);
            lcd.Print(value);

            Thread.Sleep(3 * 1000);
        }

        private static void I2c_Color()
        {
            var settings = new I2cConnectionSettings(1);
            using (var sensor = new RgbColorSensor(settings))
            {
                bool ok = sensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing sensor");
                    return;
                }

                Console.WriteLine($"Color (rgb)\t\tTemperature (K)\tLuminosity (lux)");
                Console.WriteLine();

                for (var i = 0; i < 5; ++i)
                {
                    sensor.ReadSensor();

                    Console.WriteLine($"{ToRgbString(sensor.Color)}\t{sensor.Temperature:0.00} K\t{sensor.Luminosity:0.00} lux");
                    Thread.Sleep(1 * 1000);
                }
            }
        }

        private static string ToRgbString(Drawing.Color color)
        {
            return $"R: {color.R}, G: {color.G}, B: {color.B}";
        }
    }
}
