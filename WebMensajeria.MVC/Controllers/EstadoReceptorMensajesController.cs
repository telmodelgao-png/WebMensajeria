
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
using Microsoft.AspNetCore.Authorization;
[Authorize]
public class EstadoReceptorMensajesController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var estadoreceptormensaje = CRUD<EstadoReceptorMensaje>.GetAll();
        return View(estadoreceptormensaje);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var estadoreceptormensaje = CRUD<EstadoReceptorMensaje>.GetById(id);
        if (estadoreceptormensaje == null)
        {
            return NotFound();
        }
        else
        {
            return View(estadoreceptormensaje);
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
    public ActionResult Create(EstadoReceptorMensaje estadoreceptormensaje)
    {
        try
        {
            CRUD<EstadoReceptorMensaje>.Create(estadoreceptormensaje);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(estadoreceptormensaje);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var estadoreceptormensaje = CRUD<EstadoReceptorMensaje>.GetById(id);
        if (estadoreceptormensaje == null)
        {
            return NotFound();
        }
        else
        {
            return View(estadoreceptormensaje);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, EstadoReceptorMensaje estadoreceptormensaje)
    {
        try
        {
            CRUD<EstadoReceptorMensaje>.Update(id, estadoreceptormensaje);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(estadoreceptormensaje);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var estadoreceptormensaje = CRUD<EstadoReceptorMensaje>.GetById(id);
        if (estadoreceptormensaje == null)
        {
            return NotFound();
        }
        else
        {
            return View(estadoreceptormensaje);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, EstadoReceptorMensaje estadoreceptormensaje)
    {
        try
        {
            CRUD<EstadoReceptorMensaje>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
