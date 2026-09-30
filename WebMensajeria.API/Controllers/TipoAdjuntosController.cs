using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class TipoAdjuntosController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public TipoAdjuntosController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/TipoAdjunto
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoAdjunto>>> GetTipoAdjunto()
    {
        return await _context.TipoAdjunto.ToListAsync();
    }

    // GET: api/TipoAdjunto/5
    [HttpGet("{idtipoadjunto}")]
    public async Task<ActionResult<TipoAdjunto>> GetTipoAdjunto(int idtipoadjunto)
    {
        var tipoadjunto = await _context.TipoAdjunto.FindAsync(idtipoadjunto);

        if (tipoadjunto == null)
        {
            return NotFound();
        }

        return tipoadjunto;
    }

    // PUT: api/TipoAdjunto/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idtipoadjunto}")]
    public async Task<IActionResult> PutTipoAdjunto(int? idtipoadjunto, TipoAdjunto tipoadjunto)
    {
        if (idtipoadjunto != tipoadjunto.idTipoAdjunto)
        {
            return BadRequest();
        }

        _context.Entry(tipoadjunto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TipoAdjuntoExists(idtipoadjunto))
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

    // POST: api/TipoAdjunto
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TipoAdjunto>> PostTipoAdjunto(TipoAdjunto tipoadjunto)
    {
        _context.TipoAdjunto.Add(tipoadjunto);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTipoAdjunto", new { idtipoadjunto = tipoadjunto.idTipoAdjunto }, tipoadjunto);
    }

    // DELETE: api/TipoAdjunto/5
    [HttpDelete("{idtipoadjunto}")]
    public async Task<IActionResult> DeleteTipoAdjunto(int? idtipoadjunto)
    {
        var tipoadjunto = await _context.TipoAdjunto.FindAsync(idtipoadjunto);
        if (tipoadjunto == null)
        {
            return NotFound();
        }

        _context.TipoAdjunto.Remove(tipoadjunto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TipoAdjuntoExists(int? idtipoadjunto)
    {
        return _context.TipoAdjunto.Any(e => e.idTipoAdjunto == idtipoadjunto);
    }
}
