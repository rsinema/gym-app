using System.Text;
using GymApp.Services.LocalStorage;
using GymApp.Services.AuthService;
using GymApp.Services.UserService;
using GymApp.Web.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using GymApp.Repositories.UserRepository;
using GymApp.Repositories.ExerciseRepository;
using GymApp.Services.ExerciseService;

// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json and appsettings.{Environment}.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Register LocalStorageService
builder.Services.AddScoped<LocalStorageService>();

// Register AuthService
builder.Services.AddScoped<IAuthService>(provider => {
    var signingKey = builder.Configuration["SigningKey"] ?? "YourSuperSecretSigningKey123!";
    return new AuthService(signingKey);
});

// Register UserRepository
builder.Services.AddScoped<IUserRepository>(provider => {
    var connectionString = builder.Configuration.GetConnectionString("Mongo") ?? "mongodb://localhost:27017";
    return new UserRepository(connectionString);
});

// Register UserService
builder.Services.AddScoped<IUserService>(provider => {
    var userRepository = provider.GetRequiredService<IUserRepository>();
    var authService = provider.GetRequiredService<IAuthService>();
    var logger = provider.GetRequiredService<ILogger<UserService>>();
    return new UserService(userRepository, authService, logger);
});

// Register ExerciseRepository
builder.Services.AddScoped<IExerciseRepository>(provider => {
    var connectionString = builder.Configuration.GetConnectionString("Mongo") ?? "mongodb://localhost:27017";
    return new ExerciseRepository(connectionString);
});

// Register ExerciseService
builder.Services.AddScoped<IExerciseService>(provider => {
    var exerciseReposity = provider.GetRequiredService<IExerciseRepository>();
    return new ExerciseService(exerciseReposity);
});

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your_issuer",
            ValidAudience = "your_audience",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["SigningKey"] ?? "YourSuperSecretSigningKey123!"))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();