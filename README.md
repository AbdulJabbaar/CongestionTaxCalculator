# Congestion Tax Calculator

## Getting started
CongestionTaxCalculator follows the principle of domain-driven design (DDD) and clean architecture. The solution includes various layers like Domain, Application, Infrastructure and Presentation. To validate the business logic, there is a UnitTest project which validate the business logic.

This project is designed to calculate the congestion toll tax fee for vehicles with in specified city. Currently it is only setup with Gothenburg city, but you can add other cities like Stockholm by simply updating the seed method.

There is no need to setup the connection string in the project. It is using EFCore InMemory database package.

## Prerequisites
Install the following prerequisites:

- **[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)**
  If you have .NET installed, use the dotnet --info command to determine which SDK you're using.


