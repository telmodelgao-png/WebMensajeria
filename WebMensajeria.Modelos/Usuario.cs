using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
namespace WebMensajeria.Modelos;

[Table("usuarios")]

public class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }
    [Required]
    [MaxLength(50)]
    [Column("nombre_usuario")]
    public string nombreUsuario { get; set; }
    [Required]
    [MaxLength(100)]
    public string correoelectronico { get; set; }
    [Required]
    public string password { get; set; }
    [InverseProperty(nameof(Contacto.usuario_principal))]
    public List<Contacto> misContactos { get; set; } = new List<Contacto>();
    [InverseProperty(nameof(Contacto.usuario_contacto))]
    public List<Contacto> soyContacto { get; set; } =new List<Contacto>();
    public List<ParticipanteChat> participantesChats { get; set; } = new List<ParticipanteChat>();
    public List<Mensaje> mensajesEnviados { get; set; } = new List<Mensaje>();
    public List<EstadoReceptorMensaje> estadoReceptorMensajes { get; set; } = new List<EstadoReceptorMensaje>();
    public List<ReaccionMensaje> reaccionMensajes { get; set; } = new List<ReaccionMensaje>();


}
