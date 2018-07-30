// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Runtime.InteropServices;

namespace System.Devices.Gpio
{
    public class UnixSpiDevice : SpiDevice
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

        private enum SpiSettings : uint
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
        private enum UnixSpiMode : byte
        {
            None = 0x00,
            SPI_CPHA = 0x01,
            SPI_CPOL = 0x02,
            SPI_LSB_FIRST = 0x08,
            SPI_CS_HIGH = 0x04,
            SPI_3WIRE = 0x10,
            SPI_LOOP = 0x20,
            SPI_NO_CS = 0x40,
            SPI_READY = 0x80,
            SPI_MODE_0 = None,
            SPI_MODE_1 = SPI_CPHA,
            SPI_MODE_2 = SPI_CPOL,
            SPI_MODE_3 = SPI_CPOL | SPI_CPHA
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

        private const uint SPI_IOC_MESSAGE_1 = 0x40206b00;

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int ioctl(int fd, uint request, IntPtr argp);

        #endregion

        private const string DefaultDevicePath = "/dev/spidev";

        private int _deviceFileDescriptor = -1;

        public UnixSpiDevice(SpiConnectionSettings settings)
            : base(settings)
        {
            DevicePath = DefaultDevicePath;
        }

        public override void Dispose()
        {
            if (_deviceFileDescriptor >= 0)
            {
                close(_deviceFileDescriptor);
                _deviceFileDescriptor = -1;
            }
        }

        public string DevicePath { get; set; }

        private unsafe void Initialize()
        {
            if (_deviceFileDescriptor >= 0) return;

            string deviceFileName = $"{DevicePath}{_settings.BusId}.{_settings.ChipSelectLine}";
            _deviceFileDescriptor = open(deviceFileName, FileOpenFlags.O_RDWR);

            if (_deviceFileDescriptor < 0)
            {
                throw new IOException($"Cannot open Spi device file '{deviceFileName}'");
            }

            UnixSpiMode mode = SpiModeToUnixSpiMode(_settings.Mode);
            var ptr = new IntPtr(&mode);

            int ret = ioctl(_deviceFileDescriptor, (uint)SpiSettings.SPI_IOC_WR_MODE, ptr);
            if (ret == -1)
            {
                throw new GpioException($"Cannot set Spi mode to '{_settings.Mode}'");
            }

            byte bits = (byte)_settings.DataBitLength;
            ptr = new IntPtr(&bits);

            ret = ioctl(_deviceFileDescriptor, (uint)SpiSettings.SPI_IOC_WR_BITS_PER_WORD, ptr);
            if (ret == -1)
            {
                throw new GpioException($"Cannot set Spi data bit length to '{_settings.DataBitLength}'");
            }

            uint speed = (uint)_settings.ClockFrequency;
            ptr = new IntPtr(&speed);

            ret = ioctl(_deviceFileDescriptor, (uint)SpiSettings.SPI_IOC_WR_MAX_SPEED_HZ, ptr);
            if (ret == -1)
            {
                throw new GpioException($"Cannot set Spi clock frequency to '{_settings.ClockFrequency}'");
            }
        }

        public override unsafe void Read(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            Initialize();

            fixed (byte* rxPtr = buffer)
            {
                var tr = new spi_ioc_transfer()
                {
                    tx_buf = 0,
                    rx_buf = (ulong)rxPtr,
                    len = (uint)buffer.Length,
                    speed_hz = (uint)_settings.ClockFrequency,
                    bits_per_word = (byte)_settings.DataBitLength,
                    delay_usecs = 0,
                };

                int ret = ioctl(_deviceFileDescriptor, SPI_IOC_MESSAGE_1, new IntPtr(&tr));
                if (ret < 1)
                {
                    throw new GpioException("Error performing Spi data transfer");
                }
            }
        }

        public override unsafe void Write(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            Initialize();

            fixed (byte* txPtr = buffer)
            {
                var tr = new spi_ioc_transfer()
                {
                    tx_buf = (ulong)txPtr,
                    rx_buf = 0,
                    len = (uint)buffer.Length,
                    speed_hz = (uint)_settings.ClockFrequency,
                    bits_per_word = (byte)_settings.DataBitLength,
                    delay_usecs = 0,
                };

                int ret = ioctl(_deviceFileDescriptor, SPI_IOC_MESSAGE_1, new IntPtr(&tr));
                if (ret < 1)
                {
                    throw new GpioException("Error performing Spi data transfer");
                }
            }
        }

        public override unsafe void TransferFullDuplex(byte[] writeBuffer, byte[] readBuffer)
        {
            if (writeBuffer == null)
            {
                throw new ArgumentNullException(nameof(writeBuffer));
            }

            if (readBuffer == null)
            {
                throw new ArgumentNullException(nameof(readBuffer));
            }

            if (writeBuffer.Length != readBuffer.Length)
            {
                throw new ArgumentException($"Parameters '{nameof(writeBuffer)}' and '{nameof(readBuffer)}' must have the same length");
            }

            Initialize();

            fixed (byte* txPtr = writeBuffer, rxPtr = readBuffer)
            {
                var tr = new spi_ioc_transfer()
                {
                    tx_buf = (ulong)txPtr,
                    rx_buf = (ulong)rxPtr,
                    len = (uint)writeBuffer.Length,
                    speed_hz = (uint)_settings.ClockFrequency,
                    bits_per_word = (byte)_settings.DataBitLength,
                    delay_usecs = 0,
                };

                int ret = ioctl(_deviceFileDescriptor, SPI_IOC_MESSAGE_1, new IntPtr(&tr));
                if (ret < 1)
                {
                    throw new GpioException("Error performing Spi data transfer");
                }
            }
        }

        private UnixSpiMode SpiModeToUnixSpiMode(SpiMode mode)
        {
            UnixSpiMode result;

            switch (mode)
            {
                case SpiMode.Mode0:
                    result = UnixSpiMode.SPI_MODE_0;
                    break;

                case SpiMode.Mode1:
                    result = UnixSpiMode.SPI_MODE_1;
                    break;

                case SpiMode.Mode2:
                    result = UnixSpiMode.SPI_MODE_2;
                    break;

                case SpiMode.Mode3:
                    result = UnixSpiMode.SPI_MODE_3;
                    break;

                default:
                    throw new GpioException($"Invalid Spi mode '{mode}'");
            }

            return result;
        }
    }
}
