# Congestion Tax Calculator

## Getting started
The CongestionTaxCalculator is designed with Domain-Driven Design (DDD) and follows the principles of Clean Architecture. The solution is structured into multiple layers, including Domain, Application, Infrastructure, and Presentation layers, promoting modularity and separation of concerns.

## Features
- Calculates congestion toll tax fees for vehicles within a specified city.
- Currently configured for Gothenburg, with easy extensibility for additional cities, such as Stockholm, by updating the seed method.
- In-memory database using EFCore’s InMemory package, eliminating the need for connection string configuration.

## Testing
### Unit Tests
The project includes a Unit Test suite to validate business logic, ensuring reliable calculations within the Domain layer.

### Integration Tests
An Integration Test suite has been integrated to verify end-to-end functionality across multiple layers, emulating real-world scenarios. You can expand this suite with additional test scenarios to validate different cases.

## Prerequisites
Install the following prerequisites:

- **[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**
  If you have .NET installed, use the dotnet --info command to determine which SDK you're using.


