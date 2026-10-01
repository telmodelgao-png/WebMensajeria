
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Modelos;
using WebMensajeria.Consumer;
using Microsoft.AspNetCore.Authorization;
[Authorize]
public class RolParticipantesController : Controller
{


    // GET: ADJUNTOMENSAJES
    public ActionResult Index()
    {
        var rolparticipantes = CRUD<RolParticipante>.GetAll();
        return View(rolparticipantes);
    }

    // GET: ADJUNTOMENSAJES/Details/5
    public ActionResult Details(int id)
    {
        var rolparticipantes = CRUD<RolParticipante>.GetById(id);
        if (rolparticipantes == null)
        {
            return NotFound();
        }
        else
        {
            return View(rolparticipantes);
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
    public ActionResult Create(RolParticipante rolparticipantes)
    {
        try
        {
            CRUD<RolParticipante>.Create(rolparticipantes);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(rolparticipantes);
        }
    }

    // GET: ADJUNTOMENSAJES/Edit/5
    public ActionResult Edit(int id)
    {
        var rolparticipantes = CRUD<RolParticipante>.GetById(id);
        if (rolparticipantes == null)
        {
            return NotFound();
        }
        else
        {
            return View(rolparticipantes);
        }
    }

    // POST: ADJUNTOMENSAJES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, RolParticipante rolparticipantes)
    {
        try
        {
            CRUD<RolParticipante>.Update(id, rolparticipantes);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(rolparticipantes);
        }
    }

    // GET: ADJUNTOMENSAJES/Delete/5
    public ActionResult Delete(int id)
    {
        var rolparticipantes = CRUD<RolParticipante>.GetById(id);
        if (rolparticipantes == null)
        {
            return NotFound();
        }
        else
        {
            return View(rolparticipantes);
        }
    }

    // POST: ADJUNTOMENSAJES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, RolParticipante rolparticipantes)
    {
        try
        {
            CRUD<RolParticipante>.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();

        }
    }
}
