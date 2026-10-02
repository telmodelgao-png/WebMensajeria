using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;
using BCrypt.Net;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public UsuariosController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/Usuario
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
    {
        return await _context.Usuario.ToListAsync();
    }
    [HttpGet("buscar")]
    public async Task<ActionResult> Buscar(string texto, int idActual) 
    {
        if (string.IsNullOrEmpty(texto) || texto.Trim().Length < 2)
            return Ok(new List<object>());

        var t = texto.Trim().ToLower();

        var resultado = await _context.Usuario.
            Where(u=>u.IdUsuario !=idActual && u.nombreUsuario.ToLower().Contains(t)).
            OrderBy(u=>u.nombreUsuario).
            Select(u=> new { u.IdUsuario,u.nombreUsuario}).
            Take(20).ToListAsync();
        return Ok(resultado);
            
    }
    [HttpGet("{idusuario}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int idusuario)
    {
        var usuario = await _context.Usuario.FindAsync(idusuario);

        if (usuario == null)
        {
            return NotFound();
        }

        return usuario;
    }

    // PUT: api/Usuario/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idusuario}")]
    public async Task<IActionResult> PutUsuario(int? idusuario, Usuario usuario)
    {
        if (idusuario != usuario.IdUsuario)
        {
            return BadRequest();
        }
        var usuarioExiste = await _context.Usuario.FindAsync(idusuario);
        if (usuarioExiste == null)
        {
            return NotFound();
        }
        usuarioExiste.nombreUsuario = usuario.nombreUsuario;
        usuarioExiste.correoelectronico=usuario.correoelectronico;
        if (!string.IsNullOrEmpty(usuario.password))
        {
            usuarioExiste.password = BCrypt.Net.BCrypt.HashPassword(usuario.password);
        }
        await _context.SaveChangesAsync();
        return NoContent();
    
        }

    // POST: api/Usuario
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
       var existeUsuario = await _context.Usuario.AnyAsync(u=>u.correoelectronico.ToLower()
       == usuario.correoelectronico.ToLower());
        if (existeUsuario)
        {
            return Conflict("Ya se registro ese corre");
        }
        usuario.password=BCrypt.Net.BCrypt.HashPassword(usuario.password);
        _context.Usuario.Add(usuario);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsuario), new { idusuario = usuario.IdUsuario }, usuario);
    }

    // DELETE: api/Usuario/5
    [HttpDelete("{idusuario}")]
    public async Task<IActionResult> DeleteUsuario(int? idusuario)
    {
        var usuario = await _context.Usuario.FindAsync(idusuario);
        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuario.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UsuarioExists(int? idusuario)
    {
        return _context.Usuario.Any(e => e.IdUsuario == idusuario);
    }
}
