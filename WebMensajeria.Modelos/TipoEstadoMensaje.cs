using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("tipoestados")]
    public class TipoEstadoMensaje
    {
        [Key]
        [Column("id_tipo_estado")]
        public int idTipoEstado { get; set; }
        [Required]
        [MaxLength(50)]
        public string tipoEstado { get; set; }
        public List<EstadoReceptorMensaje> estadoReceptorMensajes { get; set; } = new List<EstadoReceptorMensaje>();

    }
}
