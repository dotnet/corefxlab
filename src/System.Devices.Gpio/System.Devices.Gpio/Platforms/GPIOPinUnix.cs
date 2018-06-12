using System;
using System.Collections.Generic;
using System.Devices.Gpio.Devices;
using System.IO;
using System.Text;

namespace System.Devices.Gpio
{
    internal class GPIOPinUnix : GPIOPin
    {
        private const string gpioPath = "/sys/class/gpio";

        private readonly int bcmNumber;
        private readonly string pinPath;

        public override GPIOPinStatus Status { get; protected set; }

        public override GPIOPinMode Mode
        {
            get
            {
                if (this.Status != GPIOPinStatus.Open)
                {
                    throw new InvalidOperationException("The pin is not open");
                }

                var stringMode = File.ReadAllText($"{pinPath}/direction");
                var mode = StringModeToPinMode(stringMode);
                return mode;
            }
            set
            {
                if (this.Status != GPIOPinStatus.Open)
                {
                    throw new InvalidOperationException("The pin is not open");
                }

                var stringMode = ModeToStringMode(value);
                File.WriteAllText($"{pinPath}/direction", stringMode);
            }
        }

        public GPIOPinUnix(GPIODeviceInfo deviceInfo, GPIOScheme numbering, int number)
            : base(deviceInfo, number, numbering)
        {
            bcmNumber = deviceInfo.ConvertPinNumber(number, numbering, GPIOScheme.BCM);
            pinPath = $"{gpioPath}/gpio{bcmNumber}";
        }

        public override void Open()
        {
            if (this.Status == GPIOPinStatus.Open) return;

            File.WriteAllText($"{gpioPath}/export", Convert.ToString(bcmNumber));
            this.Status = GPIOPinStatus.Open;
        }

        public override void Close()
        {
            if (this.Status == GPIOPinStatus.Closed) return;

            File.WriteAllText($"{gpioPath}/unexport", Convert.ToString(bcmNumber));
            this.Status = GPIOPinStatus.Closed;
        }

        public override bool Read()
        {
            var stringValue = File.ReadAllText($"{pinPath}/value");
            var value = StringValueToBool(stringValue);
            return value;
        }

        public override void Write(bool value)
        {
            var stringValue = BoolToStringValue(value);
            File.WriteAllText($"{pinPath}/value", stringValue);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    this.Close();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        #endregion

        #region Private Methods

        private GPIOPinMode StringModeToPinMode(string value)
        {
            GPIOPinMode result;

            switch (value)
            {
                case "in": result = GPIOPinMode.Input; break;
                case "out": result = GPIOPinMode.Output; break;
                default: throw new Exception("Invalid GPIO Pin mode");
            }

            return result;
        }

        private string ModeToStringMode(GPIOPinMode value)
        {
            string result;

            switch (value)
            {
                case GPIOPinMode.Input: result = "in"; break;
                case GPIOPinMode.Output: result = "out"; break;
                default: throw new Exception("Invalid GPIO Pin mode");
            }

            return result;
        }

        private bool StringValueToBool(string value)
        {
            bool result;
            value = value.Trim();

            switch (value)
            {
                case "0": result = false; break;
                case "1": result = true; break;
                default: throw new Exception("Invalid GPIO Pin value");
            }

            return result;
        }

        private string BoolToStringValue(bool value)
        {
            return value ? "1" : "0";
        }

        #endregion
    }
}
