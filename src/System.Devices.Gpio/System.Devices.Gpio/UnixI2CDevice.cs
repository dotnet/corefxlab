// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Runtime.InteropServices;

namespace System.Devices.Gpio
{
    public class UnixI2cDevice : I2cDevice
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

        private enum I2cSettings : uint
        {
            /// <summary>Combined R/W transfer (one STOP only)</summary>
            I2C_RDWR = 0x0707,
            /// <summary>Smbus transfer</summary>
            I2C_SMBUS = 0x0720,
            /// <summary>Get the adapter functionality mask</summary>
            I2C_FUNCS = 0x0705,
            /// <summary>Use this slave address, even if it is already in use by a driver</summary>
            I2C_SLAVE_FORCE = 0x0706
        }

        /// To determine what functionality is supported
        [Flags]
        private enum I2cFunctionalityFlags : ulong
        {
            I2C_FUNC_I2C = 0x00000001,
            I2C_FUNC_SMBUS_BLOCK_DATA = 0x03000000
        }

        [Flags]
        private enum I2cMessageFlags : ushort
        {
            /// <summary>Write data to slave</summary>
            I2C_M_WR = 0x0000,
            /// <summary>Read data from slave</summary>
            I2C_M_RD = 0x0001
        }

        private unsafe struct i2c_msg
        {
            public ushort addr;
            public I2cMessageFlags flags;
            public ushort len;
            public byte* buf;
        };

        /// <summary>Used in the <see cref="I2C_RDWR"/> <see cref="ioctl"/> call</summary>
        private unsafe struct i2c_rdwr_ioctl_data
        {
            public i2c_msg* msgs;
            public uint nmsgs;
        };

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int ioctl(int fd, uint request, IntPtr argp);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int ioctl(int fd, uint request, ulong argp);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int read(int fd, IntPtr buf, int count);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int write(int fd, IntPtr buf, int count);

        #endregion

        private enum TrasnferKind
        {
            Read,
            Write
        }

        private const string DefaultDevicePath = "/dev/i2c";

        private int _deviceFileDescriptor = -1;
        private I2cFunctionalityFlags _functionalities;

        public UnixI2cDevice(I2cConnectionSettings settings)
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
            if (_deviceFileDescriptor >= 0)
            {
                return;
            }

            string deviceFileName = $"{DevicePath}-{_settings.BusId}";
            _deviceFileDescriptor = open(deviceFileName, FileOpenFlags.O_RDWR);

            if (_deviceFileDescriptor < 0)
            {
                throw new IOException($"Cannot open I2c device file '{deviceFileName}'");
            }
        }

        public override void Read(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            Initialize();

            Transfer(buffer, TrasnferKind.Read);
        }

        public override void Write(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            Initialize();

            Transfer(buffer, TrasnferKind.Write);
        }

        public override unsafe void WriteRead(byte[] writeBuffer, byte[] readBuffer)
        {
            if (writeBuffer == null)
            {
                throw new ArgumentNullException(nameof(writeBuffer));
            }

            if (readBuffer == null)
            {
                throw new ArgumentNullException(nameof(readBuffer));
            }

            Initialize();

            Transfer(writeBuffer, TrasnferKind.Write);
            Transfer(readBuffer, TrasnferKind.Read);
        }

        private unsafe void Transfer(byte[] buffer, TrasnferKind kind)
        {
            if (_functionalities == 0)
            {
                I2cFunctionalityFlags functionalities;

                int ret = ioctl(_deviceFileDescriptor, (uint)I2cSettings.I2C_FUNCS, new IntPtr(&functionalities));
                if (ret < 0)
                {
                    throw new GpioException("Error performing I2c data transfer");
                }

                _functionalities = functionalities;
            }

            if (_functionalities.HasFlag(I2cFunctionalityFlags.I2C_FUNC_I2C))
            {
                //Console.WriteLine("Using I2c RdWr interface");

                I2cMessageFlags flags = TransferKindToI2cMessageFlags(kind);
                RdWrInterfaceTransfer(buffer, flags);
            }
            else
            {
                //Console.WriteLine("Using I2c file interface");

                FileInterfaceTransfer(buffer, kind);
            }
        }

        private static I2cMessageFlags TransferKindToI2cMessageFlags(TrasnferKind kind)
        {
            switch (kind)
            {
                case TrasnferKind.Read: return I2cMessageFlags.I2C_M_RD;
                case TrasnferKind.Write: return I2cMessageFlags.I2C_M_WR;

                default: throw new NotSupportedException();
            }
        }

        private unsafe void RdWrInterfaceTransfer(byte[] buffer, I2cMessageFlags flags)
        {
            fixed (byte* txPtr = buffer)
            {
                var message = new i2c_msg()
                {
                    addr = (ushort)_settings.DeviceAddress,
                    len = (ushort)buffer.Length,
                    buf = txPtr,
                    flags = flags
                };

                var tr = new i2c_rdwr_ioctl_data()
                {
                    msgs = &message,
                    nmsgs = 1 // 1 message
                };

                int ret = ioctl(_deviceFileDescriptor, (uint)I2cSettings.I2C_RDWR, new IntPtr(&tr));
                if (ret < 1)
                {
                    throw new GpioException("Error performing I2c data transfer");
                }
            }
        }

        private unsafe void FileInterfaceTransfer(byte[] buffer, TrasnferKind kind)
        {
            int ret = ioctl(_deviceFileDescriptor, (uint)I2cSettings.I2C_SLAVE_FORCE, (ulong)_settings.DeviceAddress);
            if (ret < 0)
            {
                throw new GpioException("Error performing I2c data transfer");
            }

            fixed (byte* txPtr = buffer)
            {
                switch (kind)
                {
                    case TrasnferKind.Read:
                        ret = read(_deviceFileDescriptor, new IntPtr(txPtr), buffer.Length);
                        break;

                    case TrasnferKind.Write:
                        ret = write(_deviceFileDescriptor, new IntPtr(txPtr), buffer.Length);
                        break;

                    default:
                        throw new NotSupportedException();
                }

                if (ret < 0)
                {
                    throw new GpioException("Error performing I2c data transfer");
                }
            }
        }
    }
}
