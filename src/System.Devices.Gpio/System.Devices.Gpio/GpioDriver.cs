// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public abstract class GpioDriver : IDisposable
    {
        public abstract int PinCount { get; }

        public abstract int ConvertPinNumber(int number, GpioNumberingScheme from, GpioNumberingScheme to);

        public abstract void SetPinMode(int pin, GpioPinMode mode);

        public abstract GpioPinMode GetPinMode(int pin);

        public abstract void Output(int pin, GpioPinValue value);

        public abstract GpioPinValue Input(int pin);

        public abstract void ClearDetectedEvent(int pin);

        public abstract bool EventWasDetected(int pin);

        public abstract void SetEventDetection(int pin, GpioEventKind kind, bool enabled);

        public abstract bool GetEventDetection(int pin, GpioEventKind kind);

        public abstract void Dispose();
    }
}
