// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
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

        public PinEvent NotifyEvents
        {
            get => Controller.Driver.GetPinEventsToDetect(BcmNumber);
            set => Controller.Driver.SetPinEventsToDetect(BcmNumber, value);
        }

        public bool EnableRaisingEvents
        {
            get => Controller.Driver.GetEnableRaisingPinEvents(BcmNumber);
            set => Controller.Driver.SetEnableRaisingPinEvents(BcmNumber, value);
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
