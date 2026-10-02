
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.IO;
using WebMensajeria.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException(
        "Connection string 'Postgres' not found."
    );

// Compartir las claves de protección con MVC.
var carpetaClaves = Path.GetFullPath(
    Path.Combine(
        builder.Environment.ContentRootPath,
        "..",
        "shared-keys"
    )
);

Directory.CreateDirectory(carpetaClaves);

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(carpetaClaves))
    .SetApplicationName("WebMensajeria");

// Base de datos PostgreSQL.
builder.Services.AddDbContext<WebMensajeriaAPIContext>(
    options => options.UseNpgsql(connectionString)
);

AppContext.SetSwitch(
    "Npgsql.EnableLegacyTimestampBehavior",
    true
);

// Controladores.
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Una sola configuración de autenticación.
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".AspNetCore.Cookies";

        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };

        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

// SignalR.
builder.Services.AddSignalR();
builder.Services.AddSingleton<
    Microsoft.AspNetCore.SignalR.IUserIdProvider,
    WebMensajeria.API.Hubs.UsuarioIdProvider>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MVC", policy =>
    {
        policy
            .WithOrigins("https://localhost:7179")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
// Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("MVC");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();