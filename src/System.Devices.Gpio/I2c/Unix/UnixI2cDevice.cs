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

        [DllImport(LibraryName, EntryPoint = "close", SetLastError = true)]
        private static extern int Close(int fileDescriptor);

        [DllImport(LibraryName, EntryPoint = "ioctl", SetLastError = true)]
        private static extern int IoCtl(int fileDescriptor, uint request, IntPtr arguments);

        [DllImport(LibraryName, EntryPoint = "ioctl", SetLastError = true)]
        private static extern int IoCtl(int fileDescriptor, uint request, ulong arguments);

        [DllImport(LibraryName, EntryPoint = "open", SetLastError = true)]
        private static extern int Open([MarshalAs(UnmanagedType.LPStr)] string pathname, I2cFileOpenFlags flags);

        [DllImport(LibraryName, EntryPoint = "read", SetLastError = true)]
        private static extern int Read(int fileDescriptor, IntPtr buffer, int numberOfBytes);

        [DllImport(LibraryName, EntryPoint = "write", SetLastError = true)]
        private static extern int Write(int fileDescriptor, IntPtr buffer, int numberOfBytes);

        #endregion

        private const string DefaultDevicePath = "/dev/i2c";
        private int _deviceFileDescriptor = -1;
        private I2cFunctionalityFlags _functionalities;

        public UnixI2cDevice(I2cConnectionSettings connectionSettings)
            : base(connectionSettings)
        {
            DevicePath = DefaultDevicePath;
        }

        public string DevicePath { get; set; }

        private unsafe void Initialize()
        {
            if (_deviceFileDescriptor >= 0)
            {
                return;
            }

            string deviceFileName = $"{DevicePath}-{_connectionSettings.BusId}";
            _deviceFileDescriptor = Open(deviceFileName, I2cFileOpenFlags.ReadWrite);

            if (_deviceFileDescriptor < 0)
            {
                throw Utils.CreateIOException($"Cannot open I2C device file '{deviceFileName}'", _deviceFileDescriptor);
            }

            fixed (I2cFunctionalityFlags* functionalitiesPtr = &_functionalities)
            {
                int result = IoCtl(_deviceFileDescriptor, (uint)I2cMessageFlags.Functionality, new IntPtr(functionalitiesPtr));

                if (result < 0)
                {
                    _functionalities = 0;
                }
            }
        }

        private unsafe void Transfer(byte* writeBuffer, int writeBufferLength, byte* readBuffer, int readBufferLength)
        {
            if (_functionalities.HasFlag(I2cFunctionalityFlags.I2c))
            {
                RdWrInterfaceTransfer(writeBuffer, writeBufferLength, readBuffer, readBufferLength);
            }
            else
            {
                FileInterfaceTransfer(writeBuffer, writeBufferLength, readBuffer, readBufferLength);
            }
        }

        private unsafe void RdWrInterfaceTransfer(byte* writeBuffer, int writeBufferLength, byte* readBuffer, int readBufferLength)
        {
            int messageCount = 0;

            if (writeBuffer != null)
            {
                messageCount++;
            }

            if (readBuffer != null)
            {
                messageCount++;
            }

            I2cMessage* messagesPtr = stackalloc I2cMessage[messageCount];
            messageCount = 0;

            if (writeBuffer != null)
            {
                messagesPtr[messageCount++] = new I2cMessage()
                {
                    Flags = I2cMessageFlags.Write,
                    Address = (ushort)_connectionSettings.DeviceAddress,
                    Length = (ushort)writeBufferLength,
                    Buffer = writeBuffer
                };
            }

            if (readBuffer != null)
            {
                messagesPtr[messageCount++] = new I2cMessage()
                {
                    Flags = I2cMessageFlags.Read,
                    Address = (ushort)_connectionSettings.DeviceAddress,
                    Length = (ushort)readBufferLength,
                    Buffer = readBuffer
                };
            }

            I2cRdWrIoctlData i2cRdWrIoctlData = new I2cRdWrIoctlData()
            {
                Messages = messagesPtr,
                NumberOfMessages = (uint)messageCount
            };

            int result = IoCtl(_deviceFileDescriptor, (uint)I2cMessageFlags.ReadWrite, new IntPtr(&i2cRdWrIoctlData));

            if (result < 0)
            {
                throw Utils.CreateIOException("Error performing I2C data transfer", result);
            }
        }

        private unsafe void FileInterfaceTransfer(byte* writeBuffer, int writeBufferLength, byte* readBuffer, int readBufferLength)
        {
            int result = IoCtl(_deviceFileDescriptor, (uint)I2cMessageFlags.ForceChangeSlaveAddress, _connectionSettings.DeviceAddress);

            if (result < 0)
            {
                throw Utils.CreateIOException("Error performing I2C data transfer", result);
            }

            if (writeBuffer != null)
            {
                result = Write(_deviceFileDescriptor, new IntPtr(writeBuffer), writeBufferLength);

                if (result < 0)
                {
                    throw Utils.CreateIOException("Error performing I2C data transfer", result);
                }
            }

            if (readBuffer != null)
            {
                result = Read(_deviceFileDescriptor, new IntPtr(readBuffer), readBufferLength);

                if (result < 0)
                {
                    throw Utils.CreateIOException("Error performing I2C data transfer", result);
                }
            }
        }

        public override void Dispose()
        {
            if (_deviceFileDescriptor >= 0)
            {
                Close(_deviceFileDescriptor);
                _deviceFileDescriptor = -1;
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
                Transfer(null, 0, rxPtr, buffer.Length);
            }
        }

        public override unsafe byte Read8()
        {
            Initialize();

            int length = sizeof(byte);
            byte result = 0;
            Transfer(null, 0, &result, length);
            return result;
        }

        public override unsafe ushort Read16()
        {
            Initialize();

            int length = sizeof(ushort);
            ushort result = 0;
            Transfer(null, 0, (byte*)&result, length);

            result = Utils.SwapBytes(result);
            return result;
        }

        public override unsafe uint Read24()
        {
            Initialize();

            const int length = 3;
            uint result = 0;
            Transfer(null, 0, (byte*)&result, length);

            result = result << 8;
            result = Utils.SwapBytes(result);
            return result;
        }

        public override unsafe uint Read32()
        {
            Initialize();

            int length = sizeof(uint);
            uint result = 0;
            Transfer(null, 0, (byte*)&result, length);

            result = Utils.SwapBytes(result);
            return result;
        }

        public override unsafe ulong Read64()
        {
            Initialize();

            int length = sizeof(ulong);
            ulong result = 0;
            Transfer(null, 0, (byte*)&result, length);

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
                Transfer(txPtr, buffer.Length, null, 0);
            }
        }

        public override unsafe void Write8(byte value)
        {
            Initialize();

            int length = sizeof(byte);
            Transfer(&value, length, null, 0);
        }

        public override unsafe void Write16(ushort value)
        {
            Initialize();

            int length = sizeof(ushort);
            Transfer((byte*)&value, length, null, 0);
        }

        public override unsafe void Write24(uint value)
        {
            Initialize();

            value = value & 0xFFFFFF;
            const int length = 3;
            Transfer((byte*)&value, length, null, 0);
        }

        public override unsafe void Write32(uint value)
        {
            Initialize();

            int length = sizeof(uint);
            Transfer((byte*)&value, length, null, 0);
        }

        public override unsafe void Write64(ulong value)
        {
            Initialize();

            int length = sizeof(ulong);
            Transfer((byte*)&value, length, null, 0);
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

            fixed (byte* writeBufferPtr = writeBuffer, readBufferPtr = readBuffer)
            {
                Transfer(writeBufferPtr, writeBuffer.Length, readBufferPtr, readBuffer.Length);
            }
        }
    }
}
