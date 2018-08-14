// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    [Flags]
    public enum PinEvent
    {
        None = 0,
        Low = 1,
        High = 2,
        SyncFallingEdge = 4,
        SyncRisingEdge = 8,
        AsyncFallingEdge = 16,
        AsyncRisingEdge = 32,

        LowHigh = Low | High,
        SyncFallingRisingEdge = SyncFallingEdge | SyncRisingEdge,
        AsyncFallingRisingEdge = AsyncFallingEdge | AsyncRisingEdge,

        Any = LowHigh | SyncFallingRisingEdge | AsyncFallingRisingEdge
    }
}
