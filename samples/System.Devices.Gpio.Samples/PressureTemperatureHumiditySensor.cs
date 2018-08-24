// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Devices.I2c;
using System.Devices.Spi;
using System.Runtime.CompilerServices;

namespace System.Devices.Gpio.Samples
{
    /// <summary>
    /// Supports BME280 Pressure, Temperature and Humidity sensor
    /// </summary>
    public class PressureTemperatureHumiditySensor : IDisposable
    {
        public const byte DefaultI2cAddress = 0x77;
        public const byte AlternativeI2cAddress = 0x76;

        // Name of Registers used in the BME280

        private const byte BME280_DIG_T1_REG = 0x88;
        private const byte BME280_DIG_T2_REG = 0x8A;
        private const byte BME280_DIG_T3_REG = 0x8C;
        private const byte BME280_DIG_P1_REG = 0x8E;
        private const byte BME280_DIG_P2_REG = 0x90;
        private const byte BME280_DIG_P3_REG = 0x92;
        private const byte BME280_DIG_P4_REG = 0x94;
        private const byte BME280_DIG_P5_REG = 0x96;
        private const byte BME280_DIG_P6_REG = 0x98;
        private const byte BME280_DIG_P7_REG = 0x9A;
        private const byte BME280_DIG_P8_REG = 0x9C;
        private const byte BME280_DIG_P9_REG = 0x9E;

        private const byte BME280_DIG_H1_REG = 0xA1;
        private const byte BME280_DIG_H2_REG = 0xE1;
        private const byte BME280_DIG_H3_REG = 0xE3;
        private const byte BME280_DIG_H4_REG = 0xE4;
        private const byte BME280_DIG_H5_REG = 0xE5;
        private const byte BME280_DIG_H6_REG = 0xE7;

        private const byte BME280_REGISTER_CHIPID = 0xD0;
        private const byte BME280_REGISTER_VERSION = 0xD1;
        private const byte BME280_REGISTER_SOFTRESET = 0xE0;
        private const byte BME280_REGISTER_CAL26 = 0xE1;
        private const byte BME280_REGISTER_CONTROLHUMID = 0xF2;
        private const byte BME280_REGISTER_CONTROL = 0xF4;
        private const byte BME280_REGISTER_CONFIG = 0xF5;
        private const byte BME280_REGISTER_PRESSUREDATA = 0xF7;
        private const byte BME280_REGISTER_TEMPDATA = 0xFA;
        private const byte BME280_REGISTER_HUMIDDATA = 0xFD;

        // Structure to hold the calibration data that is
        // programmed into the sensor in the factory
        // during manufacture
        private struct Bme280_Calibration_Data
        {
            public ushort dig_T1;
            public short dig_T2;
            public short dig_T3;
            public ushort dig_P1;
            public short dig_P2;
            public short dig_P3;
            public short dig_P4;
            public short dig_P5;
            public short dig_P6;
            public short dig_P7;
            public short dig_P8;
            public short dig_P9;
            public byte dig_H1;
            public short dig_H2;
            public byte dig_H3;
            public short dig_H4;
            public short dig_H5;
            public sbyte dig_H6;
        };

        private enum ConnectionProtocol
        {
            Spi,
            I2c
        }

        private Bme280_Calibration_Data _calibrationData;

        private float _temperature;
        private float _humidity;
        private float _pressure;
        private int _tFine;

        private readonly SpiConnectionSettings _spiSettings;
        private SpiDevice _spiDevice;

        private readonly I2cConnectionSettings _i2cSettings;
        private I2cDevice _i2cDevice;

        private readonly ConnectionProtocol _protocol;
        private readonly GpioPin _csPin;

        public PressureTemperatureHumiditySensor(GpioPin chipSelectLine, SpiConnectionSettings spiSettings)
        {
            _csPin = chipSelectLine ?? throw new ArgumentNullException(nameof(chipSelectLine));
            _spiSettings = spiSettings ?? throw new ArgumentNullException(nameof(spiSettings));
            _protocol = ConnectionProtocol.Spi;
        }

        public PressureTemperatureHumiditySensor(I2cConnectionSettings i2cSettings)
        {
            _i2cSettings = i2cSettings ?? throw new ArgumentNullException(nameof(i2cSettings));
            _protocol = ConnectionProtocol.I2c;
        }

        public void Dispose()
        {
            if (_spiDevice != null)
            {
                _spiDevice.Dispose();
                _spiDevice = null;
            }

            if (_i2cDevice != null)
            {
                _i2cDevice.Dispose();
                _i2cDevice = null;
            }
        }

        /// <summary>
        /// Gets or sets the temperature calibration offset in degrees celsius.
        /// </summary>
        public float TemperatureCalibrationOffset { get; set; }

