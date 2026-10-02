
using System.Net.Http.Json;
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
        private int IdActual =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string ApiChats =>
            CRUD<Chat>.Endpoint;

        private string ApiMensajes =>
            CRUD<Mensaje>.Endpoint;

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Mensajeria/Conversaciones
        [HttpGet]
        public async Task<IActionResult> Conversaciones()
        {
            using var client = new HttpClient();

            var url = $"{ApiChats}/usuario/{IdActual}";
            return await Reenviar(await client.GetAsync(url));
        }

        // GET: /Mensajeria/BuscarUsuarios?texto=ju
        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios(string? texto)
        {
            using var client = new HttpClient();

            var url =
                $"{CRUD<Usuario>.Endpoint}/buscar?texto={Uri.EscapeDataString(texto ?? "")}&idActual={IdActual}";

            return await Reenviar(await client.GetAsync(url));
        }

        // GET: /Mensajeria/ChatPrivado?idOtro=5
        [HttpGet]
        public async Task<IActionResult> ChatPrivado(int idOtro)
        {
            using var client = new HttpClient();

            var url =
                $"{ApiChats}/privado?idUsuario={IdActual}&idOtro={idOtro}";

            return await Reenviar(await client.GetAsync(url));
        }

        // GET: /Mensajeria/Mensajes?idChat=7
        [HttpGet]
        public async Task<IActionResult> Mensajes(int idChat)
        {
            using var client = new HttpClient();

            var url =
                $"{ApiMensajes}/chat/{idChat}?idUsuario={IdActual}";

            return await Reenviar(await client.GetAsync(url));
        }

        public class EnviarDto
        {
            public int? IdChat { get; set; }
            public int? IdDestinatario { get; set; }
            public string? Texto { get; set; }
        }

        // POST: /Mensajeria/EnviarMensaje
        [HttpPost]
        public async Task<IActionResult> EnviarMensaje(
            [FromBody] EnviarDto dto)
        {
            using var client = new HttpClient();

            // El envío por API requiere un endpoint específico.
            // Por ahora no se cambia el flujo SignalR existente.
            return StatusCode(
                StatusCodes.Status501NotImplemented,
                "El envío debe realizarse mediante el flujo SignalR configurado.");
        }

        // POST: /Mensajeria/EliminarMensaje?idMensaje=12
        [HttpPost]
        public async Task<IActionResult> EliminarMensaje(int idMensaje)
        {
            using var client = new HttpClient();

            var url = $"{ApiMensajes}/{idMensaje}";
            return await Reenviar(await client.DeleteAsync(url));
        }

        private async Task<IActionResult> Reenviar(HttpResponseMessage r)
        {
            var cuerpo = await r.Content.ReadAsStringAsync();

            if (!r.IsSuccessStatusCode)
                return StatusCode((int)r.StatusCode, cuerpo);

            if (string.IsNullOrEmpty(cuerpo))
                return NoContent();

            return Content(cuerpo, "application/json");
        }
    }
}