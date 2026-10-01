using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(string correoelectronico, string password);
        Task<bool> Register(string nombreUsuario,
            string correoelectronico,
            string password);
    }
}
