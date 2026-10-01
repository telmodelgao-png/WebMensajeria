
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
public class ContactosController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var contacto = CRUD<Contacto>.GetAll();
        return View(contacto);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var contacto = CRUD<Contacto>.GetById(id);
        if (contacto == null)
        {
            return NotFound();
        }
        else
        {
            return View(contacto);
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
    public ActionResult Create(Contacto contacto)
    {
        try
        {
            CRUD<Contacto>.Create(contacto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(contacto);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var contacto = CRUD<Contacto>.GetById(id);
        if (contacto == null)
        {
            return NotFound();
        }
        else
        {
            return View(contacto);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, Contacto contacto)
    {
        try
        {
            CRUD<Contacto>.Update(id, contacto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(contacto);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var contacto = CRUD<Contacto>.GetById(id);
        if (contacto == null)
        {
            return NotFound();
        }
        else
        {
            return View(contacto);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, Contacto contacto)
    {
        try
        {
            CRUD<Contacto>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