        public void ReadSensor()
        {
            ReadTemperature();
            ReadHumidity();
            ReadPressure();
        }

        public float SeaLevelPressureInHectopascals { get; set; }

        public float TemperatureInCelsius => _temperature + TemperatureCalibrationOffset;

        public float TemperatureInFahrenheit => TemperatureInCelsius * 1.8F + 32;

        public float Humidity => _humidity;

        public float PressureInPascals => _pressure;

        public float PressureInHectopascals => _pressure / 100;

        public float PressureInMillibars => PressureInHectopascals;

        public float AltitudInFeet => AltitudeInMeters / 0.3048f;

        public bool Begin()
        {
            Dispose();

            switch (_protocol)
            {
                case ConnectionProtocol.Spi:
                    _csPin.Mode = PinMode.Output;
                    DigitalWrite(_csPin, PinValue.High);

                    _spiSettings.Mode = SpiMode.Mode0;
                    _spiSettings.DataBitLength = 8; // 1 byte

                    _spiDevice = new UnixSpiDevice(_spiSettings);
                    break;

                case ConnectionProtocol.I2c:
                    _i2cDevice = new UnixI2cDevice(_i2cSettings);
                    break;

                default:
                    throw new NotSupportedException($"Connection protocol '{_protocol}' not supported");
            }

            byte chipId = Read8(BME280_REGISTER_CHIPID);

            if (chipId != 0x60)
            {
                Console.WriteLine($"chipId = '{chipId:X2}'");
                return false;
            }

            ReadSensorCoefficients();

            // Set Humidity oversampling to 1
            // Set before CONTROL (DS 5.4.3):
            // "Changes to this register only became effective
            // after a write operation to CONTROL register."
            Write8(BME280_REGISTER_CONTROLHUMID, 0x01);
            Write8(BME280_REGISTER_CONTROL, 0x3F);
            return true;
        }

        public float AltitudeInMeters
        {
            get
            {
                // From BMP180 datasheet (page 16):
                // http://www.adafruit.com/datasheets/BST-BMP180-DS000-09.pdf

                float result = 44330f * (1f - (float)Math.Pow(PressureInHectopascals / SeaLevelPressureInHectopascals, 0.1903));
                return result;
            }
        }

        private void ReadTemperature()
        {
            int var1, var2;

            uint adc_T = Read24(BME280_REGISTER_TEMPDATA);
            adc_T >>= 4;

            var1 = (((int)((adc_T >> 3) - (_calibrationData.dig_T1 << 1))) *
                     _calibrationData.dig_T2) >> 11;

            var2 = ((((int)((adc_T >> 4) - ((int)_calibrationData.dig_T1)) *
                       (int)((adc_T >> 4) - ((int)_calibrationData.dig_T1))) >> 12) *
                     _calibrationData.dig_T3) >> 14;

            _tFine = var1 + var2;
            _temperature = (_tFine * 5 + 128) >> 8;
            _temperature = _temperature / 100;
        }

        private void ReadPressure()
        {
            long var1, var2, p;

            uint adc_P = Read24(BME280_REGISTER_PRESSUREDATA);
            adc_P >>= 4;

            var1 = ((long)_tFine) - 128000;
            var2 = var1 * var1 * _calibrationData.dig_P6;
            var2 = var2 + ((var1 * _calibrationData.dig_P5) << 17);
            var2 = var2 + (((long)_calibrationData.dig_P4) << 35);
            var1 = ((var1 * var1 * _calibrationData.dig_P3) >> 8) +
                   ((var1 * _calibrationData.dig_P2) << 12);

            var1 = (((((long)1) << 47) + var1) * _calibrationData.dig_P1) >> 33;

            if (var1 == 0)
            {
                // Avoid divide by zero exception
                _pressure = 0;
            }
            else
            {
                p = 1048576 - adc_P;
                p = (((p << 31) - var2) * 3125) / var1;

                var1 = (_calibrationData.dig_P9 * (p >> 13) * (p >> 13)) >> 25;
                var2 = (_calibrationData.dig_P8 * p) >> 19;

                p = ((p + var1 + var2) >> 8) + (((long)_calibrationData.dig_P7) << 4);
                _pressure = (float)p / 256;
            }
        }

