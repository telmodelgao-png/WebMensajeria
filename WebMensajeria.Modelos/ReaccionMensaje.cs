using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    public class ReaccionMensaje
    {
        [Key]
        [Column("id_reaccion")]
        public int idReaccion { get; set; }

        [ForeignKey("mensajes")]
        [Column("id_mensaje")]
        public int idMensaje { get; set; }
        [ForeignKey("usuario")]
        [Column("id_usuario")]
        public int idUsuario { get; set; }
        [ForeignKey("tipoReaccion")]
        [Column("id_tipo_reaccion")]
        public int idTipoReaccion { get; set; }

        public Mensaje? mensajes { get; set; }
        public Usuario? usuario { get; set; }
        public TipoReaccion? TipoReaccion { get; set; }
    }
}
