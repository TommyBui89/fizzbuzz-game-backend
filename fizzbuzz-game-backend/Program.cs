using FizzBuzzGameBackend.Services;
using FizzBuzzGameBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FizzBuzz API", Version = "v1" });
});

// Configure EF Core to use SQLite
builder.Services.AddDbContext<FizzBuzzDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Register in-memory session manager
builder.Services.AddSingleton<GameSessionManager>();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Configure Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FizzBuzz API v1");
    c.RoutePrefix = "swagger";
});

// ✅ Bind to 0.0.0.0:8080 instead of localhost
app.Run("http://0.0.0.0:8080");