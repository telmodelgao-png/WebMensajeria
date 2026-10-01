
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
public class DetalleGruposController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var detallegrupo = CRUD<DetalleGrupo>.GetAll();
        return View(detallegrupo);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var detallegrupo = CRUD<DetalleGrupo>.GetById(id);
        if (detallegrupo == null)
        {
            return NotFound();
        }
        else
        {
            return View(detallegrupo);
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
    public ActionResult Create(DetalleGrupo detallegrupo)
    {
        try
        {
            CRUD<DetalleGrupo>.Create(detallegrupo);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(detallegrupo);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var detallegrupo = CRUD<DetalleGrupo>.GetById(id);
        if (detallegrupo == null)
        {
            return NotFound();
        }
        else
        {
            return View(detallegrupo);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, DetalleGrupo detallegrupo)
    {
        try
        {
            CRUD<DetalleGrupo>.Update(id, detallegrupo);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(detallegrupo);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var detallegrupo = CRUD<DetalleGrupo>.GetById(id);
        if (detallegrupo == null)
        {
            return NotFound();
        }
        else
        {
            return View(detallegrupo);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, DetalleGrupo detallegrupo)
    {
        try
        {
            CRUD<DetalleGrupo>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
