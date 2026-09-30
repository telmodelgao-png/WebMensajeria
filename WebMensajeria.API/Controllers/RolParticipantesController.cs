using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class RolParticipantesController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public RolParticipantesController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/RolParticipante
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RolParticipante>>> GetRolParticipante()
    {
        return await _context.RolParticipante.ToListAsync();
    }

    // GET: api/RolParticipante/5
    [HttpGet("{idrol}")]
    public async Task<ActionResult<RolParticipante>> GetRolParticipante(int idrol)
    {
        var rolparticipante = await _context.RolParticipante.FindAsync(idrol);

        if (rolparticipante == null)
        {
            return NotFound();
        }

        return rolparticipante;
    }

    // PUT: api/RolParticipante/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idrol}")]
    public async Task<IActionResult> PutRolParticipante(int? idrol, RolParticipante rolparticipante)
    {
        if (idrol != rolparticipante.IdRol)
        {
            return BadRequest();
        }

        _context.Entry(rolparticipante).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RolParticipanteExists(idrol))
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

    // POST: api/RolParticipante
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<RolParticipante>> PostRolParticipante(RolParticipante rolparticipante)
    {
        _context.RolParticipante.Add(rolparticipante);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetRolParticipante", new { idrol = rolparticipante.IdRol }, rolparticipante);
    }

    // DELETE: api/RolParticipante/5
    [HttpDelete("{idrol}")]
    public async Task<IActionResult> DeleteRolParticipante(int? idrol)
    {
        var rolparticipante = await _context.RolParticipante.FindAsync(idrol);
        if (rolparticipante == null)
        {
            return NotFound();
        }

        _context.RolParticipante.Remove(rolparticipante);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool RolParticipanteExists(int? idrol)
    {
        return _context.RolParticipante.Any(e => e.IdRol == idrol);
    }
}
