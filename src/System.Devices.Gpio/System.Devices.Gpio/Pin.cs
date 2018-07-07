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
    public enum PinEvent
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
        EdgeBoth = SyncBoth | AsyncBoth,

        RisingEdge = SyncRisingEdge | AsyncRisingEdge,
        FallingEdge = SyncFallingEdge | AsyncFallingEdge,
    }

    public enum PinNumberingScheme
    {
        Board,
        BCM
    }

    public class Pin
    {
        protected GpioDriver _driver;

        public int BCMNumber { get; }

        public PinMode Mode
        {
            get => _driver.GetPinMode(BCMNumber);
            set => _driver.SetPinMode(BCMNumber, value);
        }

        public Pin(GpioDriver driver, PinNumberingScheme numbering, int number, PinMode mode)
        {
            _driver = driver;
            BCMNumber = driver.ConvertPinNumber(number, numbering, PinNumberingScheme.BCM);
            Mode = mode;
        }

        public int GetNumber(PinNumberingScheme numbering)
        {
            return _driver.ConvertPinNumber(BCMNumber, PinNumberingScheme.BCM, numbering);
        }

        public PinValue Read()
        {
            return _driver.Input(BCMNumber);
        }

        public void Write(PinValue value)
        {
            _driver.Output(BCMNumber, value);
        }
    }
}
