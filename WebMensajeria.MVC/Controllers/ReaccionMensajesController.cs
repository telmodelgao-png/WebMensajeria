
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
public class ReaccionMensajesController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var reaccionmensajes = CRUD<ReaccionMensaje>.GetAll();
        return View(reaccionmensajes);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var reaccionmensajes = CRUD<ReaccionMensaje>.GetById(id);
        if (reaccionmensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(reaccionmensajes);
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
    public ActionResult Create(ReaccionMensaje reaccionmensajes)
    {
        try
        {
            CRUD<ReaccionMensaje>.Create(reaccionmensajes);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(reaccionmensajes);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var reaccionmensajes = CRUD<ReaccionMensaje>.GetById(id);
        if (reaccionmensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(reaccionmensajes);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, ReaccionMensaje reaccionmensajes)
    {
        try
        {
            CRUD<ReaccionMensaje>.Update(id, reaccionmensajes);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(reaccionmensajes);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var reaccionmensajes = CRUD<ReaccionMensaje>.GetById(id);
        if (reaccionmensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(reaccionmensajes);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, ReaccionMensaje reaccionmensajes)
    {
        try
        {
            CRUD<ReaccionMensaje>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
