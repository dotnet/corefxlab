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
        private enum DeviceKind
        {
            Unknown,
            RaspberryPi = 1,
            Odroid = 2,
            Hummingboard = 3
        }

        private enum RaspberryPiSettings
        {
            Led = 26,
            Button = 18,
            SpiBusId = 0,
            SpiChipSelectLine = 8,
            I2cBusId = 1
        }

        private enum OdroidSettings
        {
            Led = 30,
            Button = 29,
            SpiBusId = 1,
            SpiChipSelectLine = 31,
            I2cBusId = 1
        }

        private enum HummingboardSettings
        {
            Led = 73,
            Button = 69,
            SpiBusId = 1,
            SpiChipSelectLine = 68,
            I2cBusId = 2
        }

        private static int s_ledPinNumber;
        private static int s_buttonPinNumber;
        private static uint s_spiBusId;
        private static int s_chipSelectLinePinNumber;
        private static uint s_i2cBusId;

        internal static uint I2cBusId => s_i2cBusId;

        private static void Main(string[] args)
        {
            try
            {
                DeviceKind device = DeviceKind.Unknown;
                int option = -1;

                if (args.Length == 2)
                {
                    Enum.TryParse(args[0], out device);
                    option = Convert.ToInt32(args[1]);
                }
                else
                {
                    ShowUsage();
                    return;
                }

                switch (device)
                {
                    case DeviceKind.RaspberryPi:
                        s_ledPinNumber = (int)RaspberryPiSettings.Led;
                        s_buttonPinNumber = (int)RaspberryPiSettings.Button;
                        s_spiBusId = (uint)RaspberryPiSettings.SpiBusId;
                        s_chipSelectLinePinNumber = (int)RaspberryPiSettings.SpiChipSelectLine;
                        s_i2cBusId = (uint)RaspberryPiSettings.I2cBusId;
                        break;

                    case DeviceKind.Odroid:
                        s_ledPinNumber = (int)OdroidSettings.Led;
                        s_buttonPinNumber = (int)OdroidSettings.Button;
                        s_spiBusId = (uint)OdroidSettings.SpiBusId;
                        s_chipSelectLinePinNumber = (int)OdroidSettings.SpiChipSelectLine;
                        s_i2cBusId = (uint)OdroidSettings.I2cBusId;
                        break;

                    case DeviceKind.Hummingboard:
                        s_ledPinNumber = (int)HummingboardSettings.Led;
                        s_buttonPinNumber = (int)HummingboardSettings.Button;
                        s_spiBusId = (uint)HummingboardSettings.SpiBusId;
                        s_chipSelectLinePinNumber = (int)HummingboardSettings.SpiChipSelectLine;
                        s_i2cBusId = (uint)HummingboardSettings.I2cBusId;
                        break;

                    default:
                        Console.WriteLine("Unknown device");
                        ShowUsage();
                        return;
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

                    case 33:
                        AzureIoTSendData();
                        break;
                    case 34:
                        AzureIoTSendCommands();
                        break;
                    case 35:
                        AzureIoTReceiveCommands();
                        break;

                    default:
                        Console.WriteLine("Unknown sample");
                        ShowUsage();
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Done!");
            }
        }

        private static void ShowUsage()
        {
            string assemblyName = Reflection.Assembly.GetEntryAssembly().GetName().Name;

            Console.WriteLine($"Usage: {assemblyName} <device> <sample>");
            Console.WriteLine("       where <device> can be any of the following options:");
            Console.WriteLine();
            Console.WriteLine($"        {(int)DeviceKind.RaspberryPi } -> {nameof(DeviceKind.RaspberryPi)}");
            Console.WriteLine($"        {(int)DeviceKind.Odroid      } -> {nameof(DeviceKind.Odroid)}");
            Console.WriteLine($"        {(int)DeviceKind.Hummingboard} -> {nameof(DeviceKind.Hummingboard)}");
            Console.WriteLine();
            Console.WriteLine("       and <sample> can be any of the following options:");
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
            Console.WriteLine($"       33 -> {nameof(AzureIoTSendData)}");
            Console.WriteLine($"       34 -> {nameof(AzureIoTSendCommands)}");
            Console.WriteLine($"       35 -> {nameof(AzureIoTReceiveCommands)}");
            Console.WriteLine();
        }

        internal static void Unix_BlinkingLed()
        {
            Console.WriteLine(nameof(Unix_BlinkingLed));
            BlinkingLed(new UnixDriver());
        }

        private static void Unix_ButtonLed()
        {
            Console.WriteLine(nameof(Unix_ButtonLed));
            ButtonLed(new UnixDriver());
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
            using (var controller = new GpioController(driver))
            {
                GpioPin led = controller.OpenPin(s_ledPinNumber, PinMode.Output);

                for (var i = 0; i < 5; ++i)
                {
                    led.Write(PinValue.High);
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    led.Write(PinValue.Low);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private static void ButtonLed(GpioDriver driver)
        {
            using (var controller = new GpioController(driver))
            {
                GpioPin button = controller.OpenPin(s_buttonPinNumber, PinMode.Input);
                GpioPin led = controller.OpenPin(s_ledPinNumber, PinMode.Output);

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
            Driver_BlinkingLed(new UnixDriver());
        }

        private static void UnixDriver_ButtonLed()
        {
            Console.WriteLine(nameof(UnixDriver_ButtonLed));
            Driver_ButtonLed(new UnixDriver());
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
            using (driver)
            {
                driver.OpenPin(s_ledPinNumber);
                driver.SetPinMode(s_ledPinNumber, PinMode.Output);

                for (var i = 0; i < 5; ++i)
                {
                    driver.Output(s_ledPinNumber, PinValue.High);
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    driver.Output(s_ledPinNumber, PinValue.Low);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private static void Driver_ButtonLed(GpioDriver driver)
        {
            using (driver)
            {
                driver.OpenPin(s_buttonPinNumber);
                driver.SetPinMode(s_buttonPinNumber, PinMode.Input);

                driver.OpenPin(s_ledPinNumber);
                driver.SetPinMode(s_ledPinNumber, PinMode.Output);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    PinValue value = driver.Input(s_buttonPinNumber);
                    driver.Output(s_ledPinNumber, value);
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
            using (driver)
            {
                driver.OpenPin(s_buttonPinNumber);
                driver.SetPinMode(s_buttonPinNumber, PinMode.InputPullDown);

                driver.OpenPin(s_ledPinNumber);
                driver.SetPinMode(s_ledPinNumber, PinMode.Output);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    PinValue value = driver.Input(s_buttonPinNumber);
                    driver.Output(s_ledPinNumber, value);
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
            using (var controller = new GpioController(driver))
            {
                GpioPin button = controller.OpenPin(s_buttonPinNumber, PinMode.InputPullDown);
                GpioPin led = controller.OpenPin(s_ledPinNumber, PinMode.Output);

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
            Driver_DetectButton(new UnixDriver());
        }

        private static void RaspberryPiDriver_DetectButton()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_DetectButton));
            Driver_DetectButton(new RaspberryPiDriver());
        }

        private static void Driver_DetectButton(GpioDriver driver)
        {
            using (driver)
            {
                PinMode buttonMode = PinMode.Input;

                if (driver.IsPinModeSupported(PinMode.InputPullDown))
                {
                    buttonMode = PinMode.InputPullDown;
                }

                driver.OpenPin(s_buttonPinNumber);
                driver.SetPinMode(s_buttonPinNumber, buttonMode);

                driver.SetDebounce(s_buttonPinNumber, TimeSpan.FromMilliseconds(100));
                driver.SetPinEventsToDetect(s_buttonPinNumber, PinEvent.SyncFallingRisingEdge);
                driver.ValueChanged += OnPinValueChanged1;
                driver.SetEnableRaisingPinEvents(s_buttonPinNumber, true);

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
            DetectButton(new UnixDriver());
        }

        private static void RaspberryPi_DetectButton()
        {
            Console.WriteLine(nameof(RaspberryPi_DetectButton));
            DetectButton(new RaspberryPiDriver());
        }

        private static void DetectButton(GpioDriver driver)
        {
            using (var controller = new GpioController(driver))
            {
                GpioPin button = controller.OpenPin(s_buttonPinNumber, PinMode.Input);

                if (button.IsModeSupported(PinMode.InputPullDown))
                {
                    button.Mode = PinMode.InputPullDown;
                }

                button.DebounceTimeout = TimeSpan.FromMilliseconds(100);
                button.NotifyEvents = PinEvent.SyncFallingRisingEdge;
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
                Console.WriteLine();
            }

            s_buttonPressed = !s_buttonPressed;
        }

        private static void UnixDriver_DetectButtonLed()
        {
            Console.WriteLine(nameof(UnixDriver_DetectButtonLed));
            Driver_DetectButtonLed(new UnixDriver());
        }

        private static void RaspberryPiDriver_DetectButtonLed()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_DetectButtonLed));
            Driver_DetectButtonLed(new RaspberryPiDriver());
        }

        private static void Driver_DetectButtonLed(GpioDriver driver)
        {
            using (driver)
            {
                PinMode buttonMode = PinMode.Input;

                if (driver.IsPinModeSupported(PinMode.InputPullDown))
                {
                    buttonMode = PinMode.InputPullDown;
                }

                driver.OpenPin(s_buttonPinNumber);
                driver.SetPinMode(s_buttonPinNumber, buttonMode);

                driver.OpenPin(s_ledPinNumber);
                driver.SetPinMode(s_ledPinNumber, PinMode.Output);

                driver.SetDebounce(s_buttonPinNumber, TimeSpan.FromSeconds(1));
                driver.SetPinEventsToDetect(s_buttonPinNumber, PinEvent.SyncFallingEdge);
                driver.ValueChanged += OnPinValueChanged2;
                driver.SetEnableRaisingPinEvents(s_buttonPinNumber, true);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private static void Unix_DetectButtonLed()
        {
            Console.WriteLine(nameof(Unix_DetectButtonLed));
            DetectButtonLed(new UnixDriver());
        }

        private static void RaspberryPi_DetectButtonLed()
        {
            Console.WriteLine(nameof(RaspberryPi_DetectButtonLed));
            DetectButtonLed(new RaspberryPiDriver());
        }

        private static void DetectButtonLed(GpioDriver driver)
        {
            using (var controller = new GpioController(driver))
            {
                GpioPin button = controller.OpenPin(s_buttonPinNumber, PinMode.Input);

                if (button.IsModeSupported(PinMode.InputPullDown))
                {
                    button.Mode = PinMode.InputPullDown;
                }

                GpioPin led = controller.OpenPin(s_ledPinNumber, PinMode.Output);

                button.DebounceTimeout = TimeSpan.FromSeconds(1);
                button.NotifyEvents = PinEvent.SyncFallingEdge;
                button.ValueChanged += OnPinValueChanged2;
                button.EnableRaisingEvents = true;

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private static PinValue s_currentLedValue = PinValue.Low;

        private static void OnPinValueChanged2(object sender, PinValueChangedEventArgs e)
        {
            s_currentLedValue = s_currentLedValue == PinValue.High ? PinValue.Low : PinValue.High;
            Console.WriteLine($"Button pressed! Led value {s_currentLedValue}");

            if (sender is GpioDriver)
            {
                GpioDriver driver = sender as GpioDriver;
                driver.Output(s_ledPinNumber, s_currentLedValue);
            }
            else if (sender is GpioPin)
            {
                GpioPin button = sender as GpioPin;
                GpioController controller = button.Controller;
                GpioPin led = controller[s_ledPinNumber];
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
            Driver_ButtonWait(new UnixDriver());
        }

        private static void RaspberryPiDriver_ButtonWait()
        {
            Console.WriteLine(nameof(RaspberryPiDriver_ButtonWait));
            Driver_ButtonWait(new RaspberryPiDriver());
        }

        private static void Driver_ButtonWait(GpioDriver driver)
        {
            using (driver)
            {
                PinMode buttonMode = PinMode.Input;

                if (driver.IsPinModeSupported(PinMode.InputPullDown))
                {
                    buttonMode = PinMode.InputPullDown;
                }

                driver.OpenPin(s_buttonPinNumber);
                driver.SetPinMode(s_buttonPinNumber, buttonMode);

                driver.SetDebounce(s_buttonPinNumber, TimeSpan.FromSeconds(1));
                driver.SetPinEventsToDetect(s_buttonPinNumber, PinEvent.SyncRisingEdge);

                Stopwatch watch = Stopwatch.StartNew();

                while (watch.Elapsed.TotalSeconds < 15)
                {
                    bool eventDetected = driver.WaitForPinEvent(s_buttonPinNumber, TimeSpan.FromSeconds(1));

                    if (eventDetected)
                    {
                        Console.WriteLine("Button pressed!");
                    }
                }
            }
        }

        private static void Unix_ButtonWait()
        {
            Console.WriteLine(nameof(Unix_ButtonWait));
            ButtonWait(new UnixDriver());
        }

        private static void RaspberryPi_ButtonWait()
        {
            Console.WriteLine(nameof(RaspberryPi_ButtonWait));
            ButtonWait(new RaspberryPiDriver());
        }

        private static void ButtonWait(GpioDriver driver)
        {
            using (var controller = new GpioController(driver))
            {
                GpioPin button = controller.OpenPin(s_buttonPinNumber, PinMode.Input);

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
                        Console.WriteLine("Button pressed!");
                    }
                }
            }
        }

        private static void Unix_Lcd()
        {
            Console.WriteLine(nameof(Unix_Lcd));
            Lcd(new UnixDriver());
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

            using (var controller = new GpioController(driver))
            {
                GpioPin registerSelectPin = controller.OpenPin(registerSelectPinNumber);
                GpioPin enablePin = controller.OpenPin(enablePinNumber);
                GpioPin[] dataPins = controller.OpenPins(dataPinNumbers);

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

        internal static void Lcd(string message)
        {
            const int registerSelectPinNumber = 0;
            const int enablePinNumber = 5;
            int[] dataPinNumbers = { 6, 16, 20, 21 };

            using (var driver = new UnixDriver())
            using (var controller = new GpioController(driver))
            {
                GpioPin registerSelectPin = controller.OpenPin(registerSelectPinNumber);
                GpioPin enablePin = controller.OpenPin(enablePinNumber);
                GpioPin[] dataPins = controller.OpenPins(dataPinNumbers);

                var lcd = new LcdController(registerSelectPin, enablePin, dataPins);
                lcd.Begin(16, 2);
                lcd.Print(message);
            }
        }

        private static void Spi_Roundtrip()
        {
            Console.WriteLine(nameof(Spi_Roundtrip));

            // For this sample connect SPI MOSI with SPI MISO.
            var settings = new SpiConnectionSettings(s_spiBusId, 0);
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
            Spi_Pressure(new UnixDriver());
        }

        private static void RaspberryPi_Spi_Pressure()
        {
            Console.WriteLine(nameof(RaspberryPi_Spi_Pressure));
            Spi_Pressure(new RaspberryPiDriver());
        }

        private static void Spi_Pressure(GpioDriver driver)
        {
            using (var controller = new GpioController(driver))
            {
                GpioPin chipSelectPin = controller.OpenPin(s_chipSelectLinePinNumber);

                var settings = new SpiConnectionSettings(s_spiBusId, 0);
                var sensor = new PressureTemperatureHumiditySensor(chipSelectPin, settings);
                Pressure(sensor);
            }
        }

        private static void I2c_Pressure()
        {
            Console.WriteLine(nameof(I2c_Pressure));

            var settings = new I2cConnectionSettings(s_i2cBusId, PressureTemperatureHumiditySensor.DefaultI2cAddress);
            var sensor = new PressureTemperatureHumiditySensor(settings);
            Pressure(sensor);
        }

        private static void Pressure(PressureTemperatureHumiditySensor sensor)
        {
            using (sensor)
            {
                sensor.SeaLevelPressureInHectopascals = 1013.25f;
                bool ok = sensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing sensor");
                    return;
                }

                for (var i = 0; i < 5; ++i)
                {
                    sensor.ReadSensor();

                    Console.WriteLine($"Pressure:    {sensor.PressureInHectopascals:0.00} hPa");
                    Console.WriteLine($"Humdity:     {sensor.Humidity:0.00} %");
                    Console.WriteLine($"Temperature: {sensor.TemperatureInCelsius:0.00} C, {sensor.TemperatureInFahrenheit:0.00} F");
                    Console.WriteLine($"Altitude:    {sensor.AltitudeInMeters:0.00} m, {sensor.AltitudInFeet:0.00} ft");
                    Console.WriteLine();

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private static void Unix_Spi_Pressure_Lcd()
        {
            Console.WriteLine(nameof(Unix_Spi_Pressure_Lcd));
            Spi_Pressure_Lcd(new UnixDriver());
        }

        private static void RaspberryPi_Spi_Pressure_Lcd()
        {
            Console.WriteLine(nameof(RaspberryPi_Spi_Pressure_Lcd));
            Spi_Pressure_Lcd(new RaspberryPiDriver());
        }

        internal static void Unix_I2c_Pressure_Lcd()
        {
            Console.WriteLine(nameof(Unix_I2c_Pressure_Lcd));
            I2c_Pressure_Lcd(new UnixDriver());
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

            using (var controller = new GpioController(driver))
            {
                GpioPin registerSelectPin = controller.OpenPin(registerSelectPinNumber);
                GpioPin enablePin = controller.OpenPin(enablePinNumber);
                GpioPin[] dataPins = controller.OpenPins(dataPinNumbers);

                var lcd = new LcdController(registerSelectPin, enablePin, dataPins);
                lcd.Begin(16, 2);

                GpioPin chipSelectLinePin = controller.OpenPin(chipSelectLinePinNumber);

                var settings = new SpiConnectionSettings(s_spiBusId, 0);
                var sensor = new PressureTemperatureHumiditySensor(chipSelectLinePin, settings);
                Pressure_Lcd(lcd, sensor);
            }
        }

        private static void I2c_Pressure_Lcd(GpioDriver driver)
        {
            const int registerSelectPinNumber = 0;
            const int enablePinNumber = 5;
            int[] dataPinNumbers = { 6, 16, 20, 21 };

            using (var controller = new GpioController(driver))
            {
                GpioPin registerSelectPin = controller.OpenPin(registerSelectPinNumber);
                GpioPin enablePin = controller.OpenPin(enablePinNumber);
                GpioPin[] dataPins = controller.OpenPins(dataPinNumbers);

                var lcd = new LcdController(registerSelectPin, enablePin, dataPins);
                lcd.Begin(16, 2);

                var settings = new I2cConnectionSettings(s_i2cBusId, PressureTemperatureHumiditySensor.DefaultI2cAddress);
                var sensor = new PressureTemperatureHumiditySensor(settings);
                Pressure_Lcd(lcd, sensor);
            }
        }

        private static void Pressure_Lcd(LcdController lcd, PressureTemperatureHumiditySensor sensor)
        {
            using (sensor)
            {
                sensor.SeaLevelPressureInHectopascals = 1013.25f;
                bool ok = sensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing sensor");
                    return;
                }

                for (var i = 0; i < 3; ++i)
                {
                    sensor.ReadSensor();

                    Console.WriteLine($"Pressure:    {sensor.PressureInHectopascals:0.00} hPa");
                    Console.WriteLine($"Humdity:     {sensor.Humidity:0.00} %");
                    Console.WriteLine($"Temperature: {sensor.TemperatureInCelsius:0.00} C, {sensor.TemperatureInFahrenheit:0.00} F");
                    Console.WriteLine($"Altitude:    {sensor.AltitudeInMeters:0.00} m, {sensor.AltitudInFeet:0.00} ft");
                    Console.WriteLine();

                    ShowInfo(lcd, "Pressure", $"{sensor.PressureInHectopascals:0.00} hPa/mb");
                    ShowInfo(lcd, "Humdity", $"{sensor.Humidity:0.00} %");
                    ShowInfo(lcd, "Temperature", $"{sensor.TemperatureInCelsius:0.00} C, {sensor.TemperatureInFahrenheit:0.00} F");
                    ShowInfo(lcd, "Altitude", $"{sensor.AltitudeInMeters:0.00} m, {sensor.AltitudInFeet:0.00} ft");
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

            Thread.Sleep(TimeSpan.FromSeconds(3));
        }

        private static void I2c_Color()
        {
            Console.WriteLine(nameof(I2c_Color));

            var settings = new I2cConnectionSettings(s_i2cBusId, RgbColorSensor.DefaultI2cAddress);
            using (var sensor = new RgbColorSensor(settings))
            {
                bool ok = sensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing sensor");
                    return;
                }

                for (var i = 0; i < 5; ++i)
                {
                    sensor.ReadSensor();

                    Console.WriteLine($"Color:       {ToRgbString(sensor.Color)}");
                    Console.WriteLine($"Temperature: {sensor.Temperature:0.00} K");
                    Console.WriteLine($"Luminosity:  {sensor.Luminosity:0.00} lux");
                    Console.WriteLine();

                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        internal static string ToRgbString(Drawing.Color color)
        {
            return $"R {color.R} G {color.G} B {color.B}";
        }

        internal static void Unix_I2c_Color_Lcd()
        {
            Console.WriteLine(nameof(Unix_I2c_Color_Lcd));
            I2c_Color_Lcd(new UnixDriver());
        }

        private static void RaspberryPi_I2c_Color_Lcd()
        {
            Console.WriteLine(nameof(RaspberryPi_I2c_Color_Lcd));
            I2c_Color_Lcd(new RaspberryPiDriver());
        }

        private static void I2c_Color_Lcd(GpioDriver driver)
        {
            const int registerSelectPinNumber = 0;
            const int enablePinNumber = 5;
            int[] dataPinNumbers = { 6, 16, 20, 21 };

            using (var controller = new GpioController(driver))
            {
                GpioPin registerSelectPin = controller.OpenPin(registerSelectPinNumber);
                GpioPin enablePin = controller.OpenPin(enablePinNumber);
                GpioPin[] dataPins = controller.OpenPins(dataPinNumbers);

                var lcd = new LcdController(registerSelectPin, enablePin, dataPins);
                lcd.Begin(16, 2);

                var settings = new I2cConnectionSettings(s_i2cBusId, RgbColorSensor.DefaultI2cAddress);
                var sensor = new RgbColorSensor(settings);
                Color_Lcd(lcd, sensor);
            }
        }

        private static void Color_Lcd(LcdController lcd, RgbColorSensor sensor)
        {
            using (sensor)
            {
                bool ok = sensor.Begin();

                if (!ok)
                {
                    Console.WriteLine($"Error initializing sensor");
                    return;
                }

                for (var i = 0; i < 3; ++i)
                {
                    sensor.ReadSensor();

                    Console.WriteLine($"Color:       {ToRgbString(sensor.Color)}");
                    Console.WriteLine($"Temperature: {sensor.Temperature:0.00} K");
                    Console.WriteLine($"Luminosity:  {sensor.Luminosity:0.00} lux");
                    Console.WriteLine();

                    ShowInfo(lcd, "Color", ToRgbString(sensor.Color));
                    ShowInfo(lcd, "Temperature", $"{sensor.Temperature:0.00} K");
                    ShowInfo(lcd, "Luminosity", $"{sensor.Luminosity:0.00} lux");
                }
            }
        }

        private static void AzureIoTSendData()
        {
            Console.WriteLine(nameof(AzureIoTSendData));

            var sample = new AzureIoTSample();
            sample.StartSendingData();
        }

        private static void AzureIoTSendCommands()
        {
            Console.WriteLine(nameof(AzureIoTSendCommands));

            var sample = new AzureIoTSample();
            sample.StartSendingCommands();
        }

        private static void AzureIoTReceiveCommands()
        {
            Console.WriteLine(nameof(AzureIoTReceiveCommands));

            var sample = new AzureIoTSample();
            sample.StartReceivingCommands();
        }
    }
}
