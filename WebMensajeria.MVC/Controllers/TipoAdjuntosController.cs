
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
using System.Security.Cryptography;
public class TipoAdjuntosController : Controller
{

      
    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var tipoadjunto = CRUD<TipoAdjunto>.GetAll();
        return View(tipoadjunto);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var tipoadjunto = CRUD<TipoAdjunto>.GetById(id);
        if (tipoadjunto == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipoadjunto);
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
    public ActionResult Create(TipoAdjunto tipoadjunto)
    {
        try
        {
            CRUD<TipoAdjunto>.Create(tipoadjunto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(tipoadjunto);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var tipoadjunto = CRUD<TipoAdjunto>.GetById(id);
        if (tipoadjunto == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipoadjunto);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, TipoAdjunto tipoadjunto)
    {
        try
        {
            CRUD<TipoAdjunto>.Update(id, tipoadjunto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(tipoadjunto);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var tipoadjunto = CRUD<TipoAdjunto>.GetById(id);
        if (tipoadjunto == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipoadjunto);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, TipoAdjunto tipoadjunto)
    {
        try
        {
            CRUD<TipoAdjunto>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
