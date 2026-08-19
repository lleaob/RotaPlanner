# Team Rota Planner (.NET)

## Build & Development Commands
- Run project: `dotnet run` or `dotnet watch`
- Build solution: `dotnet build`
- Run tests: `dotnet test`

## Architecture & Tech Stack
- Backend/App: .NET 9 (Blazor)
- Language: C# 13
- Core Types: Use `DateOnly` for calendar dates and `TimeOnly` for shift starts.

## Development Rules
- Maintain separation of concerns: Keep the Rota Generation Engine independent of the UI/Web layers.
- Prefer explicit types over `var` when it improves clarity for AI parsing.