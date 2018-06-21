using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Devices.Gpio
{
    public abstract class GpioDriver : IDisposable
    {
        public abstract int PinCount { get; }

        public abstract int ConvertPinNumber(int number, GpioScheme from, GpioScheme to);

        public abstract void SetPinMode(int pin, GpioPinMode mode);

        public abstract GpioPinMode GetPinMode(int pin);

        public abstract void Output(int pin, GpioPinValue value);

        public abstract GpioPinValue Input(int pin);

        public abstract void Dispose();
    }
}
