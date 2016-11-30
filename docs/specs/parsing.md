# Parsing

## Goals

* Parses the following types with minimal allocations, ideally none:
    - `bool`
    - `byte`, `sbyte`
    - `Int16`, `UInt16`
    - `Int32`, `UInt32`
    - `Int64`, `UInt64`
    - `Single`, `Double`
    - `Decimal`
    - `DateTime`, `DateTimeOffset`
    - `TimeSpan`
    - `Guid`
* Parses different cultures
    - e.g. English and German dates
    - Needs to handle all cultures .NET can handle
* Parse from these encodings
    - UTF8      <------------- THATS WHAT WE CARE ABOUT
    - ASCII
    - UTF16-BE
    - UTF16-LE
* Parses from existing buffers without knowing up-front how many bytes the
  parsing needs to consume

## Discussion

* We could only support UTF8 and have a hand-optimized version of that and
  support other encodings by converting that representation to UTF8
