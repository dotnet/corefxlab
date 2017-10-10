# Native-Sized Number Types

## Summary
The .NET platform has two native size  primitive types: IntPtr and UIntPtr.
These types were designed specifically for representing pointers,
and so they have subtly different semantics than integer types (e.g. Int32).
In particular, the types differ from integers in their approach to Conversions
(checked vs unchecked), arithmetic operations, and globalization support for
text operations (e.g. ToString).

Yet there are many scenarios where developers need native-sized integer and
floating point types with the same semantics as the existing fixed-size integer
and floating point types.

We have considered adopting IntPtr and UIntPtr for these scenarios,
but we decided against it because of compatibility reasons (it would require
changing behavior of UIntPtr and IntPtr). We decided that separate types for
integer and pointer scenarios is the best approach at this point.

Moreover, many OSX/iOS APIs, especially in the graphics space, use native
floating point type CGFloat. Currently, there is not even a remotely reasonable
way to interop with CGFloat APIs from the .NET platform.

## Scenarios

### Interop

There are cases where native OS APIs are defined in terms of the natural size of
the platform. Mono uses a poor man's approach to solve this problem by
introducing structures System.nint, System.nuint and System.nfloat that get
converted to corresponding integer types by the runtime.

Interop with a broad variety of programming environments is an important
scenario for us.  We should have a first class support for these
native-sized number types.

### Performance

There are performance sensitive scenarios where programs want to process memory
buffers in native integer chunks. IntPtr, not being a real integer, makes such
processing often tedious and error prone.

### Representing Sizes

System.Runtime.InteropServices.AllocHGlobal(IntPtr) has IntPtr argument,
even though it has nothing to do with pointers. It should have been nint
instead.

## Requirements

1.	[Pri 1] Provide native size signed and unsigned integer types
2.	[Pri 1] Provide native size floating point type
3.	[Pri 1] Semantics and APIs of these types match corresponding integer and
floating point types
4.	[Pri 1] Support down-level platforms (e.g. .NET 4.5) via NuGet adaptive
package
5.	[Pri 1] P/Invoke support
6.	[Pri 1] Language support for checked operations
7.	[Pri 2] Codegen on par with integers
8.	[Pri 2] The types will be added to future runtimes and .NET Standard.
9.	[Pri 2] The ability to use IntN and UIntN as array indexers
10.	[Pri 3] Provide C# language aliases for these types
11.	[Pri 3] Provide F# language aliases for these types

## API Design
We are going to introduce three types: IntN, UIntN, and FloatN.
These three types will have APIs and semantics same as the existing integer and floating point types,
with the exceptions noted in comments below, and adjusted to take the appropriate type, i.e IntN.Equals has an IntN parameter.

```C#
namespace System {
    // does not implement ICOmparable and IConvertible
    public struct IntN : IComparable<IntN>, IEquatable<IntN>, IFormattable {
        public static IntN MaxValue { get; } // note that Int32 uses const
        public static IntN MinValue { get; }

        // all other APIs that are on Int64/32
        …
    }

    // does not implement ICOmparable and IConvertible
    public struct UIntN : IComparable<UIntN>, IEquatable<UIntN>, IFormattable {
        public static UIntN MaxValue { get; }
        public static UIntN MinValue { get; }

        // all other APIs that are on UInt64/32
        …

    }
    // does not implement IComparable and IConvertible
    public struct FloatN : IComparable<UIntN>, IEquatable<UIntN>, IFormattable {
        public static FloatN MaxValue { get; }
        public static FloatN MinValue { get; }

        public static FloatN NegativeInfinity { get; }
        public static FloatN PositiveInfinity { get; }
        public static FloatN Epsilon { get; }
        public static FloatN NaN { get; }

        // all other APIs that are on Double/Single
        …
    }
}
```

### Conversions
Conversion operators will be provided for converting between these new native
types and existing integral and floating point types.
These operators, except the ones marked with *, will be implemented in the
compiler (not as library operator overloads) to support checked and unchecked
options. The operators that never overflow or underflow are implicit. Those that
do are explicit:

