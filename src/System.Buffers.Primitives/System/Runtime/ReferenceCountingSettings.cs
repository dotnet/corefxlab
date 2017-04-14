// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Runtime
{
    public enum ReferenceCountingMethod
    {
        Interlocked,
        ReferenceCounter,
        None
    };

    public class ReferenceCountingSettings
    {
        public static ReferenceCountingMethod OwnedMemory = ReferenceCountingMethod.Interlocked;
    }
}