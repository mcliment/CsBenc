# CsBenc

[![Build Status](https://travis-ci.org/mcliment/CsBenc.svg?branch=master)](https://travis-ci.org/mcliment/CsBenc)
[![Build status](https://ci.appveyor.com/api/projects/status/fjci7fei2wyqqu45?svg=true)](https://ci.appveyor.com/project/mcliment/csbenc)
[![Coverage Status](https://coveralls.io/repos/github/mcliment/CsBenc/badge.svg?branch=master)](https://coveralls.io/github/mcliment/CsBenc?branch=master)

A base-n encoder for the .NET ecosystem.

The aim is to provide a .NET Core compatible arbitrary base-N encoder for the adventurous.

For the not so adventurous, a comprehensive set of predefined encoders are available,
from the classic RFC compliant Base32 and Base64 string-to-string encodings to modern
byte-to-string encodings like Base58 used for Bitcoin hashes.

## Features

* Built-in common encoders (Base64, Base32, Crockford, Base58)
* Ability to create new encoders from custom dictionaries (with arbitrary length)

*NOTE: This project is in early stages and may change a lot in the future*

## Usage

The `Encoder` class contains most of the functionality, allowing the quick instantiation
of predefined encoders or the creation of new ones with custom dictionaries and lenghts as
well as some extra features like checksums.

Example:
```csharp
// Create a new Base64 encoder instance
var encoder = Encoder.RfcBase64();
    
var text = "This is a test"; // Text to encode
var encoded = encoder.Encode(text); // encoded == "VGhpcyBpcyBhIHRlc3Q="
```
    
Or a simpler usage:
```csharp
var encoded = Encoder.RfcBase32().Encode("This is a test"); // encoded == "KRUGS4ZANFZSAYJAORSXG5A="
```    

## Roadmap

* Implement more common encodings
* Improve and add more Base58 features
* Better performance (both memory and speed)

## Contributing

Feel free to send a pull request to this project, report any issue you encounter or suggest improvements and features.
