using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("contactos")]
    public class Contacto
    {
        [Key]
        [Column("id_contacto")]
        public int idContacto { get; set; }

        [ForeignKey(nameof(usuario_principal))]
        [Column("id_usuario_principal")]
        public int idUsuarioPrincipal { get; set; }
        [ForeignKey(nameof(usuario_contacto))]
        [Column("id_usuario_contacto")]
        public int idUsuarioContacto { get; set; }
        //objetos de navegacion 
        public Usuario? usuario_principal { get; set; }
        public Usuario? usuario_contacto { get; set; } 
    }
}
