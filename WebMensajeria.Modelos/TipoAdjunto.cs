using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("tipoAdjuntos")]
    public class TipoAdjunto
    {
        [Key]
        [Column("id_tipo_adjunto")]
        public int idTipoAdjunto { get; set; }
        [Required]
        [MaxLength(50)]
        [Column("nombre_tipo")]
        public string nombreTipo { get; set; }
        public List<AdjuntoMensaje> adjuntos { get; set; } = new List<AdjuntoMensaje>();

    }
}
