# Introduction
The purpose of this project is to get started with NHibernate quickly.
It includes a small recipe database and a minimal ASP.NET Core UI so a new project can start from a working baseline.

# Getting Started

1. Create a database on your local SQL Server called `RecipeDb`.
2. Run the SQL script from `DbCreation/RecipeDb.sql`.
3. Initialize local secrets for the `WebUI` project:
   `dotnet user-secrets set "SQLUSERNAME" "your-username" --project WebUI/WebUI.csproj`
   `dotnet user-secrets set "SQLUSERPASSWORD" "your-password" --project WebUI/WebUI.csproj`
4. Run the test suite:
   `dotnet test DotnetFullWebApp.sln`
5. Start the application:
   `dotnet run --project WebUI/WebUI.csproj`

# Notes
The NHibernate session factory is registered once, and repositories manage their own session lifecycle lazily. This keeps the template closer to production-ready usage and avoids binding a scoped `ISession` for every request by default.

# Project by Brian Staal @ www.wisesoft.dk
