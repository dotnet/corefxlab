// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public abstract class GpioDriver : IDisposable
    {
        public abstract int PinCount { get; }

        public abstract int ConvertPinNumber(int number, PinNumberingScheme from, PinNumberingScheme to);

        public abstract void SetPinMode(int pin, PinMode mode);

        public abstract PinMode GetPinMode(int pin);

        public abstract void Output(int pin, PinValue value);

        public abstract PinValue Input(int pin);

        public abstract void ClearDetectedEvent(int pin);

        public abstract bool EventWasDetected(int pin);

        public abstract void SetEventDetection(int pin, EventKind kind, bool enabled);

        public abstract bool GetEventDetection(int pin, EventKind kind);

        public abstract void Dispose();
    }
}
