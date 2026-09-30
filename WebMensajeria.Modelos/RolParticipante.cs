using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMensajeria.Modelos
{
    [Table("rolparticipantes")]
    public class RolParticipante
    {
        [Key]
        [Column("id_rol")]
        public int IdRol { get; set; }
        
        [Required]
        [Column("nombe_rol")]
        [MaxLength(50)]
        public string nombreRol { get; set; }

        public List<ParticipanteChat> participantesChats { get; set; } = new List<ParticipanteChat>();
    }
}
