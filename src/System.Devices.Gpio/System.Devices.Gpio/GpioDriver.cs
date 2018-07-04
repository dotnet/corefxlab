// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("System.Devices.Gpio.Samples, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f33a29044fa9d740c9b3213a93e57c84b472c84e0b8a0e1ae48e67a9f8f6de9d5f7f3d52ac23e48ac51801f1dc950abe901da34d2a9e3baadb141a17c77ef3c565dd5ee5054b91cf63bb3c6ab83f72ab3aafe93d0fc3c2348b764fafb0b1c0733de51459aeab46580384bf9d74c4e28164b7cde247f891ba07891c9d872ad2bb")]

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

        protected internal abstract bool WasEventDetected(int bcmPinNumber);

        protected internal abstract void SetEventsToDetect(int bcmPinNumber, EventKind events);

        protected internal abstract EventKind GetEventsToDetect(int bcmPinNumber);

        protected internal void OnPinValueChanged(int bcmPinNumber)
        {
            PinValueChanged?.Invoke(this, bcmPinNumber);
        }

        public abstract void Dispose();
    }
}
