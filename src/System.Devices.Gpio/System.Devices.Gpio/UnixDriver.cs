// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Devices.Gpio
{
    public class UnixDriver : GpioDriver
    {
        private const string GpioPath = "/sys/class/gpio";

        private BitArray _exportedPins;

        public UnixDriver(int pinCount)
        {
            PinCount = pinCount;
            _exportedPins = new BitArray(pinCount);
        }

        public override void Dispose()
        {
            for (int i = 0; i < _exportedPins.Length; ++i)
            {
                if (_exportedPins[i])
                {
                    UnexportPin(i);
                }
            }
        }

        public override int PinCount { get; }

        public override PinMode GetPinMode(int pin)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            ExportPin(pin);

            string directionPath = $"{GpioPath}/gpio{pin}/direction";
            string stringMode = File.ReadAllText(directionPath);
            PinMode mode = StringModeToPinMode(stringMode);
            return mode;
        }

        public override void SetPinMode(int pin, PinMode mode)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            ExportPin(pin);

            string directionPath = $"{GpioPath}/gpio{pin}/direction";
            string stringMode = ModeToStringMode(mode);
            File.WriteAllText(directionPath, stringMode);
        }

        public override PinValue Input(int pin)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            ExportPin(pin);

            string valuePath = $"{GpioPath}/gpio{pin}/value";
            string stringValue = File.ReadAllText(valuePath);
            PinValue value = StringValueToPinValue(stringValue);
            return value;
        }

        public override void Output(int pin, PinValue value)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            ExportPin(pin);

            string valuePath = $"{GpioPath}/gpio{pin}/value";
            string stringValue = PinValueToStringValue(value);
            File.WriteAllText(valuePath, stringValue);
        }

        public override void ClearDetectedEvent(int pin)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            throw new NotImplementedException();
        }

        public override bool EventWasDetected(int pin)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            throw new NotImplementedException();
        }

        public override void SetEventDetection(int pin, EventKind kind, bool enabled)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            throw new NotImplementedException();
        }

        public override bool GetEventDetection(int pin, EventKind kind)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            throw new NotImplementedException();
        }

        public override int ConvertPinNumber(int pin, PinNumberingScheme from, PinNumberingScheme to)
        {
            if (pin < 0 || pin >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(pin));
            }

            if (from != PinNumberingScheme.BCM || to != PinNumberingScheme.BCM)
                throw new NotSupportedException("Only BCM numbering scheme is supported");

            return pin;
        }

        #region Private Methods

        private void ExportPin(int pin)
        {
            string pinPath = $"{GpioPath}/gpio{pin}";

            if (!Directory.Exists(pinPath))
            {
                File.WriteAllText($"{GpioPath}/export", Convert.ToString(pin));
            }

            _exportedPins.Set(pin, true);
        }

        private void UnexportPin(int pin)
        {
            string pinPath = $"{GpioPath}/gpio{pin}";

            if (Directory.Exists(pinPath))
            {
                File.WriteAllText($"{GpioPath}/unexport", Convert.ToString(pin));
            }

            _exportedPins.Set(pin, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private PinMode StringModeToPinMode(string value)
        {
            PinMode result;

            switch (value)
            {
                case "in": result = PinMode.Input; break;
                case "out": result = PinMode.Output; break;
                default: throw new NotSupportedPinModeException(value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ModeToStringMode(PinMode value)
        {
            string result;

            switch (value)
            {
                case PinMode.Input: result = "in"; break;
                case PinMode.Output: result = "out"; break;
                default: throw new NotSupportedPinModeException(value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private PinValue StringValueToPinValue(string value)
        {
            PinValue result;
            value = value.Trim();

            switch (value)
            {
                case "0": result = PinValue.Low; break;
                case "1": result = PinValue.High; break;
                default: throw new InvalidPinValueException(value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string PinValueToStringValue(PinValue value)
        {
            string result;

            switch (value)
            {
                case PinValue.Low: result = "0"; break;
                case PinValue.High: result = "1"; break;
                default: throw new InvalidPinValueException(value);
            }

            return result;
        }

        #endregion
    }
}
