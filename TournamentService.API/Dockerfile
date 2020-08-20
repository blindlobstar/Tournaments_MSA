FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY Common.Core/Common.Core.csproj Common.Core/
COPY Common.Contracts/Common.Contracts.csproj Common.Contracts/
COPY Common.Data.EFCore/Common.Data.EFCore.csproj Common.Data.EFCore/
COPY Common.EventBus.RabbitMq/Common.EventBus.RabbitMq.csproj Common.EventBus.RabbitMq/
COPY TournamentService.Core/TournamentService.Core.csproj TournamentService.Core/
COPY TournamentService.Data/TournamentService.Data.csproj TournamentService.Data/
COPY TournamentService.API/TournamentService.API.csproj TournamentService.API/
COPY TournamentService.Test/TournamentService.Test.csproj TournamentService.Test/

RUN dotnet restore TournamentService.API/TournamentService.API.csproj

COPY . .
RUN dotnet publish -c Release -o /app

FROM build as test
WORKDIR /src/TournamentService.Test
RUN dotnet test

FROM base as final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TournamentService.API.dll"]