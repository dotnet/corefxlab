using System;
using System.Collections.Generic;
using System.Devices.Gpio.Devices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Devices.Gpio
{
    public enum GpioPinMode
    {
        Input = 0,
        Output = 1
    }

    public enum GpioPinStatus
    {
        Closed,
        Open
    }

    public enum GpioScheme
    {
        Board,
        BCM
    }

    public enum GpioDeviceKind
    {
        RaspberryPi
    }

    public abstract class GpioPin : IDisposable
    {
        private readonly GpioDeviceInfo deviceInfo;

        public int BoardNumber { get; }
        public abstract GpioPinStatus Status { get; protected set; }
        public abstract GpioPinMode Mode { get; set; }

        public static GpioPin Create(GpioDeviceKind deviceKind, GpioScheme numbering, int number)
        {
            GpioPin pin;

            var deviceInfo = GpioDeviceInfo.Create(deviceKind);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                pin = new GpioPinUnix(deviceInfo, numbering, number);
            }
            else
            {
                throw new Exception("Unsupported OS platform");
            }

            return pin;
        }

        protected private GpioPin(GpioDeviceInfo deviceInfo, int number, GpioScheme numberKind)
        {
            this.deviceInfo = deviceInfo;
            this.BoardNumber = deviceInfo.ConvertPinNumber(number, numberKind, GpioScheme.Board);
        }

        public int GetNumber(GpioScheme kind)
        {
            return deviceInfo.ConvertPinNumber(this.BoardNumber, GpioScheme.Board, kind);
        }

        public void Open(GpioPinMode mode)
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
