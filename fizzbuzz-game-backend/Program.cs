using FizzBuzzGameBackend.Data;
using FizzBuzzGameBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Configure Services -------------------

// Add controllers
builder.Services.AddControllers();

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FizzBuzz API",
        Version = "v1",
        Description = "An API for managing customizable FizzBuzz games"
    });
});

// Entity Framework Core with SQLite
builder.Services.AddDbContext<FizzBuzzDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS Policy for Frontend (running at http://localhost:4173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4173")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register custom services for business logic and in-memory session management
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameSessionService, GameSessionService>();
builder.Services.AddSingleton<GameSessionManager>();

var app = builder.Build();

// ------------------- Configure Middleware -------------------

// Use CORS policy
app.UseCors("AllowFrontend");

// Enable Swagger UI for API documentation
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FizzBuzz API v1");
    c.RoutePrefix = "swagger";
});

app.UseRouting();
app.UseAuthorization();

// Map attribute-based API controllers
app.MapControllers();

// Bind to all network interfaces on port 8080 (essential for Docker)
app.Run("http://0.0.0.0:8080");
