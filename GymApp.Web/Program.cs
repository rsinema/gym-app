using GymApp.Repositories;
using GymApp.Repositories.UserRepository;
using GymApp.Services.UserService;
using GymApp.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add this before building the app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register your repository and service
builder.Services.AddScoped<IUserRepository, UserRepository>(provider =>
    new UserRepository(builder.Configuration.GetConnectionString("Mongo") ?? "MongoKey"));

builder.Services.AddScoped<IUserService, UserService>(provider => {
    var repository = provider.GetRequiredService<IUserRepository>();
    var signingKey = builder.Configuration["SigningKey"] ?? "Key";
    return new UserService(repository, signingKey);
});

var app = builder.Build();

// Add this after var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseAntiforgery();
app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
