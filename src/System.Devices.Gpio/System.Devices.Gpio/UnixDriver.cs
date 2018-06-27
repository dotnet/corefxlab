using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Devices.Gpio
{
    public class UnixDriver : GpioDriver
    {
        private const string GpioPath = "/sys/class/gpio";

        private ISet<int> ExportedPins;

        public UnixDriver()
        {
            ExportedPins = new HashSet<int>();
        }

        public override void Dispose()
        {
            while (ExportedPins.Count > 0)
            {
                var pin = ExportedPins.First();
                UnexportPin(pin);
            }
        }

        public override GpioPinMode GetPinMode(int pin)
        {
            ExportPin(pin);

            var directionPath = $"{GpioPath}/gpio{pin}/direction";
            var stringMode = File.ReadAllText(directionPath);
            var mode = StringModeToPinMode(stringMode);
            return mode;
        }

        public override void SetPinMode(int pin, GpioPinMode mode)
        {
            ExportPin(pin);

            var directionPath = $"{GpioPath}/gpio{pin}/direction";
            var stringMode = ModeToStringMode(mode);
            File.WriteAllText(directionPath, stringMode);
        }

        public override GpioPinValue Input(int pin)
        {
            var valuePath = $"{GpioPath}/gpio{pin}/value";
            var stringValue = File.ReadAllText(valuePath);
            var value = StringValueToPinValue(stringValue);
            return value;
        }

        public override void Output(int pin, GpioPinValue value)
        {
            var valuePath = $"{GpioPath}/gpio{pin}/value";
            var stringValue = PinValueToStringValue(value);
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

        public override int ConvertPinNumber(int number, GpioScheme from, GpioScheme to)
        {
            if (from != GpioScheme.BCM || to != GpioScheme.BCM)
                throw new NotSupportedException("Only BCM numbering scheme is supported");

            return number;
        }

        #region Private Methods

        private void ExportPin(int pin)
        {
            var pinPath = $"{GpioPath}/gpio{pin}";

            if (!Directory.Exists(pinPath))
            {
                File.WriteAllText($"{GpioPath}/export", Convert.ToString(pin));
            }

            ExportedPins.Add(pin);
        }

        private void UnexportPin(int pin)
        {
            var pinPath = $"{GpioPath}/gpio{pin}";

            if (Directory.Exists(pinPath))
            {
                File.WriteAllText($"{GpioPath}/unexport", Convert.ToString(pin));
            }

            ExportedPins.Remove(pin);
        }

        private GpioPinMode StringModeToPinMode(string value)
        {
            GpioPinMode result;

            switch (value)
            {
                case "in": result = GpioPinMode.Input; break;
                case "out": result = GpioPinMode.Output; break;
                default: throw new Exception("Invalid GPIO Pin mode");
            }

            return result;
        }

        private string ModeToStringMode(GpioPinMode value)
        {
            string result;

            switch (value)
            {
                case GpioPinMode.Input: result = "in"; break;
                case GpioPinMode.Output: result = "out"; break;
                default: throw new Exception("Invalid GPIO Pin mode");
            }

            return result;
        }

        private GpioPinValue StringValueToPinValue(string value)
        {
            GpioPinValue result;
            value = value.Trim();

            switch (value)
            {
                case "0": result = GpioPinValue.Low; break;
                case "1": result = GpioPinValue.High; break;
                default: throw new Exception("Invalid GPIO Pin value");
            }

            return result;
        }

        private string PinValueToStringValue(GpioPinValue value)
        {
            string result;

            switch (value)
            {
                case GpioPinValue.Low: result = "0"; break;
                case GpioPinValue.High: result = "1"; break;
                default: throw new Exception("Invalid GPIO Pin value");
            }

            return result;
        }

        #endregion
    }
}
