// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Devices.Gpio
{
    public class UnixDriver : GpioDriver
    {
        private const string GpioPath = "/sys/class/gpio";

        private ISet<int> _exportedPins;

        public UnixDriver()
        {
            _exportedPins = new HashSet<int>();
        }

        public override void Dispose()
        {
            while (_exportedPins.Count > 0)
            {
                int pin = _exportedPins.First();
                UnexportPin(pin);
            }
        }

        public override GpioPinMode GetPinMode(int pin)
        {
            ExportPin(pin);

            string directionPath = $"{GpioPath}/gpio{pin}/direction";
            string stringMode = File.ReadAllText(directionPath);
            GpioPinMode mode = StringModeToPinMode(stringMode);
            return mode;
        }

        public override void SetPinMode(int pin, GpioPinMode mode)
        {
            ExportPin(pin);

            string directionPath = $"{GpioPath}/gpio{pin}/direction";
            string stringMode = ModeToStringMode(mode);
            File.WriteAllText(directionPath, stringMode);
        }

        public override GpioPinValue Input(int pin)
        {
            ExportPin(pin);

            string valuePath = $"{GpioPath}/gpio{pin}/value";
            string stringValue = File.ReadAllText(valuePath);
            GpioPinValue value = StringValueToPinValue(stringValue);
            return value;
        }

        public override void Output(int pin, GpioPinValue value)
        {
            ExportPin(pin);

            string valuePath = $"{GpioPath}/gpio{pin}/value";
            string stringValue = PinValueToStringValue(value);
            File.WriteAllText(valuePath, stringValue);
        }

        public override void ClearDetectedEvent(int pin)
        {
            throw new NotImplementedException();
        }

        public override bool EventWasDetected(int pin)
        {
            throw new NotImplementedException();
        }

        public override void SetEventDetection(int pin, GpioEventKind kind, bool enabled)
        {
            throw new NotImplementedException();
        }

        public override bool GetEventDetection(int pin, GpioEventKind kind)
        {
            throw new NotImplementedException();
        }

        public override int PinCount => throw new NotSupportedException();

        public override int ConvertPinNumber(int number, GpioNumberingScheme from, GpioNumberingScheme to)
        {
            if (from != GpioNumberingScheme.BCM || to != GpioNumberingScheme.BCM)
                throw new NotSupportedException("Only BCM numbering scheme is supported");

            return number;
        }

        #region Private Methods

        private void ExportPin(int pin)
        {
            string pinPath = $"{GpioPath}/gpio{pin}";

            if (!Directory.Exists(pinPath))
            {
                File.WriteAllText($"{GpioPath}/export", Convert.ToString(pin));
            }

            _exportedPins.Add(pin);
        }

        private void UnexportPin(int pin)
        {
            string pinPath = $"{GpioPath}/gpio{pin}";

            if (Directory.Exists(pinPath))
            {
                File.WriteAllText($"{GpioPath}/unexport", Convert.ToString(pin));
            }

            _exportedPins.Remove(pin);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GpioPinMode StringModeToPinMode(string value)
        {
            GpioPinMode result;

            switch (value)
            {
                case "in": result = GpioPinMode.Input; break;
                case "out": result = GpioPinMode.Output; break;
                default: throw new NotSupportedGpioPinModeException(value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ModeToStringMode(GpioPinMode value)
        {
            string result;

            switch (value)
            {
                case GpioPinMode.Input: result = "in"; break;
                case GpioPinMode.Output: result = "out"; break;
                default: throw new NotSupportedGpioPinModeException(value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GpioPinValue StringValueToPinValue(string value)
        {
            GpioPinValue result;
            value = value.Trim();

            switch (value)
            {
                case "0": result = GpioPinValue.Low; break;
                case "1": result = GpioPinValue.High; break;
                default: throw new InvalidGpioPinValueException(value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string PinValueToStringValue(GpioPinValue value)
        {
            string result;

            switch (value)
            {
                case GpioPinValue.Low: result = "0"; break;
                case GpioPinValue.High: result = "1"; break;
                default: throw new InvalidGpioPinValueException(value);
            }

            return result;
        }

        #endregion
    }
}
