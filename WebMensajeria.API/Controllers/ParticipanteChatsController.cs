using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class ParticipanteChatsController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public ParticipanteChatsController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/ParticipanteChat
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParticipanteChat>>> GetParticipanteChat()
    {
        return await _context.ParticipanteChat.ToListAsync();
    }

    // GET: api/ParticipanteChat/5
    [HttpGet("{idparticipante}")]
    public async Task<ActionResult<ParticipanteChat>> GetParticipanteChat(int idparticipante)
    {
        var participantechat = await _context.ParticipanteChat.FindAsync(idparticipante);

        if (participantechat == null)
        {
            return NotFound();
        }

        return participantechat;
    }

    // PUT: api/ParticipanteChat/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idparticipante}")]
    public async Task<IActionResult> PutParticipanteChat(int? idparticipante, ParticipanteChat participantechat)
    {
        if (idparticipante != participantechat.idParticipante)
        {
            return BadRequest();
        }

        _context.Entry(participantechat).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ParticipanteChatExists(idparticipante))
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

    // POST: api/ParticipanteChat
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ParticipanteChat>> PostParticipanteChat(ParticipanteChat participantechat)
    {
        _context.ParticipanteChat.Add(participantechat);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetParticipanteChat", new { idparticipante = participantechat.idParticipante }, participantechat);
    }

    // DELETE: api/ParticipanteChat/5
    [HttpDelete("{idparticipante}")]
    public async Task<IActionResult> DeleteParticipanteChat(int? idparticipante)
    {
        var participantechat = await _context.ParticipanteChat.FindAsync(idparticipante);
        if (participantechat == null)
        {
            return NotFound();
        }

        _context.ParticipanteChat.Remove(participantechat);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ParticipanteChatExists(int? idparticipante)
    {
        return _context.ParticipanteChat.Any(e => e.idParticipante == idparticipante);
    }
}
