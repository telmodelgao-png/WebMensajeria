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
    [Table("estadoreceptormesajes")]
    public class EstadoReceptorMensaje
    {
        [Key]
        [Column("id_estado_recpetor")]
        public int idEstadoReceptor { get; set; }

        [ForeignKey("mensajes")]
        [Column("id_mensaje")]
        public int idMensaje { get; set; }
        [ForeignKey("usuarioReceptor")]
        [Column("id_usuario_receptor")]
        public int idUsuario { get; set; }
        [ForeignKey("tipoestado")]
        [Column("id_tipo_estado")]
        public int tipoEstado { get; set; }
        //objetos de navegacion 
        public Mensaje? mensajes { get; set; }
        public Usuario? usuarioReceptor { get; set; }
        public TipoEstadoMensaje? tipoestado { get; set; }
    }
}
