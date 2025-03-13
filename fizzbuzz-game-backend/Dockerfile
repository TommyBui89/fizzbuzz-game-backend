# ------------------- Build Stage -------------------
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
#    This step reads your existing Migrations folder and outputs a .sql file
RUN dotnet ef migrations script \
    --project "./fizzbuzz-game-backend.csproj" \
    --output "/app/migrations.sql"

# 6. Publish the application
RUN dotnet publish "./fizzbuzz-game-backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ------------------- Final Stage (Runtime) -------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# 7. Install SQLite in the runtime container
RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

# 8. Copy published output from build stage
COPY --from=build /app/publish .

# 9. Copy the generated migrations script
COPY --from=build /app/migrations.sql /app/migrations.sql

# 10. Expose port (optional)
EXPOSE 8080

# 11. Ensure the DB directory if you want /data
RUN mkdir -p /data

# 12. On startup, run migrations script, then start the app
CMD ["sh", "-c", "sqlite3 /data/fizzbuzz.db < /app/migrations.sql && dotnet fizzbuzz-game-backend.dll"]