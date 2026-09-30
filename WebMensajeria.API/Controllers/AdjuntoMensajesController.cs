using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class AdjuntoMensajesController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public AdjuntoMensajesController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/AdjuntoMensaje
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdjuntoMensaje>>> GetAdjuntoMensaje()
    {
        return await _context.AdjuntoMensaje.ToListAsync();
    }

    // GET: api/AdjuntoMensaje/5
    [HttpGet("{idadjunto}")]
    public async Task<ActionResult<AdjuntoMensaje>> GetAdjuntoMensaje(int idadjunto)
    {
        var adjuntomensaje = await _context.AdjuntoMensaje.FindAsync(idadjunto);

        if (adjuntomensaje == null)
        {
            return NotFound();
        }

        return adjuntomensaje;
    }

    // PUT: api/AdjuntoMensaje/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idadjunto}")]
    public async Task<IActionResult> PutAdjuntoMensaje(int? idadjunto, AdjuntoMensaje adjuntomensaje)
    {
        if (idadjunto != adjuntomensaje.idAdjunto)
        {
            return BadRequest();
        }

        _context.Entry(adjuntomensaje).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AdjuntoMensajeExists(idadjunto))
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

    // POST: api/AdjuntoMensaje
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<AdjuntoMensaje>> PostAdjuntoMensaje(AdjuntoMensaje adjuntomensaje)
    {
        _context.AdjuntoMensaje.Add(adjuntomensaje);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAdjuntoMensaje", new { idadjunto = adjuntomensaje.idAdjunto }, adjuntomensaje);
    }

    // DELETE: api/AdjuntoMensaje/5
    [HttpDelete("{idadjunto}")]
    public async Task<IActionResult> DeleteAdjuntoMensaje(int? idadjunto)
    {
        var adjuntomensaje = await _context.AdjuntoMensaje.FindAsync(idadjunto);
        if (adjuntomensaje == null)
        {
            return NotFound();
        }

        _context.AdjuntoMensaje.Remove(adjuntomensaje);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AdjuntoMensajeExists(int? idadjunto)
    {
        return _context.AdjuntoMensaje.Any(e => e.idAdjunto == idadjunto);
    }
}
