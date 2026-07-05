using PhoneHub.API.Services;
using PhoneHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Logging
var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/phonehub-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=phonehub_db;Username=phonehub_user;Password=PhoneHub@123456";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] 
    ?? "SuperSecretKeyThatIsAtLeast32CharactersLongForHS256Encryption!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "PhoneHub.API";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "PhoneHub.Client";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDesktopClient", policy =>
    {
        policy.WithOrigins("https://localhost:7001", "http://localhost:5000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithWebSocketSupport();
    });
});

// Services
builder.Services.AddScoped<ISilentModeService, SilentModeService>();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowDesktopClient");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PhoneHub.API.Hubs.SilentModeHub>("/hubs/silentmode");

app.Run();
