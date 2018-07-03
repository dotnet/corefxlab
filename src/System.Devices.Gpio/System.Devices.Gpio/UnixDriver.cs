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

        public override PinMode GetPinMode(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            ExportPin(bcmPinNumber);

            string directionPath = $"{GpioPath}/gpio{bcmPinNumber}/direction";
            string stringMode = File.ReadAllText(directionPath);
            PinMode mode = StringModeToPinMode(stringMode);
            return mode;
        }

        public override void SetPinMode(int bcmPinNumber, PinMode mode)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            ExportPin(bcmPinNumber);

            string directionPath = $"{GpioPath}/gpio{bcmPinNumber}/direction";
            string stringMode = ModeToStringMode(mode);
            File.WriteAllText(directionPath, stringMode);
        }

        public override PinValue Input(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            ExportPin(bcmPinNumber);

            string valuePath = $"{GpioPath}/gpio{bcmPinNumber}/value";
            string stringValue = File.ReadAllText(valuePath);
            PinValue value = StringValueToPinValue(stringValue);
            return value;
        }

        public override void Output(int bcmPinNumber, PinValue value)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            ExportPin(bcmPinNumber);

            string valuePath = $"{GpioPath}/gpio{bcmPinNumber}/value";
            string stringValue = PinValueToStringValue(value);
            File.WriteAllText(valuePath, stringValue);
        }

        public override void ClearDetectedEvent(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            throw new NotImplementedException();
        }

        public override bool WasEventDetected(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            throw new NotImplementedException();
        }

        public override void SetEventsToDetect(int bcmPinNumber, EventKind kind)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            throw new NotImplementedException();
        }

        public override EventKind GetEventsToDetect(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            throw new NotImplementedException();
        }

        public override int ConvertPinNumber(int bcmPinNumber, PinNumberingScheme from, PinNumberingScheme to)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            if (from != PinNumberingScheme.BCM || to != PinNumberingScheme.BCM)
            {
                throw new NotSupportedException("Only BCM numbering scheme is supported");
            }

            return bcmPinNumber;
        }

        #region Private Methods

        private void ExportPin(int bcmPinNumber)
        {
            string pinPath = $"{GpioPath}/gpio{bcmPinNumber}";

            if (!Directory.Exists(pinPath))
            {
                File.WriteAllText($"{GpioPath}/export", Convert.ToString(bcmPinNumber));
            }

            _exportedPins.Set(bcmPinNumber, true);
        }

        private void UnexportPin(int bcmPinNumber)
        {
            string pinPath = $"{GpioPath}/gpio{bcmPinNumber}";

            if (Directory.Exists(pinPath))
            {
                File.WriteAllText($"{GpioPath}/unexport", Convert.ToString(bcmPinNumber));
            }

            _exportedPins.Set(bcmPinNumber, false);
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
