// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.Devices.Gpio.Samples
{
    public class Bme280 : IDisposable
    {
        private const byte BME280_ADDRESS = 0x77; // Default I2C address

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

        private Bme280_Calibration_Data _calibrationData;

        private float _temperature;
        private float _humidity;
        private float _pressure;
        private int _tFine;

        private SpiConnectionSettings _spiSettings;
        private SpiDevice _spiDevice;

        private Pin _csPin;

        public Bme280(Pin chipSelectLine, SpiConnectionSettings spiSettings)
        {
            _csPin = chipSelectLine ?? throw new ArgumentNullException(nameof(chipSelectLine));
            _spiSettings = spiSettings ?? throw new ArgumentNullException(nameof(spiSettings));
        }

        public void Dispose()
        {
            if (_spiDevice != null)
            {
                _spiDevice.Dispose();
                _spiDevice = null;
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

        public float TemperatureInCelsius => _temperature + TemperatureCalibrationOffset;

        public float TemperatureInFahrenheit => TemperatureInCelsius * 1.8F + 32;

        public float Humidity => _humidity;

        public float PressureInPascals => _pressure;

        public float PressureInHectopascals => _pressure / 100;

        public float PressureInMillibars => PressureInHectopascals;

        public bool Begin()
        {
            Dispose();

            _csPin.Mode = PinMode.Output;
            DigitalWrite(_csPin, PinValue.High);

            if (_spiSettings != null)
            {
                _spiSettings.Mode = SpiMode.Mode0;
                _spiSettings.DataBitLength = 8; // 1 byte

                _spiDevice = new UnixSpiDevice(_spiSettings);
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

        /// <summary>
        /// Transfers data over the SPI bus
        /// </summary>
        private byte SpiTransfer(byte x = 0)
        {
            var readBuffer = new byte[1];

            if (x == 0)
            {
                _spiDevice.Read(readBuffer);
            }
            else
            {
                var writeBuffer = new byte[] { x };

                _spiDevice.TransferFullDuplex(writeBuffer, readBuffer);
            }

            byte result = readBuffer[0];
            return result;
        }

        private void Write8(byte reg, byte value)
        {
            DigitalWrite(_csPin, PinValue.Low);
            SpiTransfer((byte)(reg & ~0x80)); // write, bit 7 low
            SpiTransfer(value);
            DigitalWrite(_csPin, PinValue.High);
        }

        private byte Read8(byte reg)
        {
            DigitalWrite(_csPin, PinValue.Low);
            SpiTransfer((byte)(reg | 0x80)); // read, bit 7 high
            byte value = SpiTransfer();
            DigitalWrite(_csPin, PinValue.High);
            return value;
        }

        private ushort Read16(byte reg)
        {
            ushort result = 0;

            DigitalWrite(_csPin, PinValue.Low);
            SpiTransfer((byte)(reg | 0x80)); // read, bit 7 high

            for (int i = 0; i < 2; ++i)
            {
                byte value = SpiTransfer();
                result = (ushort)((result << 8) | value);
            }

            DigitalWrite(_csPin, PinValue.High);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort Read16LittleEndian(byte reg)
        {
            ushort temp = Read16(reg);
            return (ushort)((temp >> 8) | (temp << 8));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short ReadS16(byte reg)
        {
            return (short)Read16(reg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short ReadS16LittleEndian(byte reg)
        {
            return (short)Read16LittleEndian(reg);
        }

        private uint Read24(byte reg)
        {
            uint result = 0;

            DigitalWrite(_csPin, PinValue.Low);
            SpiTransfer((byte)(reg | 0x80)); // read, bit 7 high

            for (int i = 0; i < 3; ++i)
            {
                byte value = SpiTransfer();
                result = (result << 8) | value;
            }

            DigitalWrite(_csPin, PinValue.High);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DigitalWrite(Pin pin, int value)
        {
            PinValue state = HasFlag(value, 0x01) ? PinValue.High : PinValue.Low;
            DigitalWrite(pin, state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DigitalWrite(Pin pin, PinValue state)
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
