﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
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

            ///<summary>GPIO Pin Output Set, 2x32 bits, W</summary>
            [FieldOffset(0x1C)]
            public fixed UInt32 GPSET[2];

            ///<summary>GPIO Pin Output Clear, 2x32 bits, W</summary>
            [FieldOffset(0x28)]
            public fixed UInt32 GPCLR[2];

            ///<summary>GPIO Pin Level, 2x32 bits, R</summary>
            [FieldOffset(0x34)]
            public fixed UInt32 GPLEV[2];

            ///<summary>GPIO Pin Event Detect Status, 2x32 bits, R/W</summary>
            [FieldOffset(0x40)]
            public fixed UInt32 GPEDS[2];

            ///<summary>GPIO Pin Rising Edge Detect Enable, 2x32 bits, R/W</summary>
            [FieldOffset(0x4C)]
            public fixed UInt32 GPREN[2];

            ///<summary>GPIO Pin Falling Edge Detect Enable, 2x32 bits, R/W</summary>
            [FieldOffset(0x58)]
            public fixed UInt32 GPFEN[2];

            ///<summary>GPIO Pin High Detect Enable, 2x32 bits, R/W</summary>
            [FieldOffset(0x64)]
            public fixed UInt32 GPHEN[2];

            ///<summary>GPIO Pin Low Detect Enable, 2x32 bits, R/W</summary>
            [FieldOffset(0x70)]
            public fixed UInt32 GPLEN[2];

            ///<summary>GPIO Pin Async. Rising Edge Detect, 2x32 bits, R/W</summary>
            [FieldOffset(0x7C)]
            public fixed UInt32 GPAREN[2];

            ///<summary>GPIO Pin Async. Falling Edge Detect, 2x32 bits, R/W</summary>
            [FieldOffset(0x88)]
            public fixed UInt32 GPAFEN[2];

            ///<summary>GPIO Pin Pull-up/down Enable, 32 bits, R/W</summary>
            [FieldOffset(0x94)]
            public UInt32 GPPUD;

            ///<summary>GPIO Pin Pull-up/down Enable Clock, 2x32 bits, R/W</summary>
            [FieldOffset(0x98)]
            public fixed UInt32 GPPUDCLK[2];
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

        private RegisterView* _registerViewPointer = null;
        private bool _eventDetectionEnabled;
        private int _pinsToDetectEventsCount;
        private BitArray _pinsToDetectEvents;
        private Thread _eventDetectionThread;
        private TimeSpan[] _debounceTimeouts;
        private DateTime[] _lastEvent;

        public RaspberryPiDriver()
        {
        }

        private void Initialize()
        {
            if (_registerViewPointer != null) return;

            int fileDescriptor = open(GpioMemoryFilePath, FileOpenFlags.O_RDWR | FileOpenFlags.O_SYNC);

            if (fileDescriptor < 0)
            {
                throw new GpioException($"open error number: {Marshal.GetLastWin32Error()}");
            }

            //Console.WriteLine($"file descriptor = {fileDescriptor}");

            IntPtr mapPointer = mmap(IntPtr.Zero, Environment.SystemPageSize, MemoryMappedProtections.PROT_READ | MemoryMappedProtections.PROT_WRITE, MemoryMappedFlags.MAP_SHARED, fileDescriptor, GpioBaseOffset);

            if (mapPointer.ToInt32() < 0)
            {
                throw new GpioException($"mmap error number: {Marshal.GetLastWin32Error()}");
            }

            //Console.WriteLine($"mmap returned address = {mapPointer.ToInt32():X16}");

            close(fileDescriptor);

            _registerViewPointer = (RegisterView*)mapPointer;

            _pinsToDetectEvents = new BitArray(PinCount);
            _debounceTimeouts = new TimeSpan[PinCount];
            _lastEvent = new DateTime[PinCount];
        }

        public override void Dispose()
        {
            if (_registerViewPointer != null)
            {
                munmap((IntPtr)_registerViewPointer, 0);
                _registerViewPointer = null;
            }
        }

        protected internal override int PinCount => 54;

        protected internal override void SetPinMode(int bcmPinNumber, PinMode mode)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            Initialize();

            switch (mode)
            {
                case PinMode.Input:
                case PinMode.InputPullDown:
                case PinMode.InputPullUp:
                    SetInputPullMode(bcmPinNumber, PinModeToGPPUD(mode));
                    break;

                case PinMode.Output:
                    // Do nothing
                    break;

                default:
                    throw new NotSupportedException($"Not supported GPIO pin mode '{mode}'");
            }

            SetPinMode(bcmPinNumber, PinModeToGPFSEL(mode));
        }

        private void SetInputPullMode(int bcmPinNumber, uint mode)
        {
            /*
             * The GPIO Pull - up/down Clock Registers control the actuation of internal pull-downs on the respective GPIO pins.
             * These registers must be used in conjunction with the GPPUD register to effect GPIO Pull-up/down changes.
             * The following sequence of events is required: 
             * 
             * 1. Write to GPPUD to set the required control signal (i.e.Pull-up or Pull-Down or neither to remove the current Pull-up/down)
             * 2. Wait 150 cycles – this provides the required set-up time for the control signal 
             * 3. Write to GPPUDCLK0/1 to clock the control signal into the GPIO pads you wish to modify
             *    – NOTE only the pads which receive a clock will be modified, all others will retain their previous state.
             * 4. Wait 150 cycles – this provides the required hold time for the control signal 
             * 5. Write to GPPUD to remove the control signal 
             * 6. Write to GPPUDCLK0/1 to remove the clock
             */

            //SetBits(RegisterViewPointer->GPPUD, pin, 1, 2, mode);
            //Thread.SpinWait(150);
            //SetBit(RegisterViewPointer->GPPUDCLK, pin);
            //Thread.SpinWait(150);
            //SetBit(RegisterViewPointer->GPPUDCLK, pin, 0U);

            uint* gppudPointer = &_registerViewPointer->GPPUD;

            //Console.WriteLine($"{nameof(RegisterView.GPPUD)} register address = {(long)gppudPointer:X16}");

            uint register = *gppudPointer;

            //Console.WriteLine($"{nameof(RegisterView.GPPUD)} original register value = {register:X8}");

            register &= ~0b11U;
            register |= mode;

            //Console.WriteLine($"{nameof(RegisterView.GPPUD)} new register value = {register:X8}");

            *gppudPointer = register;

            // Wait 150 cycles – this provides the required set-up time for the control signal
            Thread.SpinWait(150);

            int index = bcmPinNumber / 32;
            int shift = bcmPinNumber % 32;
            uint* gppudclkPointer = &_registerViewPointer->GPPUDCLK[index];

            //Console.WriteLine($"{nameof(RegisterView.GPPUDCLK)} register address = {(long)gppudclkPointer:X16}");

            register = *gppudclkPointer;

            //Console.WriteLine($"{nameof(RegisterView.GPPUDCLK)} original register value = {register:X8}");

            register |= 1U << shift;

            //Console.WriteLine($"{nameof(RegisterView.GPPUDCLK)} new register value = {register:X8}");

            *gppudclkPointer = register;

            // Wait 150 cycles – this provides the required hold time for the control signal 
            Thread.SpinWait(150);

            register = *gppudPointer;
            register &= ~0b11U;

            //Console.WriteLine($"{nameof(RegisterView.GPPUD)} new register value = {register:X8}");
            //Console.WriteLine($"{nameof(RegisterView.GPPUDCLK)} new register value = {0:X8}");

            *gppudPointer = register;
            *gppudclkPointer = 0;
        }

        private void SetPinMode(int bcmPinNumber, uint mode)
        {
            //SetBits(RegisterViewPointer->GPFSEL, pin, 10, 3, mode);

            int index = bcmPinNumber / 10;
            int shift = (bcmPinNumber % 10) * 3;
            uint* registerPointer = &_registerViewPointer->GPFSEL[index];

            //Console.WriteLine($"{nameof(RegisterView.GPFSEL)} register address = {(long)registerPointer:X16}");

            uint register = *registerPointer;

            //Console.WriteLine($"{nameof(RegisterView.GPFSEL)} original register value = {register:X8}");

            register &= ~(0b111U << shift);
            register |= mode << shift;

            //Console.WriteLine($"{nameof(RegisterView.GPFSEL)} new register value = {register:X8}");

            *registerPointer = register;
        }

        protected internal override PinMode GetPinMode(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            Initialize();

            //var mode = GetBits(RegisterViewPointer->GPFSEL, pin, 10, 3);

            int index = bcmPinNumber / 10;
            int shift = (bcmPinNumber % 10) * 3;
            uint register = _registerViewPointer->GPFSEL[index];
            uint mode = (register >> shift) & 0b111U;

            PinMode result = GPFSELToPinMode(mode);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint PinModeToGPPUD(PinMode mode)
        {
            uint result;

            switch (mode)
            {
                case PinMode.Input:
                    result = 0;
                    break;
                case PinMode.InputPullDown:
                    result = 1;
                    break;
                case PinMode.InputPullUp:
                    result = 2;
                    break;

                default:
                    throw new NotSupportedException($"Not supported GPIO pin mode '{mode}'");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint PinModeToGPFSEL(PinMode mode)
        {
            uint result;

            switch (mode)
            {
                case PinMode.Input:
                case PinMode.InputPullDown:
                case PinMode.InputPullUp:
                    result = 0;
                    break;

                case PinMode.Output:
                    result = 1;
                    break;

                default:
                    throw new NotSupportedException($"Not supported GPIO pin mode '{mode}'");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private PinMode GPFSELToPinMode(uint gpfselValue)
        {
            PinMode result;

            switch (gpfselValue)
            {
                case 0:
                    result = PinMode.Input;
                    break;
                case 1:
                    result = PinMode.Output;
                    break;

                default:
                    throw new NotSupportedException($"Not supported GPIO pin mode '{gpfselValue}'");
            }

            return result;
        }

        protected internal override void Output(int bcmPinNumber, PinValue value)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            Initialize();

            //switch (value)
            //{
            //    case GpioPinValue.High:
            //        SetBit(RegisterViewPointer->GPSET, pin);
            //        break;

            //    case GpioPinValue.Low:
            //        SetBit(RegisterViewPointer->GPCLR, pin);
            //        break;

            //    default: throw new InvalidGpioPinValueException(value);
            //}

            int index = bcmPinNumber / 32;
            int shift = bcmPinNumber % 32;
            uint* registerPointer = null;
            string registerName = string.Empty;

            switch (value)
            {
                case PinValue.High:
                    registerPointer = &_registerViewPointer->GPSET[index];
                    registerName = nameof(RegisterView.GPSET);
                    break;

                case PinValue.Low:
                    registerPointer = &_registerViewPointer->GPCLR[index];
                    registerName = nameof(RegisterView.GPCLR);
                    break;

                default:
                    throw new ArgumentException($"Invalid GPIO pin value '{value}'");
            }

            //Console.WriteLine($"{registerName} register address = {(long)registerPointer:X16}");

            uint register = *registerPointer;

            //Console.WriteLine($"{registerName} original register value = {register:X8}");

            register |= 1U << shift;

            //Console.WriteLine($"{registerName} new register value = {register:X8}");

            *registerPointer = register;
        }

        protected internal override PinValue Input(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            Initialize();

            //var value = GetBit(RegisterViewPointer->GPLEV, pin);

            int index = bcmPinNumber / 32;
            int shift = bcmPinNumber % 32;
            uint register = _registerViewPointer->GPLEV[index];
            uint value = (register >> shift) & 1;

            PinValue result = Convert.ToBoolean(value) ? PinValue.High : PinValue.Low;
            return result;
        }

        protected internal override void SetDebounce(int bcmPinNumber, TimeSpan timeout)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            _debounceTimeouts[bcmPinNumber] = timeout;
        }

        protected internal override TimeSpan GetDebounce(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            TimeSpan timeout = _debounceTimeouts[bcmPinNumber];
            return timeout;
        }

        protected internal override void SetPinEventsToDetect(int bcmPinNumber, PinEvent events)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            Initialize();

            PinEvent kind = PinEvent.Low;
            bool enabled = events.HasFlag(kind);
            SetEventDetection(bcmPinNumber, kind, enabled);

            kind = PinEvent.High;
            enabled = events.HasFlag(kind);
            SetEventDetection(bcmPinNumber, kind, enabled);

            kind = PinEvent.SyncRisingEdge;
            enabled = events.HasFlag(kind);
            SetEventDetection(bcmPinNumber, kind, enabled);

            kind = PinEvent.SyncFallingEdge;
            enabled = events.HasFlag(kind);
            SetEventDetection(bcmPinNumber, kind, enabled);

            kind = PinEvent.AsyncRisingEdge;
            enabled = events.HasFlag(kind);
            SetEventDetection(bcmPinNumber, kind, enabled);

            kind = PinEvent.AsyncFallingEdge;
            enabled = events.HasFlag(kind);
            SetEventDetection(bcmPinNumber, kind, enabled);

            ClearDetectedEvent(bcmPinNumber);

            bool pinEventsEnabled = _pinsToDetectEvents[bcmPinNumber];

            if (events == PinEvent.None)
            {
                if (pinEventsEnabled)
                {
                    _pinsToDetectEvents.Set(bcmPinNumber, false);
                    _pinsToDetectEventsCount--;
                }
            }
            else
            {
                if (!pinEventsEnabled)
                {
                    _pinsToDetectEvents.Set(bcmPinNumber, true);
                    _pinsToDetectEventsCount++;
                }
            }
        }

        private void SetEventDetection(int bcmPinNumber, PinEvent kind, bool enabled)
        {
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

            //    default: throw new InvalidGpioEventKindException(kind);
            //}

            int index = bcmPinNumber / 32;
            int shift = bcmPinNumber % 32;
            uint* registerPointer = null;
            string registerName = string.Empty;

            switch (kind)
            {
                case PinEvent.High:
                    registerPointer = &_registerViewPointer->GPHEN[index];
                    registerName = nameof(RegisterView.GPHEN);
                    break;

                case PinEvent.Low:
                    registerPointer = &_registerViewPointer->GPLEN[index];
                    registerName = nameof(RegisterView.GPLEN);
                    break;

                case PinEvent.SyncRisingEdge:
                    registerPointer = &_registerViewPointer->GPREN[index];
                    registerName = nameof(RegisterView.GPREN);
                    break;

                case PinEvent.SyncFallingEdge:
                    registerPointer = &_registerViewPointer->GPFEN[index];
                    registerName = nameof(RegisterView.GPFEN);
                    break;

                case PinEvent.AsyncRisingEdge:
                    registerPointer = &_registerViewPointer->GPAREN[index];
                    registerName = nameof(RegisterView.GPAREN);
                    break;

                case PinEvent.AsyncFallingEdge:
                    registerPointer = &_registerViewPointer->GPAFEN[index];
                    registerName = nameof(RegisterView.GPAFEN);
                    break;

                default:
                    throw new ArgumentException($"Invalid GPIO event kind '{kind}'");
            }

            //Console.WriteLine($"{registerName} register address = {(long)registerPointer:X16}");

            uint register = *registerPointer;

            //Console.WriteLine($"{registerName} original register value = {register:X8}");

            if (enabled)
            {
                register |= 1U << shift;
            }
            else
            {
                register &= ~(1U << shift);
            }

            //Console.WriteLine($"{registerName} new register value = {register:X8}");

            *registerPointer = register;
        }

        protected internal override PinEvent GetPinEventsToDetect(int bcmPinNumber)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            Initialize();

            PinEvent result = PinEvent.None;
            PinEvent kind = PinEvent.Low;
            bool enabled = GetEventDetection(bcmPinNumber, kind);
            if (enabled) result |= kind;

            kind = PinEvent.High;
            enabled = GetEventDetection(bcmPinNumber, kind);
            if (enabled) result |= kind;

            kind = PinEvent.SyncRisingEdge;
            enabled = GetEventDetection(bcmPinNumber, kind);
            if (enabled) result |= kind;

            kind = PinEvent.SyncFallingEdge;
            enabled = GetEventDetection(bcmPinNumber, kind);
            if (enabled) result |= kind;

            kind = PinEvent.AsyncRisingEdge;
            enabled = GetEventDetection(bcmPinNumber, kind);
            if (enabled) result |= kind;

            kind = PinEvent.AsyncFallingEdge;
            enabled = GetEventDetection(bcmPinNumber, kind);
            if (enabled) result |= kind;

            return result;
        }

        private bool GetEventDetection(int bcmPinNumber, PinEvent kind)
        {
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

            //    default: throw new InvalidGpioEventKindException(kind);
            //}

            int index = bcmPinNumber / 32;
            int shift = bcmPinNumber % 32;
            string registerName = string.Empty;
            uint register = 0U;

            switch (kind)
            {
                case PinEvent.High:
                    register = _registerViewPointer->GPHEN[index];
                    break;

                case PinEvent.Low:
                    register = _registerViewPointer->GPLEN[index];
                    break;

                case PinEvent.SyncRisingEdge:
                    register = _registerViewPointer->GPREN[index];
                    break;

                case PinEvent.SyncFallingEdge:
                    register = _registerViewPointer->GPFEN[index];
                    break;

                case PinEvent.AsyncRisingEdge:
                    register = _registerViewPointer->GPAREN[index];
                    break;

                case PinEvent.AsyncFallingEdge:
                    register = _registerViewPointer->GPAFEN[index];
                    break;

                default:
                    throw new ArgumentException($"Invalid GPIO event kind '{kind}'");
            }

            uint value = (register >> shift) & 1;

            bool result = Convert.ToBoolean(value);
            return result;
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

                if (_eventDetectionThread == null)
                {
                    _eventDetectionThread = new Thread(DetectEvents)
                    {
                        IsBackground = true
                    };

                    _eventDetectionThread.Start();
                }
            }
            else if (_eventDetectionEnabled && !value)
            {
                // Disable events detection
                _eventDetectionEnabled = false;
            }
        }

        private void DetectEvents()
        {
            while (_eventDetectionEnabled && _pinsToDetectEventsCount > 0)
            {
                for (int i = 0; i < _pinsToDetectEvents.Length; ++i)
                {
                    bool pinEventsEnabled = _pinsToDetectEvents[i];

                    if (pinEventsEnabled)
                    {
                        bool eventDetected = WasEventDetected(i);

                        if (eventDetected)
                        {
                            //Console.WriteLine($"Event detected for pin {i}");
                            OnPinValueChanged(i);
                        }
                    }
                }
            }

            _eventDetectionThread = null;
        }

        protected internal override bool WaitForPinEvent(int bcmPinNumber, TimeSpan timeout)
        {
            if (bcmPinNumber < 0 || bcmPinNumber >= PinCount)
            {
                throw new ArgumentOutOfRangeException(nameof(bcmPinNumber));
            }

            Initialize();

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
            //var value = GetBit(RegisterViewPointer->GPEDS, pin);

            int index = bcmPinNumber / 32;
            int shift = bcmPinNumber % 32;
            uint register = _registerViewPointer->GPEDS[index];
            uint value = (register >> shift) & 1;

            bool result = Convert.ToBoolean(value);

            if (result)
            {
                ClearDetectedEvent(bcmPinNumber);

                TimeSpan timeout = _debounceTimeouts[bcmPinNumber];
                DateTime last = _lastEvent[bcmPinNumber];
                DateTime now = DateTime.UtcNow;

                if (now.Subtract(last) > timeout)
                {
                    _lastEvent[bcmPinNumber] = now;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        private void ClearDetectedEvent(int bcmPinNumber)
        {
            //SetBit(RegisterViewPointer->GPEDS, pin);

            int index = bcmPinNumber / 32;
            int shift = bcmPinNumber % 32;
            uint* registerPointer = &_registerViewPointer->GPEDS[index];

            //Console.WriteLine($"{nameof(RegisterView.GPEDS)} register address = {(long)registerPointer:X16}");

            uint register = *registerPointer;

            //Console.WriteLine($"{nameof(RegisterView.GPEDS)} original register value = {register:X8}");

            register |= 1U << shift;

            //Console.WriteLine($"{nameof(RegisterView.GPEDS)} new register value = {register:X8}");

            *registerPointer = register;

            // Wait 150 cycles
            Thread.SpinWait(150);

            *registerPointer = 0;
        }

        protected internal override int ConvertPinNumber(int pinNumber, PinNumberingScheme from, PinNumberingScheme to)
        {
            int result = -1;

            switch (from)
            {
                case PinNumberingScheme.BCM:
                    switch (to)
                    {
                        case PinNumberingScheme.BCM:
                            result = pinNumber;
                            break;

                        case PinNumberingScheme.Board:
                            //throw new NotImplementedException();
                            break;

                        default:
                            throw new NotSupportedException($"Unsupported GPIO pin numbering scheme {to}");
                    }
                    break;

                case PinNumberingScheme.Board:
                    switch (to)
                    {
                        case PinNumberingScheme.Board:
                            result = pinNumber;
                            break;

                        case PinNumberingScheme.BCM:
                            //throw new NotImplementedException();
                            break;

                        default:
                            throw new NotSupportedException($"Unsupported GPIO pin numbering scheme {to}");
                    }
                    break;

                default:
                    throw new NotSupportedException($"Unsupported GPIO pin numbering scheme {from}");
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
