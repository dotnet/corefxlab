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

        private readonly BitArray _exportedPins;
        private readonly TimeSpan[] _debounceTimeouts;
        private bool _eventDetectionEnabled;

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

        protected internal override bool IsPinModeSupported(PinMode mode)
        {
            bool result;

            switch (mode)
            {
                case PinMode.Input:
                case PinMode.Output:
                    result = true;
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }

        protected internal override PinMode GetPinMode(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);
            ExportPin(bcmPinNumber);

            string directionPath = $"{GpioPath}/gpio{bcmPinNumber}/direction";
            string stringMode = File.ReadAllText(directionPath);
            PinMode mode = StringModeToPinMode(stringMode);
            return mode;
        }

        protected internal override void SetPinMode(int bcmPinNumber, PinMode mode)
        {
            ValidatePinNumber(bcmPinNumber);
            ValidatePinMode(mode);
            ExportPin(bcmPinNumber);

            string directionPath = $"{GpioPath}/gpio{bcmPinNumber}/direction";
            string stringMode = PinModeToStringMode(mode);
            File.WriteAllText(directionPath, stringMode);
        }

        protected internal override PinValue Input(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);
            ExportPin(bcmPinNumber);

            string valuePath = $"{GpioPath}/gpio{bcmPinNumber}/value";
            string stringValue = File.ReadAllText(valuePath);
            PinValue value = StringValueToPinValue(stringValue);
            return value;
        }

        protected internal override void Output(int bcmPinNumber, PinValue value)
        {
            ValidatePinNumber(bcmPinNumber);
            ValidatePinValue(value);
            ExportPin(bcmPinNumber);

            string valuePath = $"{GpioPath}/gpio{bcmPinNumber}/value";
            string stringValue = PinValueToStringValue(value);
            File.WriteAllText(valuePath, stringValue);
        }

        protected internal override void SetDebounce(int bcmPinNumber, TimeSpan timeout)
        {
            ValidatePinNumber(bcmPinNumber);

            _debounceTimeouts[bcmPinNumber] = timeout;
        }

        protected internal override TimeSpan GetDebounce(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);

            TimeSpan timeout = _debounceTimeouts[bcmPinNumber];
            return timeout;
        }

        protected internal override void SetEventsToDetect(int bcmPinNumber, EventKind kind)
        {
            ValidatePinNumber(bcmPinNumber);
            ExportPin(bcmPinNumber);

            string edgePath = $"{GpioPath}/gpio{bcmPinNumber}/edge";
            string stringValue = EventKindToStringValue(kind);
            File.WriteAllText(edgePath, stringValue);
        }

        protected internal override EventKind GetEventsToDetect(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);
            ExportPin(bcmPinNumber);

            string edgePath = $"{GpioPath}/gpio{bcmPinNumber}/edge";
            string stringValue = File.ReadAllText(edgePath);
            EventKind value = StringValueToEventKind(stringValue);
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

        protected internal override bool WaitForEvent(int bcmPinNumber, TimeSpan timeout)
        {
            ValidatePinNumber(bcmPinNumber);
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
            ValidatePinNumber(bcmPinNumber);

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
        private void ValidatePinMode(PinMode mode)
        {
            bool supportedPinMode = IsPinModeSupported(mode);

            if (!supportedPinMode)
            {
                throw new NotSupportedException($"Not supported GPIO pin mode '{mode}'");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidatePinValue(PinValue value)
        {
            switch (value)
            {
                case PinValue.Low:
                case PinValue.High:
                    // Do nothing
                    break;

                default:
                    throw new ArgumentException($"Invalid GPIO pin value '{value}'");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ValidatePinNumber(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }
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
        private string EventKindToStringValue(EventKind kind)
        {
            string result;

            if (kind == EventKind.None)
            {
                result = "none";
            }
            else if (kind.HasFlag(EventKind.SyncBoth) ||
                     kind.HasFlag(EventKind.AsyncBoth))
            {
                result = "both";
            }
            else if (kind.HasFlag(EventKind.SyncRisingEdge) ||
                     kind.HasFlag(EventKind.AsyncRisingEdge))
            {
                result = "rising";
            }
            else if (kind.HasFlag(EventKind.SyncFallingEdge) ||
                     kind.HasFlag(EventKind.AsyncFallingEdge))
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
        private EventKind StringValueToEventKind(string kind)
        {
            EventKind result;
            kind = kind.Trim();

            switch (kind)
            {
                case "none":
                    result = EventKind.None;
                    break;
                case "rising":
                    result = EventKind.SyncRisingEdge | EventKind.AsyncRisingEdge;
                    break;
                case "falling":
                    result = EventKind.SyncFallingEdge | EventKind.AsyncFallingEdge;
                    break;
                case "both":
                    result = EventKind.SyncBoth | EventKind.AsyncBoth;
                    break;
                default:
                    throw new NotSupportedException($"Not supported GPIO event kind '{kind}'");
            }

            return result;
        }

        #endregion
    }
}
