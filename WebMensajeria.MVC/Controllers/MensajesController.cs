
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
public class MensajesController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var mensajes = CRUD<Mensaje>.GetAll();
        return View(mensajes);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var mensajes = CRUD<Mensaje>.GetById(id);
        if (mensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(mensajes);
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
    public ActionResult Create(Mensaje mensajes)
    {
        try
        {
            CRUD<Mensaje>.Create(mensajes);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(mensajes);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var mensajes = CRUD<Mensaje>.GetById(id);
        if (mensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(mensajes);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Mensaje mensajes)
    {
        try
        {
            CRUD<Mensaje>.Update(id, mensajes);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(mensajes);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var mensajes = CRUD<Mensaje>.GetById(id);
        if (mensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(mensajes);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, Mensaje mensajes)
    {
        try
        {
            CRUD<Mensaje>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
