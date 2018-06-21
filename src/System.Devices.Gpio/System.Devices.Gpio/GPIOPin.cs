using System;
using System.Collections.Generic;
using System.Text;

namespace System.Devices.Gpio
{
    public enum GpioPinMode
    {
        Input = 0,
        Output = 1
    }

    public enum GpioPinValue
    {
        Low = 0,
        High = 1
    }

    public enum GpioScheme
    {
        Board,
        BCM
    }

    public class GpioPin
    {
        protected GpioDriver Driver;

        public int Number { get; }

        public GpioPinMode Mode
        {
            get
            {
                return Driver.GetPinMode(Number);
            }
            set
            {
                Driver.SetPinMode(Number, value);
            }
        }

        public GpioPin(GpioDriver driver, GpioScheme numberKind, int number, GpioPinMode mode)
        {
            Driver = driver;
            Number = driver.ConvertPinNumber(number, numberKind, GpioScheme.BCM);
            Mode = mode;
        }

        public int GetNumber(GpioScheme kind)
        {
            return Driver.ConvertPinNumber(Number, GpioScheme.BCM, kind);
        }

        public GpioPinValue Read()
        {
            return Driver.Input(Number);
        }

        public void Write(GpioPinValue value)
        {
            Driver.Output(Number, value);
        }
    }
}
