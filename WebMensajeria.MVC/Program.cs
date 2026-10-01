using Humanizer;
using WebMensajeria.Consumer;
using WebMensajeria.Modelos;
using Microsoft.EntityFrameworkCore;

CRUD<AdjuntoMensaje>.Endpoint = "https://localhost:7215/api/AdjuntoMensajes";
CRUD<Chat>.Endpoint = "https://localhost:7215/api/Chats";
CRUD<Contacto>.Endpoint = "https://localhost:7215/api/Contactos";
CRUD<DetalleGrupo>.Endpoint = "https://localhost:7215/api/DetalleGrupos";
CRUD<EstadoReceptorMensaje>.Endpoint = "https://localhost:7215/api/EstadoReceptorMensajes";
CRUD<Mensaje>.Endpoint = "https://localhost:7215/api/Mensajes";
CRUD<ParticipanteChat>.Endpoint = "https://localhost:7215/api/ParticipanteChats";
CRUD<ReaccionMensaje>.Endpoint = "https://localhost:7215/api/ReaccionMensajes";
CRUD<RolParticipante>.Endpoint = "https://localhost:7215/api/RolParticipantes";
CRUD<TipoAdjunto>.Endpoint = "https://localhost:7215/api/TipoAdjuntos";
CRUD<TipoChat>.Endpoint = "https://localhost:7215/api/TipoChats";
CRUD<TipoEstadoMensaje>.Endpoint = "https://localhost:7215/api/TipoEstadoMensajes";
CRUD<TipoReaccion>.Endpoint = "https://localhost:7215/api/TipoReacciones";

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebMensajeriaAPIContext") ?? throw new InvalidOperationException("Connection string 'WebMensajeriaAPIContext' not found.");

builder.Services.AddDbContext<WebMensajeriaAPIContext>(options => options.UseNpgsql(connectionString));


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
