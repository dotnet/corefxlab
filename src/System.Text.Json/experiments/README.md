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
public JsonReader.JsonTokenType TokenType;
public JsonReader(string str);
public JsonReader(Utf8String str);
public void Dispose();
public JsonReader.JsonValueType GetJsonValueType();
public bool Read();
public Utf8String GetName();
public object ReadValue();

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