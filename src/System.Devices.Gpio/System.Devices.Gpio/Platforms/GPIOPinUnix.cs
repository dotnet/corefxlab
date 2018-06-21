using System;
using System.Collections.Generic;
using System.Devices.Gpio.Devices;
using System.IO;
using System.Text;

namespace System.Devices.Gpio
{
    internal class GpioPinUnix : GpioPin
    {
        private const string gpioPath = "/sys/class/gpio";

        private readonly int bcmNumber;
        private readonly string pinPath;

        public override GpioPinStatus Status { get; protected set; }

        public override GpioPinMode Mode
        {
            get
            {
                if (this.Status != GpioPinStatus.Open)
                {
                    throw new InvalidOperationException("The pin is not open");
                }

                var stringMode = File.ReadAllText($"{pinPath}/direction");
                var mode = StringModeToPinMode(stringMode);
                return mode;
            }
            set
            {
                if (this.Status != GpioPinStatus.Open)
                {
                    throw new InvalidOperationException("The pin is not open");
                }

                var stringMode = ModeToStringMode(value);
                File.WriteAllText($"{pinPath}/direction", stringMode);
            }
        }

        public GpioPinUnix(GpioDeviceInfo deviceInfo, GpioScheme numbering, int number)
            : base(deviceInfo, number, numbering)
        {
            bcmNumber = deviceInfo.ConvertPinNumber(number, numbering, GpioScheme.BCM);
            pinPath = $"{gpioPath}/gpio{bcmNumber}";
        }

        public override void Open()
        {
            if (this.Status == GpioPinStatus.Open) return;

            File.WriteAllText($"{gpioPath}/export", Convert.ToString(bcmNumber));
            this.Status = GpioPinStatus.Open;
        }

        public override void Close()
        {
            if (this.Status == GpioPinStatus.Closed) return;

            File.WriteAllText($"{gpioPath}/unexport", Convert.ToString(bcmNumber));
            this.Status = GpioPinStatus.Closed;
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
