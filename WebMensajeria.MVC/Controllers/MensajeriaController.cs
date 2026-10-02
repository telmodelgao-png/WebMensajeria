using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMensajeria.MVC.Controllers
{
    // Solo muestra la pantalla del chat. Aquí agregarás después tu lógica (o la llamas desde el JavaScript).
    [Authorize]
    public class MensajeriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
