using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class ReaccionMensajesController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public ReaccionMensajesController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/ReaccionMensaje
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReaccionMensaje>>> GetReaccionMensaje()
    {
        return await _context.ReaccionMensaje.ToListAsync();
    }

    // GET: api/ReaccionMensaje/5
    [HttpGet("{idreaccion}")]
    public async Task<ActionResult<ReaccionMensaje>> GetReaccionMensaje(int idreaccion)
    {
        var reaccionmensaje = await _context.ReaccionMensaje.FindAsync(idreaccion);

        if (reaccionmensaje == null)
        {
            return NotFound();
        }

        return reaccionmensaje;
    }

    // PUT: api/ReaccionMensaje/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idreaccion}")]
    public async Task<IActionResult> PutReaccionMensaje(int? idreaccion, ReaccionMensaje reaccionmensaje)
    {
        if (idreaccion != reaccionmensaje.idReaccion)
        {
            return BadRequest();
        }

        _context.Entry(reaccionmensaje).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ReaccionMensajeExists(idreaccion))
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

    // POST: api/ReaccionMensaje
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ReaccionMensaje>> PostReaccionMensaje(ReaccionMensaje reaccionmensaje)
    {
        _context.ReaccionMensaje.Add(reaccionmensaje);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetReaccionMensaje", new { idreaccion = reaccionmensaje.idReaccion }, reaccionmensaje);
    }

    // DELETE: api/ReaccionMensaje/5
    [HttpDelete("{idreaccion}")]
    public async Task<IActionResult> DeleteReaccionMensaje(int? idreaccion)
    {
        var reaccionmensaje = await _context.ReaccionMensaje.FindAsync(idreaccion);
        if (reaccionmensaje == null)
        {
            return NotFound();
        }

        _context.ReaccionMensaje.Remove(reaccionmensaje);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReaccionMensajeExists(int? idreaccion)
    {
        return _context.ReaccionMensaje.Any(e => e.idReaccion == idreaccion);
    }
}
