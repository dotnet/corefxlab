// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Devices.I2c.Unix
{
    internal enum I2cFunctionalityFlags : uint
    {
        I2c = 0x00000001,
        TenBitAddressing = 0x00000002,
        ProtocolMangling = 0x00000004,              /* I2C_M_{REV_DIR_ADDR,NOSTART,..} */
        SmBusHwPecCalc = 0x00000008,                /* SMBus 2.0 */
        SmBusReadWordDataPec = 0x00000800,          /* SMBus 2.0 */
        SmBusWriteWordDataPec = 0x00001000,         /* SMBus 2.0 */
        SmBusProcCallPec = 0x00002000,              /* SMBus 2.0 */
        SmBusBlockProcCallPec = 0x00004000,         /* SMBus 2.0 */
        SmBusBlockProcCall = 0x00008000,            /* SMBus 2.0 */
        SmBusQuick = 0x00010000,
        SmBusReadByte = 0x00020000,
        SmBusWriteByte = 0x00040000,
        SmBusReadByteData = 0x00080000,
        SmBusWriteByteData = 0x00100000,
        SmBusReadWordData = 0x00200000,
        SmBusWriteWordData = 0x00400000,
        SmBusProcCall = 0x00800000,
        SmBusReadBlockData = 0x01000000,
        SmBusWriteBlockData = 0x02000000,
        SmBusReadI2cBlock = 0x04000000,             /* I2C-like block xfer  */
        SmBusWriteI2cBlock = 0x08000000,            /* w/ 1-byte reg. addr. */
        SmBusReadI2cBlock2 = 0x10000000,            /* I2C-like block xfer  */
        SmBusWriteI2cBlock2 = 0x20000000,           /* w/ 2-byte reg. addr. */
        SmBusReadBlockDataPec = 0x40000000,         /* SMBus 2.0 */
        SmBusWriteBlockDataPec = 0x80000000,        /* SMBus 2.0 */

        SmBusByte = SmBusReadByte | SmBusWriteByte,
        SmBusByteData = SmBusReadByteData | SmBusWriteByteData,
        SmBusWordData = SmBusReadWordData | SmBusWriteWordData,
        SmBusBlockData = SmBusReadBlockData | SmBusWriteBlockData,
        SmBusI2cBlock = SmBusReadI2cBlock | SmBusWriteI2cBlock,
        SmBusI2cBlock2 = SmBusReadI2cBlock2 | SmBusWriteI2cBlock2,
        SmBusBlockDataPec = SmBusReadBlockDataPec | SmBusWriteBlockDataPec,
        SmBusWordDataPec = SmBusReadWordDataPec | SmBusWriteWordDataPec,

        SmBusReadBytePec = SmBusReadByteData,
        SmBusWriteBytePec = SmBusWriteByteData,
        SmBusReadByteDataPec = SmBusReadWordData,
        SmBusWriteByteDataPec = SmBusWriteWordData,
        SmBusBytePec = SmBusByteData,
        SmBusByteDataPec = SmBusWordData,

        SmBusEmul = SmBusQuick |
            SmBusByte |
            SmBusByteData |
            SmBusWordData |
            SmBusProcCall |
            SmBusWriteBlockData |
            SmBusWriteBlockDataPec |
            SmBusI2cBlock
    }

    ///// To determine what functionality is supported
    //[Flags]
    //private enum I2cFunctionalityFlags : ulong
    //{
    //    I2C_FUNC_I2C = 0x00000001,
    //    I2C_FUNC_SMBUS_BLOCK_DATA = 0x03000000
    //}
}
