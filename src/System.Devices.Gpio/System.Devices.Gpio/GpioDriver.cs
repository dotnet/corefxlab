// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public abstract class GpioDriver : IDisposable
    {
        public abstract int PinCount { get; }

        public abstract int ConvertPinNumber(int pinNumber, PinNumberingScheme from, PinNumberingScheme to);

        public abstract void SetPinMode(int bcmPinNumber, PinMode mode);

        public abstract PinMode GetPinMode(int bcmPinNumber);

        public abstract void Output(int bcmPinNumber, PinValue value);

        public abstract PinValue Input(int bcmPinNumber);

        public abstract void ClearDetectedEvent(int bcmPinNumber);

        public abstract bool WasEventDetected(int bcmPinNumber);

        public abstract void SetEventsToDetect(int bcmPinNumber, EventKind events);

        public abstract EventKind GetEventsToDetect(int bcmPinNumber);

        public abstract void Dispose();
    }
}
