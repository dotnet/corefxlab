// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System
{
    public enum VariantType
    {
        Object,
        Byte,
        SByte,
        Char,
        Boolean,
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        DateTime,
        DateTimeOffset,
        TimeSpan,
        Single,
        Double,
        Decimal,
        Guid

        // TODO:
        //
        // We can support arbitrary enums, see comments near the Union definition.
        // 
        // We should also support Memory<char>. This would require access to the internals
        // so that we can save the object and the offset/index. (Memory<char> is object/int/int).
        //
        // Supporting Span<char> would require making Variant a ref struct and access to
        // internals at the very least. It isn't clear if we could simply stick a ByReference<char>
        // in here or if there would be additional need for runtime changes.
        //
        // A significant drawback of making Variant a ref struct is that you would no longer be
        // able to create Variant[] or Span<Variant>.
    }
}
