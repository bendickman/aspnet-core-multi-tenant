# Multi-Tenant .NET 6 Web API

## Development Environment

The project was built using Visual Studio 2022 targeting .NET 6

## Architecture Overview

The architecture approach for this application was based on the following blog post(https://codewithmukesh.com/blog/multitenancy-in-aspnet-core/)

Multi-tenancy can be achieved a number of different ways including Query String, Domain binding, IP address, or Claims Based. 

This application handles multi-tenancy using a Request Header and highlights how tenants can share a database or, for more complex tenants, have their own database instance.

### MultiTenant.Api

This layer is a Web API targeting .NET Core 6 that provides all the endpoints of the application (details to follow)

### MultiTenant.ApiCore

This project contains all of the Core API setup including Exception Handling, Swagger, Health Checks, Versioning etc...

#### Health Checks

The Health Check to ensure a connection can be made to the database can be located at:

`_health?tenant={tenantId}` & `/_health/json?tenant={tenantId}`

#### Authentication & Authorization

This project utilizes Json Web Tokens (JWT) to provide access to the system. The JWT contains details of the tenant the user has registered with and will deny access when any attempt is made to another tenant.

### MultiTenant.Core

This layer contains all domain specific entities used throughout the application, interfaces and settings (appsettings).

### MultiTenant.Infrastructure

This layer contains the implmentation logic along with the persistance code which is handled using Entity Framework Core.

### MultiTenant.UnitTests

This project contains all of the units tests for the application, which are designed to be run both during development and any deployments (CD/CI)

## API Endpoints

- Get a Product based on the Id (GUID)

*Http GET* `/v1/product/{Id}`

- Post a new product passing Name, Description & Rate

*HTTP POST* `/v1/product'

The easiest way to test the API is via the Swagger definition located at `/index.html`

## Login Details

|||
|---|---|
|Username|user@test.co.uk|
|Password|sh=H5QM5T&?fH7XH|

## Technologies

* [.NET 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
* [Swagger](https://swagger.io/)
* [Serilog](https://serilog.net/)
* [Moq](https://github.com/moq/moq4)
* [NUnit](https://nunit.org/)
* [Integration Testing](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0)
* [JWT Authentication & Authorization](https://jwt.io/)
* [Health Checks](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0)
* [HashIds](https://hashids.org/)


