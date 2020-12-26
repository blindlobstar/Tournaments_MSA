#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Services/IdentityService/IdentityService.API/IdentityService.API.csproj", "Services/IdentityService/IdentityService.API/"]
COPY ["Common/Logic/Common.Logic.csproj", "Common/Logic/"]
COPY ["Common/Core/Common.Core.csproj", "Common/Core/"]
COPY ["Common/Contracts/Common.Contracts.csproj", "Common/Contracts/"]
COPY ["Common/EventBus.RabbitMq/Common.EventBus.RabbitMq.csproj", "Common/EventBus.RabbitMq/"]
COPY ["Common/Data.MongoDB/Common.Data.MongoDB.csproj", "Common/Data.MongoDB/"]
RUN dotnet restore "Services/IdentityService/IdentityService.API/IdentityService.API.csproj"
COPY . .
WORKDIR "/src/Services/IdentityService/IdentityService.API"
RUN dotnet build "IdentityService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]