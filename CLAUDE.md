# GivingWithLove — Backend

REST API for a charity organization (volunteers, projects, volunteering records). ASP.NET Core 8, EF Core 8, SQL Server LocalDB.
The Angular frontend lives in a separate repository (GivingWithLove-frontend).

## Solution overview (`givingWithLove.sln`)
- **Web** — ASP.NET Core host: `Program.cs` (DI, CORS, Swagger) and one controller per entity under `Web/Controllers` (routes `api/[controller]`; `VolunteerDomainController` uses `api/[controller]/[action]`). Depends on Bl only.
- **Bl** — business layer: service interfaces in `Bl/BLApi` (`IBl`, `BlICrud<T>`), services in `Bl/BLServices`, models returned to the API in `Bl/BLModels`. `BlManager` exposes all services through `IBl`. Depends on Dal.
- **Dal** — data access: EF Core context `Dal/Models/DbManager.cs` and entities in `Dal/Models`, repository interfaces in `Dal/Api` (`IDal`, `ICrud<T>`), implementations in `Dal/Services`. `DalManager` exposes them through `IDal`.
- Entities: Eichud, Volunteer, Volunteering, Project, SubProject, Position, VolunteerDomain (see section 7 of the rules for their meaning).
- `WeatherForecastController` / `WeatherForecast.cs` are template leftovers.

## Build and run
```bash
dotnet build
dotnet run --project Web
```
Swagger (Development): http://localhost:5269/swagger (profile `http`) or https://localhost:7016/swagger (profile `https`, use `dotnet run --project Web --launch-profile https`).
The database file `Dal/GWLData.mdf` is local only (not in git). Never delete, move or overwrite it.

## Known issues (planned tasks in the owner's plan)
- Singleton `DbManager` and private `ServiceCollection`s in `BlManager` / `DalManager` break parallel requests — BE-01.
- Connection string is hard-coded in `DbManager.OnConfiguring`; `appsettings.json` is ignored — BE-01 (moves it to user-secrets).
- `.Result` is used everywhere instead of `await`; tables are loaded and searched in memory — BE-02.
- Dal `Update` methods don't save (`db.X.ToList()[i] = item`) and swallow errors — BE-02.
- `GET /api/Volunteering` crashes: `PoorManCode` (an Eichud) is looked up as a Volunteer, with N+1 queries per row — BE-03.

@docs/agent-rules.md
