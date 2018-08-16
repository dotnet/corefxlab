﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace System.Devices.Gpio
{
    public class GpioController : IDisposable
    {
        private readonly IDictionary<int, Pin> _pins;

        public GpioController(GpioDriver driver, PinNumberingScheme numbering = PinNumberingScheme.Gpio)
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

        public bool IsPinOpen(int pinNumber)
        {
            int gpioNumber = Driver.ConvertPinNumber(pinNumber, Numbering, PinNumberingScheme.Gpio);
            return _pins.ContainsKey(gpioNumber);
        }

        public Pin this[int pinNumber]
        {
            get
            {
                int gpioNumber = Driver.ConvertPinNumber(pinNumber, Numbering, PinNumberingScheme.Gpio);
                bool isOpen = _pins.TryGetValue(gpioNumber, out Pin pin);

                if (!isOpen)
                {
                    throw new GpioException("The pin must be already open");
                }

                return pin;
            }
        }

        public Pin OpenPin(int pinNumber)
        {
            int gpioNumber = Driver.ConvertPinNumber(pinNumber, Numbering, PinNumberingScheme.Gpio);
            bool isOpen = _pins.TryGetValue(gpioNumber, out Pin pin);

            if (isOpen)
            {
                throw new GpioException("Pin already open");
            }

            Driver.OpenPin(gpioNumber);
            pin = new Pin(this, gpioNumber);
            _pins[gpioNumber] = pin;
            return pin;
        }

        public void ClosePin(int pinNumber)
        {
            int gpioNumber = Driver.ConvertPinNumber(pinNumber, Numbering, PinNumberingScheme.Gpio);
            bool isOpen = _pins.TryGetValue(gpioNumber, out Pin pin);

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
            int gpioNumber = pin.GpioNumber;
            _pins.Remove(gpioNumber);
            Driver.ClosePin(gpioNumber);
        }

        private void OnPinValueChanged(object sender, PinValueChangedEventArgs e)
        {
            Pin pin = _pins[e.GpioPinNumber];
            pin?.OnValueChanged(e);
        }
    }
}