// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace System.Devices.Gpio
{
    public class GpioController : IDisposable
    {
        private readonly Pin[] _pins;

        public GpioController(GpioDriver driver, PinNumberingScheme numbering)
        {
            Driver = driver;
            Numbering = numbering;
            _pins = new Pin[driver.PinCount];

            driver.ValueChanged += OnPinValueChanged;
        }

        public void Dispose()
        {
            for (int i = 0; i < _pins.Length; ++i)
            {
                Pin pin = _pins[i];

                if (pin != null)
                {
                    pin.Dispose();
                    _pins[i] = null;
                }
            }

            Driver.Dispose();
        }

        internal GpioDriver Driver { get; }

        public PinNumberingScheme Numbering { get; set; }

        public int PinCount => Driver.PinCount;

        public IEnumerable<Pin> OpenPins => _pins.Where(p => p != null);

        public bool IsPinOpen(int number)
        {
            int bcmNumber = Driver.ConvertPinNumber(number, Numbering, PinNumberingScheme.Bcm);
            Pin pin = _pins[bcmNumber];
            return pin != null;
        }

        public Pin this[int pinNumber]
        {
            get
            {
                int bcmNumber = Driver.ConvertPinNumber(pinNumber, Numbering, PinNumberingScheme.Bcm);
                Pin pin = _pins[bcmNumber];

                if (pin == null)
                {
                    throw new GpioException("The pin must be already open");
                }

                return pin;
            }
        }

        public Pin OpenPin(int number)
        {
            int bcmNumber = Driver.ConvertPinNumber(number, Numbering, PinNumberingScheme.Bcm);
            Pin pin = _pins[bcmNumber];

            if (pin != null)
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
            Pin pin = _pins[bcmNumber];

            if (pin != null)
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
            _pins[bcmNumber] = null;
            Driver.ClosePin(bcmNumber);
        }

        private void OnPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            Pin pin = _pins[e.BcmPinNumber];
            pin?.OnValueChanged(e);
        }
    }
}
