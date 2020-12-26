#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Services/UserService/UserService.API/UserService.API.csproj", "Services/UserService/UserService.API/"]
COPY ["Services/UserService/UserService.Core/UserService.Core.csproj", "Services/UserService/UserService.Core/"]
COPY ["Common/Core/Common.Core.csproj", "Common/Core/"]
COPY ["Common/Data.MongoDB/Common.Data.MongoDB.csproj", "Common/Data.MongoDB/"]
COPY ["Common/Contracts/Common.Contracts.csproj", "Common/Contracts/"]
COPY ["Services/UserService/UserService.Data/UserService.Data.csproj", "Services/UserService/UserService.Data/"]
COPY ["Common/EventBus.RabbitMq/Common.EventBus.RabbitMq.csproj", "Common/EventBus.RabbitMq/"]
RUN dotnet restore "Services/UserService/UserService.API/UserService.API.csproj"
COPY . .
WORKDIR "/src/Services/UserService/UserService.API"
RUN dotnet build "UserService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]