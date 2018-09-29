// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Devices.Gpio;
using System.Runtime.InteropServices;

namespace System.Devices.I2c.Unix
{
    public class UnixI2cDevice : I2cDevice
    {
        #region Interop

        private const string LibraryName = "libc";

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int open([MarshalAs(UnmanagedType.LPStr)] string pathname, I2cFileOpenFlags flags);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int close(int fd);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int ioctl(int fd, uint request, IntPtr argp);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int ioctl(int fd, uint request, ulong argp);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int read(int fd, IntPtr buf, int count);

        [DllImport(LibraryName, SetLastError = true)]
        private static extern int write(int fd, IntPtr buf, int count);

        #endregion

        private const string DefaultDevicePath = "/dev/i2c";

        private int _deviceFileDescriptor = -1;
        private I2cFunctionalityFlags _functionalities;

        public UnixI2cDevice(I2cConnectionSettings connectionSettings)
            : base(connectionSettings)
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

            string deviceFileName = $"{DevicePath}-{_connectionSettings.BusId}";
            _deviceFileDescriptor = open(deviceFileName, I2cFileOpenFlags.ReadWrite);

            if (_deviceFileDescriptor < 0)
            {
                throw Utils.CreateIOException($"Cannot open I2C device file '{deviceFileName}'", _deviceFileDescriptor);
            }

            fixed (I2cFunctionalityFlags* functionalitiesPtr = &_functionalities)
            {
                int ret = ioctl(_deviceFileDescriptor, (uint)I2cMessageFlags.Functionality, new IntPtr(functionalitiesPtr));
                if (ret < 0)
                {
                    _functionalities = 0;
                }
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
                Transfer(null, rxPtr, 0, buffer.Length);
            }
        }

        public override unsafe byte Read8()
        {
            Initialize();

            int length = sizeof(byte);
            byte result = 0;
            Transfer(null, &result, 0, length);
            return result;
        }

        public override unsafe ushort Read16()
        {
            Initialize();

            int length = sizeof(ushort);
            ushort result = 0;
            Transfer(null, (byte*)&result, 0, length);

            result = Utils.SwapBytes(result);
            return result;
        }

        public override unsafe uint Read24()
        {
            Initialize();

            const int length = 3;
            uint result = 0;
            Transfer(null, (byte*)&result, 0, length);

            result = result << 8;
            result = Utils.SwapBytes(result);
            return result;
        }

        public override unsafe uint Read32()
        {
            Initialize();

            int length = sizeof(uint);
            uint result = 0;
            Transfer(null, (byte*)&result, 0, length);

            result = Utils.SwapBytes(result);
            return result;
        }

        public override unsafe ulong Read64()
        {
            Initialize();

            int length = sizeof(ulong);
            ulong result = 0;
            Transfer(null, (byte*)&result, 0, length);

            result = Utils.SwapBytes(result);
            return result;
        }

        public override unsafe void Write(params byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            Initialize();

            fixed (byte* txPtr = buffer)
            {
                Transfer(txPtr, null, buffer.Length, 0);
            }
        }

        public override unsafe void Write8(byte value)
        {
            Initialize();

            int length = sizeof(byte);
            Transfer(&value, null, length, 0);
        }

        public override unsafe void Write16(ushort value)
        {
            Initialize();

            int length = sizeof(ushort);
            Transfer((byte*)&value, null, length, 0);
        }

        public override unsafe void Write24(uint value)
        {
            Initialize();

            value = value & 0xFFFFFF;
            const int length = 3;
            Transfer((byte*)&value, null, length, 0);
        }

        public override unsafe void Write32(uint value)
        {
            Initialize();

            int length = sizeof(uint);
            Transfer((byte*)&value, null, length, 0);
        }

        public override unsafe void Write64(ulong value)
        {
            Initialize();

            int length = sizeof(ulong);
            Transfer((byte*)&value, null, length, 0);
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

            fixed (byte* txPtr = writeBuffer, rxPtr = readBuffer)
            {
                Transfer(txPtr, rxPtr, writeBuffer.Length, readBuffer.Length);
            }
        }

        private unsafe void Transfer(byte* writeBuffer, byte* readBuffer, int writeBufferLength, int readBufferLength)
        {
            if (_functionalities.HasFlag(I2cFunctionalityFlags.I2c))
            {
                //Console.WriteLine("Using I2C RdWr interface");

                RdWrInterfaceTransfer(writeBuffer, readBuffer, writeBufferLength, readBufferLength);
            }
            else
            {
                //Console.WriteLine("Using I2C file interface");

                FileInterfaceTransfer(writeBuffer, readBuffer, writeBufferLength, readBufferLength);
            }
        }

        private unsafe void RdWrInterfaceTransfer(byte* writeBufferPtr, byte* readBufferPtr, int writeBufferLength, int readBufferLength)
        {
            int messageCount = 0;

            if (writeBufferPtr != null)
            {
                messageCount++;
            }

            if (readBufferPtr != null)
            {
                messageCount++;
            }

            I2cMessage* messagesPtr = stackalloc I2cMessage[messageCount];
            messageCount = 0;

            if (writeBufferPtr != null)
            {
                messagesPtr[messageCount++] = new I2cMessage()
                {
                    Flags = I2cMessageFlags.Write,
                    Address = (ushort)_connectionSettings.DeviceAddress,
                    Length = (ushort)writeBufferLength,
                    Buffer = writeBufferPtr
                };
            }

            if (readBufferPtr != null)
            {
                messagesPtr[messageCount++] = new I2cMessage()
                {
                    Flags = I2cMessageFlags.Read,
                    Address = (ushort)_connectionSettings.DeviceAddress,
                    Length = (ushort)readBufferLength,
                    Buffer = readBufferPtr
                };
            }

            var tr = new I2cRdWrIoctlData()
            {
                Messages = messagesPtr,
                NumberOfMessages = (uint)messageCount
            };

            int ret = ioctl(_deviceFileDescriptor, (uint)I2cMessageFlags.ReadWrite, new IntPtr(&tr));
            if (ret < 0)
            {
                throw Utils.CreateIOException("Error performing I2C data transfer", ret);
            }
        }

        private unsafe void FileInterfaceTransfer(byte* writeBufferPtr, byte* readBufferPtr, int writeBufferLength, int readBufferLength)
        {
            int ret = ioctl(_deviceFileDescriptor, (uint)I2cMessageFlags.ForceChangeSlaveAddress, _connectionSettings.DeviceAddress);
            if (ret < 0)
            {
                throw Utils.CreateIOException("Error performing I2C data transfer", ret);
            }

            if (writeBufferPtr != null)
            {
                ret = write(_deviceFileDescriptor, new IntPtr(writeBufferPtr), writeBufferLength);

                if (ret < 0)
                {
                    throw Utils.CreateIOException("Error performing I2C data transfer", ret);
                }
            }

            if (readBufferPtr != null)
            {
                ret = read(_deviceFileDescriptor, new IntPtr(readBufferPtr), readBufferLength);

                if (ret < 0)
                {
                    throw Utils.CreateIOException("Error performing I2C data transfer", ret);
                }
            }
        }
    }
}
