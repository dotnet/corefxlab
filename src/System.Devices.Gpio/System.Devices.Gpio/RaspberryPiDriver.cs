using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Devices.Gpio
{
    public unsafe class RaspberryPiDriver : GpioDriver
    {
        #region RegisterView        

        [StructLayout(LayoutKind.Explicit)]
        private struct RegisterView
        {
            ///<summary>GPIO Function Select, 6x32 bits, R/W</summary>
            [FieldOffset(0x00)]
            public fixed UInt32 GPFSEL[6];

            /////<summary>GPIO Function Select 0, 32 bits, R/W</summary>
            //[FieldOffset(0x00)]
            //public UInt32 GPFSEL0;

            /////<summary>GPIO Function Select 1, 32 bits, R/W</summary>
            //[FieldOffset(0x04)]
            //public UInt32 GPFSEL1;

            /////<summary>GPIO Function Select 2, 32 bits, R/W</summary>
            //[FieldOffset(0x08)]
            //public UInt32 GPFSEL2;

            /////<summary>GPIO Function Select 3, 32 bits, R/W</summary>
            //[FieldOffset(0x0C)]
            //public UInt32 GPFSEL3;

            /////<summary>GPIO Function Select 4, 32 bits, R/W</summary>
            //[FieldOffset(0x10)]
            //public UInt32 GPFSEL4;

            /////<summary>GPIO Function Select 5, 32 bits, R/W</summary>
            //[FieldOffset(0x14)]
            //public UInt32 GPFSEL5;

            //[FieldOffset(0x18)]
            //- Reserved - - 

            ///<summary>GPIO Pin Output Set, 2x32 bits, W</summary>
            [FieldOffset(0x1C)]
            public fixed UInt32 GPSET[2];

            /////<summary>GPIO Pin Output Set 0, 32 bits, W</summary>
            //[FieldOffset(0x1C)]
            //public UInt32 GPSET0;

            /////<summary>GPIO Pin Output Set 1, 32 bits, W</summary>
            //[FieldOffset(0x20)]
            //public UInt32 GPSET1;

            //[FieldOffset(0x24)]
            //- Reserved - - 

            ///<summary>GPIO Pin Output Clear, 2x32 bits, W</summary>
            [FieldOffset(0x28)]
            public fixed UInt32 GPCLR[2];

            /////<summary>GPIO Pin Output Clear 0, 32 bits, W</summary>
            //[FieldOffset(0x28)]
            //public UInt32 GPCLR0;

            /////<summary>GPIO Pin Output Clear 1, 32 bits, W</summary>
            //[FieldOffset(0x2C)]
            //public UInt32 GPCLR1;

            //[FieldOffset(0x30)]
            //- Reserved - - 

            ///<summary>GPIO Pin Level, 2x32 bits, R</summary>
            [FieldOffset(0x34)]
            public fixed UInt32 GPLEV[2];

            /////<summary>GPIO Pin Level 0, 32 bits, R</summary>
            //[FieldOffset(0x34)]
            //public UInt32 GPLEV0;

            /////<summary>GPIO Pin Level 1, 32 bits, R</summary>
            //[FieldOffset(0x38)]
            //public UInt32 GPLEV1;

            //[FieldOffset(0x3C)]
            //- Reserved - - 

            ///<summary>GPIO Pin Event Detect Status, 2x32 bits, R/W</summary>
            [FieldOffset(0x40)]
            public fixed UInt32 GPEDS[2];

            /////<summary>GPIO Pin Event Detect Status 0, 32 bits, R/W</summary>
            //[FieldOffset(0x40)]
            //public UInt32 GPEDS0;

            /////<summary>GPIO Pin Event Detect Status 1, 32 bits, R/W</summary>
            //[FieldOffset(0x44)]
            //public UInt32 GPEDS1;

            //[FieldOffset(0x48)]
            //- Reserved - - 

            ///<summary>GPIO Pin Rising Edge Detect Enable, 2x32 bits, R/W</summary>
            [FieldOffset(0x4C)]
            public fixed UInt32 GPREN[2];

            /////<summary>GPIO Pin Rising Edge Detect Enable 0, 32 bits, R/W</summary>
            //[FieldOffset(0x4C)]
            //public UInt32 GPREN0;

            /////<summary>GPIO Pin Rising Edge Detect Enable 1, 32 bits, R/W</summary>
            //[FieldOffset(0x50)]
            //public UInt32 GPREN1;

            //[FieldOffset(0x54)]
            //- Reserved - - 

            ///<summary>GPIO Pin Falling Edge Detect Enable, 2x32 bits, R/W</summary>
            [FieldOffset(0x58)]
            public fixed UInt32 GPFEN[2];

            /////<summary>GPIO Pin Falling Edge Detect Enable 0, 32 bits, R/W</summary>
            //[FieldOffset(0x58)]
            //public UInt32 GPFEN0;

            /////<summary>GPIO Pin Falling Edge Detect Enable 1, 32 bits, R/W</summary>
            //[FieldOffset(0x5C)]
            //public UInt32 GPFEN1;

            //[FieldOffset(0x60)]
            //- Reserved - - 

            ///<summary>GPIO Pin High Detect Enable, 2x32 bits, R/W</summary>
            [FieldOffset(0x64)]
            public fixed UInt32 GPHEN[2];

            /////<summary>GPIO Pin High Detect Enable 0, 32 bits, R/W</summary>
            //[FieldOffset(0x64)]
            //public UInt32 GPHEN0;

            /////<summary>GPIO Pin High Detect Enable 1, 32 bits, R/W</summary>
            //[FieldOffset(0x68)]
            //public UInt32 GPHEN1;

            //[FieldOffset(0x6C)]
            //- Reserved - - 

            ///<summary>GPIO Pin Low Detect Enable, 2x32 bits, R/W</summary>
            [FieldOffset(0x70)]
            public fixed UInt32 GPLEN[2];

            /////<summary>GPIO Pin Low Detect Enable 0, 32 bits, R/W</summary>
            //[FieldOffset(0x70)]
            //public UInt32 GPLEN0;

            /////<summary>GPIO Pin Low Detect Enable 1, 32 bits, R/W</summary>
            //[FieldOffset(0x74)]
            //public UInt32 GPLEN1;

            //[FieldOffset(0x78)]
            //- Reserved - - 

            ///<summary>GPIO Pin Async. Rising Edge Detect, 2x32 bits, R/W</summary>
            [FieldOffset(0x7C)]
            public fixed UInt32 GPAREN[2];

            /////<summary>GPIO Pin Async. Rising Edge Detect 0, 32 bits, R/W</summary>
            //[FieldOffset(0x7C)]
            //public UInt32 GPAREN0;

            /////<summary>GPIO Pin Async. Rising Edge Detect 1, 32 bits, R/W</summary>
            //[FieldOffset(0x80)]
            //public UInt32 GPAREN1;

            //[FieldOffset(0x84)]
            //- Reserved - - 

            ///<summary>GPIO Pin Async. Falling Edge Detect, 2x32 bits, R/W</summary>
            [FieldOffset(0x88)]
            public fixed UInt32 GPAFEN[2];

            /////<summary>GPIO Pin Async. Falling Edge Detect 0, 32 bits, R/W</summary>
            //[FieldOffset(0x88)]
            //public UInt32 GPAFEN0;

            /////<summary>GPIO Pin Async. Falling Edge Detect 1, 32 bits, R/W</summary>
            //[FieldOffset(0x8C)]
            //public UInt32 GPAFEN1;

            //[FieldOffset(0x90)]
            //- Reserved - - 

            ///<summary>GPIO Pin Pull-up/down Enable, 32 bits, R/W</summary>
            [FieldOffset(0x94)]
            public UInt32 GPPUD;

            ///<summary>GPIO Pin Pull-up/down Enable Clock, 2x32 bits, R/W</summary>
            [FieldOffset(0x98)]
            public fixed UInt32 GPPUDCLK[2];

            /////<summary>GPIO Pin Pull-up/down Enable Clock 0, 32 bits, R/W</summary>
            //[FieldOffset(0x98)]
            //public UInt32 GPPUDCLK0;

            /////<summary>GPIO Pin Pull-up/down Enable Clock 1, 32 bits, R/W</summary>
            //[FieldOffset(0x9C)]
            //public UInt32 GPPUDCLK1;

            //[FieldOffset(0xA0)]
            //- Reserved - - 

            //[FieldOffset(0xB0)]
            //- Test 4 bits R/W
        }

        #endregion

        #region Interop

        private const string LibraryName = "libc";

        [Flags]
        private enum FileOpenFlags
        {
            O_RDWR = 0x02,
            O_SYNC = 0x101000
        }

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int open([MarshalAs(UnmanagedType.LPStr)] string pathname, FileOpenFlags flags);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int close(int fd);

        [Flags]
        private enum MemoryMappedProtections
        {
            PROT_NONE = 0x0,
            PROT_READ = 0x1,
            PROT_WRITE = 0x2,
            PROT_EXEC = 0x4
        }

        [Flags]
        private enum MemoryMappedFlags
        {
            MAP_SHARED = 0x01,
            MAP_PRIVATE = 0x02,
            MAP_FIXED = 0x10
        }

        [DllImport(LibraryName, SetLastError = true)]
        private static extern IntPtr mmap(IntPtr addr, int length, MemoryMappedProtections prot, MemoryMappedFlags flags, int fd, int offset);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int munmap(IntPtr addr, int length);

        #endregion

        private const string GpioMemoryFilePath = "/dev/gpiomem";
        private const int GpioBaseOffset = 0;

        private RegisterView* RegisterViewPointer = null;

        public RaspberryPiDriver()
        {
            var fileDescriptor = open(GpioMemoryFilePath, FileOpenFlags.O_RDWR | FileOpenFlags.O_SYNC);

            if (fileDescriptor < 0)
            {
                throw new IOException($"open error number: {Marshal.GetLastWin32Error()}");
            }

            Console.WriteLine($"file descriptor = {fileDescriptor}");

            var mapPointer = mmap(IntPtr.Zero, Environment.SystemPageSize, MemoryMappedProtections.PROT_READ | MemoryMappedProtections.PROT_WRITE, MemoryMappedFlags.MAP_SHARED, fileDescriptor, GpioBaseOffset);

            if (mapPointer.ToInt32() < 0)
            {
                throw new IOException($"mmap error number: {Marshal.GetLastWin32Error()}");
            }

            Console.WriteLine($"mmap returned address = {mapPointer.ToInt32():X16}");

            close(fileDescriptor);

            RegisterViewPointer = (RegisterView*)mapPointer;
        }

        public override void Dispose()
        {
            if (RegisterViewPointer != null)
            {
                munmap((IntPtr)RegisterViewPointer, 0);
            }
        }

        public override void SetPinMode(int pin, GpioPinMode mode)
        {
            if (pin < 0 || pin >= PinCount) throw new ArgumentOutOfRangeException(nameof(pin));
            if (mode < 0 || mode > GpioPinMode.InputPullUp) throw new ArgumentOutOfRangeException(nameof(mode));

            switch (mode)
            {
                case GpioPinMode.Input:
                case GpioPinMode.InputPullDown:
                case GpioPinMode.InputPullUp:
                    SetInputPullMode(pin, PinModeToGPPUD(mode));
                    break;
            }                

            SetPinMode(pin, PinModeToGPFSEL(mode));
        }

        private void SetInputPullMode(int pin, uint mode)
        {
            //SetBits(RegisterViewPointer->GPPUD, pin, 1, 2, mode);
            //Thread.SpinWait(150);
            //SetBit(RegisterViewPointer->GPPUDCLK, pin);
            //Thread.SpinWait(150);
            //SetBit(RegisterViewPointer->GPPUDCLK, pin, 0U);

            uint* gppudPointer = &RegisterViewPointer->GPPUD;

            Console.WriteLine($"{nameof(RegisterView.GPPUD)} register address = {(long)gppudPointer:X16}");

            var register = *gppudPointer;

            Console.WriteLine($"{nameof(RegisterView.GPPUD)} original register value = {register:X8}");

            register &= ~0b11U;
            register |= mode;

            Console.WriteLine($"{nameof(RegisterView.GPPUD)} new register value = {register:X8}");

            *gppudPointer = register;

            Thread.SpinWait(150);

            var index = pin / 32;
            var shift = pin % 32;
            uint* gppudclkPointer = &RegisterViewPointer->GPPUDCLK[index];

            Console.WriteLine($"{nameof(RegisterView.GPPUDCLK)} register address = {(long)gppudclkPointer:X16}");

            register = *gppudclkPointer;

            Console.WriteLine($"{nameof(RegisterView.GPPUDCLK)} original register value = {register:X8}");

            register |= 1U << shift;

            Console.WriteLine($"{nameof(RegisterView.GPPUDCLK)} new register value = {register:X8}");

            *gppudclkPointer = register;

            Thread.SpinWait(150);

            register = *gppudPointer;
            register &= ~0b11U;

            Console.WriteLine($"{nameof(RegisterView.GPPUD)} new register value = {register:X8}");
            Console.WriteLine($"{nameof(RegisterView.GPPUDCLK)} new register value = {0:X8}");

            *gppudPointer = register;
            *gppudclkPointer = 0;
        }

        private void SetPinMode(int pin, uint mode)
        {
            //SetBits(RegisterViewPointer->GPFSEL, pin, 10, 3, mode);

            var index = pin / 10;
            var shift = (pin % 10) * 3;
            uint* registerPointer = &RegisterViewPointer->GPFSEL[index];

            //Console.WriteLine($"{nameof(RegisterView.GPFSEL)} register address = {(long)registerPointer:X16}");

            var register = *registerPointer;

            //Console.WriteLine($"{nameof(RegisterView.GPFSEL)} original register value = {register:X8}");

            register &= ~(0b111U << shift);
            register |= mode << shift;

            //Console.WriteLine($"{nameof(RegisterView.GPFSEL)} new register value = {register:X8}");

            *registerPointer = register;
        }

        public override GpioPinMode GetPinMode(int pin)
        {
            if (pin < 0 || pin >= PinCount) throw new ArgumentOutOfRangeException(nameof(pin));

            //var mode = GetBits(RegisterViewPointer->GPFSEL, pin, 10, 3);

            var index = pin / 10;
            var shift = (pin % 10) * 3;
            var register = RegisterViewPointer->GPFSEL[index];
            var mode = (register >> shift) & 0b111U;

            var result = GPFSELToPinMode(mode);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint PinModeToGPPUD(GpioPinMode mode)
        {
            uint result;

            switch (mode)
            {
                case GpioPinMode.Input: result = 0; break;
                case GpioPinMode.InputPullDown: result = 1; break;
                case GpioPinMode.InputPullUp: result = 2; break;

                default: throw new ArgumentOutOfRangeException(nameof(mode));
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint PinModeToGPFSEL(GpioPinMode mode)
        {
            uint result;

            switch (mode)
            {
                case GpioPinMode.Input:
                case GpioPinMode.InputPullDown:
                case GpioPinMode.InputPullUp: result = 0; break;
                
                case GpioPinMode.Output: result = 1; break;

                default: throw new ArgumentOutOfRangeException(nameof(mode));
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GpioPinMode GPFSELToPinMode(uint gpfselValue)
        {
            GpioPinMode result;

            switch (gpfselValue)
            {
                case 0: result = GpioPinMode.Input; break;
                case 1: result = GpioPinMode.Output; break;

                default: throw new ArgumentOutOfRangeException(nameof(gpfselValue));
            }

            return result;
        }

        public override void Output(int pin, GpioPinValue value)
        {
            if (pin < 0 || pin >= PinCount) throw new ArgumentOutOfRangeException(nameof(pin));
            if (value < 0 || value > GpioPinValue.High) throw new ArgumentOutOfRangeException(nameof(value));

            //switch (value)
            //{
            //    case GpioPinValue.High:
            //        SetBit(RegisterViewPointer->GPSET, pin);
            //        break;

            //    case GpioPinValue.Low:
            //        SetBit(RegisterViewPointer->GPCLR, pin);
            //        break;

            //    default: throw new ArgumentOutOfRangeException(nameof(value));
            //}

            var index = pin / 32;
            var shift = pin % 32;
            uint* registerPointer = null;
            var registerName = string.Empty;

            switch (value)
            {
                case GpioPinValue.High:
                    registerPointer = &RegisterViewPointer->GPSET[index];
                    registerName = nameof(RegisterView.GPSET);
                    break;

                case GpioPinValue.Low:
                    registerPointer = &RegisterViewPointer->GPCLR[index];
                    registerName = nameof(RegisterView.GPCLR);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(value));
            }

            //Console.WriteLine($"{registerName} register address = {(long)registerPointer:X16}");

            var register = *registerPointer;

            //Console.WriteLine($"{registerName} original register value = {register:X8}");

            register |= 1U << shift;

            //Console.WriteLine($"{registerName} new register value = {register:X8}");

            *registerPointer = register;
        }

        public override GpioPinValue Input(int pin)
        {
            if (pin < 0 || pin >= PinCount) throw new ArgumentOutOfRangeException(nameof(pin));

            //var value = GetBit(RegisterViewPointer->GPLEV, pin);

            var index = pin / 32;
            var shift = pin % 32;
            var register = RegisterViewPointer->GPLEV[index];
            var value = (register >> shift) & 1;

            var result = Convert.ToBoolean(value) ? GpioPinValue.High : GpioPinValue.Low;
            return result;
        }

        public override void ClearDetectedEvent(int pin)
        {
            if (pin < 0 || pin >= PinCount) throw new ArgumentOutOfRangeException(nameof(pin));

            //SetBit(RegisterViewPointer->GPEDS, pin);

            var index = pin / 32;
            var shift = pin % 32;
            uint* registerPointer = &RegisterViewPointer->GPEDS[index];

            Console.WriteLine($"{nameof(RegisterView.GPEDS)} register address = {(long)registerPointer:X16}");

            var register = *registerPointer;

            Console.WriteLine($"{nameof(RegisterView.GPEDS)} original register value = {register:X8}");

            register |= 1U << shift;

            Console.WriteLine($"{nameof(RegisterView.GPEDS)} new register value = {register:X8}");

            *registerPointer = register;

            Thread.SpinWait(150);

            *registerPointer = 0;
        }

        public override bool EventWasDetected(int pin)
        {
            if (pin < 0 || pin >= PinCount) throw new ArgumentOutOfRangeException(nameof(pin));

            //var value = GetBit(RegisterViewPointer->GPEDS, pin);

            var index = pin / 32;
            var shift = pin % 32;
            var register = RegisterViewPointer->GPEDS[index];
            var value = (register >> shift) & 1;

            var result = Convert.ToBoolean(value);

            if (result)
            {
                ClearDetectedEvent(pin);
            }

            return result;
        }

        public override void SetEventDetection(int pin, GpioEventKind kind, bool enabled)
        {
            if (pin < 0 || pin >= PinCount) throw new ArgumentOutOfRangeException(nameof(pin));
            if (kind < 0 || kind > GpioEventKind.AsyncRisingEdge) throw new ArgumentOutOfRangeException(nameof(kind));

            //switch (kind)
            //{
            //    case GpioEventKind.High:
            //        SetBit(RegisterViewPointer->GPHEN, pin);
            //        break;

            //    case GpioEventKind.Low:
            //        SetBit(RegisterViewPointer->GPLEN, pin);
            //        break;

            //    case GpioEventKind.SyncRisingEdge:
            //        SetBit(RegisterViewPointer->GPREN, pin);
            //        break;

            //    case GpioEventKind.SyncFallingEdge:
            //        SetBit(RegisterViewPointer->GPFEN, pin);
            //        break;

            //    case GpioEventKind.AsyncRisingEdge:
            //        SetBit(RegisterViewPointer->GPAREN, pin);
            //        break;

            //    case GpioEventKind.AsyncFallingEdge:
            //        SetBit(RegisterViewPointer->GPAFEN, pin);
            //        break;

            //    default: throw new ArgumentOutOfRangeException(nameof(kind));
            //}

            var index = pin / 32;
            var shift = pin % 32;
            uint* registerPointer = null;
            var registerName = string.Empty;

            switch (kind)
            {
                case GpioEventKind.High:
                    registerPointer = &RegisterViewPointer->GPHEN[index];
                    registerName = nameof(RegisterView.GPHEN);
                    break;

                case GpioEventKind.Low:
                    registerPointer = &RegisterViewPointer->GPLEN[index];
                    registerName = nameof(RegisterView.GPLEN);
                    break;

                case GpioEventKind.SyncRisingEdge:
                    registerPointer = &RegisterViewPointer->GPREN[index];
                    registerName = nameof(RegisterView.GPREN);
                    break;

                case GpioEventKind.SyncFallingEdge:
                    registerPointer = &RegisterViewPointer->GPFEN[index];
                    registerName = nameof(RegisterView.GPFEN);
                    break;

                case GpioEventKind.AsyncRisingEdge:
                    registerPointer = &RegisterViewPointer->GPAREN[index];
                    registerName = nameof(RegisterView.GPAREN);
                    break;

                case GpioEventKind.AsyncFallingEdge:
                    registerPointer = &RegisterViewPointer->GPAFEN[index];
                    registerName = nameof(RegisterView.GPAFEN);
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kind));
            }

            Console.WriteLine($"{registerName} register address = {(long)registerPointer:X16}");

            var register = *registerPointer;

            Console.WriteLine($"{registerName} original register value = {register:X8}");

            if (enabled)
            {
                register |= 1U << shift;
            }
            else
            {
                register &= ~(1U << shift);
            }

            Console.WriteLine($"{registerName} new register value = {register:X8}");

            *registerPointer = register;

            ClearDetectedEvent(pin);
        }

        public override bool GetEventDetection(int pin, GpioEventKind kind)
        {
            if (pin < 0 || pin >= PinCount) throw new ArgumentOutOfRangeException(nameof(pin));
            if (kind < 0 || kind > GpioEventKind.AsyncRisingEdge) throw new ArgumentOutOfRangeException(nameof(kind));

            //switch (kind)
            //{
            //    case GpioEventKind.High:
            //        value = GetBit(RegisterViewPointer->GPHEN, pin);
            //        break;

            //    case GpioEventKind.Low:
            //        value = GetBit(RegisterViewPointer->GPLEN, pin);
            //        break;

            //    case GpioEventKind.SyncRisingEdge:
            //        value = GetBit(RegisterViewPointer->GPREN, pin);
            //        break;

            //    case GpioEventKind.SyncFallingEdge:
            //        value = GetBit(RegisterViewPointer->GPFEN, pin);
            //        break;

            //    case GpioEventKind.AsyncRisingEdge:
            //        value = GetBit(RegisterViewPointer->GPAREN, pin);
            //        break;

            //    case GpioEventKind.AsyncFallingEdge:
            //        value = GetBit(RegisterViewPointer->GPAFEN, pin);
            //        break;

            //    default: throw new ArgumentOutOfRangeException(nameof(kind));
            //}

            var index = pin / 32;
            var shift = pin % 32;
            var registerName = string.Empty;
            var register = 0U;

            switch (kind)
            {
                case GpioEventKind.High:
                    register = RegisterViewPointer->GPHEN[index];
                    break;

                case GpioEventKind.Low:
                    register = RegisterViewPointer->GPLEN[index];
                    break;

                case GpioEventKind.SyncRisingEdge:
                    register = RegisterViewPointer->GPREN[index];
                    break;

                case GpioEventKind.SyncFallingEdge:
                    register = RegisterViewPointer->GPFEN[index];
                    break;

                case GpioEventKind.AsyncRisingEdge:
                    register = RegisterViewPointer->GPAREN[index];
                    break;

                case GpioEventKind.AsyncFallingEdge:
                    register = RegisterViewPointer->GPAFEN[index];
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kind));
            }

            var value = (register >> shift) & 1;

            var result = Convert.ToBoolean(value);
            return result;
        }

        

        public override int PinCount => 54;

        public override int ConvertPinNumber(int number, GpioScheme from, GpioScheme to)
        {
            int result = -1;

            switch (from)
            {
                case GpioScheme.BCM:
                    switch (to)
                    {
                        case GpioScheme.BCM:
                            result = number;
                            break;

                        case GpioScheme.Board:
                            //throw new NotImplementedException();
                            break;

                        default: throw new Exception($"Unsupported GPIO scheme '{to}'");
                    }
                    break;

                case GpioScheme.Board:
                    switch (to)
                    {
                        case GpioScheme.Board:
                            result = number;
                            break;

                        case GpioScheme.BCM:
                            //throw new NotImplementedException();
                            break;

                        default: throw new Exception($"Unsupported GPIO scheme '{to}'");
                    }
                    break;

                default: throw new Exception($"Unsupported GPIO Pin scheme '{from}'");
            }

            return result;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static void SetBit(UInt32* pointer, int bit, uint value = 1)
        //{
        //    SetBits(pointer, bit, 32, 1, value);
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static bool GetBit(UInt32* pointer, int bit)
        //{
        //    var result = GetBits(pointer, bit, 32, 1);
        //    return Convert.ToBoolean(result);
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static void SetBits(UInt32* pointer, int item, int itemsPerRegister, int bitsPerItem, uint value)
        //{
        //    var index = item / itemsPerRegister;
        //    var shift = (item % itemsPerRegister) * bitsPerItem;
        //    var mask = (uint)(1 << bitsPerItem) - 1;
        //    uint* registerPointer = &pointer[index];
        //    var register = *registerPointer;
        //    register &= ~(mask << shift);
        //    register |= value << shift;
        //    *registerPointer = register;
        //}

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static uint GetBits(UInt32* pointer, int item, int itemsPerSlot, int bitsPerItem)
        //{
        //    var index = item / itemsPerSlot;
        //    var shift = (item % itemsPerSlot) * bitsPerItem;
        //    var mask = (uint)(1 << bitsPerItem) - 1;
        //    var register = pointer[index];
        //    var value = (register >> shift) & mask;
        //    return value;
        //}
    }
}
