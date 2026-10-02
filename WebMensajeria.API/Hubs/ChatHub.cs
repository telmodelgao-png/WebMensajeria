
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

namespace WebMensajeria.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly WebMensajeriaAPIContext _context;

    public ChatHub(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    private int ObtenerUsuarioId()
    {
        var claimId = Context.User?.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? Context.User?.FindFirst("sub")?.Value;

        if (!int.TryParse(claimId, out var usuarioId))
        {
            throw new HubException(
                "No se pudo identificar al usuario autenticado.");
        }

        return usuarioId;
    }

    public async Task<object> EnviarMensajePrivado(
        int destinatarioId,
        string contenido)
    {
        var remitenteId = ObtenerUsuarioId();
        contenido = contenido?.Trim() ?? "";

        if (destinatarioId <= 0 || destinatarioId == remitenteId)
            throw new HubException("Destinatario no válido.");

        if (string.IsNullOrWhiteSpace(contenido) ||
            contenido.Length > 4000)
            throw new HubException("El mensaje no es válido.");

        if (!await _context.Usuario.AnyAsync(
            u => u.IdUsuario == destinatarioId))
            throw new HubException("El destinatario no existe.");

        const int tipoPrivado = 1;
        const int rolMiembro = 2;

        if (!await _context.TipoChat.AnyAsync(
            t => t.idTipoChat == tipoPrivado))
            throw new HubException(
                "No existe el tipo de chat privado.");

        // Buscar una conversación privada entre ambos usuarios.
        var chatsPosibles = await _context.Chat
            .Where(c => c.idTipoChat == tipoPrivado)
            .Where(c => c.participantesChats.Any(
                p => p.idUsuario == remitenteId))
            .Where(c => c.participantesChats.Any(
                p => p.idUsuario == destinatarioId))
            .Where(c => c.participantesChats.Count == 2)
            .Select(c => c.idChat)
            .ToListAsync();

        var chatId = chatsPosibles.FirstOrDefault();

        // Crear la conversación si todavía no existe.
        if (chatId == 0)
        {
            if (!await _context.RolParticipante.AnyAsync(
                r => r.IdRol == rolMiembro))
                throw new HubException(
                    "No existe el rol de miembro.");

            await using var transaccion =
                await _context.Database.BeginTransactionAsync();

            var chat = new Chat
            {
                idTipoChat = tipoPrivado
            };

            _context.Chat.Add(chat);
            await _context.SaveChangesAsync();

            _context.ParticipanteChat.AddRange(
                new ParticipanteChat
                {
                    idChat = chat.idChat,
                    idUsuario = remitenteId,
                    idRol = rolMiembro
                },
                new ParticipanteChat
                {
                    idChat = chat.idChat,
                    idUsuario = destinatarioId,
                    idRol = rolMiembro
                });

            await _context.SaveChangesAsync();
            await transaccion.CommitAsync();

            chatId = chat.idChat;
        }

        // Guardar el mensaje.
        var mensaje = new Mensaje
        {
            idChat = chatId,
            idUsuario = remitenteId,
            mensaje = contenido,
            fechaEnvio = DateTime.UtcNow
        };

        _context.Mensaje.Add(mensaje);
        await _context.SaveChangesAsync();

        var datos = new
        {
            idMensaje = mensaje.idMensaje,
            idChat = mensaje.idChat,
            idUsuario = mensaje.idUsuario,
            mensaje = mensaje.mensaje,
            fechaEnvio = mensaje.fechaEnvio
        };

        // Notificar a las conexiones de ambos usuarios.
        await Clients.Users(
            remitenteId.ToString(),
            destinatarioId.ToString()
        ).SendAsync("MensajeRecibido", datos);

        // SignalR devuelve este objeto a quien envió el mensaje.
        return datos;
    }
}