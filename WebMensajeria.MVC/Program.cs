
using Humanizer;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using WebMensajeria.Consumer;
using WebMensajeria.Modelos;
using WebMensajeria.Services;
using WebMensajeria.Services.Interfaces;

CRUD<AdjuntoMensaje>.Endpoint = "https://localhost:7215/api/AdjuntoMensajes";
CRUD<Chat>.Endpoint = "https://localhost:7215/api/Chats";
CRUD<Contacto>.Endpoint = "https://localhost:7215/api/Contactos";
CRUD<DetalleGrupo>.Endpoint = "https://localhost:7215/api/DetalleGrupos";
CRUD<EstadoReceptorMensaje>.Endpoint = "https://localhost:7215/api/EstadoReceptorMensajes";
CRUD<Mensaje>.Endpoint = "https://localhost:7215/api/Mensajes";
CRUD<ParticipanteChat>.Endpoint = "https://localhost:7215/api/ParticipanteChats";
CRUD<ReaccionMensaje>.Endpoint = "https://localhost:7215/api/ReaccionMensajes";
CRUD<RolParticipante>.Endpoint = "https://localhost:7215/api/RolParticipantes";
CRUD<TipoAdjunto>.Endpoint = "https://localhost:7215/api/TipoChats";
CRUD<TipoEstadoMensaje>.Endpoint = "https://localhost:7215/api/TipoEstadoMensajes";
CRUD<TipoReaccion>.Endpoint = "https://localhost:7215/api/TipoReacciones";
CRUD<Usuario>.Endpoint = "https://localhost:7215/api/Usuarios";

var builder = WebApplication.CreateBuilder(args);

var carpetaClaves = Path.GetFullPath(
    Path.Combine(builder.Environment.ContentRootPath, "..", "shared-keys")
);

Directory.CreateDirectory(carpetaClaves);

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(carpetaClaves))
    .SetApplicationName("WebMensajeria");

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Index";
        options.Cookie.Name = ".AspNetCore.Cookies";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();