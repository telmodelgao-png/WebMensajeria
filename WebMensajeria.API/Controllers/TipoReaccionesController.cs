using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class TipoReaccionesController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public TipoReaccionesController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/TipoReaccion
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoReaccion>>> GetTipoReaccion()
    {
        return await _context.TipoReaccion.ToListAsync();
    }

    // GET: api/TipoReaccion/5
    [HttpGet("{idtiporeaccion}")]
    public async Task<ActionResult<TipoReaccion>> GetTipoReaccion(int idtiporeaccion)
    {
        var tiporeaccion = await _context.TipoReaccion.FindAsync(idtiporeaccion);

        if (tiporeaccion == null)
        {
            return NotFound();
        }

        return tiporeaccion;
    }

    // PUT: api/TipoReaccion/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idtiporeaccion}")]
    public async Task<IActionResult> PutTipoReaccion(int? idtiporeaccion, TipoReaccion tiporeaccion)
    {
        if (idtiporeaccion != tiporeaccion.idTipoReaccion)
        {
            return BadRequest();
        }

        _context.Entry(tiporeaccion).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TipoReaccionExists(idtiporeaccion))
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

    // POST: api/TipoReaccion
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TipoReaccion>> PostTipoReaccion(TipoReaccion tiporeaccion)
    {
        _context.TipoReaccion.Add(tiporeaccion);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTipoReaccion", new { idtiporeaccion = tiporeaccion.idTipoReaccion }, tiporeaccion);
    }

    // DELETE: api/TipoReaccion/5
    [HttpDelete("{idtiporeaccion}")]
    public async Task<IActionResult> DeleteTipoReaccion(int? idtiporeaccion)
    {
        var tiporeaccion = await _context.TipoReaccion.FindAsync(idtiporeaccion);
        if (tiporeaccion == null)
        {
            return NotFound();
        }

        _context.TipoReaccion.Remove(tiporeaccion);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TipoReaccionExists(int? idtiporeaccion)
    {
        return _context.TipoReaccion.Any(e => e.idTipoReaccion == idtiporeaccion);
    }
}
