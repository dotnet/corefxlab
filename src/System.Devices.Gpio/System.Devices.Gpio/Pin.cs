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

    [Flags]
    public enum EventKind
    {
        None = 0,
        Low = 1,
        High = 2,
        SyncFallingEdge = 4,
        SyncRisingEdge = 8,
        AsyncFallingEdge = 16,
        AsyncRisingEdge = 32,

        Both = Low | High,
        SyncBoth = SyncFallingEdge | SyncRisingEdge,
        AsyncBoth = AsyncFallingEdge | AsyncRisingEdge,
    }

    public enum PinNumberingScheme
    {
        Board,
        BCM
    }

    public class Pin
    {
        protected GpioDriver _driver;

        public int BcmNumber { get; }

        public PinMode Mode
        {
            get => _driver.GetPinMode(BcmNumber);
            set => _driver.SetPinMode(BcmNumber, value);
        }

        public Pin(GpioDriver driver, PinNumberingScheme numbering, int number, PinMode mode)
        {
            _driver = driver;
            BcmNumber = driver.ConvertPinNumber(number, numbering, PinNumberingScheme.BCM);
            Mode = mode;
        }

        public int GetNumber(PinNumberingScheme numbering)
        {
            return _driver.ConvertPinNumber(BcmNumber, PinNumberingScheme.BCM, numbering);
        }

        public PinValue Read()
        {
            return _driver.Input(BcmNumber);
        }

        public void Write(PinValue value)
        {
            _driver.Output(BcmNumber, value);
        }
    }
}
