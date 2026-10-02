using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Consumer;
using WebMensajeria.Modelos;

namespace WebMensajeria.MVC.Controllers
{
    [Authorize]
    public class MensajeriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Mensajeria/BuscarUsuarios?texto=ju
        // El usuario actual sale de la cookie, nunca del navegador.
        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios(string? texto)
        {
            var idActual = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            using var client = new HttpClient();
            var url = $"{CRUD<Usuario>.Endpoint}/buscar?texto={Uri.EscapeDataString(texto ?? "")}&idActual={idActual}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}
