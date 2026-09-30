using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class ContactosController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public ContactosController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/Contacto
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contacto>>> GetContacto()
    {
        return await _context.Contacto.ToListAsync();
    }

    // GET: api/Contacto/5
    [HttpGet("{idcontacto}")]
    public async Task<ActionResult<Contacto>> GetContacto(int idcontacto)
    {
        var contacto = await _context.Contacto.FindAsync(idcontacto);

        if (contacto == null)
        {
            return NotFound();
        }

        return contacto;
    }

    // PUT: api/Contacto/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idcontacto}")]
    public async Task<IActionResult> PutContacto(int? idcontacto, Contacto contacto)
    {
        if (idcontacto != contacto.idContacto)
        {
            return BadRequest();
        }

        _context.Entry(contacto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContactoExists(idcontacto))
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

    // POST: api/Contacto
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Contacto>> PostContacto(Contacto contacto)
    {
        _context.Contacto.Add(contacto);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetContacto", new { idcontacto = contacto.idContacto }, contacto);
    }

    // DELETE: api/Contacto/5
    [HttpDelete("{idcontacto}")]
    public async Task<IActionResult> DeleteContacto(int? idcontacto)
    {
        var contacto = await _context.Contacto.FindAsync(idcontacto);
        if (contacto == null)
        {
            return NotFound();
        }

        _context.Contacto.Remove(contacto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ContactoExists(int? idcontacto)
    {
        return _context.Contacto.Any(e => e.idContacto == idcontacto);
    }
}
