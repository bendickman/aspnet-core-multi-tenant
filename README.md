# Multi-Tenant .NET 6 Web API

## Development Environment

The project was built using Visual Studio 2022 targeting .NET 6

## Architecture Overview

The architecture approach for this application was based on the following blog post(https://codewithmukesh.com/blog/multitenancy-in-aspnet-core/)

### MultiTenant.Api

This layer is a Web API targeting .NET Core 6 that provides all the endpoints of the application (details to follow)

### MultiTenant.Core

This layer contains all domain specific entities used throughout the application, interfaces and settings (appsettings).

### MultiTenant.Infrastructure

This layer contains the implmentation logic along with the persistance code which is handled using Entity Framework Core.

### MultiTenant.UnitTests

This layer contains all of the units tests for the application, which are designed to be run both during development and any deployments (CD/CI)

## API Endpoints

- Get a Product based on the Id (GUID)

*Http GET* `/v1/product/{Id}`

- Post a new product passing Name, Description & Rate

*HTTP POST* `/v1/product'

The easiest way to test the API is via the Swagger definition located at `/swagger/index.html`

## Technologies

* [.NET 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)


