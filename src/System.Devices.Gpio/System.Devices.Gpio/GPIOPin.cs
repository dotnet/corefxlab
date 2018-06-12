using System;
using System.Collections.Generic;
using System.Devices.Gpio.Devices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Devices.Gpio
{
    public enum GPIOPinMode
    {
        Input,
        Output
    }

    public enum GPIOPinStatus
    {
        Closed,
        Open
    }

    public enum GPIOScheme
    {
        Board,
        BCM
    }

    public enum GPIODeviceKind
    {
        RaspberryPi
    }

    public abstract class GPIOPin : IDisposable
    {
        private readonly GPIODeviceInfo deviceInfo;

        public int BoardNumber { get; }
        public abstract GPIOPinStatus Status { get; protected set; }
        public abstract GPIOPinMode Mode { get; set; }

        public static GPIOPin Create(GPIODeviceKind deviceKind, GPIOScheme numbering, int number)
        {
            GPIOPin pin;

            var deviceInfo = GPIODeviceInfo.Create(deviceKind);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                pin = new GPIOPinUnix(deviceInfo, numbering, number);
            }
            else
            {
                throw new Exception("Unsupported OS platform");
            }

            return pin;
        }

        protected private GPIOPin(GPIODeviceInfo deviceInfo, int number, GPIOScheme numberKind)
        {
            this.deviceInfo = deviceInfo;
            this.BoardNumber = deviceInfo.ConvertPinNumber(number, numberKind, GPIOScheme.Board);
        }

        public int GetNumber(GPIOScheme kind)
        {
            return deviceInfo.ConvertPinNumber(this.BoardNumber, GPIOScheme.Board, kind);
        }

        public void Open(GPIOPinMode mode)
        {
            this.Open();
            this.Mode = mode;
        }

        public abstract void Open();
        public abstract void Close();
        public abstract bool Read();
        public abstract void Write(bool value);

        #region IDisposable Support

        protected abstract void Dispose(bool disposing);

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GPIOPin() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
