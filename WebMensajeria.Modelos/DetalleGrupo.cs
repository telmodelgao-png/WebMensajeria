using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("detallesGrupos")]
    public class DetalleGrupo
    {
        [Key]
        [Column("id_detalle_grupo")]
        public int idDetalleGrupo { get; set; }
        [Required]
        [Column("nombre_grupo")]
        public string nombreGrupo { get; set; }

        [Required]
        public string descripcion { get; set; }
        [ForeignKey("idChatC")]
        [Column("Id_chat")]
        public int idChat { get; set; }

        public Chat? idChatC { get; set; }
    }
}
