using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("adjuntoMensajes")]
    public class AdjuntoMensaje
    {
        [Key]
        [Column("id_adjunto")]
        public int idAdjunto { get; set; }
        [Required]
        [Column("url_archivo")]
        public string urlArchivo { get; set; }

        [ForeignKey("mensajes")]
        [Column("id_mensaje")]
        public int idMensaje { get; set; }
        [ForeignKey("tiposAdjutnos")]
        [Column("id_tipo_adjunto")]
        public int idTipoAdjunto { get; set; }

        public Mensaje? mensajes { get; set; }
        public TipoAdjunto? tiposAdjuntos { get; set; }
    }
}
