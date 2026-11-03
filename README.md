# GivingWithLove — Backend

REST API for managing a charity organization: volunteers, projects, sub-projects, positions and volunteering records.
Built with **C# / ASP.NET Core** in a clean three-layer architecture. Works together with the [Angular frontend](https://github.com/YaffiAdler/GivingWithLove-frontend).

## Tech stack
- C# / ASP.NET Core Web API
- Entity Framework Core + SQL Server (LocalDB)
- Swagger / OpenAPI
- CORS configured for the Angular dev server (`http://localhost:4200`)

## Architecture
```
givingWithLove.sln
├── Web   – API controllers, startup, Swagger, CORS
├── Bl    – Business logic: services, models (DTOs), custom exceptions
└── Dal   – Data access: EF Core DbContext, entities, repositories
```
- Each layer talks to the next only through interfaces (`IBl`, `IDal`, generic `ICrud`), so layers are easy to test and replace.
- `BlManager` / `DalManager` expose the services; the Web layer receives `IBl` through dependency injection.

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
2. Set the connection string in `Web/appsettings.json` (`ConnectionStrings:DefaultConnection`) to point at your local `GWLData.mdf`.
3. Run the API
   ```bash
   dotnet run --project Web
   ```
4. Open Swagger at `https://localhost:<port>/swagger`
   (the frontend expects `https://localhost:7016`).

## Roadmap
- Authentication & authorization (JWT)
- DTO validation and consistent error responses
- Unit tests for the business layer
