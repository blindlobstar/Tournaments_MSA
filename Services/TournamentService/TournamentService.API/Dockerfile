#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TournamentService.API/TournamentService.API.csproj", "TournamentService.API/"]
COPY ["TournamentService.Data/TournamentService.Data.csproj", "TournamentService.Data/"]
COPY ["TournamentService.Core/TournamentService.Core.csproj", "TournamentService.Core/"]
COPY ["Common.Data.EFCore/Common.Data.EFCore.csproj", "Common.Data.EFCore/"]
COPY ["Common.Core/Common.Core.csproj", "Common.Core/"]
COPY ["Common.EventBus.RabbitMq/Common.EventBus.RabbitMq.csproj", "Common.EventBus.RabbitMq/"]
COPY ["Common.Contracts/Common.Contracts.csproj", "Common.Contracts/"]
RUN dotnet restore "TournamentService.API/TournamentService.API.csproj"
COPY . .
WORKDIR "/src/TournamentService.API"
RUN dotnet build "TournamentService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TournamentService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TournamentService.API.dll"]