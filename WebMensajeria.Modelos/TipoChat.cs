using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("tipoChats")]
    public class TipoChat
    {
        [Key]
        [Column("id_tipo_chat")]
        public int idTipoChat { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nombre_tipo_chat")]
        public string nombreTipoChat { get; set; }

        public List<Chat> chats { get; set; } = new List<Chat>();
    }
}
