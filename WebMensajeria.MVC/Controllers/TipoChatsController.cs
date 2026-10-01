
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
using System.Security.Cryptography;
public class TipoChatsController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var tipochat = CRUD<TipoChat>.GetAll();
        return View(tipochat);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var tipochat = CRUD<TipoChat>.GetById(id);
        if (tipochat == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipochat);
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
    public ActionResult Create(TipoChat tipochat)
    {
        try
        {
            CRUD<TipoChat>.Create(tipochat);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(tipochat);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var tipochat = CRUD<TipoChat>.GetById(id);
        if (tipochat == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipochat);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, TipoChat tipochat)
    {
        try
        {
            CRUD<TipoChat>.Update(id, tipochat);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(tipochat);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var tipochat = CRUD<TipoChat>.GetById(id);
        if (tipochat == null)
        {
            return NotFound();
        }
        else
        {
            return View(tipochat);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, TipoChat tipochat)
    {
        try
        {
            CRUD<TipoChat>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
