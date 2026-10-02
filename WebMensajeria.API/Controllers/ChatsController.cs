using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;

    public ChatsController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // =========================================================
    // LISTAR LAS CONVERSACIONES DE UN USUARIO
    // GET: api/Chats/usuario/5
    // =========================================================
    [HttpGet("usuario/{idUsuario}")]
    public async Task<ActionResult> Conversaciones(int idUsuario)
    {
        const int tipoPrivado = 1;

        var conversaciones = await _context.Chat
            .Where(c =>
                c.idTipoChat == tipoPrivado &&
                c.participantesChats.Any(p => p.idUsuario == idUsuario))
            .Select(c => new
            {
                idChat = c.idChat,

                idOtro = c.participantesChats
                    .Where(p => p.idUsuario != idUsuario)
                    .Select(p => p.idUsuario)
                    .FirstOrDefault(),

                nombre = c.participantesChats
                    .Where(p => p.idUsuario != idUsuario)
                    .Select(p => p.usuarios!.nombreUsuario)
                    .FirstOrDefault(),

                ultimoMensaje = c.mensajes
                    .OrderByDescending(m => m.fechaEnvio)
                    .ThenByDescending(m => m.idMensaje)
                    .Select(m => m.mensaje)
                    .FirstOrDefault(),

                fecha = c.mensajes
                    .Max(m => (DateTime?)m.fechaEnvio)
            })
            .OrderByDescending(c => c.fecha)
            .ToListAsync();

        return Ok(conversaciones);
    }


    // =========================================================
    // BUSCAR CHAT PRIVADO ENTRE DOS USUARIOS
    // GET: api/Chats/privado?idUsuario=1&idOtro=2
    // =========================================================
    [HttpGet("privado")]
    public async Task<ActionResult> Privado(
        int idUsuario,
        int idOtro)
    {
        const int tipoPrivado = 1;

        var chat = await _context.Chat
            .Where(c =>
                c.idTipoChat == tipoPrivado &&
                c.participantesChats.Any(
                    p => p.idUsuario == idUsuario) &&
                c.participantesChats.Any(
                    p => p.idUsuario == idOtro) &&
                c.participantesChats.Count == 2)
            .Select(c => new
            {
                idChat = c.idChat
            })
            .FirstOrDefaultAsync();

        if (chat == null)
            return NotFound();

        return Ok(chat);
    }


    // =========================================================
    // CRUD ORIGINAL
    // =========================================================

    // GET: api/Chats
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Chat>>> GetChat()
    {
        return await _context.Chat.ToListAsync();
    }


    // GET: api/Chats/5
    [HttpGet("{idchat}")]
    public async Task<ActionResult<Chat>> GetChat(int idchat)
    {
        var chat = await _context.Chat.FindAsync(idchat);

        if (chat == null)
            return NotFound();

        return chat;
    }


    // PUT: api/Chats/5
    [HttpPut("{idchat}")]
    public async Task<IActionResult> PutChat(
        int? idchat,
        Chat chat)
    {
        if (idchat != chat.idChat)
            return BadRequest();

        _context.Entry(chat).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ChatExists(idchat))
                return NotFound();

            throw;
        }

        return NoContent();
    }


    // POST: api/Chats
    [HttpPost]
    public async Task<ActionResult<Chat>> PostChat(Chat chat)
    {
        _context.Chat.Add(chat);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            "GetChat",
            new { idchat = chat.idChat },
            chat);
    }


    // DELETE: api/Chats/5
    [HttpDelete("{idchat}")]
    public async Task<IActionResult> DeleteChat(int? idchat)
    {
        var chat = await _context.Chat.FindAsync(idchat);

        if (chat == null)
            return NotFound();

        _context.Chat.Remove(chat);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    private bool ChatExists(int? idchat)
    {
        return _context.Chat.Any(
            e => e.idChat == idchat);
    }
}