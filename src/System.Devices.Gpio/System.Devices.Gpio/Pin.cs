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
    }

    public enum PinNumberingScheme
    {
        Board,
        BCM
    }

    public class Pin : IDisposable
    {
        internal Pin(GpioController controller, int bcmNumber)
        {
            Controller = controller;
            BcmNumber = bcmNumber;
        }

        public void Dispose()
        {
            Controller.ClosePin(this);
        }

        public event EventHandler<PinValueChangedEventArgs> ValueChanged;

        public GpioController Controller { get; }

        public int BcmNumber { get; }

        public TimeSpan DebounceTimeout
        {
            get => Controller.Driver.GetDebounce(BcmNumber);
            set => Controller.Driver.SetDebounce(BcmNumber, value);
        }

        public PinMode Mode
        {
            get => Controller.Driver.GetPinMode(BcmNumber);
            set => Controller.Driver.SetPinMode(BcmNumber, value);
        }

        public PinEvent DetectedEvents
        {
            get => Controller.Driver.GetPinEventsToDetect(BcmNumber);
            set => Controller.Driver.SetPinEventsToDetect(BcmNumber, value);
        }

        public bool IsModeSupported(PinMode mode) => Controller.Driver.IsPinModeSupported(mode);

        public int GetNumber(PinNumberingScheme numbering) => Controller.Driver.ConvertPinNumber(BcmNumber, PinNumberingScheme.BCM, numbering);

        public PinValue Read() => Controller.Driver.Input(BcmNumber);

        public void Write(PinValue value) => Controller.Driver.Output(BcmNumber, value);

        public bool WaitForEvent(TimeSpan timeout) => Controller.Driver.WaitForPinEvent(BcmNumber, timeout);

        internal void OnValueChanged(PinValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}
