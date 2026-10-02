using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class MensajesController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public MensajesController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/Mensaje
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mensaje>>> GetMensaje()
    {
        return await _context.Mensaje.ToListAsync();
    }

    // GET: api/Mensaje/5
    [HttpGet("{idmensaje}")]
    public async Task<ActionResult<Mensaje>> GetMensaje(int idmensaje)
    {
        var mensaje = await _context.Mensaje.FindAsync(idmensaje);

        if (mensaje == null)
        {
            return NotFound();
        }

        return mensaje;
    }
    // GET: api/Mensajes/chat/5?idUsuario=1
    [HttpGet("chat/{idchat}")]
    public async Task<ActionResult> GetMensajesChat(
        int idchat,
        int idUsuario)
    {
        // Comprobar que el usuario pertenece al chat.
        var pertenece = await _context.ParticipanteChat
            .AnyAsync(p =>
                p.idChat == idchat &&
                p.idUsuario == idUsuario);

        if (!pertenece)
            return Forbid();

        var mensajes = await _context.Mensaje
            .Where(m => m.idChat == idchat)
            .OrderBy(m => m.fechaEnvio)
            .ThenBy(m => m.idMensaje)
            .Select(m => new
            {
                idMensaje = m.idMensaje,
                idChat = m.idChat,
                idUsuario = m.idUsuario,
                mensaje = m.mensaje,
                fechaEnvio = m.fechaEnvio
            })
            .ToListAsync();

        return Ok(mensajes);
    }
    // PUT: api/Mensaje/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idmensaje}")]
    public async Task<IActionResult> PutMensaje(int? idmensaje, Mensaje mensaje)
    {
        if (idmensaje != mensaje.idMensaje)
        {
            return BadRequest();
        }

        _context.Entry(mensaje).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MensajeExists(idmensaje))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Mensaje
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Mensaje>> PostMensaje(Mensaje mensaje)
    {
        _context.Mensaje.Add(mensaje);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMensaje", new { idmensaje = mensaje.idMensaje }, mensaje);
    }

    // DELETE: api/Mensaje/5
    [HttpDelete("{idmensaje}")]
    public async Task<IActionResult> DeleteMensaje(int? idmensaje)
    {
        var mensaje = await _context.Mensaje.FindAsync(idmensaje);
        if (mensaje == null)
        {
            return NotFound();
        }

        _context.Mensaje.Remove(mensaje);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MensajeExists(int? idmensaje)
    {
        return _context.Mensaje.Any(e => e.idMensaje == idmensaje);
    }
}
