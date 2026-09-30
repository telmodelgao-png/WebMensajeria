using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class TipoEstadoMensajesController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public TipoEstadoMensajesController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/TipoEstadoMensaje
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoEstadoMensaje>>> GetTipoEstadoMensaje()
    {
        return await _context.TipoEstadoMensaje.ToListAsync();
    }

    // GET: api/TipoEstadoMensaje/5
    [HttpGet("{idtipoestado}")]
    public async Task<ActionResult<TipoEstadoMensaje>> GetTipoEstadoMensaje(int idtipoestado)
    {
        var tipoestadomensaje = await _context.TipoEstadoMensaje.FindAsync(idtipoestado);

        if (tipoestadomensaje == null)
        {
            return NotFound();
        }

        return tipoestadomensaje;
    }

    // PUT: api/TipoEstadoMensaje/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idtipoestado}")]
    public async Task<IActionResult> PutTipoEstadoMensaje(int? idtipoestado, TipoEstadoMensaje tipoestadomensaje)
    {
        if (idtipoestado != tipoestadomensaje.idTipoEstado)
        {
            return BadRequest();
        }

        _context.Entry(tipoestadomensaje).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TipoEstadoMensajeExists(idtipoestado))
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

    // POST: api/TipoEstadoMensaje
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TipoEstadoMensaje>> PostTipoEstadoMensaje(TipoEstadoMensaje tipoestadomensaje)
    {
        _context.TipoEstadoMensaje.Add(tipoestadomensaje);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTipoEstadoMensaje", new { idtipoestado = tipoestadomensaje.idTipoEstado }, tipoestadomensaje);
    }

    // DELETE: api/TipoEstadoMensaje/5
    [HttpDelete("{idtipoestado}")]
    public async Task<IActionResult> DeleteTipoEstadoMensaje(int? idtipoestado)
    {
        var tipoestadomensaje = await _context.TipoEstadoMensaje.FindAsync(idtipoestado);
        if (tipoestadomensaje == null)
        {
            return NotFound();
        }

        _context.TipoEstadoMensaje.Remove(tipoestadomensaje);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TipoEstadoMensajeExists(int? idtipoestado)
    {
        return _context.TipoEstadoMensaje.Any(e => e.idTipoEstado == idtipoestado);
    }
}
