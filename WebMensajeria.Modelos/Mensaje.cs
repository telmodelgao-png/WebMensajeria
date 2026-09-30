using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("mensajes")]
    public class Mensaje
    {
        [Key]
        [Column("id_mensaje")]
        public int idMensaje { get; set; }

        [Required]
        public string mensaje { get; set; }
        [Required]
        [Column("Fecha_envio", TypeName = "timestamp")]
        public DateTime fechaEnvio { get; set; }

        [ForeignKey("chats")]
        [Column("id_chat")]
        public int idChat { get; set; }
        [ForeignKey("usuarioRemitente")]
        [Column("id_Usuario_remitente")]
        public int idUsuario { get; set; }
        public Chat? chats { get; set; }
        public Usuario? usuarioRemitente { get; set; }
        public List<AdjuntoMensaje> adjuntos { get; set; }=new List<AdjuntoMensaje>();
        public List<EstadoReceptorMensaje> estadoReceptorMensajes { get; set; } = new List<EstadoReceptorMensaje>();
        public List<ReaccionMensaje> reaccionMensajes { get;set; }=new List<ReaccionMensaje>();
    }
}
