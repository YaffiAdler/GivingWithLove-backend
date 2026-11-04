# GivingWithLove — Backend

REST API for managing a charity organization: volunteers, projects, sub-projects, positions and volunteering records.
Built with **C# / ASP.NET Core** in a clean three-layer architecture. Works together with the [Angular frontend](https://github.com/YaffiAdler/GivingWithLove-frontend).

## Tech stack
- C# / ASP.NET Core Web API
- Entity Framework Core + SQL Server (LocalDB)
- Swagger / OpenAPI
- CORS: in Development any `http(s)://localhost:<port>` origin; other environments use `Cors:AllowedOrigins`

## Architecture
```
givingWithLove.sln
├── Web   – API controllers, startup, Swagger, CORS
├── Bl    – Business logic: services, models (DTOs), custom exceptions
└── Dal   – Data access: EF Core DbContext, entities, repositories
```
- Each layer talks to the next only through interfaces (one per entity, generic `BlICrud` / `ICrud`), so layers are easy to test and replace.
- Services are registered in ASP.NET Core dependency injection with a scoped `DbManager` (one per request):
  `Dal/DependencyInjection.cs` (`AddDataAccess`) and `Bl/DependencyInjection.cs` (`AddBusinessLogic`), called from `Web/Program.cs`.
  Controllers receive the specific Bl interface they need.

## Domain entities
Volunteer · Volunteer Domain · Volunteering · Project · Sub-Project · Position · Eichud

## API
Each entity has a controller with the standard CRUD endpoints, for example:

| Method | Route | Description |
|---|---|---|
| GET | `/api/Volunteer` | List all volunteers |
| GET | `/api/Volunteer/{id}` | Get one volunteer |
| POST | `/api/Volunteer` | Create |
| PUT | `/api/Volunteer/{id}` | Update |
| DELETE | `/api/Volunteer` | Delete |

The same pattern applies to `Project`, `SubProject`, `Position`, `Volunteering`, `VolunteerDomain` and `Eichud`.
Full interactive documentation is available in Swagger when running in Development.

## Getting started
**Requirements:** .NET SDK, SQL Server LocalDB, Visual Studio 2022 (or the `dotnet` CLI).

1. Clone the repository
   ```bash
   git clone https://github.com/YaffiAdler/GivingWithLove-backend.git
   ```
2. Set the connection string with user-secrets (it is never stored in `appsettings.json`):
   ```bash
   dotnet user-secrets init --project Web
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your LocalDB connection string>" --project Web
   ```
   Example value: `Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=<full path to>\GWLData.mdf;Integrated Security=True;Connect Timeout=30`.
   The API stops at startup with a clear message if the connection string is missing.
3. Run the API
   ```bash
   dotnet run --project Web
   ```
4. Open Swagger at `https://localhost:<port>/swagger`
   (the frontend expects `https://localhost:7016`).

## Local database
- The database file (`GWLData.mdf` / `.ldf`) is **not** stored in git (`*.mdf` and `*.ldf` are ignored).
- The owner keeps it locally in `Dal/`. Never delete, move or overwrite it.
- The connection string is set per machine with `dotnet user-secrets` (see Getting started, step 2).

## Roadmap
- Authentication & authorization (JWT)
- DTO validation and consistent error responses
- Unit tests for the business layer
