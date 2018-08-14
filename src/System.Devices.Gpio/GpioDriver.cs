﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public abstract class GpioDriver : IDisposable
    {
        public event EventHandler<PinValueChangedEventArgs> ValueChanged;

        protected internal abstract int PinCount { get; }

        protected internal abstract int ConvertPinNumber(int pinNumber, PinNumberingScheme from, PinNumberingScheme to);

        protected internal abstract bool IsPinModeSupported(PinMode mode);

        protected internal abstract void OpenPin(int gpioPinNumber);

        protected internal abstract void ClosePin(int gpioPinNumber);

        protected internal abstract void SetPinMode(int gpioPinNumber, PinMode mode);

        protected internal abstract PinMode GetPinMode(int gpioPinNumber);

        protected internal abstract void Output(int gpioPinNumber, PinValue value);

        protected internal abstract PinValue Input(int gpioPinNumber);

        protected internal abstract void SetDebounce(int gpioPinNumber, TimeSpan timeout);

        protected internal abstract TimeSpan GetDebounce(int gpioPinNumber);

        protected internal abstract void SetPinEventsToDetect(int gpioPinNumber, PinEvent events);

        protected internal abstract PinEvent GetPinEventsToDetect(int gpioPinNumber);

        protected internal abstract void SetEnableRaisingPinEvents(int gpioPinNumber, bool enable);

        protected internal abstract bool GetEnableRaisingPinEvents(int gpioPinNumber);

        protected internal abstract bool WaitForPinEvent(int gpioPinNumber, TimeSpan timeout);

        protected internal void OnPinValueChanged(int gpioPinNumber)
        {
            var e = new PinValueChangedEventArgs(gpioPinNumber);
            ValueChanged?.Invoke(this, e);
        }

        public abstract void Dispose();
    }
}
