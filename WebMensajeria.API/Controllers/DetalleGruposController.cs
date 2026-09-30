using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class DetalleGruposController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public DetalleGruposController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/DetalleGrupo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DetalleGrupo>>> GetDetalleGrupo()
    {
        return await _context.DetalleGrupo.ToListAsync();
    }

    // GET: api/DetalleGrupo/5
    [HttpGet("{iddetallegrupo}")]
    public async Task<ActionResult<DetalleGrupo>> GetDetalleGrupo(int iddetallegrupo)
    {
        var detallegrupo = await _context.DetalleGrupo.FindAsync(iddetallegrupo);

        if (detallegrupo == null)
        {
            return NotFound();
        }

        return detallegrupo;
    }

    // PUT: api/DetalleGrupo/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{iddetallegrupo}")]
    public async Task<IActionResult> PutDetalleGrupo(int? iddetallegrupo, DetalleGrupo detallegrupo)
    {
        if (iddetallegrupo != detallegrupo.idDetalleGrupo)
        {
            return BadRequest();
        }

        _context.Entry(detallegrupo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DetalleGrupoExists(iddetallegrupo))
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

    // POST: api/DetalleGrupo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<DetalleGrupo>> PostDetalleGrupo(DetalleGrupo detallegrupo)
    {
        _context.DetalleGrupo.Add(detallegrupo);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDetalleGrupo", new { iddetallegrupo = detallegrupo.idDetalleGrupo }, detallegrupo);
    }

    // DELETE: api/DetalleGrupo/5
    [HttpDelete("{iddetallegrupo}")]
    public async Task<IActionResult> DeleteDetalleGrupo(int? iddetallegrupo)
    {
        var detallegrupo = await _context.DetalleGrupo.FindAsync(iddetallegrupo);
        if (detallegrupo == null)
        {
            return NotFound();
        }

        _context.DetalleGrupo.Remove(detallegrupo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DetalleGrupoExists(int? iddetallegrupo)
    {
        return _context.DetalleGrupo.Any(e => e.idDetalleGrupo == iddetallegrupo);
    }
}
