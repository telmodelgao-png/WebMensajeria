using Microsoft.EntityFrameworkCore;

public class WebMensajeriaAPIContext(DbContextOptions<WebMensajeriaAPIContext> options) : DbContext(options)
{
    public DbSet<WebMensajeria.Modelos.AdjuntoMensaje> AdjuntoMensaje { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.Chat> Chat { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.Contacto> Contacto { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.DetalleGrupo> DetalleGrupo { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.EstadoReceptorMensaje> EstadoReceptorMensaje { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.Mensaje> Mensaje { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.ParticipanteChat> ParticipanteChat { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.ReaccionMensaje> ReaccionMensaje { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.RolParticipante> RolParticipante { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.TipoAdjunto> TipoAdjunto { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.TipoChat> TipoChat { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.TipoEstadoMensaje> TipoEstadoMensaje { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.TipoReaccion> TipoReaccion { get; set; } = default!;
    public DbSet<WebMensajeria.Modelos.Usuario> Usuario { get; set; } = default!;

}
