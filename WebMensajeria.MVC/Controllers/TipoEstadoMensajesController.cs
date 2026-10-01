
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
[Authorize]
public class TipoEstadoMensajesController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var tipoestadomensajes = CRUD<TipoEstadoMensaje>.GetAll();
        return View(tipoestadomensajes);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var tipoestadomensajes = CRUD<TipoEstadoMensaje>.GetById(id);
        if (tipoestadomensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipoestadomensajes);
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
    public ActionResult Create(TipoEstadoMensaje tipoestadomensajes)
    {
        try
        {
            CRUD<TipoEstadoMensaje>.Create(tipoestadomensajes);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(tipoestadomensajes);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var tipoestadomensajes = CRUD<TipoEstadoMensaje>.GetById(id);
        if (tipoestadomensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipoestadomensajes);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, TipoEstadoMensaje tipoestadomensajes)
    {
        try
        {
            CRUD<TipoEstadoMensaje>.Update(id, tipoestadomensajes);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(tipoestadomensajes);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var tipoestadomensajes = CRUD<TipoEstadoMensaje>.GetById(id);
        if (tipoestadomensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipoestadomensajes);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, TipoEstadoMensaje tipoestadomensajes)
    {
        try
        {
            CRUD<TipoEstadoMensaje>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
