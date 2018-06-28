// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace System.Devices.Gpio
{
    public enum GpioPinMode
    {
        Input,
        Output,
        InputPullDown,
        InputPullUp
    }

    public enum GpioPinValue
    {
        Low = 0,
        High = 1
    }

    public enum GpioEventKind
    {
        Low,
        High,
        SyncFallingEdge,
        SyncRisingEdge,
        AsyncFallingEdge,
        AsyncRisingEdge
    }

    public enum GpioNumberingScheme
    {
        Board,
        BCM
    }

    public class GpioPin
    {
        protected GpioDriver _driver;

        public int Number { get; }

        public GpioPinMode Mode
        {
            get
            {
                return _driver.GetPinMode(Number);
            }
            set
            {
                _driver.SetPinMode(Number, value);
            }
        }

        public GpioPin(GpioDriver driver, GpioNumberingScheme numbering, int number, GpioPinMode mode)
        {
            _driver = driver;
            Number = driver.ConvertPinNumber(number, numbering, GpioNumberingScheme.BCM);
            Mode = mode;
        }

        public int GetNumber(GpioNumberingScheme numbering)
        {
            return _driver.ConvertPinNumber(Number, GpioNumberingScheme.BCM, numbering);
        }

        public GpioPinValue Read()
        {
            return _driver.Input(Number);
        }

        public void Write(GpioPinValue value)
        {
            _driver.Output(Number, value);
        }
    }
}
