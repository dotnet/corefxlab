// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public static class Extensions
    {
        public static Pin OpenPin(this GpioController controller, int number, PinMode mode)
        {
            Pin pin = controller.OpenPin(number);
            pin.Mode = mode;
            return pin;
        }

        public static Pin[] OpenPins(this GpioController controller, PinMode mode, params int[] numbers)
        {
            Pin[] pins = controller.OpenPins(numbers);

            foreach (Pin pin in pins)
            {
                pin.Mode = mode;
            }

            return pins;
        }

        public static Pin[] OpenPins(this GpioController controller, params int[] numbers)
        {
            var pins = new Pin[numbers.Length];

            for (int i = 0; i < numbers.Length; ++i)
            {
                int number = numbers[i];
                Pin pin = controller.OpenPin(number);
                pins[i] = pin;
            }

            return pins;
        }

        public static void Set(this Pin pin)
        {
            pin.Write(PinValue.High);
        }

        public static void Clear(this Pin pin)
        {
            pin.Write(PinValue.Low);
        }

        public static void Toggle(this Pin pin)
        {
            PinValue value = pin.Read();

            switch (value)
            {
                case PinValue.Low: value = PinValue.High; break;
                case PinValue.High: value = PinValue.Low; break;
            }

            pin.Write(value);
        }
    }
}
