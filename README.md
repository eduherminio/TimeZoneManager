# TimeZone Manager

## Local setup

### Requirements

- [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [Node v12.14.1](https://nodejs.org/en/)

### Instructions

- (Optional) Configure database connection and backend port in `backend\TimeZoneManager.Api\appsettings.json`.
- (Optional) Configure frontend port in `run.bat`.
- **Run `run.bat`**.
- By default, web client will be available in [`localhost:8002`](http://localhost:8002/) and backend api in [`localhost:8001`](http://localhost:8001/swagger/index.html).
- Once backend successfully starts, the database will be pre-populated with the following users:
  - Username: `timezone`, Password: `timezone`, Role: regular user.
  - Username: `user`, Password: `user`, Role: user manager.
  - Username: `admin`, Password: `admin`, Role: admin.

---

## Solution description

TimeZone Manager is a PoC solution for managing timezones. It consists on:

### Backend

- `TimeZoneManager`: .NET Standard 2.1 library that implements all the functionality.

- `TimeZoneManager.Api`: .NET Core 3.1 WebApi that exposes the previous library.

- Test projects for both the library and the WebApi.

### Frontend

- TypeScript client, based on React + Redux.

## Technical specs and details

### Backend architecture

TimeZoneManager Backend is an example of a multilayer architecture:

- Data Access Layer, formed by Data Access Objects that provide a common interface and an implementation using an ORM (Entity Framework Core in this case). Model entities are used in this layer.

- Service layer, implemented using facade pattern. Both model entities and DTOs are used in this layer, although publicly exposed methods only accept and return DTOs.

- Api layer, without any logic and that just invokes service methods. Model DTOs are used in this layer.

### Aspects, filters and interceptors

- A number of aspects, filters and interceptors are used in this library in order to take care of the following issues:
  - Authorization.
  - Authentication.
  - Transactionality.
  - Exception handling.
  - Invocation logging.

[AspectCore](https://github.com/dotnetcore/AspectCore-Framework) has been used for this purpose.

### Client generation

There isn't a single manual invocation in the whole project.

- A OpenApi 3 specification is generated from our WebApi.

- C# clients are generated from that specification and used in integration tests project.

- TypeScript clients are also generated from that specification and used in frontend project.

All clients are generated on (a debug) build and automatically copied to where they are used.

The tool used for Swagger specification and client generation is [NSwag](https://github.com/RicoSuter/NSwag).

It also exposes a Swagger endpoints page, very useful for dev and testing purposes in {backendUrl}/swagger/ (i.e., `http://localhost:8001/swagger`) and which has been configure to allow configuring authorization.

### Authentication

JWT tokens have been used for authentication, which is handled at WebApi layer, so that there are no unintended unauthenticated service invocations.

### Authorization

A Permission|Role|User schema has been used for authorization purposes, so that a user (through their roles) needs to have a certain permission to invoke a certain method.

There's a SuperAdmin permission, totally invisible and unexposed to users, that is able to to access all functionalities and that helps the system invoking data initialization.

An Admin permission could be created to grant certain type of users access to most of the system too, but it doesn't currently exists in the system.

### Data initialization

The project is prepared to use relational databases (although it could easily use whatever database EF Core has a provider for) and is pre-configured to use SQLServer.

It could be very easily modified to use SQLite too (for instance, integration tests project uses a SQLite database).
