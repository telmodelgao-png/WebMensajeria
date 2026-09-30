using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class TipoChatsController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public TipoChatsController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/TipoChat
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoChat>>> GetTipoChat()
    {
        return await _context.TipoChat.ToListAsync();
    }

    // GET: api/TipoChat/5
    [HttpGet("{idtipochat}")]
    public async Task<ActionResult<TipoChat>> GetTipoChat(int idtipochat)
    {
        var tipochat = await _context.TipoChat.FindAsync(idtipochat);

        if (tipochat == null)
        {
            return NotFound();
        }

        return tipochat;
    }

    // PUT: api/TipoChat/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idtipochat}")]
    public async Task<IActionResult> PutTipoChat(int? idtipochat, TipoChat tipochat)
    {
        if (idtipochat != tipochat.idTipoChat)
        {
            return BadRequest();
        }

        _context.Entry(tipochat).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TipoChatExists(idtipochat))
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

    // POST: api/TipoChat
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TipoChat>> PostTipoChat(TipoChat tipochat)
    {
        _context.TipoChat.Add(tipochat);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTipoChat", new { idtipochat = tipochat.idTipoChat }, tipochat);
    }

    // DELETE: api/TipoChat/5
    [HttpDelete("{idtipochat}")]
    public async Task<IActionResult> DeleteTipoChat(int? idtipochat)
    {
        var tipochat = await _context.TipoChat.FindAsync(idtipochat);
        if (tipochat == null)
        {
            return NotFound();
        }

        _context.TipoChat.Remove(tipochat);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TipoChatExists(int? idtipochat)
    {
        return _context.TipoChat.Any(e => e.idTipoChat == idtipochat);
    }
}
