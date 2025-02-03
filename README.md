# FakeDataGenerator

## Overview

`FakeDataGenerator` is a static class designed to generate realistic dummy data using the [Bogus](https://github.com/bchavez/Bogus) library. The class provides a method for generating fake data based on common property name patterns in a given class type. 

## Features

- Generates fake data for a variety of common property names such as `id`, `email`, `phone`, `address`, etc.
- Supports numeric, boolean, date, and string property types.
- Configurable seed and locale for generating data in different languages.
- Generates realistic data like names, addresses, emails, and more.

## Installation

To use the `FakeDataGenerator`, you need to install the [Bogus library](https://github.com/bchavez/Bogus). You can add it via NuGet package manager:


### .NET CLI
```
dotnet add package Alcuino.FakeDataGenerator --version 8.0.0
```
### Package Manager
```
NuGet\Install-Package Alcuino.FakeDataGenerator -Version 8.0.0
```

## Usage

### Configuration

The `FakeDataGenerator` class is configured using the `Config` object. You can set the following parameters:

- **Seed**: An integer used to seed the random number generator.
- **Locale**: The locale for generating data, e.g., `"en"` for English.
- **MoneySymbol**: The currency symbol used for generating price data.

Example configuration:

```csharp
FakeDataGenerator.Config = new Config
{
    Seed = 123,
    Locale = "en",
    MoneySymbol = "$"
};
```

### Generate Fake Data

You can generate fake data for any class type by calling the GenerateFakeData extension method. The method will return a collection of fake data based on the specified type.

```
//Example
var fakeDataList = new List<MyClass>().GenerateFakeData(total: 100).ToList();
```

### Supported Data Patterns
The GenerateFakeData method uses common property name patterns to generate appropriate fake data: (many more)

- **id**: A sequential integer or a GUID.
- **email**: A randomly generated email address.
- **phone**: A randomly generated phone number.
- **fullname**: A randomly generated full name.
- **price**: A randomly generated price using the configured currency symbol.
- **quantity**: A randomly generated quantity (1-10).
- **amount**: A randomly generated amount (between 10 and 1000).
- **date-related fields**: Random past dates.

The method applies patterns based on the property name, e.g., `email`, `address`, `qty`, `price`, etc.

### Example Class
```
public class MyClass
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```
Output
```
[
    {
        "Name": "John Doe",
        "Email": "johndoe@example.com",
        "Quantity": 5,
        "Price": "$456.78"
    },
    {
        "Name": "Jane Smith",
        "Email": "janesmith@example.com",
        "Quantity": 3,
        "Price": "$234.50"
    }
]
```
