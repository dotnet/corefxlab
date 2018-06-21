using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Devices.Gpio
{
    public enum GpioPinValue
    {
        Low = 0,
        High = 1
    }

    public abstract class GpioDriver : IDisposable
    {
        public abstract void SetPinMode(int pin, GpioPinMode mode);

        public abstract GpioPinMode GetPinMode(int pin);

        public abstract void Output(int pin, GpioPinValue value);

        public abstract GpioPinValue Input(int pin);

        public abstract void Dispose();
    }
}
