using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("tiporeacciones")]

    public class TipoReaccion
    {
        [Key]
        [Column("id_tipo_reaccion")]
        public int idTipoReaccion { get; set; }
        [Required]
        [MaxLength(10)]
        public string simbolo { get; set; }
        public List<ReaccionMensaje> reaccionMensajes { get; set; } = new List<ReaccionMensaje>();

    }
}
