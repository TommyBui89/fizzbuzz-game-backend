﻿# ------------------- Build Stage -------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copy csproj for caching
COPY ["fizzbuzz-game-backend.csproj", "./"]
RUN dotnet restore "./fizzbuzz-game-backend.csproj"

# 2. Copy the rest of your source
COPY . .

# 3. Install EF Tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# 4. Build the project
RUN dotnet build "./fizzbuzz-game-backend.csproj" -c Release -o /app/build

# 5. Generate a SQL script from your EF migrations
RUN dotnet ef migrations script \
    --project "./fizzbuzz-game-backend.csproj" \
    --output "/app/migrations.sql"

# 6. Publish the application
RUN dotnet publish "./fizzbuzz-game-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ------------------- Final Stage (Runtime) -------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# ✅ Install SQLite + sh
RUN apt-get update && apt-get install -y sqlite3 bash && rm -rf /var/lib/apt/lists/*

# Copy app and migration script
COPY --from=build /app/publish .
COPY --from=build /app/migrations.sql /app/migrations.sql

# Create DB dir
RUN mkdir -p /data

# Expose port
EXPOSE 8080

# ✅ Start by applying migrations and launching app
CMD ["sh", "-c", "sqlite3 /data/fizzbuzz.db < /app/migrations.sql && dotnet fizzbuzz-game-backend.dll"]
