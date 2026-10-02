using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMensajeria.Consumer;
using WebMensajeria.Modelos;

namespace WebMensajeria.MVC.Controllers
{
    [Authorize]
    public class MensajeriaController : Controller
    {
        // El usuario actual sale SIEMPRE de la cookie, nunca del navegador.
        private int IdActual =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private string ApiChats => CRUD<Chat>.Endpoint;
        private string ApiMensajes => CRUD<Mensaje>.Endpoint;

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Mensajeria/Conversaciones
        [HttpGet]
        public async Task<IActionResult> Conversaciones()
        {
            using var client = new HttpClient();
            return await Reenviar(await client.GetAsync($"{ApiChats}/usuario/{IdActual}"));
        }

        // GET: /Mensajeria/BuscarUsuarios?texto=ju
        [HttpGet]
        public async Task<IActionResult> BuscarUsuarios(string? texto)
        {
            using var client = new HttpClient();
            var url = $"{CRUD<Usuario>.Endpoint}/buscar?texto={Uri.EscapeDataString(texto ?? "")}&idActual={IdActual}";
            return await Reenviar(await client.GetAsync(url));
        }

        // GET: /Mensajeria/ChatPrivado?idOtro=5
        [HttpGet]
        public async Task<IActionResult> ChatPrivado(int idOtro)
        {
            using var client = new HttpClient();
            var url = $"{ApiChats}/privado?idUsuario={IdActual}&idOtro={idOtro}";
            return await Reenviar(await client.GetAsync(url));
        }

        // GET: /Mensajeria/Mensajes?idChat=7
        [HttpGet]
        public async Task<IActionResult> Mensajes(int idChat)
        {
            using var client = new HttpClient();
            var url = $"{ApiMensajes}/chat/{idChat}?idUsuario={IdActual}";
            return await Reenviar(await client.GetAsync(url));
        }

        // POST: /Mensajeria/EliminarMensaje?idMensaje=12
        // Solo el autor puede eliminar su mensaje.
        [HttpPost]
        public async Task<IActionResult> EliminarMensaje(int idMensaje)
        {
            using var client = new HttpClient();

            var consulta = await client.GetAsync($"{ApiMensajes}/{idMensaje}");
            if (!consulta.IsSuccessStatusCode)
                return StatusCode((int)consulta.StatusCode);

            using var doc = JsonDocument.Parse(await consulta.Content.ReadAsStringAsync());
            var esAutor = doc.RootElement.TryGetProperty("idUsuario", out var autor)
                          && autor.GetInt32() == IdActual;

            if (!esAutor)
                return StatusCode(StatusCodes.Status403Forbidden, "Solo puedes eliminar tus propios mensajes.");

            return await Reenviar(await client.DeleteAsync($"{ApiMensajes}/{idMensaje}"));
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
