// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public delegate void PinValueChangedEventHandler(GpioDriver driver, int bcmPinNumber);

    public abstract class GpioDriver : IDisposable
    {
        public event PinValueChangedEventHandler PinValueChanged;

        protected internal abstract int PinCount { get; }

        protected internal abstract TimeSpan Debounce { get; set; }

        protected internal abstract int ConvertPinNumber(int pinNumber, PinNumberingScheme from, PinNumberingScheme to);

        protected internal abstract void SetPinMode(int bcmPinNumber, PinMode mode);

        protected internal abstract PinMode GetPinMode(int bcmPinNumber);

        protected internal abstract void Output(int bcmPinNumber, PinValue value);

        protected internal abstract PinValue Input(int bcmPinNumber);

        protected internal abstract bool WasPinEventDetected(int bcmPinNumber);

        protected internal abstract void SetPinEventsToDetect(int bcmPinNumber, PinEvent pinEvents);

        protected internal abstract PinEvent GetPinEventsToDetect(int bcmPinNumber);

        protected internal void OnPinValueChanged(int bcmPinNumber)
        {
            PinValueChanged?.Invoke(this, bcmPinNumber);
        }

        public abstract void Dispose();
    }
}
