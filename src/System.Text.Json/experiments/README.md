# System.Text.Json

The purpose of this library is to make a json reader and writer that has low allocation and high performance.

The experiment solution compares the runtime and memory allocation of System.Text.Json with other Json reader/writers, namely Json.NET.

**Notes:**
* The results are based on 1000 iterations.
* The test string is a typical project.lock.json file (approx 400 KB, 10000 lines).
* A TextReader is used for the string when using Json.NET while a Utf8String is used for the string when using System.Text.Json

[API and Usage](#api-usage) is shown below.

## Experiment Results

Shown below are the results of the comparison between the System.Text.Json and Json.NET reader/writers in terms of memory allocation and runtime.

There are differences in the two JSON APIs that affect the runtime results.
The JSON.Net JsonReader returns 'values' as object type whereas System.Text.Json JsonReader returns 'values' as Utf8String.
Due to this, there is no casting done to different types from the input (which is Ut8fString) in the System.Text.Json.
However, in JSON.Net, the input is casted to object from the input (which is string text reader). This is one of the main reasons for the performance difference.
Other than that, JSON.Net provides additional JSON validation and tracks the line number and path for syntax error reporting.
Lastly, JSON.Net provides type specific Read APIs (such as ReadAsDecimal, ReadAsString, etc), which are not provided currently in System.Text.Json. The GetValue and GetName APIs only return Utf8String.

### JSON Reader
#### Memory Allocation
![alt tag](Reader_Allocation.png?raw=true "JSON Reader Memory Allocation")

#### Runtime
![alt tag](Reader_Runtime.png?raw=true "JSON Reader Runtime")

### JSON Writer
#### Memory Allocation
![alt tag](Writer_Allocation.png?raw=true "JSON Writer Memory Allocation")

#### Runtime
![alt tag](Writer_Runtime.png?raw=true "JSON Writer Runtime")

## Next Steps

* Can we build a low allocation JSON DOM layer on top of the reader?
* Can we build a seriealizer and deserializer?
* Are the APIs usable?
* Further improvements in runtime performance of JSON Reader and Writer.

## API Usage

### JsonReader APIs

```C#
public JsonTokenType TokenType;
public JsonReader(string str);
public JsonReader(Utf8String str);
public void Dispose();
public JsonValueType GetJsonValueType();
public bool Read();
public Utf8String GetName();
public Utf8String GetValue();

public enum JsonTokenType {
    // Start = 0 state reserved for internal use
    ObjectStart = 1,
    ObjectEnd = 2,
    ArrayStart = 3,
    ArrayEnd = 4,
    Property = 5,
    Value = 6
}

public enum JsonValueType {
    String = 0,
    Number = 1,
    Object = 2,
    Array = 3,
    True = 4,
    False = 5,
    Null = 6
}
```

### Read Json Usage

```C#
var reader = new JsonReader(jsonStr);
while (reader.Read())
{
    var tokenType = reader.TokenType;
    switch (tokenType)
    {
        case JsonReader.JsonTokenType.ObjectStart:
        case JsonReader.JsonTokenType.ObjectEnd:
        case JsonReader.JsonTokenType.ArrayStart:
        case JsonReader.JsonTokenType.ArrayEnd:
            break;
        case JsonReader.JsonTokenType.Property:
            var name = reader.GetName();
            var value = reader.GetValue();
            break;
        case JsonReader.JsonTokenType.Value:
            value = reader.GetValue();
            break;
        default:
            break;
    }
}
```