
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
public class AdjuntoMensajesController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()    
    {
        var adjuntomensajes = CRUD<AdjuntoMensaje>.GetAll();
        return View(adjuntomensajes);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var adjuntomensajes=CRUD<AdjuntoMensaje>.GetById(id);
        if (adjuntomensajes == null)
        {
            return NotFound();
        }
        else
        {
            return View(adjuntomensajes);
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
    public ActionResult Create( AdjuntoMensaje adjuntomensaje)
    {
        try
        {
            CRUD<AdjuntoMensaje>.Create(adjuntomensaje);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) 
        {
            ModelState.AddModelError("",ex.Message);
            return View(adjuntomensaje);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var adjuntomensajes = CRUD<AdjuntoMensaje>.GetById(id);
        if (adjuntomensajes == null)
        {
            return NotFound();
        }
        else {
            return View(adjuntomensajes);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, AdjuntoMensaje adjuntomensaje)
    {
        try 
        {
            CRUD<AdjuntoMensaje>.Update(id,adjuntomensaje);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(adjuntomensaje);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var adjuntomensaje = CRUD<AdjuntoMensaje>.GetById(id);
        if (adjuntomensaje == null)
        {
            return NotFound();
        }
        else {
            return View(adjuntomensaje); 
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id,AdjuntoMensaje adjuntomensaje)
    {
        try
        {
            CRUD<AdjuntoMensaje>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) {
            ModelState.AddModelError("", ex.Message);
            return View();
        
         }
    }
}
