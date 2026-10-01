
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
[Authorize]
public class TipoReaccionesController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var tiporeaccion = CRUD<TipoReaccion>.GetAll();
        return View(tiporeaccion);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var tiporeaccion = CRUD<TipoReaccion>.GetById(id);
        if (tiporeaccion == null)
        {
            return NotFound();
        }
        else
        {
            return View(tiporeaccion);
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
    public ActionResult Create(TipoReaccion tiporeaccion)
    {
        try
        {
            CRUD<TipoReaccion>.Create(tiporeaccion);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(tiporeaccion);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var tiporeaccion = CRUD<TipoReaccion>.GetById(id);
        if (tiporeaccion == null)
        {
            return NotFound();
        }
        else
        {
            return View(tiporeaccion);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, TipoReaccion tiporeaccion)
    {
        try
        {
            CRUD<TipoReaccion>.Update(id, tiporeaccion);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(tiporeaccion);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var tiporeaccion = CRUD<TipoReaccion>.GetById(id);
        if (tiporeaccion == null)
        {
            return NotFound();
        }
        else
        {
            return View(tiporeaccion);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, TipoReaccion tiporeaccion)
    {
        try
        {
            CRUD<TipoReaccion>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
