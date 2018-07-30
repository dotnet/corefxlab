// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Devices.Gpio
{
    public class PinValueChangedEventArgs : EventArgs
    {
        public PinValueChangedEventArgs(int bcmPinNumber) => BcmPinNumber = bcmPinNumber;

        public int BcmPinNumber { get; }
    }
}
