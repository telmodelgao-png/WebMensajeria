
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
using Microsoft.AspNetCore.Authorization;
[Authorize]
public class ChatsController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var chats = CRUD<Chat>.GetAll();
        return View(chats);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var chats = CRUD<Chat>.GetById(id);
        if (chats == null)
        {
            return NotFound();
        }
        else
        {
            return View(chats);
        }
    }

    // GET: ADJUNTOMENSAJES/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: ADJUNTOMENSAJES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Chat chats)
    {
        try
        {
            CRUD<Chat>.Create(chats);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(chats);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var chats = CRUD<Chat>.GetById(id);
        if (chats == null)
        {
            return NotFound();
        }
        else
        {
            return View(chats);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Chat chat)
    {
        try
        {
            CRUD<Chat>.Update(id, chat);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(chat);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var chat = CRUD<Chat>.GetById(id);
        if (chat == null)
        {
            return NotFound();
        }
        else
        {
            return View(chat);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, Chat chat)
    {
        try
        {
            CRUD<Chat>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
