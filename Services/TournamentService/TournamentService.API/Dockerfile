#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Services/TournamentService/TournamentService.API/TournamentService.API.csproj", "Services/TournamentService/TournamentService.API/"]
COPY ["Common/Contracts/Common.Contracts.csproj", "Common/Contracts/"]
COPY ["Common/Core/Common.Core.csproj", "Common/Core/"]
COPY ["Services/TournamentService/TournamentService.Data/TournamentService.Data.csproj", "Services/TournamentService/TournamentService.Data/"]
COPY ["Services/TournamentService/TournamentService.Core/TournamentService.Core.csproj", "Services/TournamentService/TournamentService.Core/"]
COPY ["Services/TournamentService/Tournament.FunctioanlTest/Tournament.FunctionalTest.csproj", "Services/TournamentService/Tournament.FunctioanlTest/"]
COPY ["Common/Data.EFCore/Common.Data.EFCore.csproj", "Common/Data.EFCore/"]
COPY ["Common/EventBus.RabbitMq/Common.EventBus.RabbitMq.csproj", "Common/EventBus.RabbitMq/"]
RUN dotnet restore "Services/TournamentService/TournamentService.API/TournamentService.API.csproj"
RUN dotnet restore "Services/TournamentService/Tournament.FunctioanlTest/Tournament.FunctionalTest.csproj"
COPY . .
WORKDIR "/src/Services/TournamentService/TournamentService.API"
RUN dotnet build "TournamentService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TournamentService.API.csproj" -c Release -o /app/publish

FROM build AS functionaltest
WORKDIR "/src/Services/TournamentService/Tournament.FunctioanlTest"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TournamentService.API.dll"]