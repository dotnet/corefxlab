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
        private bool _eventDetectionEnabled;
        private TimeSpan[] _debounceTimeouts;

        public UnixDriver(int pinCount)
        {
            PinCount = pinCount;
            _exportedPins = new BitArray(pinCount);
            _debounceTimeouts = new TimeSpan[pinCount];
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

        protected internal override int PinCount { get; }

        protected internal override PinMode GetPinMode(int bcmPinNumber)
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

        protected internal override void SetPinMode(int bcmPinNumber, PinMode mode)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            ExportPin(bcmPinNumber);

            string directionPath = $"{GpioPath}/gpio{bcmPinNumber}/direction";
            string stringMode = PinModeToStringMode(mode);
            File.WriteAllText(directionPath, stringMode);
        }

        protected internal override PinValue Input(int bcmPinNumber)
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

        protected internal override void Output(int bcmPinNumber, PinValue value)
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

        protected internal override void SetDebounce(int bcmPinNumber, TimeSpan timeout)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            _debounceTimeouts[bcmPinNumber] = timeout;
        }

        protected internal override TimeSpan GetDebounce(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            TimeSpan timeout = _debounceTimeouts[bcmPinNumber];
            return timeout;
        }

        protected internal override void SetPinEventsToDetect(int bcmPinNumber, PinEvent kind)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            ExportPin(bcmPinNumber);

            string edgePath = $"{GpioPath}/gpio{bcmPinNumber}/edge";
            string stringValue = EventKindToStringValue(kind);
            File.WriteAllText(edgePath, stringValue);
        }

        protected internal override PinEvent GetPinEventsToDetect(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            ExportPin(bcmPinNumber);

            string edgePath = $"{GpioPath}/gpio{bcmPinNumber}/edge";
            string stringValue = File.ReadAllText(edgePath);
            PinEvent value = StringValueToEventKind(stringValue);
            return value;
        }

        protected internal override bool EnableEventsDetection
        {
            get => _eventDetectionEnabled;
            set => EnableEvents(value);
        }

        private void EnableEvents(bool value)
        {
            if (!_eventDetectionEnabled && value)
            {
                // Enable events detection
                _eventDetectionEnabled = true;
            }
            else if (_eventDetectionEnabled && !value)
            {
                // Disable events detection
                _eventDetectionEnabled = false;
            }

            throw new NotImplementedException();
        }

        protected internal override bool WaitForPinEvent(int bcmPinNumber, TimeSpan timeout)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            ExportPin(bcmPinNumber);

            DateTime initial = DateTime.UtcNow;
            TimeSpan elapsed;
            bool eventDetected;

            do
            {
                eventDetected = WasEventDetected(bcmPinNumber);
                elapsed = DateTime.UtcNow.Subtract(initial);
            }
            while (!eventDetected && elapsed < timeout);

            return eventDetected;
        }

        private bool WasEventDetected(int bcmPinNumber)
        {
            //string valuePath = $"{GpioPath}/gpio{bcmPinNumber}/value";
            //string stringValue = File.ReadAllText(valuePath);
            //PinValue value = StringValueToPinValue(stringValue);
            //bool result = value == PinValue.High;
            //return result;

            throw new NotImplementedException();
        }

        protected internal override int ConvertPinNumber(int bcmPinNumber, PinNumberingScheme from, PinNumberingScheme to)
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
        private PinMode StringModeToPinMode(string mode)
        {
            PinMode result;

            switch (mode)
            {
                case "in": result = PinMode.Input; break;
                case "out": result = PinMode.Output; break;
                default: throw new NotSupportedException($"Not supported GPIO pin mode '{mode}'");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string PinModeToStringMode(PinMode mode)
        {
            string result;

            switch (mode)
            {
                case PinMode.Input: result = "in"; break;
                case PinMode.Output: result = "out"; break;
                default: throw new NotSupportedException($"Not supported GPIO pin mode '{mode}'");
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
                default: throw new ArgumentException($"Invalid GPIO pin value '{value}'");
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
                default: throw new ArgumentException($"Invalid GPIO pin value '{value}'");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string EventKindToStringValue(PinEvent kind)
        {
            string result;

            if (kind == PinEvent.None)
            {
                result = "none";
            }
            else if (kind.HasFlag(PinEvent.SyncBoth) ||
                     kind.HasFlag(PinEvent.AsyncBoth))
            {
                result = "both";
            }
            else if (kind.HasFlag(PinEvent.SyncRisingEdge) ||
                     kind.HasFlag(PinEvent.AsyncRisingEdge))
            {
                result = "rising";
            }
            else if (kind.HasFlag(PinEvent.SyncFallingEdge) ||
                     kind.HasFlag(PinEvent.AsyncFallingEdge))
            {
                result = "falling";
            }
            else
            {
                throw new NotSupportedException($"Not supported GPIO event kind '{kind}'");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private PinEvent StringValueToEventKind(string kind)
        {
            PinEvent result;
            kind = kind.Trim();

            switch (kind)
            {
                case "none":
                    result = PinEvent.None;
                    break;
                case "rising":
                    result = PinEvent.SyncRisingEdge | PinEvent.AsyncRisingEdge;
                    break;
                case "falling":
                    result = PinEvent.SyncFallingEdge | PinEvent.AsyncFallingEdge;
                    break;
                case "both":
                    result = PinEvent.SyncBoth | PinEvent.AsyncBoth;
                    break;
                default:
                    throw new NotSupportedException($"Not supported GPIO event kind '{kind}'");
            }

            return result;
        }

        #endregion
    }
}
