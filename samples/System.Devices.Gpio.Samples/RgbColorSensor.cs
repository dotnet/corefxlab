// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Devices.I2c;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Devices.Gpio.Samples
{
    public enum RgbColorSensorIntegrationTime : byte
    {
        Milliseconds2_4 = 0xFF,  // 2.4ms -  1 cycle   - Max Count: 1024
        Milliseconds24 = 0xF6,   //  24ms - 10 cycles  - Max Count: 10240
        Milliseconds50 = 0xEB,   //  50ms - 20 cycles  - Max Count: 20480
        Milliseconds101 = 0xD5,  // 101ms - 42 cycles  - Max Count: 43008
        Milliseconds154 = 0xC0,  // 154ms - 64 cycles  - Max Count: 65535
        Milliseconds700 = 0x00   // 700ms - 256 cycles - Max Count: 65535
    }

    public enum RgbColorSensorGain : byte
    {
        Gain1X = 0x00,   //  No gain
        Gain4X = 0x01,   //  2x gain
        Gain16X = 0x02,  //  16x gain
        Gain60X = 0x03   //  60x gain
    }

    /// <summary>
    /// Supports TCS34725 RGB Color sensor
    /// </summary>
    public class RgbColorSensor : IDisposable
    {
        public const byte DefaultI2cAddress = 0x29;
        public const byte AlternativeI2cAddress = 0x39;

        private const byte COMMAND_BIT = 0x80;
        private const byte REGISTER_ENABLE = 0x00;
        private const byte REGISTER_ATIME = 0x01;
        private const byte REGISTER_AILT = 0x04;
        private const byte REGISTER_AIHT = 0x06;
        private const byte REGISTER_ID = 0x12;
        private const byte REGISTER_APERS = 0x0c;
        private const byte REGISTER_CONTROL = 0x0f;
        private const byte REGISTER_SENSORID = 0x12;
        private const byte REGISTER_STATUS = 0x13;
        private const byte REGISTER_CDATA = 0x14;
        private const byte REGISTER_RDATA = 0x16;
        private const byte REGISTER_GDATA = 0x18;
        private const byte REGISTER_BDATA = 0x1a;
        private const byte ENABLE_AIEN = 0x10;
        private const byte ENABLE_WEN = 0x08;
        private const byte ENABLE_AEN = 0x02;
        private const byte ENABLE_PON = 0x01;

        private static readonly byte[] s_cycles = { 0, 1, 2, 3, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60 };

        private struct RawColor
        {
            public ushort Red, Green, Blue, Clear;
        }

        private readonly I2cConnectionSettings _i2cSettings;
        private I2cDevice _i2cDevice;

        private RgbColorSensorIntegrationTime _integrationTime;
        private RawColor _rawColor;

        public RgbColorSensor(I2cConnectionSettings i2cSettings)
        {
            _i2cSettings = i2cSettings ?? throw new ArgumentNullException(nameof(i2cSettings));
        }

        public void Dispose()
        {
            if (_i2cDevice != null)
            {
                _i2cDevice.Dispose();
                _i2cDevice = null;
            }
        }

        /// <summary>
        /// Gets the detected color.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Gets the detected color temperature in degrees.
        /// </summary>
        public float Temperature { get; private set; }

        /// <summary>
        /// Gets the detected light level in lux.
        /// </summary>
        public float Luminosity { get; private set; }

        /// <summary>
        /// Gets the active state of the sensor.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Sets the active state of the sensor.
        /// </summary>
        public void SetActive(bool value)
        {
            if (IsActive == value)
            {
                return;
            }

            IsActive = value;
            byte enable = Read8(REGISTER_ENABLE);

            if (value)
            {
                Write8(REGISTER_ENABLE, (byte)(enable | ENABLE_PON));
                Thread.Sleep(3);
                Write8(REGISTER_ENABLE, (byte)(enable | ENABLE_PON | ENABLE_AEN));
            }
            else
            {
                Write8(REGISTER_ENABLE, (byte)(enable & ~(ENABLE_PON | ENABLE_AEN)));
            }
        }

        /// <summary>
        /// Gets the integration time of the sensor.
        /// </summary>
        public RgbColorSensorIntegrationTime GetIntegrationTime()
        {
            byte control = Read8(REGISTER_ATIME);
            RgbColorSensorIntegrationTime result = (RgbColorSensorIntegrationTime)control;
            return result;
        }

        /// <summary>
        /// Sets the integration time of the sensor.
        /// </summary>
        public void SetIntegrationTime(RgbColorSensorIntegrationTime value)
        {
            _integrationTime = value;
            Write8(REGISTER_ATIME, (byte)value);
        }

        /// <summary>
        /// Gets the gain of the sensor.
        /// </summary>
        public RgbColorSensorGain GetGain()
        {
            byte control = Read8(REGISTER_CONTROL);
            RgbColorSensorGain result = (RgbColorSensorGain)control;
            return result;
        }

        /// <summary>
        /// Sets the gain of the sensor.
        /// </summary>
        public void SetGain(RgbColorSensorGain value)
        {
            Write8(REGISTER_CONTROL, (byte)value);
        }

        /// <summary>
        /// Returns true if the interrupt is set.
        /// </summary>
        public bool IsInterruptEnabled()
        {
            byte result = Read8(REGISTER_STATUS);
            result = (byte)(result & ENABLE_AIEN);
            return result != 0;
        }

        /// <summary>
        /// Sets the interrupt.
        /// </summary>
        /// <param name="value"></param>
        public void SetInterrupt(bool value)
        {
            byte res = Read8(REGISTER_ENABLE);

            if (value)
            {
                res |= ENABLE_AIEN;
            }
            else
            {
                res = (byte)(res & ~ENABLE_AIEN);
            }

            Write8(REGISTER_ENABLE, res);
        }

        /// <summary>
        /// Clear the interrupt.
        /// </summary>
        public void ClearInterrupt()
        {
            _i2cDevice.Write(0x66 | COMMAND_BIT);
        }

        public bool Begin()
        {
            Dispose();

            _i2cDevice = new UnixI2cDevice(_i2cSettings);

            byte sensorId = Read8(REGISTER_SENSORID);

            if (sensorId != 0x44 && sensorId != 0x10)
            {
                Console.WriteLine($"sensorId = '{sensorId:X2}'");
                return false;
            }

            SetIntegrationTime(RgbColorSensorIntegrationTime.Milliseconds2_4);
            return true;
        }

        /// <summary>
        /// Reads the RGB color detected by the sensor.
        /// </summary>
        public void ReadSensor()
        {
            ReadColor();
            FillColor();
            FillTemperatureAndLuminosity();
        }

        /// <summary>
        /// Gets the persistence cycles of the sensor.
        /// </summary>
        public int GetCycles()
        {
            int result = -1;
            byte enable = Read8(REGISTER_ENABLE);

            if ((enable & ENABLE_AIEN) > 0)
            {
                byte apers = Read8(REGISTER_APERS);
                result = s_cycles[apers & 0x0f];
            }

            return result;
        }

        /// <summary>
        /// Sets the persistence cycles of the sensor.
        /// </summary>
        public void SetCycles(int value)
        {
            byte enable = Read8(REGISTER_ENABLE);

            if (value == -1)
            {
                Write8(REGISTER_ENABLE, (byte)(enable & ~ENABLE_AIEN));
            }
            else
            {
                int index = Array.IndexOf(s_cycles, value);

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException($"The value of parameter {nameof(value)} must be 0, 1, 2, 3, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 or 60.");
                }

                Write8(REGISTER_ENABLE, (byte)(enable | ENABLE_AIEN));
                Write8(REGISTER_APERS, (byte)index);
            }
        }

        /// <summary>
        /// Gets the minimum threshold value of the sensor.
        /// </summary>
        public ushort GetMinValue()
        {
            ushort result = Read16(REGISTER_AILT);
            return result;
        }

        /// <summary>
        /// Sets the minimum threshold value of the sensor.
        /// </summary>
        public void SetMinValue(ushort value)
        {
            Write16(REGISTER_AILT, value);
        }

        /// <summary>
        /// Gets the maximum threshold value of the sensor.
        /// </summary>
        public ushort GetMaxValue()
        {
            ushort result = Read16(REGISTER_AIHT);
            return result;
        }

        /// <summary>
        /// Sets the maximum threshold value of the sensor.
        /// </summary>
        public void SetMaxValue(ushort value)
        {
            Write16(REGISTER_AIHT, value);
        }

        /// <summary>
        /// Check if the status bit is set and the chip is ready.
        /// </summary>
        private bool IsValid()
        {
            byte result = Read8(REGISTER_STATUS);
            return result > 0;
        }

        /// <summary>
        /// Read the raw RGBC color detected by the sensor.
        /// Reads a raw color of 16-bit red, green, blue, clear component values (0-65535).
        /// </summary>
        private void ReadColor()
        {
            bool wasActive = IsActive;
            SetActive(true);

            while (!IsValid())
            {
                SleepIntegrationTime();
            }

            _rawColor.Red = Read16LittleEndian(REGISTER_RDATA);
            _rawColor.Green = Read16LittleEndian(REGISTER_GDATA);
            _rawColor.Blue = Read16LittleEndian(REGISTER_BDATA);
            _rawColor.Clear = Read16LittleEndian(REGISTER_CDATA);

            SetActive(wasActive);

            // Console.WriteLine($"R = {_rawColor.Red}, G = {_rawColor.Green}, B = {_rawColor.Blue}, C = {_rawColor.Clear}");
        }

        private void SleepIntegrationTime()
        {
            int time = 0;

            switch (_integrationTime)
            {
                case RgbColorSensorIntegrationTime.Milliseconds2_4:
                    time = 3;
                    break;

                case RgbColorSensorIntegrationTime.Milliseconds24:
                    time = 24;
                    break;

                case RgbColorSensorIntegrationTime.Milliseconds50:
                    time = 50;
                    break;

                case RgbColorSensorIntegrationTime.Milliseconds101:
                    time = 101;
                    break;

                case RgbColorSensorIntegrationTime.Milliseconds154:
                    time = 154;
                    break;

                case RgbColorSensorIntegrationTime.Milliseconds700:
                    time = 700;
                    break;

                default:
                    throw new NotSupportedException($"Unknown integration time: '{_integrationTime}'");
            }

            Thread.Sleep(time);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Write8(byte register, byte value)
        {
            WriteRegister(register, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte Read8(byte register)
        {
            return (byte)ReadRegister(register, 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Write16(byte register, ushort value)
        {
            WriteRegister(register, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort Read16(byte register)
        {
            return (ushort)ReadRegister(register, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort Read16LittleEndian(byte register)
        {
            ushort result = Read16(register);
            result = Utils.SwapBytes(result);
            return result;
        }

        private void WriteRegister(byte register, byte value)
        {
            _i2cDevice.Write((byte)(register | COMMAND_BIT), value);
        }

        private void WriteRegister(byte register, ushort value)
        {
            var buffer = new byte[]
            {
                (byte)(register | COMMAND_BIT),
                (byte)((value >> 8) & 0xFF),
                (byte)(value & 0xFF)
            };

            _i2cDevice.Write(buffer);
        }

        private uint ReadRegister(byte register, uint byteCount)
        {
            _i2cDevice.Write((byte)(register | COMMAND_BIT));
            uint result = (uint)_i2cDevice.Read(byteCount);
            return result;
        }

        private void FillColor()
        {
            byte red = (byte)(Math.Pow(((((float)_rawColor.Red / _rawColor.Clear) * 256) / 255), 2.5f) * 255);
            byte green = (byte)(Math.Pow(((((float)_rawColor.Green / _rawColor.Clear) * 256) / 255), 2.5f) * 255);
            byte blue = (byte)(Math.Pow(((((float)_rawColor.Blue / _rawColor.Clear) * 256) / 255), 2.5f) * 255);

            Color = Color.FromArgb(red, green, blue);
        }

        /// <summary>
        /// Converts RGBC data to color temperature and lux values.
        /// Returns a tuple of color temperature and luminosity.
        /// </summary>
        private void FillTemperatureAndLuminosity()
        {
            float x = -0.14282f * _rawColor.Red + 1.54924f * _rawColor.Green + -0.95641f * _rawColor.Blue;
            float y = -0.32466f * _rawColor.Red + 1.57837f * _rawColor.Green + -0.73191f * _rawColor.Blue;
            float z = -0.68202f * _rawColor.Red + 0.77073f * _rawColor.Green + 0.56332f * _rawColor.Blue;

            float divisor = x + y + z;

            if (divisor == 0)
            {
                Temperature = 0;
                Luminosity = 0;
            }
            else
            {
                float n = (x / divisor - 0.3320f) / (0.1858f - y / divisor);
                float cct = 449.0f * (float)Math.Pow(n, 3) + 3525.0f * (float)Math.Pow(n, 2) + 6823.3f * n + 5520.33f;

                Temperature = cct;
                Luminosity = y;
            }
        }
    }
}
