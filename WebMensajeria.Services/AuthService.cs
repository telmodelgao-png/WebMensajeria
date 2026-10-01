
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WebMensajeria.Consumer;
using WebMensajeria.Modelos;
using WebMensajeria.Services.Interfaces;

namespace WebMensajeria.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(string correo, string password)
        {
            var usuarios = CRUD<Usuario>.GetAll();

            var usuario = usuarios.FirstOrDefault(u =>
                u.correoelectronico.Equals(correo, StringComparison.OrdinalIgnoreCase));

            if (usuario == null)
            {
                return false;
            }

            bool passwordCorrecta =
                BCrypt.Net.BCrypt.Verify(password, usuario.password);

            if (!passwordCorrecta)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuario.correoelectronico),
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.IdUsuario.ToString()
                ),
                new Claim(
                    "NombreUsuario",
                    usuario.nombreUsuario
                )
            };

            var identity = new ClaimsIdentity(
                claims,
                "Cookies"
            );

            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext!.SignInAsync(
                "Cookies",
                principal
            );

            return true;
        }

        public async Task<bool> Register(
            string correoelectronico,
            string nombreUsuario,
            string password)
        {
            var usuarios = CRUD<Usuario>.GetAll();

            var existe = usuarios.Any(u =>
                u.correoelectronico.Equals(
                    correoelectronico,
                    StringComparison.OrdinalIgnoreCase
                ));

            if (existe)
            {
                return false;
            }

            var usuario = new Usuario
            {
                correoelectronico = correoelectronico,
                nombreUsuario = nombreUsuario,
                password = password
            };

            CRUD<Usuario>.Create(usuario);

            return true;
        }
    }
}