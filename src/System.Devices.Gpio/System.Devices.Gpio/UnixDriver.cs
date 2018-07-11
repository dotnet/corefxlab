// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Devices.Gpio
{
    public class UnixDriver : GpioDriver
    {
        #region Interop

        private const string LibraryName = "libc";

        [Flags]
        private enum FileOpenFlags
        {
            O_RDONLY = 0x00,
            O_NONBLOCK = 0x800,
            O_RDWR = 0x02,
            O_SYNC = 0x101000
        }

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int open([MarshalAs(UnmanagedType.LPStr)] string pathname, FileOpenFlags flags);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int close(int fd);

        private enum PollOperations
        {
            EPOLL_CTL_ADD = 1,
            EPOLL_CTL_DEL = 2
        }

        private enum PollEvents : uint
        {
            EPOLLIN = 0x01,
            EPOLLET = 0x80000000,
            EPOLLPRI = 0x02
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct epoll_data
        {
            [FieldOffset(0)]
            public IntPtr ptr;

            [FieldOffset(0)]
            public int fd;

            [FieldOffset(0)]
            public uint u32;

            [FieldOffset(0)]
            public ulong u64;

            [FieldOffset(0)]
            public int bcmPinNumber;
        }

        private struct epoll_event
        {
            public PollEvents events;
            public epoll_data data;
        }

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int epoll_create(int size);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int epoll_ctl(int epfd, PollOperations op, int fd, ref epoll_event events);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int epoll_wait(int epfd, out epoll_event events, int maxevents, int timeout);

        private enum SeekFlags
        {
            SEEK_SET = 0
        }

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int lseek(int fd, int offset, SeekFlags whence);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int read(int fd, IntPtr buf, int count);


        private enum SPI_IOC : uint
        {
            /// <summary> Set SPI mode</summary>
            SPI_IOC_WR_MODE = 0x40016b01,
            /// <summary> Get SPI mode</summary>
            SPI_IOC_RD_MODE = 0x80016b01,
            /// <summary> Set bits per word</summary>
            SPI_IOC_WR_BITS_PER_WORD = 0x40016b03,
            /// <summary> Get bits per word</summary>
            SPI_IOC_RD_BITS_PER_WORD = 0x80016b03,
            /// <summary> Set max speed (Hz)</summary>
            SPI_IOC_WR_MAX_SPEED_HZ = 0x40046b04,
            /// <summary>Get max speed (Hz)</summary>
            SPI_IOC_RD_MAX_SPEED_HZ = 0x80046b04
        }

        [Flags]
        private enum SPIMode : byte
        {
            None = 0x00,
            SPI_LOOP = 0x20,
            SPI_CPHA = 0x01,
            SPI_CPOL = 0x02,
            SPI_LSB_FIRST = 0x08,
            SPI_CS_HIGH = 0x04,
            SPI_3WIRE = 0x10,
            SPI_NO_CS = 0x40,
            SPI_READY = 0x80
        }

        private struct spi_ioc_transfer
        {
            public ulong tx_buf;
            public ulong rx_buf;
            public uint len;
            public uint speed_hz;
            public ushort delay_usecs;
            public byte bits_per_word;
            public byte cs_change;
            public byte tx_nbits;
            public byte rx_nbits;
            public ushort pad;
        };

        const uint SPI_IOC_MESSAGE_1 = 0x40206b00;

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int ioctl(int fd, uint request, IntPtr argp);

        public static unsafe int TestingSPI()
        {
            //string device = "/dev/spidev1.1";
            string device = "/dev/spidev0.0";
            int fd = open(device, FileOpenFlags.O_RDWR);
            if (fd < 0)
            {
                return 1;
            }

            SPIMode mode = SPIMode.None;
            var ptr = new IntPtr(&mode);

            int ret = ioctl(fd, (uint)SPI_IOC.SPI_IOC_WR_MODE, ptr);
            if (ret == -1)
            {
                return 2;
            }

            ret = ioctl(fd, (uint)SPI_IOC.SPI_IOC_RD_MODE, ptr);
            if (ret == -1)
            {
                return 3;
            }

            byte bits = 8;
            ptr = new IntPtr(&bits);

            ret = ioctl(fd, (uint)SPI_IOC.SPI_IOC_WR_BITS_PER_WORD, ptr);
            if (ret == -1)
            {
                return 4;
            }

            ret = ioctl(fd, (uint)SPI_IOC.SPI_IOC_RD_BITS_PER_WORD, ptr);
            if (ret == -1)
            {
                return 5;
            }

            uint speed = 500000;
            ptr = new IntPtr(&speed);

            ret = ioctl(fd, (uint)SPI_IOC.SPI_IOC_WR_MAX_SPEED_HZ, ptr);
            if (ret == -1)
            {
                return 6;
            }

            ret = ioctl(fd, (uint)SPI_IOC.SPI_IOC_RD_MAX_SPEED_HZ, ptr);
            if (ret == -1)
            {
                return 7;
            }

            Console.WriteLine($"spi mode: {mode}");
            Console.WriteLine($"bits per word: {bits}");
            Console.WriteLine($"max speed: {speed} Hz ({speed / 1000} KHz)");

            var tx = new byte[]
            {
                0xA, 0xB, 0xC, 0xD, 0xE, 0xF
            };

            var rx = new byte[tx.Length];

            fixed (byte* txPtr = tx, rxPtr = rx)
            {
                var tr = new spi_ioc_transfer()
                {
                    tx_buf = (ulong)txPtr,
                    rx_buf = (ulong)rxPtr,
                    len = (uint)tx.Length,
                    delay_usecs = 0,
                    speed_hz = speed,
                    bits_per_word = bits,
                };

                ret = ioctl(fd, SPI_IOC_MESSAGE_1, new IntPtr(&tr));
                if (ret < 1)
                {
                    return 8;
                }
            }

            Console.WriteLine("Sent data:");

            foreach (byte b in tx)
            {
                Console.Write("{0:X2} ", b);
            }

            Console.WriteLine();
            Console.WriteLine("Received data:");

            foreach (byte b in rx)
            {
                Console.Write("{0:X2} ", b);
            }

            Console.WriteLine();

            close(fd);
            return 0;
        }

        #endregion

        private const string GpioPath = "/sys/class/gpio";

        private readonly BitArray _exportedPins;

        private int _pollFileDescriptor = -1;
        private int[] _pinValueFileDescriptors;

        private int _pinsToDetectEventsCount;
        private BitArray _pinsToDetectEvents;
        private Thread _eventDetectionThread;
        private TimeSpan[] _debounceTimeouts;
        private DateTime[] _lastEvents;

        public UnixDriver(int pinCount)
        {
            PinCount = pinCount;
            _exportedPins = new BitArray(pinCount);
            _pinsToDetectEvents = new BitArray(pinCount);
            _debounceTimeouts = new TimeSpan[pinCount];
            _lastEvents = new DateTime[pinCount];
            _pinValueFileDescriptors = new int[pinCount];
        }

        public override void Dispose()
        {
            _pinsToDetectEventsCount = 0;

            for (int i = 0; i < _pinValueFileDescriptors.Length; ++i)
            {
                int fd = _pinValueFileDescriptors[i];

                if (fd != -1)
                {
                    close(fd);
                }
            }

            if (_pollFileDescriptor != -1)
            {
                close(_pollFileDescriptor);
                _pollFileDescriptor = -1;
            }

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

        protected internal override void OpenPin(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);
            ExportPin(bcmPinNumber);
        }

        protected internal override void ClosePin(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);

            SetPinEventsToDetect(bcmPinNumber, PinEvent.None);
            _debounceTimeouts[bcmPinNumber] = default;
            _lastEvents[bcmPinNumber] = default;
            UnexportPin(bcmPinNumber);
        }

        protected internal override PinMode GetPinMode(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);

            string directionPath = $"{GpioPath}/gpio{bcmPinNumber}/direction";
            string stringMode = File.ReadAllText(directionPath);
            PinMode mode = StringModeToPinMode(stringMode);
            return mode;
        }

        protected internal override void SetPinMode(int bcmPinNumber, PinMode mode)
        {
            ValidatePinNumber(bcmPinNumber);
            ValidatePinMode(mode);

            string directionPath = $"{GpioPath}/gpio{bcmPinNumber}/direction";
            string stringMode = PinModeToStringMode(mode);
            File.WriteAllText(directionPath, stringMode);
        }

        protected internal override PinValue Input(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);

            string valuePath = $"{GpioPath}/gpio{bcmPinNumber}/value";
            string stringValue = File.ReadAllText(valuePath);
            PinValue value = StringValueToPinValue(stringValue);
            return value;
        }

        protected internal override void Output(int bcmPinNumber, PinValue value)
        {
            ValidatePinNumber(bcmPinNumber);
            ValidatePinValue(value);

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

        protected internal override void SetPinEventsToDetect(int bcmPinNumber, PinEvent kind)
        {
            ValidatePinNumber(bcmPinNumber);

            string edgePath = $"{GpioPath}/gpio{bcmPinNumber}/edge";
            string stringValue = EventKindToStringValue(kind);
            File.WriteAllText(edgePath, stringValue);
        }

        protected internal override PinEvent GetPinEventsToDetect(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);

            string edgePath = $"{GpioPath}/gpio{bcmPinNumber}/edge";
            string stringValue = File.ReadAllText(edgePath);
            PinEvent value = StringValueToEventKind(stringValue);
            return value;
        }

        protected internal override void SetEnableRaisingPinEvents(int bcmPinNumber, bool enable)
        {
            ValidatePinNumber(bcmPinNumber);

            bool wasEnabled = _pinsToDetectEvents[bcmPinNumber];
            _pinsToDetectEvents[bcmPinNumber] = enable;

            if (enable && !wasEnabled)
            {
                // Enable pin events detection
                _pinsToDetectEventsCount++;

                AddPinToPoll(bcmPinNumber, ref _pollFileDescriptor, out _);

                if (_eventDetectionThread == null)
                {
                    _eventDetectionThread = new Thread(DetectEvents)
                    {
                        IsBackground = true
                    };

                    _eventDetectionThread.Start();
                }
            }
            else if (!enable && wasEnabled)
            {
                // Disable pin events detection
                _pinsToDetectEventsCount--;

                bool closePollFileDescriptor = (_pinsToDetectEventsCount == 0);
                RemovePinFromPoll(bcmPinNumber, ref _pollFileDescriptor, closePinValueFileDescriptor: true, closePollFileDescriptor);
            }
        }

        private void AddPinToPoll(int bcmPinNumber, ref int pollFileDescriptor, out bool closePinValueFileDescriptor)
        {
            //Console.WriteLine($"Adding pin to poll: {bcmPinNumber}");

            if (pollFileDescriptor == -1)
            {
                pollFileDescriptor = epoll_create(1);

                if (pollFileDescriptor < 0)
                {
                    throw new GpioException($"epoll_create error number: {Marshal.GetLastWin32Error()}");
                }
            }

            closePinValueFileDescriptor = false;
            int fd = _pinValueFileDescriptors[bcmPinNumber];

            if (fd <= 0)
            {
                string valuePath = $"{GpioPath}/gpio{bcmPinNumber}/value";
                fd = open(valuePath, FileOpenFlags.O_RDONLY | FileOpenFlags.O_NONBLOCK);

                //Console.WriteLine($"{valuePath} open result: {fd}");

                if (fd < 0)
                {
                    throw new GpioException($"open error number: {Marshal.GetLastWin32Error()}");
                }

                _pinValueFileDescriptors[bcmPinNumber] = fd;
                closePinValueFileDescriptor = true;
            }

            var ev = new epoll_event
            {
                events = PollEvents.EPOLLIN | PollEvents.EPOLLET | PollEvents.EPOLLPRI,
                data = new epoll_data()
                {
                    //fd = fd
                    bcmPinNumber = bcmPinNumber
                }
            };

            //Console.WriteLine($"poll_fd = {pollFileDescriptor}, pin_value_fd = {fd}");

            int r = epoll_ctl(pollFileDescriptor, PollOperations.EPOLL_CTL_ADD, fd, ref ev);

            if (r == -1)
            {
                throw new GpioException($"epoll_ctl error number: {Marshal.GetLastWin32Error()}");
            }

            // Ignore first time because it returns the current state
            epoll_wait(pollFileDescriptor, out _, 1, 0);
        }

        private void RemovePinFromPoll(int bcmPinNumber, ref int pollFileDescriptor, bool closePinValueFileDescriptor, bool closePollFileDescriptor)
        {
            //Console.WriteLine($"Removing pin from poll: {bcmPinNumber}");

            int fd = _pinValueFileDescriptors[bcmPinNumber];

            var ev = new epoll_event
            {
                events = PollEvents.EPOLLIN | PollEvents.EPOLLET | PollEvents.EPOLLPRI,
            };

            //Console.WriteLine($"poll_fd = {pollFileDescriptor}, pin_value_fd = {fd}");

            int r = epoll_ctl(pollFileDescriptor, PollOperations.EPOLL_CTL_DEL, fd, ref ev);

            if (r == -1)
            {
                throw new GpioException($"epoll_ctl error number: {Marshal.GetLastWin32Error()}");
            }

            if (closePinValueFileDescriptor)
            {
                close(fd);
                _pinValueFileDescriptors[bcmPinNumber] = -1;
            }

            if (closePollFileDescriptor)
            {
                close(pollFileDescriptor);
                pollFileDescriptor = -1;
            }
        }

        protected internal override bool GetEnableRaisingPinEvents(int bcmPinNumber)
        {
            ValidatePinNumber(bcmPinNumber);

            bool pinEventsEnabled = _pinsToDetectEvents[bcmPinNumber];
            return pinEventsEnabled;
        }

        private unsafe void DetectEvents()
        {
            char buf;
            IntPtr bufPtr = new IntPtr(&buf);

            while (_pinsToDetectEventsCount > 0)
            {
                bool eventDetected = WasEventDetected(_pollFileDescriptor, out int bcmPinNumber, timeout: -1);

                if (eventDetected)
                {
                    //Console.WriteLine($"Event detected for pin {bcmPinNumber}");
                    OnPinValueChanged(bcmPinNumber);
                }
            }

            _eventDetectionThread = null;
        }

        protected internal override bool WaitForPinEvent(int bcmPinNumber, TimeSpan timeout)
        {
            ValidatePinNumber(bcmPinNumber);

            int pollFileDescriptor = -1;
            AddPinToPoll(bcmPinNumber, ref pollFileDescriptor, out bool closePinValueFileDescriptor);

            int timeoutInMilliseconds = Convert.ToInt32(timeout.TotalMilliseconds);
            bool eventDetected = WasEventDetected(pollFileDescriptor, out _, timeoutInMilliseconds);

            RemovePinFromPoll(bcmPinNumber, ref pollFileDescriptor, closePinValueFileDescriptor, closePollFileDescriptor: true);
            return eventDetected;
        }

        private bool WasEventDetected(int pollFileDescriptor, out int bcmPinNumber, int timeout)
        {
            bool result = PollForPin(pollFileDescriptor, out bcmPinNumber, timeout);

            if (result)
            {
                TimeSpan debounce = _debounceTimeouts[bcmPinNumber];
                DateTime last = _lastEvents[bcmPinNumber];
                DateTime now = DateTime.UtcNow;

                if (now.Subtract(last) > debounce)
                {
                    _lastEvents[bcmPinNumber] = now;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        private unsafe bool PollForPin(int pollFileDescriptor, out int bcmPinNumber, int timeout)
        {
            char buf;
            IntPtr bufPtr = new IntPtr(&buf);
            bool result;

            //Console.WriteLine($"Before epoll_wait: poll_fd = {pollFileDescriptor}");

            int n = epoll_wait(pollFileDescriptor, out epoll_event events, 1, timeout);

            //Console.WriteLine($"After epoll_wait: resut = {n}");

            if (n == -1)
            {
                throw new GpioException($"epoll_wait error number: {Marshal.GetLastWin32Error()}");
            }

            if (n > 0)
            {
                bcmPinNumber = events.data.bcmPinNumber;
                int fd = _pinValueFileDescriptors[bcmPinNumber];

                lseek(fd, 0, SeekFlags.SEEK_SET);
                int r = read(fd, bufPtr, 1);

                if (r != 1)
                {
                    throw new GpioException($"read error number: {Marshal.GetLastWin32Error()}");
                }

                result = true;
            }
            else
            {
                bcmPinNumber = -1;
                result = false;
            }

            return result;
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
                case "in":
                    result = PinMode.Input;
                    break;
                case "out":
                    result = PinMode.Output;
                    break;
                default:
                    throw new NotSupportedException($"Not supported GPIO pin mode '{mode}'");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string PinModeToStringMode(PinMode mode)
        {
            string result;

            switch (mode)
            {
                case PinMode.Input:
                    result = "in";
                    break;
                case PinMode.Output:
                    result = "out";
                    break;
                default:
                    throw new NotSupportedException($"Not supported GPIO pin mode '{mode}'");
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
                case "0":
                    result = PinValue.Low;
                    break;
                case "1":
                    result = PinValue.High;
                    break;
                default:
                    throw new ArgumentException($"Invalid GPIO pin value '{value}'");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string PinValueToStringValue(PinValue value)
        {
            string result;

            switch (value)
            {
                case PinValue.Low:
                    result = "0";
                    break;
                case PinValue.High:
                    result = "1";
                    break;
                default:
                    throw new ArgumentException($"Invalid GPIO pin value '{value}'");
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
