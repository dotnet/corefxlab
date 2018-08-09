// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace System.Devices.Gpio
{
    public class GpioController : IDisposable
    {
        private readonly IDictionary<int, Pin> _pins;

        public GpioController(GpioDriver driver, PinNumberingScheme numbering)
        {
            Driver = driver;
            Numbering = numbering;
            _pins = new Dictionary<int, Pin>(driver.PinCount);

            driver.ValueChanged += OnPinValueChanged;
        }

        public void Dispose()
        {
            while (_pins.Count > 0)
            {
                Pin pin = _pins.Values.First();
                pin.Dispose();
            }

            Driver.Dispose();
        }

        internal GpioDriver Driver { get; }

        public PinNumberingScheme Numbering { get; set; }

        public int PinCount => Driver.PinCount;

        public IEnumerable<Pin> OpenPins => _pins.Values;

        public bool IsPinOpen(int number)
        {
            int bcmNumber = Driver.ConvertPinNumber(number, Numbering, PinNumberingScheme.Bcm);
            return _pins.ContainsKey(bcmNumber);
        }

        public Pin this[int pinNumber]
        {
            get
            {
                int bcmNumber = Driver.ConvertPinNumber(pinNumber, Numbering, PinNumberingScheme.Bcm);
                bool isOpen = _pins.TryGetValue(bcmNumber, out Pin pin);

                if (!isOpen)
                {
                    throw new GpioException("The pin must be already open");
                }

                return pin;
            }
        }

        public Pin OpenPin(int number)
        {
            int bcmNumber = Driver.ConvertPinNumber(number, Numbering, PinNumberingScheme.Bcm);
            bool isOpen = _pins.TryGetValue(bcmNumber, out Pin pin);

            if (isOpen)
            {
                throw new GpioException("Pin already open");
            }

            Driver.OpenPin(bcmNumber);
            pin = new Pin(this, bcmNumber);
            _pins[bcmNumber] = pin;
            return pin;
        }

        public void ClosePin(int number)
        {
            int bcmNumber = Driver.ConvertPinNumber(number, Numbering, PinNumberingScheme.Bcm);
            bool isOpen = _pins.TryGetValue(bcmNumber, out Pin pin);

            if (isOpen)
            {
                InternalClosePin(pin);
            }
        }

        public void ClosePin(Pin pin)
        {
            if (pin == null)
            {
                throw new ArgumentNullException(nameof(pin));
            }

            if (pin.Controller != this)
            {
                throw new ArgumentException("The given pin does not belong to this controller");
            }

            InternalClosePin(pin);
        }

        private void InternalClosePin(Pin pin)
        {
            int bcmNumber = pin.BcmNumber;
            _pins.Remove(bcmNumber);
            Driver.ClosePin(bcmNumber);
        }

        private void OnPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            Pin pin = _pins[e.BcmPinNumber];
            pin?.OnValueChanged(e);
        }
    }
}
