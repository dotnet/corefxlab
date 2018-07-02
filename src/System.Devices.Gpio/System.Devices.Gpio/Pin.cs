// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public enum PinMode
    {
        Input,
        Output,
        InputPullDown,
        InputPullUp
    }

    public enum PinValue
    {
        Low = 0,
        High = 1
    }

    public enum EventKind
    {
        Low,
        High,
        SyncFallingEdge,
        SyncRisingEdge,
        AsyncFallingEdge,
        AsyncRisingEdge
    }

    public enum PinNumberingScheme
    {
        Board,
        BCM
    }

    public class Pin
    {
        protected GpioDriver _driver;

        public int Number { get; }

        public PinMode Mode
        {
            get => _driver.GetPinMode(Number);
            set => _driver.SetPinMode(Number, value);
        }

        public Pin(GpioDriver driver, PinNumberingScheme numbering, int number, PinMode mode)
        {
            _driver = driver;
            Number = driver.ConvertPinNumber(number, numbering, PinNumberingScheme.BCM);
            Mode = mode;
        }

        public int GetNumber(PinNumberingScheme numbering)
        {
            return _driver.ConvertPinNumber(Number, PinNumberingScheme.BCM, numbering);
        }

        public PinValue Read()
        {
            return _driver.Input(Number);
        }

        public void Write(PinValue value)
        {
            _driver.Output(Number, value);
        }
    }
}
