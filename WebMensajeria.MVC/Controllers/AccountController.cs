using WebMensajeria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebMensajeria.MVC.Controllers
{
    public class AccountController: Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /Account/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Account/Index
        [HttpPost]
        public async Task<IActionResult> Index(
            string correoelectronico,
            string password)
        {
            if (string.IsNullOrWhiteSpace(correoelectronico) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Ingrese el correo y la contraseña.";
                return View();
            }

            correoelectronico = correoelectronico.Trim().ToLower();

            var resultado = await _authService.Login(
                correoelectronico,
                password
            );

            if (!resultado)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(
            string correoelectronico,
            string nombreUsuario,
            string password,
            string confirmarPassword)
        {
            if (string.IsNullOrWhiteSpace(correoelectronico) ||
                string.IsNullOrWhiteSpace(nombreUsuario) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Todos los campos son obligatorios.";
                return View();
            }

            if (password != confirmarPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            correoelectronico = correoelectronico.Trim().ToLower();

            var resultado = await _authService.Register(
                correoelectronico,
                nombreUsuario,
                password
            );

            if (!resultado)
            {
                ViewBag.Error = "Ya existe un usuario con ese correo.";
                return View();
            }

            TempData["Mensaje"] =
                "Registro exitoso. Ahora puede iniciar sesión.";

            return RedirectToAction("Index");
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Index");
        }
    }
}
