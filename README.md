# BinarySerializer
Lightweight package for version tolerant binary serialization and deserialization of objects.

## Getting Started
These instructions will get you a copy of the package installed and provide examples on how to use it.

### Installation
The package can be installed via the nuget package manager in visual studio or nuget cli tool.
```
nuget install CodeCompendium.BinarySerialization
```

### Usage
Using the package is very easy and should be familiar if you've used json serialization packages.
As a general rule, I highly recommend only serializing classes meant for save files - i.e. those in the Data Access (Persistence) layer of your software.

For general information see on serialization best practices see:
https://codecompendium.dev/post/binary-serialization

Serialization:
```
byte[] bytes = BinarySerializer.Serialize(myPersistenceObject);
```
The byte array generated above can then be written to a file.
```
File.WriteAllBytes("MySaveFile.myExt", bytes);
```

Deserialization:
```
byte[] bytes = File.ReadAllBytes("MySaveFile.myExt");
MyPersistenceObject obj = BinarySerializer.Deserialize<MyPersistenceObject>(bytes);
```

## License
This project is licensed under the MIT License. See LICENSE for details.