| From    | To UIntN   | To IntN   |
| ------- | ---------- | --------- |
| SByte	  | explicit	 | implicit  |
| Byte	  | implicit	 | implicit  |
| Int16	  | explicit	 | implicit  |
| UInt16  | implicit	 | implicit  |
| Int32	  | explicit	 | implicit  |
| UInt32  | implicit	 | explicit  |
| Int64	  | explicit	 | explicit  |
| UInt64  | explicit	 | explicit  |
| IntPtr  | explicit	 | implicit* |
| UIntPtr |	implicit*  | explicit  |
| FloatN	| explicit	 | explicit  |
| Single	| explicit	 | explicit  |
| Double	| explicit	 | explicit  |

| To      | From UIntN | From IntN |
| ------- | ---------- | --------- |
| SByte	  | explicit	 | explicit  |
| Byte	  | explicit	 | explicit  |
| Int16	  | explicit	 | explicit  |
| UInt16  | explicit	 | explicit  |
| Int32	  | explicit	 | explicit  |
| UInt32  | explicit	 | explicit  |
| Int64	  | explicit	 | implicit  |
| UInt64  | implicit	 | explicit  |
| IntPtr  | explicit	 | implicit* |
| UIntPtr |	implicit*	 | explicit  |
| FloatN	| implicit	 | implicit  |
| Single	| implicit	 | implicit  |
| Double	| implicit	 | implicit  |

\* this conversion will be implemented as an overloaded operator. The conversion
never fails, so the checked and unchecked behavior is the same, but we need
those to convert between these new native-size ints to IL nint type (indirectly
through [U]IntPtr).

## Language Support
Details of language support work are tracked at
https://github.com/dotnet/csharplang/issues/435. This section summarizes the
most important language features.

### Aliases
Ideally, C# (and other languages) would support aliases for these new types.
The following table lists proposed aliases for the C# language:  

| CLR type name | C# alias |
| ------------- |----------|
| IntN	        | nint     |
| UIntN	        | nuint    |
| FloatN	      | nfloat   |

### Checked Operations
Without compiler support, the following code does not throw. We need it to throw
OverflowException.
```c#
UIntN value = UIntN.MaxValue;
checked { value++; }
```

### Operators
The native-size types need to support all operators that their corresponding
fixed-size types support. The operators cannot be implemented as simple operator
overloads in UIntN, IntN, and FloatN types, because such overloads could not
respect `checked { … }` blocks and compile time switches.

## Design Considerations
1.	Should the types implement non-generic IComparable?
2.	Should the types implement IConvertible (after extending it with default
  members)?
3.	What package would these go into?
4.	How does the compiler find the types? By fully qualified name or assembly
qualified name?
5.	Do we like the type names? The currently proposed names follow the
[U]Int<size> naming pattern.
6.	Can the types be used in attributes?
7.	Are we going to allow default parameters types as native ints and float?

## Design Details

This is how UIntN will look in full detail.
IntN and FloatN will be very similar.
Note a quite large list of operators is supported through the compiler.

```c#
namespace System {
    public struct UIntN : IComparable<UIntN>, IEquatable<UIntN>, IFormattable {

        public static readonly UIntN MaxValue;
        public static readonly UIntN MinValue;

        public int CompareTo(UIntN other);
        public override bool Equals(object obj);
        public bool Equals(UIntN other);
        public override int GetHashCode();

        public static UIntN Parse(string text);
        public static UIntN Parse(string text, NumberStyles style);
        public static UIntN Parse(string text, IFormatProvider provider);
        public override string ToString();
        public string ToString(string format);
        public string ToString(string format, IFormatProvider formatProvider);
        public static bool TryParse(string text, out UIntN result);

        public static bool TryParse(string text, NumberStyles style, IFormatProvider provider, out UIntN result);
        public static UIntN Parse(string text, NumberStyles style, IFormatProvider provider);

        public static implicit operator UIntN (UIntPtr value);
        public static implicit operator UIntPtr (UIntN value);
    }
}
```

## Details of IntPtr and UIntPtr Incompatibilities

TODO

## Details of IL native int and native uint mapping to IntN and UIntN

TODO



## References
discussion of this proposal: https://github.com/dotnet/corefxlab/issues/1471

coreclr repo issue: https://github.com/dotnet/coreclr/issues/963

CGFloat: https://developer.apple.com/reference/coregraphics/cgfloat

F# language suggestion related to these types: https://github.com/fsharp/fslang-suggestions/issues/562
