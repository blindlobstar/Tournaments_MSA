FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
WORKDIR /app


COPY "Tournaments.sln" "Tournaments.sln"

COPY "Common.Core/Common.Core.csproj" "Common.Core/Common.Core.csproj"
COPY "Common.Contracts/Common.Contracts.csproj" "Common.Contracts/Common.Contracts.csproj"
COPY "Common.Logic/Common.Logic.csproj" "Common.Logic/Common.Logic.csproj"
COPY "Common.Data.EFCore/Common.Data.EFCore.csproj" "Common.Data.EFCore/Common.Data.EFCore.csproj"
COPY "Common.EventBus.RabbitMq/Common.EventBus.RabbitMq.csproj" "Common.EventBus.RabbitMq/Common.EventBus.RabbitMq.csproj"
COPY "Common.Data.MongoDB/Common.Data.MongoDB.csproj" "Common.Data.MongoDB/Common.Data.MongoDB.csproj"
COPY "IdentityService.API/IdentityService.API.csproj" "IdentityService.API/IdentityService.API.csproj"
COPY "SimpleApiGateway/SimpleApiGateway.csproj" "SimpleApiGateway/SimpleApiGateway.csproj"
COPY "TournamentService.Core/TournamentService.Core.csproj" "TournamentService.Core/TournamentService.Core.csproj"
COPY "TournamentService.Data/TournamentService.Data.csproj" "TournamentService.Data/TournamentService.Data.csproj"
COPY "TournamentService.API/TournamentService.API.csproj" "TournamentService.API/TournamentService.API.csproj"
COPY "TournamentService.Test/TournamentService.Test.csproj" "TournamentService.Test/TournamentService.Test.csproj"
COPY "UserService.API/UserService.API.csproj" "UserService.API/UserService.API.csproj"
COPY "UserService.Core/UserService.Core.csproj" "UserService.Core/UserService.Core.csproj"
COPY "UserService.Data/UserService.Data.csproj" "UserService.Data/UserService.Data.csproj"
COPY "UserService.Test/UserService.Test.csproj" "UserService.Test/UserService.Test.csproj"

RUN dotnet restore "Tournaments.sln"

COPY . .
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TournamentService.API.dll"]
# COPY ./wait-for-it.sh /wait-for-it.sh
# RUN chmod +x /wait-for-it.sh