// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public enum SpiMode
    {
        /// <summary>CPOL = 0, CPHA = 0</summary>
        Mode0 = 0,
        /// <summary> CPOL = 0, CPHA = 1</summary>
        Mode1 = 1,
        /// <summary> CPOL = 1, CPHA = 0</summary>
        Mode2 = 2,
        /// <summary> CPOL = 1, CPHA = 1</summary>
        Mode3 = 3
    }

    public sealed class SpiConnectionSettings
    {
        public int ChipSelectLine { get; set; }
        public SpiMode Mode { get; set; }
        public int DataBitLength { get; set; }
        public int ClockFrequency { get; set; }

        public SpiConnectionSettings(int chipSelectLine)
        {
            ChipSelectLine = chipSelectLine;
            DataBitLength = 8;
            ClockFrequency = 5 * 100 * 1000;
            Mode = SpiMode.Mode0;
        }
    }
}
