using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebMensajeria.Modelos;

[Route("api/[controller]")]
[ApiController]
public class ChatsController : ControllerBase
{
    private readonly WebMensajeriaAPIContext _context;
    public ChatsController(WebMensajeriaAPIContext context)
    {
        _context = context;
    }

    // GET: api/Chat
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Chat>>> GetChat()
    {
        return await _context.Chat.ToListAsync();
    }

    // GET: api/Chat/5
    [HttpGet("{idchat}")]
    public async Task<ActionResult<Chat>> GetChat(int idchat)
    {
        var chat = await _context.Chat.FindAsync(idchat);

        if (chat == null)
        {
            return NotFound();
        }

        return chat;
    }

    // PUT: api/Chat/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{idchat}")]
    public async Task<IActionResult> PutChat(int? idchat, Chat chat)
    {
        if (idchat != chat.idChat)
        {
            return BadRequest();
        }

        _context.Entry(chat).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ChatExists(idchat))
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

    // POST: api/Chat
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Chat>> PostChat(Chat chat)
    {
        _context.Chat.Add(chat);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetChat", new { idchat = chat.idChat }, chat);
    }

    // DELETE: api/Chat/5
    [HttpDelete("{idchat}")]
    public async Task<IActionResult> DeleteChat(int? idchat)
    {
        var chat = await _context.Chat.FindAsync(idchat);
        if (chat == null)
        {
            return NotFound();
        }

        _context.Chat.Remove(chat);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ChatExists(int? idchat)
    {
        return _context.Chat.Any(e => e.idChat == idchat);
    }
}
