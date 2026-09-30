using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class EstadoReceptorMensajesController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public EstadoReceptorMensajesController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/EstadoReceptorMensaje
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstadoReceptorMensaje>>> GetEstadoReceptorMensaje()
    {
        return await _context.EstadoReceptorMensaje.ToListAsync();
    }

    // GET: api/EstadoReceptorMensaje/5
    [HttpGet("{idestadoreceptor}")]
    public async Task<ActionResult<EstadoReceptorMensaje>> GetEstadoReceptorMensaje(int idestadoreceptor)
    {
        var estadoreceptormensaje = await _context.EstadoReceptorMensaje.FindAsync(idestadoreceptor);

        if (estadoreceptormensaje == null)
        {
            return NotFound();
        }

        return estadoreceptormensaje;
    }

    // PUT: api/EstadoReceptorMensaje/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idestadoreceptor}")]
    public async Task<IActionResult> PutEstadoReceptorMensaje(int? idestadoreceptor, EstadoReceptorMensaje estadoreceptormensaje)
    {
        if (idestadoreceptor != estadoreceptormensaje.idEstadoReceptor)
        {
            return BadRequest();
        }

        _context.Entry(estadoreceptormensaje).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EstadoReceptorMensajeExists(idestadoreceptor))
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

    // POST: api/EstadoReceptorMensaje
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<EstadoReceptorMensaje>> PostEstadoReceptorMensaje(EstadoReceptorMensaje estadoreceptormensaje)
    {
        _context.EstadoReceptorMensaje.Add(estadoreceptormensaje);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEstadoReceptorMensaje", new { idestadoreceptor = estadoreceptormensaje.idEstadoReceptor }, estadoreceptormensaje);
    }

    // DELETE: api/EstadoReceptorMensaje/5
    [HttpDelete("{idestadoreceptor}")]
    public async Task<IActionResult> DeleteEstadoReceptorMensaje(int? idestadoreceptor)
    {
        var estadoreceptormensaje = await _context.EstadoReceptorMensaje.FindAsync(idestadoreceptor);
        if (estadoreceptormensaje == null)
        {
            return NotFound();
        }

        _context.EstadoReceptorMensaje.Remove(estadoreceptormensaje);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EstadoReceptorMensajeExists(int? idestadoreceptor)
    {
        return _context.EstadoReceptorMensaje.Any(e => e.idEstadoReceptor == idestadoreceptor);
    }
}
