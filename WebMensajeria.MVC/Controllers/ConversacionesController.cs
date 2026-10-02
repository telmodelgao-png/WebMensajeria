using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class ConversacionesController : ControllerBase
{
    // Nombres que se buscan (o se crean si no existen) en las tablas de catálogo.
    // Si en tu BD ya tienes estos registros con otro nombre, cámbialos aquí.
    private const string TipoPrivado = "Privado";
    private const string RolMiembro = "Miembro";
    private const string EstadoEnviado = "Enviado";
    private const string EstadoLeido = "Leído";

    private const int MaxTexto = 2000; // el mismo límite que el textarea del front

    private readonly WebMensajeriaAPIContext _context;

    public ConversacionesController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    public class EnviarDto
    {
        public int IdUsuario { get; set; }
        public int? IdChat { get; set; }
        public int? IdDestinatario { get; set; }
        public string? Texto { get; set; }
    }

    // GET: api/Conversaciones?idUsuario=1
    // Lista del panel izquierdo: un chat privado por persona, el más reciente primero.
    [HttpGet]
    public async Task<ActionResult> Lista(int idUsuario)
    {
        var tipo = TipoPrivado.ToLower();
        var idLeido = await IdTipoEstado(EstadoLeido);

        var chats = await _context.Chat
            .Where(c => c.tipoChat!.nombreTipoChat.ToLower() == tipo
                     && c.participantesChats.Any(p => p.idUsuario == idUsuario))
            .Select(c => new
            {
                c.idChat,
                nombre = c.participantesChats
                    .Where(p => p.idUsuario != idUsuario)
                    .Select(p => p.usuarios!.nombreUsuario)
                    .FirstOrDefault(),
                ultimoMensaje = c.mensajes
                    .OrderByDescending(m => m.fechaEnvio)
                    .ThenByDescending(m => m.idMensaje)
                    .Select(m => m.mensaje)
                    .FirstOrDefault(),
                fecha = c.mensajes.Max(m => (DateTime?)m.fechaEnvio),
                noLeidos = c.mensajes.Count(m => m.estadoReceptorMensajes
                    .Any(e => e.idUsuario == idUsuario && e.tipoEstado != idLeido))
            })
            .ToListAsync();

        return Ok(chats.OrderByDescending(c => c.fecha));
    }

    // GET: api/Conversaciones/privado?idUsuario=1&idOtro=5
    // 200 { idChat } si ya existe el chat entre las dos personas, 404 si todavía no.
    [HttpGet("privado")]
    public async Task<ActionResult> Privado(int idUsuario, int idOtro)
    {
        var idChat = await BuscarChatPrivado(idUsuario, idOtro);
        if (idChat == null) return NotFound();
        return Ok(new { idChat });
    }

    // GET: api/Conversaciones/mensajes/7?idUsuario=1
    // Devuelve el historial y, de paso, marca como leídos los mensajes que esa persona tenía pendientes.
    [HttpGet("mensajes/{idChat}")]
    public async Task<ActionResult> Mensajes(int idChat, int idUsuario)
    {
        if (!await EsParticipante(idChat, idUsuario))
            return StatusCode(StatusCodes.Status403Forbidden);

        var idLeido = await IdTipoEstado(EstadoLeido);
        var pendientes = await _context.EstadoReceptorMensaje
            .Where(e => e.idUsuario == idUsuario
                     && e.tipoEstado != idLeido
                     && e.mensajes!.idChat == idChat)
            .ToListAsync();
        foreach (var pendiente in pendientes) pendiente.tipoEstado = idLeido;
        if (pendientes.Count > 0) await _context.SaveChangesAsync();

        var filas = await _context.Mensaje
            .Where(m => m.idChat == idChat)
            .OrderBy(m => m.fechaEnvio).ThenBy(m => m.idMensaje)
            .Select(m => new
            {
                m.idMensaje,
                m.idChat,
                m.idUsuario,
                m.mensaje,
                m.fechaEnvio,
                // estado que tiene el mensaje para el otro (solo se muestra en los mensajes propios)
                estadoNombre = m.estadoReceptorMensajes
                    .Where(e => e.idUsuario != m.idUsuario)
                    .Select(e => e.tipoEstadoMensaje!.tipoEstado)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(filas.Select(f => new
        {
            f.idMensaje,
            f.idChat,
            f.idUsuario,
            f.mensaje,
            f.fechaEnvio,
            estado = f.idUsuario == idUsuario ? Clave(f.estadoNombre ?? EstadoEnviado) : null
        }));
    }

    // POST: api/Conversaciones/enviar
    //   Con idChat    -> mensaje en un chat que ya existe.
    //   Sin idChat    -> chat privado con idDestinatario: si ya existía se reutiliza,
    //                    y si no, se crea (chat + participantes + mensaje) en una sola operación.
    [HttpPost("enviar")]
    public async Task<ActionResult> Enviar([FromBody] EnviarDto dto)
    {
        var texto = dto.Texto?.Trim();
        if (string.IsNullOrEmpty(texto)) return BadRequest("El mensaje está vacío.");
        if (texto.Length > MaxTexto) return BadRequest($"El mensaje supera los {MaxTexto} caracteres.");

        int? idChat = dto.IdChat;
        int idOtro = 0;

        if (idChat != null)
        {
            // No se puede escribir en un chat del que no se es participante.
            if (!await EsParticipante(idChat.Value, dto.IdUsuario))
                return StatusCode(StatusCodes.Status403Forbidden);
        }
        else
        {
            if (dto.IdDestinatario == null) return BadRequest("Falta el destinatario.");
            idOtro = dto.IdDestinatario.Value;
            if (idOtro == dto.IdUsuario) return BadRequest("No puedes escribirte a ti mismo.");
            if (!await _context.Usuario.AnyAsync(u => u.IdUsuario == idOtro))
                return NotFound("El destinatario no existe.");

            // Puede que el chat ya exista aunque el navegador no lo sepa
            // (por ejemplo, la otra persona lo acaba de crear).
            idChat = await BuscarChatPrivado(dto.IdUsuario, idOtro);
        }

        // Los registros de catálogo se resuelven ANTES de añadir nada al contexto:
        // si hay que crearlos, ese SaveChanges no debe arrastrar un chat a medias.
        var idEnviado = await IdTipoEstado(EstadoEnviado);

        var msg = new Mensaje
        {
            idUsuario = dto.IdUsuario,
            mensaje = texto,
            fechaEnvio = DateTime.UtcNow
        };

        if (idChat != null)
        {
            msg.idChat = idChat.Value;

            var receptores = await _context.ParticipanteChat
                .Where(p => p.idChat == idChat.Value && p.idUsuario != dto.IdUsuario)
                .Select(p => p.idUsuario)
                .ToListAsync();
            foreach (var receptor in receptores)
                msg.estadoReceptorMensajes.Add(new EstadoReceptorMensaje { idUsuario = receptor, tipoEstado = idEnviado });

            _context.Mensaje.Add(msg);
        }
        else
        {
            var idTipoChat = await IdTipoChat();
            var idRol = await IdRol();

            msg.estadoReceptorMensajes.Add(new EstadoReceptorMensaje { idUsuario = idOtro, tipoEstado = idEnviado });

            var chat = new Chat
            {
                idTipoChat = idTipoChat,
                participantesChats =
                {
                    new ParticipanteChat { idUsuario = dto.IdUsuario, idRol = idRol },
                    new ParticipanteChat { idUsuario = idOtro, idRol = idRol }
                },
                mensajes = { msg }
            };
            _context.Chat.Add(chat);
        }

        // Un único SaveChanges = una sola transacción: o se crea todo o no se crea nada.
        await _context.SaveChangesAsync();

        return Ok(new
        {
            msg.idMensaje,
            msg.idChat,
            msg.idUsuario,
            msg.mensaje,
            msg.fechaEnvio,
            estado = Clave(EstadoEnviado)
        });
    }

    /* ---------- auxiliares ---------- */

    private Task<bool> EsParticipante(int idChat, int idUsuario) =>
        _context.ParticipanteChat.AnyAsync(p => p.idChat == idChat && p.idUsuario == idUsuario);

    private async Task<int?> BuscarChatPrivado(int idA, int idB)
    {
        var tipo = TipoPrivado.ToLower();
        return await _context.Chat
            .Where(c => c.tipoChat!.nombreTipoChat.ToLower() == tipo
                     && c.participantesChats.Any(p => p.idUsuario == idA)
                     && c.participantesChats.Any(p => p.idUsuario == idB))
            .Select(c => (int?)c.idChat)
            .FirstOrDefaultAsync();
    }

    // "Leído" -> "leido": la clave que usa el front (chat.js) para pintar el estado.
    private static string Clave(string texto)
    {
        var sinTildes = texto.Normalize(NormalizationForm.FormD)
            .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
            .ToArray();
        return new string(sinTildes).ToLowerInvariant();
    }

    // Buscan el registro de catálogo por nombre y, si no existe, lo crean.
    private async Task<int> IdTipoChat()
    {
        var nombre = TipoPrivado.ToLower();
        var tipo = await _context.TipoChat.FirstOrDefaultAsync(t => t.nombreTipoChat.ToLower() == nombre);
        if (tipo == null)
        {
            tipo = new TipoChat { nombreTipoChat = TipoPrivado };
            _context.TipoChat.Add(tipo);
            await _context.SaveChangesAsync();
        }
        return tipo.idTipoChat;
    }

    private async Task<int> IdRol()
    {
        var nombre = RolMiembro.ToLower();
        var rol = await _context.RolParticipante.FirstOrDefaultAsync(r => r.nombreRol.ToLower() == nombre);
        if (rol == null)
        {
            rol = new RolParticipante { nombreRol = RolMiembro };
            _context.RolParticipante.Add(rol);
            await _context.SaveChangesAsync();
        }
        return rol.IdRol;
    }

    private async Task<int> IdTipoEstado(string nombreEstado)
    {
        var nombre = nombreEstado.ToLower();
        var estado = await _context.TipoEstadoMensaje.FirstOrDefaultAsync(t => t.tipoEstado.ToLower() == nombre);
        if (estado == null)
        {
            estado = new TipoEstadoMensaje { tipoEstado = nombreEstado };
            _context.TipoEstadoMensaje.Add(estado);
            await _context.SaveChangesAsync();
        }
        return estado.idTipoEstado;
    }
}
