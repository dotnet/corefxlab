// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class GpioPin : IDisposable
    {
        internal GpioPin(GpioController controller, int gpioNumber)
        {
            Controller = controller;
            GpioNumber = gpioNumber;
        }

        public void Dispose()
        {
            Controller.ClosePin(this);
        }

        public event EventHandler<PinValueChangedEventArgs> ValueChanged;

        public GpioController Controller { get; }

        public int GpioNumber { get; }

        public TimeSpan DebounceTimeout
        {
            get => Controller.Driver.GetDebounce(GpioNumber);
            set => Controller.Driver.SetDebounce(GpioNumber, value);
        }

        public PinMode Mode
        {
            get => Controller.Driver.GetPinMode(GpioNumber);
            set => Controller.Driver.SetPinMode(GpioNumber, value);
        }

        public PinEvent NotifyEvents
        {
            get => Controller.Driver.GetPinEventsToDetect(GpioNumber);
            set => Controller.Driver.SetPinEventsToDetect(GpioNumber, value);
        }

        public bool EnableRaisingEvents
        {
            get => Controller.Driver.GetEnableRaisingPinEvents(GpioNumber);
            set => Controller.Driver.SetEnableRaisingPinEvents(GpioNumber, value);
        }

        public bool IsModeSupported(PinMode mode) => Controller.Driver.IsPinModeSupported(mode);

        public int GetNumber(PinNumberingScheme numbering) => Controller.Driver.ConvertPinNumber(GpioNumber, PinNumberingScheme.Gpio, numbering);

        public PinValue Read() => Controller.Driver.Input(GpioNumber);

        public void Write(PinValue value) => Controller.Driver.Output(GpioNumber, value);

        public bool WaitForEvent(TimeSpan timeout) => Controller.Driver.WaitForPinEvent(GpioNumber, timeout);

        internal void OnValueChanged(PinValueChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}