        private void ReadHumidity()
        {
            int adc_H = Read16(BME280_REGISTER_HUMIDDATA);
            int v_x1_u32r;

            v_x1_u32r = _tFine - 76800;

            v_x1_u32r = (((adc_H << 14) - (_calibrationData.dig_H4 << 20) -
                            (_calibrationData.dig_H5 * v_x1_u32r) + 16384) >> 15) *
                         (((((((v_x1_u32r * _calibrationData.dig_H6) >> 10) *
                              (((v_x1_u32r * _calibrationData.dig_H3) >> 11) + 32768)) >> 10) +
                            2097152) * _calibrationData.dig_H2 + 8192) >> 14);

            v_x1_u32r = v_x1_u32r - (((((v_x1_u32r >> 15) * (v_x1_u32r >> 15)) >> 7) *
                                       _calibrationData.dig_H1) >> 4);

            v_x1_u32r = (v_x1_u32r < 0) ? 0 : v_x1_u32r;
            v_x1_u32r = (v_x1_u32r > 419430400) ? 419430400 : v_x1_u32r;

            float h = v_x1_u32r >> 12;
            _humidity = h / 1024.0f;
        }

        /// <summary>
        /// Read the values that are programmed into the sensor during manufacture
        /// </summary>
        private void ReadSensorCoefficients()
        {
            _calibrationData.dig_T1 = Read16LittleEndian(BME280_DIG_T1_REG);
            _calibrationData.dig_T2 = ReadS16LittleEndian(BME280_DIG_T2_REG);
            _calibrationData.dig_T3 = ReadS16LittleEndian(BME280_DIG_T3_REG);
            _calibrationData.dig_P1 = Read16LittleEndian(BME280_DIG_P1_REG);
            _calibrationData.dig_P2 = ReadS16LittleEndian(BME280_DIG_P2_REG);
            _calibrationData.dig_P3 = ReadS16LittleEndian(BME280_DIG_P3_REG);
            _calibrationData.dig_P4 = ReadS16LittleEndian(BME280_DIG_P4_REG);
            _calibrationData.dig_P5 = ReadS16LittleEndian(BME280_DIG_P5_REG);
            _calibrationData.dig_P6 = ReadS16LittleEndian(BME280_DIG_P6_REG);
            _calibrationData.dig_P7 = ReadS16LittleEndian(BME280_DIG_P7_REG);
            _calibrationData.dig_P8 = ReadS16LittleEndian(BME280_DIG_P8_REG);
            _calibrationData.dig_P9 = ReadS16LittleEndian(BME280_DIG_P9_REG);
            _calibrationData.dig_H1 = Read8(BME280_DIG_H1_REG);
            _calibrationData.dig_H2 = ReadS16LittleEndian(BME280_DIG_H2_REG);
            _calibrationData.dig_H3 = Read8(BME280_DIG_H3_REG);
            _calibrationData.dig_H4 = (short)((Read8(BME280_DIG_H4_REG) << 4) | (Read8(BME280_DIG_H4_REG + 1) & 0xF));
            _calibrationData.dig_H5 = (short)((Read8(BME280_DIG_H5_REG + 1) << 4) | (Read8(BME280_DIG_H5_REG) >> 4));
            _calibrationData.dig_H6 = (sbyte)Read8(BME280_DIG_H6_REG);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short ReadS16(byte register)
        {
            return (short)Read16(register);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short ReadS16LittleEndian(byte register)
        {
            return (short)Read16LittleEndian(register);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private uint Read24(byte register)
        {
            return ReadRegister(register, 3);
        }

        private void WriteRegister(byte register, byte value)
        {
            switch (_protocol)
            {
                case ConnectionProtocol.Spi:
                    DigitalWrite(_csPin, PinValue.Low);

                    // write, bit 7 low
                    _spiDevice.Write((byte)(register & ~0x80), value);

                    DigitalWrite(_csPin, PinValue.High);
                    break;

                case ConnectionProtocol.I2c:
                    _i2cDevice.Write(register, value);
                    break;

                default:
                    throw new NotSupportedException($"Connection protocol '{_protocol}' not supported");
            }
        }

        private uint ReadRegister(byte register, uint byteCount)
        {
            uint result = 0;

            switch (_protocol)
            {
                case ConnectionProtocol.Spi:
                    DigitalWrite(_csPin, PinValue.Low);

                    // read, bit 7 high
                    _spiDevice.Write8((byte)(register | 0x80));
                    result = (uint)_spiDevice.Read(byteCount);

                    DigitalWrite(_csPin, PinValue.High);
                    break;

                case ConnectionProtocol.I2c:
                    _i2cDevice.Write8(register);
                    result = (uint)_i2cDevice.Read(byteCount);
                    break;

                default:
                    throw new NotSupportedException($"Connection protocol '{_protocol}' not supported");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DigitalWrite(GpioPin pin, int value)
        {
            const int True = 1;
            PinValue state = HasFlag(value, True) ? PinValue.High : PinValue.Low;
            DigitalWrite(pin, state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DigitalWrite(GpioPin pin, PinValue state)
        {
            pin.Write(state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HasFlag(int value, int flag)
        {
            return (value & flag) == flag;
        }
    }
}
