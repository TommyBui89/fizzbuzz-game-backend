FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["fizzbuzz-game-backend.csproj", "./"]
RUN dotnet restore "./fizzbuzz-game-backend.csproj"
COPY . .
RUN dotnet build "./fizzbuzz-game-backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./fizzbuzz-game-backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "fizzbuzz-game-backend.dll"]